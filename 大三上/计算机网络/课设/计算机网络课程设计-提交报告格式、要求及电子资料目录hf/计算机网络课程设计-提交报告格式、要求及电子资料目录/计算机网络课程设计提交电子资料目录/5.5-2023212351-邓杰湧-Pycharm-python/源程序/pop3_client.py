import socket
import json

class POP3Client:
    def __init__(self, host='127.0.0.1', port=1100):
        self.host = host
        self.port = port
        self.socket = None
        self.authenticated = False
        
    def connect(self):
        """连接到POP3服务器"""
        try:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.settimeout(5)  # 设置5秒超时
            self.socket.connect((self.host, self.port))
            response = self.socket.recv(1024).decode('utf-8')
            return response.startswith('+OK')
        except socket.timeout:
            return False
        except ConnectionRefusedError:
            return False
        except Exception as e:
            print(f"POP3连接错误: {e}")
            return False
    
    def send_command(self, command):
        """发送命令并接收响应"""
        if not self.socket:
            return None
        self.socket.send(f'{command}\r\n'.encode())
        response = self.socket.recv(4096).decode('utf-8')
        return response
    
    def login(self, username, password=''):
        """登录到POP3服务器"""
        if not self.connect():
            return False, "无法连接到POP3服务器"
        
        try:
            # USER
            response = self.send_command(f'USER {username}')
            if not response.startswith('+OK'):
                return False, f"USER命令失败: {response}"
            
            # PASS
            response = self.send_command(f'PASS {password}')
            if response.startswith('+OK'):
                self.authenticated = True
                return True, "登录成功"
            else:
                return False, f"PASS命令失败: {response}"
        
        except Exception as e:
            return False, f"登录时出错: {str(e)}"
    
    def get_mail_count(self):
        """获取邮件数量"""
        if not self.authenticated:
            return 0
        
        response = self.send_command('STAT')
        if response.startswith('+OK'):
            parts = response.split()
            if len(parts) >= 2:
                return int(parts[1])
        return 0
    
    def list_mails(self):
        """列出所有邮件"""
        if not self.authenticated:
            return []
        
        response = self.send_command('LIST')
        if not response.startswith('+OK'):
            return []
        
        lines = response.split('\r\n')
        mail_list = []
        for line in lines[1:]:  # 跳过第一行+OK
            if line == '.':
                break
            if line.strip():
                parts = line.split()
                if len(parts) >= 2:
                    mail_list.append({
                        'num': int(parts[0]),
                        'size': int(parts[1])
                    })
        
        return mail_list
    
    def retrieve_mail(self, msg_num):
        """检索指定邮件"""
        if not self.authenticated:
            return None
        
        response = self.send_command(f'RETR {msg_num}')
        if not response.startswith('+OK'):
            return None
        
        # 解析响应，提取JSON数据
        lines = response.split('\r\n')
        json_lines = []
        in_json = False
        for line in lines[1:]:  # 跳过第一行+OK
            if line == '.':
                break
            if in_json or line.strip().startswith('{'):
                in_json = True
                json_lines.append(line)
        
        json_str = '\n'.join(json_lines)
        try:
            mail_data = json.loads(json_str)
            return mail_data
        except json.JSONDecodeError:
            return None
    
    def get_all_mails(self):
        """获取所有邮件"""
        if not self.authenticated:
            return []
        
        mail_list = self.list_mails()
        mails = []
        for mail_info in mail_list:
            mail_data = self.retrieve_mail(mail_info['num'])
            if mail_data:
                mails.append(mail_data)
        
        return mails
    
    def quit(self):
        """退出连接"""
        if self.socket and self.authenticated:
            self.send_command('QUIT')
        if self.socket:
            self.socket.close()
            self.socket = None
        self.authenticated = False

