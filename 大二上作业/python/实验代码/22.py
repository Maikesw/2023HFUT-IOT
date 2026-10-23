import re
text=input("请输入一段英文")
words=re.findall(r"\b[a-zA-Z]{3}\b",text)
print(f"所有长度为3个字母的单词{words}")