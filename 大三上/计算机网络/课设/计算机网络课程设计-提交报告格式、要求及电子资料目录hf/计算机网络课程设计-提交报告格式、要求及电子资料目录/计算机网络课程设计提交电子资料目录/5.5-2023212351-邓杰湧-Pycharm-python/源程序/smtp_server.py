import socket
import threading
import datetime
from email.parser import Parser
from email.utils import formatdate
from email.header import decode_header
import json
import os

class SMTPServer:
    def __init__(self, host='127.0.0.1', port=2525):
        self.host = host
        self.port = port
        self.socket = None
        self.running = False
        self.mailbox_dir = 'mailbox'
        self.ensure_mailbox_dir()
        
    def ensure_mailbox_dir(self):
        """确保邮箱目录存在"""
        if not os.path.exists(self.mailbox_dir):
            os.makedirs(self.mailbox_dir)
            
    def save_mail(self, from_addr, to_addrs, subject, body):
        """保存邮件到邮箱"""
        timestamp = datetime.datetime.now().strftime('%Y%m%d_%H%M%S_%f')
        mail_data = {
            'from': from_addr,
            'to': to_addrs,
            'subject': subject,
            'body': body,
            'date': formatdate(localtime=True),
            'timestamp': timestamp
        }
        
        # 为每个收件人保存一份邮件
        for to_addr in to_addrs:
            user_dir = os.path.join(self.mailbox_dir, to_addr)
            if not os.path.exists(user_dir):
                os.makedirs(user_dir)
            
            mail_file = os.path.join(user_dir, f'{timestamp}.json')
            with open(mail_file, 'w', encoding='utf-8') as f:
                json.dump(mail_data, f, ensure_ascii=False, indent=2)
        
        return True
    
    def handle_client(self, client_socket, addr):
        """处理客户端连接"""
        try:
            client_socket.send(b'220 SMTP Server Ready\r\n')
            
            from_addr = None
            to_addrs = []
            data_lines = []
            in_data_mode = False
            
            while True:
                data = client_socket.recv(1024).decode('utf-8', errors='ignore')
                if not data:
                    break
                
                command = data.strip().upper()
                
                if command.startswith('HELO') or command.startswith('EHLO'):
                    client_socket.send(b'250 Hello\r\n')
                
                elif command.startswith('MAIL FROM:'):
                    from_addr = command.split(':', 1)[1].strip().strip('<>')
                    client_socket.send(b'250 OK\r\n')
                
                elif command.startswith('RCPT TO:'):
                    to_addr = command.split(':', 1)[1].strip().strip('<>')
                    to_addrs.append(to_addr)
                    client_socket.send(b'250 OK\r\n')
                
                elif command == 'DATA':
                    client_socket.send(b'354 End data with <CR><LF>.<CR><LF>\r\n')
                    in_data_mode = True
                
                elif in_data_mode:
                    if data.strip() == '.':
                        # 解析邮件内容
                        mail_content = '\r\n'.join(data_lines)
                        parser = Parser()
                        msg = parser.parsestr(mail_content)
                        
                        # 解码主题（处理MIME编码）
                        subject_header = msg.get('Subject', '')
                        if subject_header:
                            decoded_parts = decode_header(subject_header)
                            subject_parts = []
                            for part, encoding in decoded_parts:
                                if isinstance(part, bytes):
                                    if encoding:
                                        try:
                                            subject_parts.append(part.decode(encoding))
                                        except:
                                            subject_parts.append(part.decode('utf-8', errors='ignore'))
                                    else:
                                        subject_parts.append(part.decode('utf-8', errors='ignore'))
                                else:
                                    subject_parts.append(part)
                            subject = ''.join(subject_parts)
                        else:
                            subject = ''
                        body = ''
                        if msg.is_multipart():
                            for part in msg.walk():
                                if part.get_content_type() == 'text/plain':
                                    body = part.get_payload(decode=True).decode('utf-8', errors='ignore')
                        else:
                            body = msg.get_payload(decode=True).decode('utf-8', errors='ignore') if msg.get_payload() else ''
                        
                        # 保存邮件
                        self.save_mail(from_addr, to_addrs, subject, body)
                        client_socket.send(b'250 Message accepted\r\n')
                        in_data_mode = False
                        data_lines = []
                        from_addr = None
                        to_addrs = []
                    else:
                        data_lines.append(data.rstrip('\r\n'))
                
                elif command == 'QUIT':
                    client_socket.send(b'221 Bye\r\n')
                    break
                
                elif command.startswith('RSET'):
                    from_addr = None
                    to_addrs = []
                    data_lines = []
                    in_data_mode = False
                    client_socket.send(b'250 OK\r\n')
                
                else:
                    client_socket.send(b'500 Command not recognized\r\n')
        
        except Exception as e:
            print(f"Error handling client {addr}: {e}")
        finally:
            client_socket.close()
    
    def start(self):
        """启动SMTP服务器"""
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.socket.bind((self.host, self.port))
        self.socket.listen(5)
        self.running = True
        
        print(f'SMTP Server started on {self.host}:{self.port}')
        
        while self.running:
            try:
                client_socket, addr = self.socket.accept()
                print(f'SMTP Client connected from {addr[0]}:{addr[1]} (server listening on {self.host}:{self.port})')
                thread = threading.Thread(target=self.handle_client, args=(client_socket, addr))
                thread.daemon = True
                thread.start()
            except Exception as e:
                if self.running:
                    print(f"SMTP Server error: {e}")
    
    def stop(self):
        """停止SMTP服务器"""
        self.running = False
        if self.socket:
            self.socket.close()
            print('SMTP Server stopped')

if __name__ == '__main__':
    server = SMTPServer()
    try:
        server.start()
    except KeyboardInterrupt:
        server.stop()

