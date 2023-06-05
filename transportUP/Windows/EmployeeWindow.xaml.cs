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

        public Entrant Entrant { get; private set; }
        public EmployeeWindow(Entrant employee)
        {
            InitializeComponent();
            Entrant = employee;
            DataContext = Entrant;
            Title = "Работник";
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Entrant.Surname) 
                || String.IsNullOrEmpty(Entrant.Name) 
                || String.IsNullOrEmpty(Entrant.Patronymic) 
                || String.IsNullOrEmpty(Entrant.PhoneNumber)
                || String.IsNullOrEmpty(Entrant.Departament)
                || String.IsNullOrEmpty(Entrant.Birthday))
            { 
                MessageBox.Show("Не все поля заполнены!", "Ошибка"); return; 
            }
            DialogResult = true;
        }
    }
}
