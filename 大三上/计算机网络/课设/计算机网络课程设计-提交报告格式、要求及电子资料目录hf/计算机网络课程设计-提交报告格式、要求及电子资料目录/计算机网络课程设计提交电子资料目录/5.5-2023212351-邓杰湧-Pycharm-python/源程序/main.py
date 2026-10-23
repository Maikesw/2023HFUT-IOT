import sys
import os
from PySide6.QtWidgets import QApplication
from email_client_gui import EmailClientGUI

# 抑制libpng警告
os.environ['QT_LOGGING_RULES'] = '*.debug=false;qt.png.warning=false'

if __name__ == '__main__':
    app = QApplication(sys.argv)
    window = EmailClientGUI()
    window.show()
    sys.exit(app.exec())

