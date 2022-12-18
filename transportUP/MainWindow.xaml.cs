using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
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
using transportUP.DB;
using transportUP.Pages;
using System.Windows.Markup;
using System.IO;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Text.Encodings.Web;
using System.Data;
using OfficeOpenXml;
using System.Globalization;
using System.Text.Unicode;

namespace transportUP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        public MainWindow()
        {   
            InitializeComponent();
            Loaded += MainWindow_Loaded;    
            Title = "Транспорт";
            employeesList.SelectionChanged += employeesList_SelectionChanged;
        }

        void employeesList_SelectionChanged(object sender, EventArgs e)
        {
            Employee? employee = employeesList.SelectedItem as Employee;
            if (employee is null) return;
            
        }

        private void MakeComboboxItems (ObservableCollection<Employee> employees)
        {
            findEmployeeCB.Items.Clear();
            foreach (Employee employee in employees)
            {
                if (employee is null) continue;
                findEmployeeCB.Items.Add(employee.Surname + " " + employee.Name + " " + employee.Patronymic);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            db.Employees.Load();
            ObservableCollection<Employee> employees = db.Employees.Local.ToObservableCollection();
            DataContext = employees;
            MakeComboboxItems(employees);
            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow EmployeeWindow = new EmployeeWindow(new Employee());
            if (EmployeeWindow.ShowDialog() == true)
            {
                Employee Employee = EmployeeWindow.Employee;
                db.Employees.Add(Employee);   
            }
            db.SaveChanges();
            ObservableCollection<Employee> employees = db.Employees.Local.ToObservableCollection();
            DataContext = employees;
            MakeComboboxItems(employees);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Employee? employee = employeesList.SelectedItem as Employee;
            if (employee is null) return;

            EmployeeWindow EmployeeWindow = new EmployeeWindow(new Employee
            { 
                Name = employee.Name,
                Identificator = employee.Identificator,
                Surname = employee.Surname,
                Birthday = employee.Birthday,
                Patronymic = employee.Patronymic,
                PhoneNumber = employee.PhoneNumber,
                Departament = employee.Departament,
                Id = employee.Id,
            });

            if (EmployeeWindow.ShowDialog() == true)
            {
                employee = db.Employees.Find(EmployeeWindow.Employee.Id);
                
                if (employee != null )
                {
                    employee.Surname = EmployeeWindow.Employee.Surname;
                    employee.Name = EmployeeWindow.Employee.Name;
                    employee.Patronymic = EmployeeWindow.Employee.Patronymic;
                    employee.Birthday = EmployeeWindow.Employee.Birthday;
                    employee.PhoneNumber = EmployeeWindow.Employee.PhoneNumber;
                    employee.Departament = EmployeeWindow.Employee.Departament;
                    employee.Identificator = EmployeeWindow.Employee.Identificator;
                }
            }
            db.SaveChanges();
            ObservableCollection<Employee> employees = db.Employees.Local.ToObservableCollection();
            MakeComboboxItems(employees);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Employee? employee = employeesList.SelectedItem as Employee;
            if (employee is null) return;
            db.Employees.Remove(employee);
            db.SaveChanges();
            ObservableCollection<Employee> employees = db.Employees.Local.ToObservableCollection();
            DataContext = employees;
            MakeComboboxItems(employees);
        }

        void OnComboboxTextChanged(object sender, RoutedEventArgs e)
        {
            findEmployeeCB.IsDropDownOpen = true;
            //убрать selection, если dropdown только открылся
            var tb = (TextBox)e.OriginalSource;
            tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(findEmployeeCB.Items);
            cv.Filter = s =>
                ((string)s).IndexOf(findEmployeeCB.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ObservableCollection<Employee> employees = db.Employees.Local.ToObservableCollection();
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    DataContext = new ObservableCollection<Employee>(employees.OrderBy(i => i.Surname));
                    break;
                case 1:
                    DataContext = new ObservableCollection<Employee>(employees.OrderByDescending(i => i.Surname));
                    break;
                case 2:
                    DataContext = new ObservableCollection<Employee>(employees.OrderBy(i => i.Name));
                    break;
                case 3:
                    DataContext = new ObservableCollection<Employee>(employees.OrderByDescending(i => i.Name));
                    break;
            }
        }  

        async private void Json_Click(object sender, RoutedEventArgs e)
        {
            db.Employees.ToList();
            ObservableCollection<Employee> _data = (db.Employees.Local.ToObservableCollection());
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            try
            {
                string json = JsonSerializer.Serialize(_data, options);
                await using FileStream createStream = File.Create(@".\..\..\..\Reports\JSON\employees.json");
                await JsonSerializer.SerializeAsync(createStream, _data);
                MessageBox.Show(@"Создался файл: employees.json");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            string valueFullName = findEmployeeCB.Text;
            List<Employee> employees = new List<Employee>();

            if (valueFullName.Trim().Equals(""))
            {
                DataContext = db.Employees.Local.ToObservableCollection();
                return;
            }

            foreach (Employee emp in db.Employees)
            {
                string fullName = string.Format("{0} {1} {2}", emp.Surname, emp.Name, emp.Patronymic);

                if (fullName.Equals(valueFullName))
                {
                    employees.Add(emp);
                }
            }

            if (employees.Count == 0)
            {
                MessageBox.Show(string.Format("Не найден ни один работник с ФИО {0} не найден!", valueFullName), "Ошибка!");
                return;
            }

            DataContext = employees;


            
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> employees = db.Employees.Local.ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string fileString = "employees " + DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss") + ".xlsx";
            FileInfo newFile = new FileInfo(@".\..\..\..\Reports\Excel\" + fileString);
            using (ExcelPackage pck = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Accounts");
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "Name";
                    worksheet.Cells[1, 3].Value = "Surname";
                    worksheet.Cells[1, 4].Value = "Patronymic";
                    worksheet.Cells[1, 5].Value = "Birthday";
                    worksheet.Cells[1, 6].Value = "PhoneNumber";
                    worksheet.Cells[1, 7].Value = "Department";
                    worksheet.Cells[1, 8].Value = "Identificator";
                    for (int i = 0; i < employees.Count(); i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = employees[i].Id.ToString();
                        worksheet.Cells[i + 2, 2].Value = employees[i].Name;
                        worksheet.Cells[i + 2, 3].Value = employees[i].Surname;
                        worksheet.Cells[i + 2, 4].Value = employees[i].Patronymic;
                        worksheet.Cells[i + 2, 5].Value = employees[i].Birthday;
                        worksheet.Cells[i + 2, 6].Value = employees[i].PhoneNumber;
                        worksheet.Cells[i + 2, 7].Value = employees[i].Departament;
                        worksheet.Cells[i + 2, 8].Value = employees[i].Identificator;
                    }
                    worksheet.Column(1).AutoFit(5);
                    worksheet.Column(2).AutoFit(5);
                    worksheet.Column(3).AutoFit(5);
                    worksheet.Column(4).AutoFit(5);
                    worksheet.Column(5).AutoFit(5);
                    worksheet.Column(6).AutoFit(5);
                    worksheet.Column(7).AutoFit(5);
                    pck.Save();
                }
                MessageBox.Show("Создался файл " + fileString);
            
        }
    }
}
