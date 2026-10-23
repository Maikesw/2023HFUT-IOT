import sys
from PySide6.QtWidgets import (QApplication, QMainWindow, QWidget, QVBoxLayout, 
                               QHBoxLayout, QPushButton, QTextEdit, QLineEdit, 
                               QLabel, QListWidget, QListWidgetItem, QMessageBox,
                               QDialog, QDialogButtonBox, QSplitter, QTableWidget,
                               QTableWidgetItem, QHeaderView, QTabWidget, QStatusBar,
                               QToolBar, QSpinBox)
from PySide6.QtCore import Qt, QThread, Signal
from PySide6.QtGui import QFont, QColor, QBrush, QTextCharFormat, QTextOption, QTextCursor, QTextBlockFormat
from smtp_client import SMTPClient
from pop3_client import POP3Client
import threading

class SendEmailThread(QThread):
    """发送邮件线程"""
    finished = Signal(bool, str)
    
    def __init__(self, from_addr, to_addrs, subject, body):
        super().__init__()
        self.from_addr = from_addr
        self.to_addrs = to_addrs
        self.subject = subject
        self.body = body
    
    def run(self):
        client = SMTPClient()
        success, message = client.send_mail(self.from_addr, self.to_addrs, self.subject, self.body)
        self.finished.emit(success, message)

class ReceiveEmailThread(QThread):
    """接收邮件线程"""
    finished = Signal(list)
    error = Signal(str)
    
    def __init__(self, username, password=''):
        super().__init__()
        self.username = username
        self.password = password
    
    def run(self):
        try:
            client = POP3Client()
            success, message = client.login(self.username, self.password)
            if success:
                mails = client.get_all_mails()
                client.quit()
                self.finished.emit(mails)
            else:
                self.error.emit(message)
        except Exception as e:
            self.error.emit(str(e))

class BatchSendEmailThread(QThread):
    """批量发送邮件线程"""
    finished = Signal(list)
    
    def __init__(self, from_addr, to_addrs_list, subject, body):
        super().__init__()
        self.from_addr = from_addr
        self.to_addrs_list = to_addrs_list
        self.subject = subject
        self.body = body
    
    def run(self):
        client = SMTPClient()
        results = client.send_batch(self.from_addr, self.to_addrs_list, self.subject, self.body)
        self.finished.emit(results)

class ComposeEmailDialog(QDialog):
    """编写邮件对话框"""
    def __init__(self, parent=None, from_addr=''):
        super().__init__(parent)
        self.setWindowTitle('编写新邮件')
        self.setMinimumSize(600, 500)
        
        layout = QVBoxLayout()
        
        # 发件人
        from_layout = QHBoxLayout()
        from_layout.addWidget(QLabel('发件人:'))
        self.from_edit = QLineEdit(from_addr)
        from_layout.addWidget(self.from_edit)
        layout.addLayout(from_layout)
        
        # 收件人
        to_layout = QHBoxLayout()
        to_layout.addWidget(QLabel('收件人:'))
        self.to_edit = QLineEdit()
        self.to_edit.setPlaceholderText('多个收件人用逗号分隔')
        to_layout.addWidget(self.to_edit)
        layout.addLayout(to_layout)
        
        # 主题
        subject_layout = QHBoxLayout()
        subject_layout.addWidget(QLabel('主题:'))
        self.subject_edit = QLineEdit()
        subject_layout.addWidget(self.subject_edit)
        layout.addLayout(subject_layout)
        
        # 正文
        layout.addWidget(QLabel('正文:'))
        self.body_edit = QTextEdit()
        self.body_edit.setMinimumHeight(300)
        layout.addWidget(self.body_edit)
        
        # 按钮
        button_box = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        button_box.accepted.connect(self.accept)
        button_box.rejected.connect(self.reject)
        layout.addWidget(button_box)
        
        self.setLayout(layout)
    
    def get_email_data(self):
        """获取邮件数据"""
        from_addr = self.from_edit.text().strip()
        to_addrs = [addr.strip() for addr in self.to_edit.text().split(',') if addr.strip()]
        subject = self.subject_edit.text().strip()
        body = self.body_edit.toPlainText()
        return from_addr, to_addrs, subject, body

