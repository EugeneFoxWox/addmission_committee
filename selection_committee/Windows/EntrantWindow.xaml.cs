using Microsoft.Win32;
using selection_committee.DB.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace selection_committee.Windows
{
    /// <summary>
    /// Логика взаимодействия для EntrantWindow.xaml
    /// </summary>
    public partial class EntrantWindow : Window
    {
        public Entrant Entrant { get; set; }
        public EntrantWindow(Entrant entrant)
        {
            InitializeComponent();
            Entrant = entrant;
            DataContext = Entrant;
            if (Entrant.Gender == "Мужской")
            {
                genderComboBox.SelectedIndex = 0;
            }
            else if (Entrant.Gender == "Женский")
            {
                genderComboBox.SelectedIndex = 1;
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Entrant.Gender = genderComboBox.Text;

            if (String.IsNullOrEmpty(Entrant.Surname) 
                || String.IsNullOrEmpty(Entrant.Name) 
                || String.IsNullOrEmpty(Entrant.Patronymic)
                || String.IsNullOrEmpty(Entrant.Gender)) 
            { 
                MessageBox.Show("Не все поля заполнены!", "Ошибка"); return; 
            }
            

            DialogResult = true;
        }

        private byte[]? LoadFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                return File.ReadAllBytes(dialog.FileName);
            }
            else
            {
                return null;
            }
        }
        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            byte[]? file = LoadFile();
            MessageBox.Show(file?.ToString(), "Файл не загружен");
        }
    }
}
