using Microsoft.EntityFrameworkCore;
using System;
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
    }
}