class BatchSendDialog(QDialog):
    """批量发送对话框"""
    def __init__(self, parent=None, from_addr=''):
        super().__init__(parent)
        self.setWindowTitle('批量发送邮件')
        self.setMinimumSize(600, 500)
        
        layout = QVBoxLayout()
        
        # 发件人
        from_layout = QHBoxLayout()
        from_layout.addWidget(QLabel('发件人:'))
        self.from_edit = QLineEdit(from_addr)
        from_layout.addWidget(self.from_edit)
        layout.addLayout(from_layout)
        
        # 收件人列表
        layout.addWidget(QLabel('收件人列表（每行一个或多个，用逗号分隔）:'))
        self.to_list_edit = QTextEdit()
        self.to_list_edit.setPlaceholderText('user1@example.com\nuser2@example.com, user3@example.com')
        self.to_list_edit.setMinimumHeight(200)
        layout.addWidget(self.to_list_edit)
        
        # 主题
        subject_layout = QHBoxLayout()
        subject_layout.addWidget(QLabel('主题:'))
        self.subject_edit = QLineEdit()
        subject_layout.addWidget(self.subject_edit)
        layout.addLayout(subject_layout)
        
        # 正文
        layout.addWidget(QLabel('正文:'))
        self.body_edit = QTextEdit()
        self.body_edit.setMinimumHeight(200)
        layout.addWidget(self.body_edit)
        
        # 按钮
        button_box = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        button_box.accepted.connect(self.accept)
        button_box.rejected.connect(self.reject)
        layout.addWidget(button_box)
        
        self.setLayout(layout)
    
    def get_email_data(self):
        """获取批量邮件数据"""
        from_addr = self.from_edit.text().strip()
        subject = self.subject_edit.text().strip()
        body = self.body_edit.toPlainText()
        
        # 解析收件人列表
        to_lines = self.to_list_edit.toPlainText().strip().split('\n')
        to_addrs_list = []
        for line in to_lines:
            if line.strip():
                addrs = [addr.strip() for addr in line.split(',') if addr.strip()]
                if addrs:
                    to_addrs_list.append(addrs)
        
        return from_addr, to_addrs_list, subject, body

