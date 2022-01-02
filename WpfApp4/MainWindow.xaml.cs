using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;


namespace WpfApp4
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Teacher> teachers = new List<Teacher>();
        List<Student> students = new List<Student>();
        List<Record> records = new List<Record>();
        List<Course> courses = new List<Course>();

        Student selectedStudent = null;
        Course selectedCourse = null;
        Teacher selectedTeacher = null;
        Record selectedRecord = null;
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
            //產生所有課程的LIST
            foreach (Teacher teacher in teachers)
            {
                foreach (Course course in teacher.Courses)
                {
                    courses.Add(course);
                    lbCourse.Items.Add(course);
                }
            }




            ReadStudents();
            cmbStudents.ItemsSource = students;
        }

        private void ReadStudents()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON|*.json|All Files|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open);

                using (JsonDocument document = JsonDocument.Parse(fs))
                {
                    JsonElement root = document.RootElement;
                    //JsonElement studentsElement = root.GetProperty("StudentID");
                    foreach (JsonElement student in root.EnumerateArray())
                    {
                        if (student.TryGetProperty("StudentID", out JsonElement IDElement) && student.TryGetProperty("StudentName", out JsonElement NameElement))
                        {
                            Student studenttmp = new Student() { StudentID = NameElement.GetString(), StudentName = IDElement.GetString() };
                            students.Add(studenttmp);
                        }
                        else
                        {
                            MessageBox.Show("讀取學生資料失敗");
                            Environment.Exit(0);
                        }
                    }
                }
            }

        }

        private void cmbStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStudent = (Student)cmbStudents.SelectedItem;
            statusLabel.Content = "選取學生:" + selectedStudent.ToString();
        }

        private void trvTeacher_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (trvTeacher.SelectedItem is Course)
            {
                selectedCourse = trvTeacher.SelectedItem as Course;
                selectedTeacher = selectedCourse.Tutor;
                statusLabel.Content = selectedTeacher.ToString() + "/" + selectedCourse.ToString();
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCourse != null && cmbStudents.SelectedItem != null)
            {
                Record currentRecord = new Record()
                {
                    SelectedStudent = selectedStudent,
                    SelectedCourse = selectedCourse,
                    TeacherName = selectedCourse.Tutor.TeacherName,
                };
                foreach (Record r in records)
                {
                    if (r.Equals(currentRecord))
                    {
                        MessageBox.Show($"{selectedStudent.StudentName} 已經選過 {selectedCourse.CourseName} 了，請重新選擇為選擇過的課程");
                        return;
                    }
                }
                records.Add(currentRecord);
                lvRegister.ItemsSource = records;
                lvRegister.Items.Refresh();
            }
            else
                MessageBox.Show("請選擇學生或課程", "資料不足");

        }

        private void lvRegister_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedRecord = (Record)lvRegister.SelectedItem;
        }

        private void withdrawButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRecord != null)
            {
                records.Remove(selectedRecord);
                lvRegister.ItemsSource = records;
                lvRegister.Items.Refresh();
            }
            else MessageBox.Show("請選擇要退選的紀錄");
        }

        private void lbCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCourse = (Course)lbCourse.SelectedItem;
            selectedTeacher = selectedCourse.Tutor;
            statusLabel.Content = selectedCourse.ToString() + " /" + selectedTeacher.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveJson();
        }

        private void SaveJson()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON|*.json|All Files|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                serializer.Formatting = Formatting.Indented;
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw); 
                {
                    serializer.Serialize(writer, records);
                }
                sw.Close();

            }


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
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public override string ToString()
        {
            return $"{StudentID} {StudentName}";
        }
    }
    public class Record
    {
        public Student SelectedStudent { get; set; }
        public Course SelectedCourse { get; set; }
        public string TeacherName { get; set; }
        public bool Equals(Record r)
        {
            if (this.SelectedStudent.StudentID == r.SelectedStudent.StudentID && this.SelectedCourse.CourseName == r.SelectedCourse.CourseName)
                return true;
            else return false;
        }
    }

}
