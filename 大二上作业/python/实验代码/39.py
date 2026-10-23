import re

def is_valid_password(password):
    """
    检查密码是否符合标准
    :param password: 密码字符串
    :return: 密码是否有效
    """
    # 密码长度检查
    if not (6 <= len(password) <= 12):
        return False

    # 至少包含1个小写字母
    if not re.search(r'[a-z]', password):
        return False

    # 至少包含1个大写字母
    if not re.search(r'[A-Z]', password):
        return False

    # 至少包含1个数字
    if not re.search(r'[0-9]', password):
        return False

    # 至少包含1个特殊字符（$#@）
    if not re.search(r'[$#@]', password):
        return False

    return True

def check_passwords(input_string):
    """
    检查输入字符串中的所有密码，并返回符合标准的密码列表
    :param input_string: 逗号分隔的密码字符串
    :return: 符合标准的密码列表
    """
    valid_passwords = []
    passwords = input_string.split(',')

    for password in passwords:
        if is_valid_password(password):
            valid_passwords.append(password)

    return valid_passwords

if __name__ == "__main__":
    # 用户输入一系列逗号分隔的密码
    input_string = input("请输入一系列逗号分隔的密码: ")

    # 检查密码
    valid_passwords = check_passwords(input_string)

    # 输出符合条件的密码
    if valid_passwords:
        print("符合条件的密码: " + ', '.join(valid_passwords))
    else:
        print("没有符合条件的密码")