class EmailClientGUI(QMainWindow):
    """邮件客户端主窗口"""
    def __init__(self):
        super().__init__()
        self.setWindowTitle('电子邮件客户端')
        self.setMinimumSize(900, 700)
        self.current_username = ''
        self.mails = []
        # 保存活跃线程引用
        self.active_threads = []
        # 跟踪已读邮件（使用邮件的时间戳作为唯一标识）
        self.read_mails = set()
        
        self.init_ui()
        self.apply_styles()
    
    def init_ui(self):
        """初始化UI"""
        central_widget = QWidget()
        self.setCentralWidget(central_widget)
        
        main_layout = QVBoxLayout()
        
        # 顶部工具栏
        toolbar_layout = QHBoxLayout()
        
        # 用户信息
        user_layout = QHBoxLayout()
        user_layout.addWidget(QLabel('用户名:'))
        self.username_edit = QLineEdit()
        self.username_edit.setPlaceholderText('输入用户名（如: user1@example.com）')
        self.username_edit.setMinimumWidth(200)
        user_layout.addWidget(self.username_edit)
        toolbar_layout.addLayout(user_layout)
        
        # 按钮
        self.compose_btn = QPushButton('编写新邮件')
        self.compose_btn.clicked.connect(self.compose_email)
        toolbar_layout.addWidget(self.compose_btn)
        
        self.batch_send_btn = QPushButton('批量发送')
        self.batch_send_btn.clicked.connect(self.batch_send_email)
        toolbar_layout.addWidget(self.batch_send_btn)
        
        self.receive_btn = QPushButton('接收邮件')
        self.receive_btn.clicked.connect(self.receive_emails)
        toolbar_layout.addWidget(self.receive_btn)
        
        self.refresh_btn = QPushButton('刷新')
        self.refresh_btn.clicked.connect(self.refresh_emails)
        toolbar_layout.addWidget(self.refresh_btn)
        
        toolbar_layout.addStretch()
        main_layout.addLayout(toolbar_layout)
        
        # 分割器
        splitter = QSplitter(Qt.Horizontal)
        
        # 左侧：邮件列表
        mail_list_widget = QWidget()
        mail_list_layout = QVBoxLayout()
        mail_list_layout.addWidget(QLabel('邮件列表:'))
        
        self.mail_table = QTableWidget()
        self.mail_table.setColumnCount(4)
        self.mail_table.setHorizontalHeaderLabels(['发件人', '主题', '日期', '预览'])
        self.mail_table.horizontalHeader().setStretchLastSection(True)
        self.mail_table.setSelectionBehavior(QTableWidget.SelectRows)
        self.mail_table.setEditTriggers(QTableWidget.NoEditTriggers)
        self.mail_table.itemDoubleClicked.connect(self.show_mail_detail)
        mail_list_layout.addWidget(self.mail_table)
        
        mail_list_widget.setLayout(mail_list_layout)
        splitter.addWidget(mail_list_widget)
        
        # 右侧：邮件详情
        detail_widget = QWidget()
        detail_layout = QVBoxLayout()
        
        # 详情工具栏
        detail_toolbar = QHBoxLayout()
        detail_toolbar.addWidget(QLabel('邮件详情:'))
        detail_toolbar.addStretch()
        
        # 字号调整
        detail_toolbar.addWidget(QLabel('字号:'))
        self.font_size_spin = QSpinBox()
        self.font_size_spin.setMinimum(8)
        self.font_size_spin.setMaximum(24)
        self.font_size_spin.setValue(12)
        self.font_size_spin.valueChanged.connect(self.change_font_size)
        detail_toolbar.addWidget(self.font_size_spin)
        
        detail_layout.addLayout(detail_toolbar)
        
        self.detail_text = QTextEdit()
        self.detail_text.setReadOnly(True)
        self.detail_text.setLineWrapMode(QTextEdit.WidgetWidth)
        self.detail_text.setWordWrapMode(QTextOption.WrapAtWordBoundaryOrAnywhere)
        self.detail_text.setAlignment(Qt.AlignLeft | Qt.AlignTop)
        detail_layout.addWidget(self.detail_text)
        
        detail_widget.setLayout(detail_layout)
        splitter.addWidget(detail_widget)
        
        splitter.setStretchFactor(0, 1)
        splitter.setStretchFactor(1, 1)
        
        main_layout.addWidget(splitter)
        
        central_widget.setLayout(main_layout)
        
        # 状态栏
        self.statusBar = QStatusBar()
        self.setStatusBar(self.statusBar)
        self.statusBar.showMessage('就绪 - 请确保邮件服务器已启动 (python start_servers.py)')
    
    def apply_styles(self):
        """应用样式表"""
        style = """
        /* 按钮样式 - 圆角 + hover效果 */
        QPushButton {
            background-color: #4A90E2;
            color: white;
            border: none;
            border-radius: 8px;
            padding: 8px 16px;
            font-size: 14px;
            font-weight: 500;
        }
        
        QPushButton:hover {
            background-color: #6BA3E8;
        }
        
        QPushButton:pressed {
            background-color: #357ABD;
        }
        
        QPushButton:disabled {
            background-color: #CCCCCC;
            color: #666666;
        }
        
        /* 输入框样式 - 边框高亮 */
        QLineEdit {
            border: 2px solid #E0E0E0;
            border-radius: 6px;
            padding: 6px 10px;
            font-size: 14px;
            background-color: white;
        }
        
        QLineEdit:focus {
            border: 2px solid #4A90E2;
            background-color: #F5F9FF;
        }
        
        QLineEdit:hover {
            border: 2px solid #B0B0B0;
        }
        
        /* 表格样式 */
        QTableWidget {
            border: 1px solid #E0E0E0;
            border-radius: 6px;
            background-color: white;
            gridline-color: #F0F0F0;
        }
        
        QTableWidget::item {
            padding: 8px;
            border: none;
        }
        
        QTableWidget::item:selected {
            background-color: #E3F2FD;
            color: black;
        }
        
        QHeaderView::section {
            background-color: #F5F5F5;
            padding: 8px;
            border: none;
            border-bottom: 2px solid #E0E0E0;
            font-weight: bold;
        }
        
        /* 文本编辑框样式 */
        QTextEdit {
            border: 1px solid #E0E0E0;
            border-radius: 6px;
            padding: 10px;
            background-color: white;
            font-size: 12px;
        }
        
        QTextEdit:focus {
            border: 2px solid #4A90E2;
        }
        
        /* 对话框按钮 */
        QDialogButtonBox QPushButton {
            min-width: 80px;
        }
        
        /* 对话框中的输入框 */
        QDialog QLineEdit {
            border: 2px solid #E0E0E0;
            border-radius: 6px;
            padding: 6px 10px;
            font-size: 14px;
            background-color: white;
        }
        
        QDialog QLineEdit:focus {
            border: 2px solid #4A90E2;
            background-color: #F5F9FF;
        }
        
        QDialog QTextEdit {
            border: 2px solid #E0E0E0;
            border-radius: 6px;
            padding: 8px;
            background-color: white;
            font-size: 14px;
        }
        
        QDialog QTextEdit:focus {
            border: 2px solid #4A90E2;
            background-color: #F5F9FF;
        }
        
        /* 状态栏 */
        QStatusBar {
            background-color: #F5F5F5;
            border-top: 1px solid #E0E0E0;
        }
        
        /* SpinBox样式 */
        QSpinBox {
            border: 2px solid #E0E0E0;
            border-radius: 6px;
            padding: 4px;
            background-color: white;
            font-size: 14px;
        }
        
        QSpinBox:focus {
            border: 2px solid #4A90E2;
            background-color: #F5F9FF;
        }
        
        QSpinBox:hover {
            border: 2px solid #B0B0B0;
        }
        """
        self.setStyleSheet(style)
    
    def compose_email(self):
        """编写新邮件"""
        from_addr = self.username_edit.text().strip() or 'user@example.com'
        dialog = ComposeEmailDialog(self, from_addr)
        if dialog.exec():
            from_addr, to_addrs, subject, body = dialog.get_email_data()
            
            if not from_addr or not to_addrs:
                QMessageBox.warning(self, '警告', '请填写发件人和收件人')
                return
            
            if not subject:
                QMessageBox.warning(self, '警告', '请填写邮件主题')
                return
            
            # 发送邮件
            self.send_email(from_addr, to_addrs, subject, body)
    
    def send_email(self, from_addr, to_addrs, subject, body):
        """发送邮件"""
        self.compose_btn.setEnabled(False)
        self.compose_btn.setText('发送中...')
        
        thread = SendEmailThread(from_addr, to_addrs, subject, body)
        thread.finished.connect(self.on_send_finished)
        thread.finished.connect(lambda: self._remove_thread(thread))
        self.active_threads.append(thread)
        thread.start()
    
    def on_send_finished(self, success, message):
        """发送完成回调"""
        self.compose_btn.setEnabled(True)
        self.compose_btn.setText('编写新邮件')
        
        if success:
            self.statusBar.showMessage('邮件发送成功', 3000)
            QMessageBox.information(self, '成功', message)
        else:
            # 提供更详细的错误信息
            if '无法连接到SMTP服务器' in message or '连接' in message:
                self.statusBar.showMessage('发送失败: 无法连接到SMTP服务器 - 请启动服务器 (python start_servers.py)', 5000)
                detailed_msg = (
                    f'发送邮件失败: {message}\n\n'
                    '可能的原因：\n'
                    '1. 邮件服务器未启动\n'
                    '2. 服务器地址或端口配置错误\n\n'
                    '解决方案：\n'
                    '请先启动邮件服务器：\n'
                    '在终端运行: python start_servers.py\n\n'
                    '服务器配置：\n'
                    '- SMTP服务器: 127.0.0.1:2525\n'
                    '- POP3服务器: 127.0.0.1:1100'
                )
                QMessageBox.warning(self, '连接错误', detailed_msg)
            else:
                self.statusBar.showMessage(f'发送失败: {message}', 5000)
                QMessageBox.warning(self, '失败', message)
    
    def batch_send_email(self):
        """批量发送邮件"""
        from_addr = self.username_edit.text().strip() or 'user@example.com'
        dialog = BatchSendDialog(self, from_addr)
        if dialog.exec():
            from_addr, to_addrs_list, subject, body = dialog.get_email_data()
            
            if not from_addr or not to_addrs_list:
                QMessageBox.warning(self, '警告', '请填写发件人和收件人列表')
                return
            
            if not subject:
                QMessageBox.warning(self, '警告', '请填写邮件主题')
                return
            
            # 批量发送
            self.batch_send_btn.setEnabled(False)
            self.batch_send_btn.setText('批量发送中...')
            
            thread = BatchSendEmailThread(from_addr, to_addrs_list, subject, body)
            thread.finished.connect(self.on_batch_send_finished)
            thread.finished.connect(lambda: self._remove_thread(thread))
            self.active_threads.append(thread)
            thread.start()
    
    def on_batch_send_finished(self, results):
        """批量发送完成回调"""
        self.batch_send_btn.setEnabled(True)
        self.batch_send_btn.setText('批量发送')
        
        # 显示结果
        success_count = sum(1 for r in results if r['success'])
        total_count = len(results)
        
        result_msg = f'批量发送完成: {success_count}/{total_count} 成功\n\n'
        for r in results:
            status = '✓' if r['success'] else '✗'
            result_msg += f"{status} {', '.join(r['to'])}: {r['message']}\n"
        
        QMessageBox.information(self, '批量发送结果', result_msg)
    
    def receive_emails(self):
        """接收邮件"""
        username = self.username_edit.text().strip()
        if not username:
            QMessageBox.warning(self, '警告', '请先输入用户名')
            return
        
        self.current_username = username
        self.receive_btn.setEnabled(False)
        self.receive_btn.setText('接收中...')
        
        thread = ReceiveEmailThread(username)
        thread.finished.connect(self.on_receive_finished)
        thread.error.connect(self.on_receive_error)
        thread.finished.connect(lambda: self._remove_thread(thread))
        thread.error.connect(lambda: self._remove_thread(thread))
        self.active_threads.append(thread)
        thread.start()
    
    def on_receive_finished(self, mails):
        """接收完成回调"""
        self.receive_btn.setEnabled(True)
        self.receive_btn.setText('接收邮件')
        self.mails = mails
        self.update_mail_list()
        self.statusBar.showMessage(f'成功接收 {len(mails)} 封邮件', 3000)
    
    def on_receive_error(self, error_msg):
        """接收错误回调"""
        self.receive_btn.setEnabled(True)
        self.receive_btn.setText('接收邮件')
        # 提供更详细的错误信息和解决方案
        if '无法连接到POP3服务器' in error_msg or '连接' in error_msg:
            self.statusBar.showMessage('接收失败: 无法连接到POP3服务器 - 请启动服务器 (python start_servers.py)', 5000)
            detailed_msg = (
                f'接收邮件失败: {error_msg}\n\n'
                '可能的原因：\n'
                '1. 邮件服务器未启动\n'
                '2. 服务器地址或端口配置错误\n\n'
                '解决方案：\n'
                '请先启动邮件服务器：\n'
                '在终端运行: python start_servers.py\n\n'
                '服务器配置：\n'
                '- POP3服务器: 127.0.0.1:1100\n'
                '- SMTP服务器: 127.0.0.1:2525'
            )
            QMessageBox.warning(self, '连接错误', detailed_msg)
        else:
            self.statusBar.showMessage(f'接收失败: {error_msg}', 5000)
            QMessageBox.warning(self, '错误', f'接收邮件失败: {error_msg}')
    
    def refresh_emails(self):
        """刷新邮件列表"""
        if self.current_username:
            self.receive_emails()
        else:
            QMessageBox.warning(self, '警告', '请先输入用户名并接收邮件')
    
    def update_mail_list(self):
        """更新邮件列表"""
        self.mail_table.setRowCount(len(self.mails))
        
        for i, mail in enumerate(self.mails):
            # 获取邮件唯一标识（使用timestamp）
            mail_id = mail.get('timestamp', '')
            is_read = mail_id in self.read_mails
            
            # 发件人
            from_item = QTableWidgetItem(mail.get('from', ''))
            self.mail_table.setItem(i, 0, from_item)
            
            # 主题 - 未读邮件加粗 + 浅蓝色背景
            subject_item = QTableWidgetItem(mail.get('subject', ''))
            if not is_read:
                font = QFont()
                font.setBold(True)
                subject_item.setFont(font)
                subject_item.setBackground(QBrush(QColor(230, 240, 255)))  # 浅蓝色背景
            self.mail_table.setItem(i, 1, subject_item)
            
            # 日期
            date_item = QTableWidgetItem(mail.get('date', ''))
            self.mail_table.setItem(i, 2, date_item)
            
            # 预览（正文前50个字符）
            body = mail.get('body', '')
            preview = body[:50] + '...' if len(body) > 50 else body
            preview_item = QTableWidgetItem(preview)
            self.mail_table.setItem(i, 3, preview_item)
            
            # 整行设置未读样式
            if not is_read:
                for col in range(4):
                    item = self.mail_table.item(i, col)
                    if item:
                        item.setBackground(QBrush(QColor(245, 250, 255)))  # 更浅的蓝色背景
        
        self.mail_table.resizeColumnsToContents()
    
    def show_mail_detail(self, item):
        """显示邮件详情"""
        row = item.row()
        if 0 <= row < len(self.mails):
            mail = self.mails[row]
            
            # 标记为已读
            mail_id = mail.get('timestamp', '')
            if mail_id:
                self.read_mails.add(mail_id)
                # 更新列表显示
                self.update_mail_list()
            
            # HTML转义函数
            def html_escape(text):
                if not text:
                    return ''
                return (str(text)
                       .replace('&', '&amp;')
                       .replace('<', '&lt;')
                       .replace('>', '&gt;')
                       .replace('"', '&quot;')
                       .replace("'", '&#x27;'))
            
            # 格式化邮件详情，增加间距和排版
            from_addr = html_escape(mail.get('from', ''))
            to_addrs = html_escape(', '.join(mail.get('to', [])))
            subject = html_escape(mail.get('subject', ''))
            date = html_escape(mail.get('date', ''))
            body = html_escape(mail.get('body', '')).replace('\n', '<br>')
            
            detail_html = f"""
            <div style='font-family: "Microsoft YaHei", Arial, sans-serif; line-height: 1.8;'>
                <div style='background-color: #F5F5F5; padding: 12px; border-radius: 6px; margin-bottom: 15px;'>
                    <p style='margin: 5px 0;'><strong>发件人:</strong> {from_addr}</p>
                    <p style='margin: 5px 0;'><strong>收件人:</strong> {to_addrs}</p>
                    <p style='margin: 5px 0;'><strong>主题:</strong> {subject}</p>
                    <p style='margin: 5px 0;'><strong>日期:</strong> {date}</p>
                </div>
                <div style='border-top: 2px solid #E0E0E0; padding-top: 15px; margin-top: 15px;'>
                    <h3 style='margin-top: 0; color: #333;'>正文内容:</h3>
                    <p style='text-align: left; white-space: pre-wrap; word-wrap: break-word; padding: 10px 0; margin: 0;'>
                        {body}
                    </p>
                </div>
            </div>
            """
            
            self.detail_text.setHtml(detail_html)
            # 应用当前字号
            self.change_font_size(self.font_size_spin.value())
            # 确保正文内容左对齐
            cursor = self.detail_text.textCursor()
            cursor.movePosition(QTextCursor.Start)
            cursor.movePosition(QTextCursor.End, QTextCursor.KeepAnchor)
            format = QTextBlockFormat()
            format.setAlignment(Qt.AlignLeft | Qt.AlignTop)
            cursor.setBlockFormat(format)
            cursor.clearSelection()
            self.detail_text.setTextCursor(cursor)
    
    def change_font_size(self, size):
        """改变邮件详情字号"""
        font = QFont()
        font.setPointSize(size)
        self.detail_text.setFont(font)
    
    def _remove_thread(self, thread):
        """从活跃线程列表中移除线程"""
        if thread in self.active_threads:
            self.active_threads.remove(thread)
    
    def closeEvent(self, event):
        """窗口关闭事件 - 等待所有线程完成"""
        # 等待所有活跃线程完成
        for thread in self.active_threads[:]:  # 使用切片复制列表
            if thread.isRunning():
                thread.wait(3000)  # 等待最多3秒
        event.accept()

def main():
    app = QApplication(sys.argv)
    window = EmailClientGUI()
    window.show()
    sys.exit(app.exec())

if __name__ == '__main__':
    main()

