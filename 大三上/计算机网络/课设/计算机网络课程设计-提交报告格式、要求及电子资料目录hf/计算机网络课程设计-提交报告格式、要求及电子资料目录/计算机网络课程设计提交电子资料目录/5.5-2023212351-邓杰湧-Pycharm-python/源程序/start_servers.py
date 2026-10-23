import threading
from smtp_server import SMTPServer
from pop3_server import POP3Server

def start_smtp_server():
    """启动SMTP服务器"""
    server = SMTPServer(host='127.0.0.1', port=2525)
    server.start()

def start_pop3_server():
    """启动POP3服务器"""
    server = POP3Server(host='127.0.0.1', port=1100)
    server.start()

if __name__ == '__main__':
    print("=" * 50)
    print("邮件服务器启动中...")
    print("SMTP服务器: 127.0.0.1:2525")
    print("POP3服务器: 127.0.0.1:1100")
    print("=" * 50)
    print("按 Ctrl+C 停止服务器")
    print()
    
    # 在新线程中启动SMTP服务器
    smtp_thread = threading.Thread(target=start_smtp_server, daemon=True)
    smtp_thread.start()
    
    # 在主线程中启动POP3服务器
    try:
        start_pop3_server()
    except KeyboardInterrupt:
        print("\n正在关闭服务器...")

