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
                    employee.PhoneNumber = EmployeeWindow.Employee.PhoneNumber;
                    employee.Departament = EmployeeWindow.Employee.Departament;
                    employee.Identificator = EmployeeWindow.Employee.Identificator;
                }
            }
            db.SaveChanges();
            ObservableCollection<Employee> employees = db.Employees.Local.ToObservableCollection();
            DataContext = employees;
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
            db.SaveChanges();
        }

        async private void Json_Click(object sender, RoutedEventArgs e)
        {
            db.Employees.ToList();
            ObservableCollection<Employee> _data = (db.Employees.Local.ToObservableCollection());
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            };
            try
            {
                string json = JsonSerializer.Serialize(_data, options);
                await using FileStream createStream = File.Create(@".\Reports\employees.json");
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
            
        }

    }
}
