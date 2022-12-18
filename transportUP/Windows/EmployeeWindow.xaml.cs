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
using System.Windows.Shapes;
using transportUP.DB;

namespace transportUP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PersonnelWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {

        public Employee Employee { get; private set; }
        public EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            Employee = employee;
            DataContext = Employee;
            Title = "Работник";
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Employee.Surname) 
                || String.IsNullOrEmpty(Employee.Name) 
                || String.IsNullOrEmpty(Employee.Patronymic) 
                || String.IsNullOrEmpty(Employee.PhoneNumber)
                || String.IsNullOrEmpty(Employee.Departament)
                || String.IsNullOrEmpty(Employee.Birthday))
            { 
                MessageBox.Show("Не все поля заполнены!", "Ошибка"); return; 
            }
            DialogResult = true;
        }
    }
}
