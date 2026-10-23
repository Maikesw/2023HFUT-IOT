//#include "Student.h"
//#include<sstream>
//Student::Student()
//{
//}
//
//Student::Student(uint32 number, const std::string& name, const std::string& grade, uint32 math, uint32 chinese, uint32 english)
//	:number(number),name(name),grade(grade),math(math),chinese(chinese),english(english)
//{
//}
//
//std::string Student::formatInfo()
//{
//	std::stringstream ss;
//	ss<< number <<"\t"<<  name << "\t" << grade << "\t" << math << "\t" << chinese << "\t" << english<<std::endl;
//	return ss.str();
//}
//
//Student Student::fromString(const std::string& str)
//{
//	Student stu;
//	std::stringstream ss(str);
//	ss >> stu.number >> stu.name >> stu.grade >> stu.chinese >> stu.math >> stu.english;
//	return stu;
//}
