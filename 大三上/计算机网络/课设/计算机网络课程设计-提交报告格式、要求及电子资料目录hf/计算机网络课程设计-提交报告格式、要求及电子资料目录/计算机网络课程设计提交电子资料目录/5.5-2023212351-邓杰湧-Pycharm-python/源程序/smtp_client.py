import socket
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

class SMTPClient:
    def __init__(self, host='127.0.0.1', port=2525):
        self.host = host
        self.port = port
        self.socket = None
        
    def connect(self):
        """连接到SMTP服务器"""
        try:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.settimeout(5)  # 设置5秒超时
            self.socket.connect((self.host, self.port))
            response = self.socket.recv(1024).decode('utf-8')
            return response.startswith('220')
        except socket.timeout:
            return False
        except ConnectionRefusedError:
            return False
        except Exception as e:
            print(f"SMTP连接错误: {e}")
            return False
    
    def send_command(self, command):
        """发送命令并接收响应"""
        if not self.socket:
            return None
        self.socket.send(f'{command}\r\n'.encode())
        response = self.socket.recv(1024).decode('utf-8')
        return response
    
    def send_mail(self, from_addr, to_addrs, subject, body):
        """发送邮件"""
        try:
            if not self.connect():
                return False, "无法连接到SMTP服务器"
        except Exception as e:
            return False, f"连接SMTP服务器失败: {str(e)}"
        
        try:
            # HELO
            response = self.send_command('HELO localhost')
            if not response.startswith('250'):
                return False, f"HELO失败: {response}"
            
            # MAIL FROM
            response = self.send_command(f'MAIL FROM:<{from_addr}>')
            if not response.startswith('250'):
                return False, f"MAIL FROM失败: {response}"
            
            # RCPT TO (支持多个收件人)
            for to_addr in to_addrs:
                response = self.send_command(f'RCPT TO:<{to_addr}>')
                if not response.startswith('250'):
                    return False, f"RCPT TO失败: {response}"
            
            # DATA
            response = self.send_command('DATA')
            if not response.startswith('354'):
                return False, f"DATA失败: {response}"
            
            # 构建邮件内容
            msg = MIMEMultipart()
            msg['From'] = from_addr
            msg['To'] = ', '.join(to_addrs)
            msg['Subject'] = subject
            msg.attach(MIMEText(body, 'plain', 'utf-8'))
            
            # 发送邮件内容
            mail_data = msg.as_string()
            self.socket.send(mail_data.encode('utf-8'))
            self.socket.send(b'\r\n.\r\n')
            
            response = self.socket.recv(1024).decode('utf-8')
            if response.startswith('250'):
                # QUIT
                self.send_command('QUIT')
                return True, "邮件发送成功"
            else:
                return False, f"发送失败: {response}"
        
        except Exception as e:
            return False, f"发送邮件时出错: {str(e)}"
        finally:
            if self.socket:
                self.socket.close()
                self.socket = None
    
    def send_batch(self, from_addr, to_addrs_list, subject, body):
        """批量发送邮件"""
        results = []
        for to_addrs in to_addrs_list:
            success, message = self.send_mail(from_addr, to_addrs, subject, body)
            results.append({
                'to': to_addrs,
                'success': success,
                'message': message
            })
        return results

