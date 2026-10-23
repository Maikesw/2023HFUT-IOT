import socket
import threading
import os
import json
import glob

class POP3Server:
    def __init__(self, host='127.0.0.1', port=1100):
        self.host = host
        self.port = port
        self.socket = None
        self.running = False
        self.mailbox_dir = 'mailbox'
        
    def get_user_mails(self, username):
        """获取用户的所有邮件"""
        user_dir = os.path.join(self.mailbox_dir, username)
        if not os.path.exists(user_dir):
            return []
        
        mail_files = sorted(glob.glob(os.path.join(user_dir, '*.json')))
        mails = []
        for mail_file in mail_files:
            try:
                with open(mail_file, 'r', encoding='utf-8') as f:
                    mail_data = json.load(f)
                    mails.append(mail_data)
            except Exception as e:
                print(f"Error reading mail {mail_file}: {e}")
        
        return mails
    
    def handle_client(self, client_socket, addr):
        """处理客户端连接"""
        try:
            client_socket.send(b'+OK POP3 Server Ready\r\n')
            
            username = None
            authenticated = False
            mails = []
            
            while True:
                data = client_socket.recv(1024).decode('utf-8', errors='ignore')
                if not data:
                    break
                
                command = data.strip().upper()
                parts = command.split()
                
                if not authenticated:
                    if parts[0] == 'USER':
                        username = parts[1] if len(parts) > 1 else ''
                        client_socket.send(b'+OK User accepted\r\n')
                    elif parts[0] == 'PASS':
                        # 简单认证，任何密码都接受
                        authenticated = True
                        mails = self.get_user_mails(username)
                        client_socket.send(f'+OK {len(mails)} messages\r\n'.encode())
                    else:
                        client_socket.send(b'-ERR Authentication required\r\n')
                
                else:
                    if parts[0] == 'STAT':
                        total_size = sum(len(json.dumps(mail, ensure_ascii=False).encode('utf-8')) for mail in mails)
                        client_socket.send(f'+OK {len(mails)} {total_size}\r\n'.encode())
                    
                    elif parts[0] == 'LIST':
                        if len(parts) > 1:
                            # LIST n - 获取特定邮件信息
                            try:
                                msg_num = int(parts[1])
                                if 1 <= msg_num <= len(mails):
                                    mail = mails[msg_num - 1]
                                    size = len(json.dumps(mail, ensure_ascii=False).encode('utf-8'))
                                    client_socket.send(f'+OK {msg_num} {size}\r\n'.encode())
                                else:
                                    client_socket.send(b'-ERR No such message\r\n')
                            except ValueError:
                                client_socket.send(b'-ERR Invalid message number\r\n')
                        else:
                            # LIST - 列出所有邮件
                            response = '+OK\r\n'
                            for i, mail in enumerate(mails, 1):
                                size = len(json.dumps(mail, ensure_ascii=False).encode('utf-8'))
                                response += f'{i} {size}\r\n'
                            response += '.\r\n'
                            client_socket.send(response.encode())
                    
                    elif parts[0] == 'RETR':
                        try:
                            msg_num = int(parts[1])
                            if 1 <= msg_num <= len(mails):
                                mail = mails[msg_num - 1]
                                mail_str = json.dumps(mail, ensure_ascii=False, indent=2)
                                response = f'+OK {len(mail_str)} bytes\r\n'
                                response += mail_str
                                response += '\r\n.\r\n'
                                client_socket.send(response.encode())
                            else:
                                client_socket.send(b'-ERR No such message\r\n')
                        except (ValueError, IndexError):
                            client_socket.send(b'-ERR Invalid command\r\n')
                    
                    elif parts[0] == 'DELE':
                        try:
                            msg_num = int(parts[1])
                            if 1 <= msg_num <= len(mails):
                                # 标记删除（实际实现中可以删除文件）
                                client_socket.send(b'+OK Message deleted\r\n')
                            else:
                                client_socket.send(b'-ERR No such message\r\n')
                        except (ValueError, IndexError):
                            client_socket.send(b'-ERR Invalid command\r\n')
                    
                    elif parts[0] == 'NOOP':
                        client_socket.send(b'+OK\r\n')
                    
                    elif parts[0] == 'RSET':
                        mails = self.get_user_mails(username)
                        client_socket.send(b'+OK Reset\r\n')
                    
                    elif parts[0] == 'QUIT':
                        client_socket.send(b'+OK Bye\r\n')
                        break
                    
                    else:
                        client_socket.send(b'-ERR Unknown command\r\n')
        
        except Exception as e:
            print(f"Error handling POP3 client {addr}: {e}")
        finally:
            client_socket.close()
    
    def start(self):
        """启动POP3服务器"""
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.socket.bind((self.host, self.port))
        self.socket.listen(5)
        self.running = True
        
        print(f'POP3 Server started on {self.host}:{self.port}')
        
        while self.running:
            try:
                client_socket, addr = self.socket.accept()
                print(f'POP3 Client connected: {addr}')
                thread = threading.Thread(target=self.handle_client, args=(client_socket, addr))
                thread.daemon = True
                thread.start()
            except Exception as e:
                if self.running:
                    print(f"POP3 Server error: {e}")
    
    def stop(self):
        """停止POP3服务器"""
        self.running = False
        if self.socket:
            self.socket.close()
            print('POP3 Server stopped')

if __name__ == '__main__':
    server = POP3Server()
    try:
        server.start()
    except KeyboardInterrupt:
        server.stop()

