def read_student_data(filename):
    """
    读取学生数据文件，返回一个包含学号、数学成绩和英语成绩的列表
    :param filename: 学生数据文件名
    :return: 学生数据列表
    """
    try:
        student_data = []
        with open(filename, 'r') as file:
            for line in file:
                data = line.strip().split()
                if len(data) == 3:
                    student_id, math_score, english_score = data
                    student_data.append((student_id, float(math_score), float(english_score)))
                else:
                    print(f"警告：忽略无效行 - {line.strip()}")
        return student_data
    except FileNotFoundError:
        print(f"文件 {filename} 未找到")
        return []
    except Exception as e:
        print(f"读取文件时发生错误: {e}")
        return []

def find_max_scores(student_data):
    """
    找出数学成绩和英语成绩的最高分
    :param student_data: 学生数据列表
    :return: 数学最高分和英语最高分
    """
    if not student_data:
        return None, None

    max_math_score = max(student_data, key=lambda x: x[1])[1]
    max_english_score = max(student_data, key=lambda x: x[2])[2]
    return max_math_score, max_english_score

def find_students_with_average_above_90(student_data):
    """
    找出两门课的平均成绩≥90分的学生
    :param student_data: 学生数据列表
    :return: 平均成绩≥90分的学生列表
    """
    high_achievers = []
    for student in student_data:
        student_id, math_score, english_score = student
        average_score = (math_score + english_score) / 2
        if average_score >= 90:
            high_achievers.append((student_id, math_score, english_score, average_score))
    return high_achievers

def main():
    # 设置文件名
    filename = "student37.txt"

    # 读取学生数据
    student_data = read_student_data(filename)

    # 找出数学和英语的最高分
    max_math_score, max_english_score = find_max_scores(student_data)
    if max_math_score is not None and max_english_score is not None:
        print(f"数学最高分: {max_math_score:.1f}")
        print(f"英语最高分: {max_english_score:.1f}")

    # 找出平均成绩≥90分的学生
    high_achievers = find_students_with_average_above_90(student_data)
    if high_achievers:
        print("平均成绩≥90分的学生:")
        for student in high_achievers:
            student_id, math_score, english_score, average_score = student
            print(f"学号: {student_id}, 数学成绩: {math_score:.1f}, 英语成绩: {english_score:.1f}, 平均成绩: {average_score:.1f}")
    else:
        print("没有平均成绩≥90分的学生")

if __name__ == "__main__":
    main()