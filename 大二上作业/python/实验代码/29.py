class Student:
    def __init__(self, student_id, name, gender, class_name, grades=None):
        """
        初始化学生对象
        :param student_id: 学号
        :param name: 姓名
        :param gender: 性别
        :param class_name: 班级
        :param grades: 成绩字典，格式为 {课程名: 分数}
        """
        self.student_id = student_id
        self.name = name
        self.gender = gender
        self.class_name = class_name
        self.grades = grades if grades else {}

    def add_grade(self, course, score):
        """
        添加或更新课程成绩
        :param course: 课程名
        :param score: 分数
        """
        self.grades[course] = score

    def update_grade(self, course, score):
        """
        更新课程成绩
        :param course: 课程名
        :param score: 新分数
        """
        if course in self.grades:
            self.grades[course] = score
        else:
            print(f"课程 {course} 不存在，添加失败。")

    def delete_grade(self, course):
        """
        删除课程成绩
        :param course: 课程名
        """
        if course in self.grades:
            del self.grades[course]
        else:
            print(f"课程 {course} 不存在，删除失败。")

    def get_average(self):
        """
        计算学生的平均成绩
        :return: 平均成绩，如果成绩字典为空则返回 0
        """
        if self.grades:
            return sum(self.grades.values()) / len(self.grades)
        else:
            return 0

    def __str__(self):
        """
        返回学生的字符串表示，包含学号、姓名、性别、班级、成绩字典和平均成绩
        :return: 学生信息的字符串
        """
        return f"{self.student_id} {self.name} {self.gender} {self.class_name} {self.grades} 平均分: {self.get_average():.2f}"


def read_students_from_file(file_path):
    """
    从文件中读取学生信息并创建学生对象列表
    :param file_path: 存储学生信息的文件路径
    :return: 学生对象列表
    """
    students = []
    with open(file_path, 'r', encoding='utf-8') as file:
        for line in file:
            data = line.strip().split()  # 去除行末的换行符并按空格分割
            student_id = data[0]
            name = data[1]
            class_name = data[2]
            gender = data[3]
            grades = {entry.split(':')[0]: int(entry.split(':')[1]) for entry in data[4:]}  # 解析课程成绩
            students.append(Student(student_id, name, gender, class_name, grades))
    return students


def find_student_by_id(students, student_id):
    """
    根据学号查找学生对象
    :param students: 学生对象列表
    :param student_id: 学号
    :return: 学生对象，如果未找到则返回 None
    """
    for student in students:
        if student.student_id == student_id:
            return student
    return None


def find_student_by_name(students, name):
    """
    根据姓名查找学生对象
    :param students: 学生对象列表
    :param name: 姓名
    :return: 学生对象，如果未找到则返回 None
    """
    for student in students:
        if student.name == name:
            return student
    return None


def update_student_grades(students, student_id, name, course, score, action):
    """
    更新学生的课程成绩
    :param students: 学生对象列表
    :param student_id: 学号
    :param name: 姓名
    :param course: 课程名
    :param score: 分数
    :param action: 操作类型，可以是 'add', 'update', 'delete'
    """
    student = find_student_by_id(students, student_id) or find_student_by_name(students, name)
    if student:
        if action == 'add':
            student.add_grade(course, score)
        elif action == 'update':
            student.update_grade(course, score)
        elif action == 'delete':
            student.delete_grade(course)
        else:
            print(f"未知操作 {action}。")
    else:
        print(f"未找到学号为 {student_id} 或姓名为 {name} 的学生。")


def calculate_average_and_rank(students):
    """
    计算每个学生的平均成绩并按平均成绩排序
    :param students: 学生对象列表
    :return: 按平均成绩排序的学生对象列表
    """
    students_with_avg = [(student, student.get_average()) for student in students]  # 计算每个学生的平均成绩
    students_with_avg.sort(key=lambda x: x[1], reverse=True)  # 按平均成绩降序排序
    for rank, (student, avg) in enumerate(students_with_avg, start=1):
        student.rank = rank  # 为每个学生添加排名属性
        student.avg = avg  # 为每个学生添加平均成绩属性
    return students_with_avg


def print_students_by_rank(students):
    """
    按照平均成绩排名输出学生信息
    :param students: 学生对象列表
    """
    students_with_avg = calculate_average_and_rank(students)  # 计算平均成绩和排名
    for student, avg in students_with_avg:
        print(f"{student.student_id} {student.name} {student.gender} {student.class_name} {student.grades} 平均分: {avg:.2f} 排名: {student.rank}")


if __name__ == "__main__":
    # 读取学生信息文件
    file_path = 'students29.txt'
    students = read_students_from_file(file_path)

    # 更新成绩示例
    update_student_grades(students, "202001", "李四", "高数", 90, "update")
    update_student_grades(students, "202001", "李四", "物理", 85, "add")
    update_student_grades(students, "202001", "李四", "英语", None, "delete")

    # 按照平均分排名并输出
    print_students_by_rank(students)