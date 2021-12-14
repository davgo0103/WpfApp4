using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp4
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Teacher> teachers = new List<Teacher>();
        List<Student> students = new List<Student>();
        public MainWindow()
        {
            InitializeComponent();

            Teacher teacher1 = new Teacher() { TeacherName = "蔡士煒" };
            teacher1.Courses.Add(new Course(teacher1) { CourseName = "視窗程式設計", Type = "選修", Point = 3, OpeningClass = "資工二甲" });
            teacher1.Courses.Add(new Course(teacher1) { CourseName = "視窗程式設計", Type = "選修", Point = 3, OpeningClass = "資工二乙" });
            teacher1.Courses.Add(new Course(teacher1) { CourseName = "視窗程式設計", Type = "必修", Point = 3, OpeningClass = "五專三甲" });
            teachers.Add(teacher1);

            Teacher teacher2 = new Teacher() { TeacherName = "邱宇軒" };
            teacher2.Courses.Add(new Course(teacher2) { CourseName = "工程數學", Type = "必修", Point = 3, OpeningClass = "資工二甲" });
            teacher2.Courses.Add(new Course(teacher2) { CourseName = "微積分", Type = "必修", Point = 3, OpeningClass = "資工二乙" });
            teacher2.Courses.Add(new Course(teacher2) { CourseName = "線性代數", Type = "必修", Point = 3, OpeningClass = "五專三甲" });
            teacher2.Courses.Add(new Course(teacher2) { CourseName = "奇蹟MU", Type = "必修", Point = 30, OpeningClass = "全年齡" });
            teachers.Add(teacher2);

            trvTeacher.ItemsSource = teachers;


            Student student1 = new Student() { StudentID = "A12345678", StudentName = "邱宇軒"};
            students.Add(student1);
            Student student2 = new Student() { StudentID = "A123124582", StudentName = "哈哈哈" };
            students.Add(student2);

            cmbStudents.ItemsSource = students;
        }
    }

    public class Teacher
    {
        public string TeacherName { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public Teacher()
        {
            this.Courses = new ObservableCollection<Course>();
        }
        public override string ToString()
        {
            return $"教師姓名:{TeacherName}";
        }
    }
    public class Course
    {
        public string CourseName { get; set; }
        public string Type { get; set; }
        public int Point { get; set; } 
        public string OpeningClass { get; set; }
        public Teacher Tutor { get; set; }


        public Course(Teacher tutor)
        {
            Tutor = tutor;

        }
        public override string ToString()
        {
            return $"{CourseName} {Type} {Point}學分 開課班級:{OpeningClass}";
        }
    }

    public class Student
    {
        public string StudentID { get; set;}
        public string StudentName { get; set;}
        public override string ToString()
        {
            return $"{StudentID} {StudentName}";
        }
    }
    public class Record
    {
        public Student SelectedStudent { get; set;}
        public Course SelectedCourse { get; set;}
    }

}
