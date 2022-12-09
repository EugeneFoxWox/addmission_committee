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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            db.Employees.Load();
            DataContext = db.Employees.Local.ToObservableCollection();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow EmployeeWindow = new EmployeeWindow(new Employee());
            if (EmployeeWindow.ShowDialog() == true)
            {
                Employee Employee = EmployeeWindow.Employee;
                db.Employees.Add(Employee);
                db.SaveChanges();
            }
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
            Loaded += MainWindow_Loaded;
            db.SaveChanges();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Employee? employee = employeesList.SelectedItem as Employee;
            if (employee is null) return;
            db.Employees.Remove(employee);
            db.SaveChanges();
        }

        void OnComboboxTextChanged(object sender, RoutedEventArgs e)
        {
            //_CBFind.IsDropDownOpen = true;
            // убрать selection, если dropdown только открылся
            //var tb = (TextBox)e.OriginalSource;
            //tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            //CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(_CBFind.ItemsSource);
            //cv.Filter = s =>
            //    ((string)s).IndexOf(_CBFind.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    DataContext = db.Employees.OrderBy(empl => empl.Surname).ToList();
                    break;
                case 1:
                    DataContext = db.Employees.OrderByDescending(empl => empl.Surname).ToList();
                    break;
                case 2:
                    DataContext = db.Employees.OrderBy(empl => empl.Name).ToList();
                    break;
                case 3:
                    DataContext = db.Employees.OrderByDescending(empl => empl.Name).ToList();
                    break;
            }
        }

        private void Find_Loaded(object sender, RoutedEventArgs e)
        {
            //var employees = db.Employees.Local.ToObservableCollection();
            //employees.Select(empl => String.Format("{0} {1} {2}", empl.Surname, empl.Name, empl.Patronymic));
            //_CBFind.DataContext = employees;

        }

        async private void Json_Click(object sender, RoutedEventArgs e)
        {
            db.Employees.Load();
            ObservableCollection<Employee> _data = (db.Employees.Local.ToObservableCollection());
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            };
            try
            {
                string json = JsonSerializer.Serialize(_data, options);
                await using FileStream createStream = File.Create(@"C:\path.json");
                await JsonSerializer.SerializeAsync(createStream, _data);
                MessageBox.Show(@"Создался файл: C:\\path.json");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
