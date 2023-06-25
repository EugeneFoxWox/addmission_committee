﻿using Microsoft.Win32;
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
using System.Diagnostics;
using System.Windows.Navigation;
using System.Reflection;
using selection_committee.Constants;

namespace selection_committee.Windows
{
    /// <summary>
    /// Логика взаимодействия для EntrantWindow.xaml
    /// </summary>
    public partial class EntrantWindow : Window
    {

        public string? TitleAb = "Абитуриент ";

        public Entrant Entrant { get; set; }

        private List<string> yesNo = EntrantComboBoxLists.YesNo;
        private List<string> genders = EntrantComboBoxLists.Genders;
        private List<string> citizenship = EntrantComboBoxLists.Citizenship;
        private List<string> variants9Or11Years = EntrantComboBoxLists.Variants9Or11Years;
        private List<string> speciality = EntrantComboBoxLists.Speciality;
        private List<string> subject = EntrantComboBoxLists.Subject;
        private List<string> district = EntrantComboBoxLists.District;
        private List<string> studyBasedList = EntrantComboBoxLists.StudyBasedList;

        public EntrantWindow(Entrant entrant)
        {
            InitializeComponent();
            Entrant = entrant;
            DataContext = Entrant;

            SetButtonBackground(hasDisabilityCertificateButton, Entrant.DisabilityCertificateScan);
            SetButtonBackground(hasOrphanageDocumentsButton, Entrant.OrphanageDocumentsScan);

            FillComboBox(enlistedComboBox, yesNo, Entrant.Enlisted);
            FillComboBox(genderComboBox, genders, Entrant.Gender);
            FillComboBox(studyBasedComboBox, studyBasedList, Entrant.StudyBased);
            FillComboBox(specialityComboBox, speciality, Entrant.Speciality);
            FillComboBox(kostroma_districtsComboBox, district, Entrant.District);
            FillComboBox(subjectComboBox, subject, Entrant.Subject);

            FillVariantsComboBox(finishedOnlyComboBox, finishedOnlyTextBox, variants9Or11Years, Entrant.Finished9Or11Grade);
            FillVariantsComboBox(citizenshipComboBox, citizenshipTextBox, citizenship, Entrant.Citizenship);

            FillYesNoComboBox(hasDisabilityCertificateComboBox, Entrant.HasDisabilityCertificate);
            SetButtonEnabled(hasDisabilityCertificateButton, hasDisabilityCertificateComboBox, 0);

            FillYesNoComboBox(hasOrphanageDocumentsComboBox, Entrant.HasOrphanageDocuments);
            SetButtonEnabled(hasOrphanageDocumentsButton, hasOrphanageDocumentsComboBox, 0);
        }

        private void SetButtonBackground(Button button, object value)
        {
            if (value != null)
                button.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            else
                button.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
        }

        private void FillComboBox(ComboBox comboBox, List<string> values, string selectedValue)
        {
            comboBox.ItemsSource = values;
            comboBox.SelectedIndex = values.IndexOf(selectedValue ?? "");
        }

        private void FillVariantsComboBox(ComboBox comboBox, TextBox textBox, List<string> values, string selectedValue)
        {
            textBox.IsEnabled = false;
            comboBox.ItemsSource = values;
            int index = values.IndexOf(selectedValue ?? "");
            comboBox.SelectedIndex = index == -1 ? 2 : index;

            if (comboBox.SelectedIndex == 2 && selectedValue != null)
            {
                textBox.Text = selectedValue;
                textBox.IsEnabled = true;
            }
        }

        private void FillYesNoComboBox(ComboBox comboBox, string selectedValue)
        {
            comboBox.ItemsSource = yesNo;
            comboBox.SelectedIndex = yesNo.IndexOf(selectedValue ?? "");
        }

        private void SetButtonEnabled(Button button, ComboBox comboBox, int index)
        {
            button.IsEnabled = false;
            if (comboBox.SelectedIndex == index)
            {
                button.IsEnabled = true;
            }
        }

        private void FillEntrantData()
        {
            try
            {
                Entrant.Gender = genderComboBox.SelectedItem?.ToString();
                Entrant.Citizenship = citizenshipComboBox.SelectedItem?.ToString();
                Entrant.Finished9Or11Grade = finishedOnlyComboBox.SelectedItem?.ToString();
                Entrant.Speciality = specialityComboBox.SelectedItem?.ToString();
                Entrant.District = kostroma_districtsComboBox.SelectedItem?.ToString();
                Entrant.Subject = subjectComboBox.SelectedItem?.ToString();
                Entrant.StudyBased = studyBasedComboBox.SelectedItem?.ToString();
                Entrant.Enlisted = enlistedComboBox.SelectedItem?.ToString();
                Entrant.HasDisabilityCertificate = hasDisabilityCertificateComboBox.SelectedItem?.ToString();
                Entrant.HasOrphanageDocuments = hasOrphanageDocumentsComboBox.SelectedItem?.ToString();
            }
            catch
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка"); return;
            }
            
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FillEntrantData();

                Entrant.YearOfEnlisted = enlistedComboBox.SelectedIndex == 0
                    ? DateTime.Now.Year.ToString()
                    : "";

                if (citizenshipComboBox.SelectedIndex == 2)
                {
                    Entrant.Citizenship = citizenshipTextBox.Text;
                }

                if (finishedOnlyComboBox.SelectedIndex == 2)
                {
                    Entrant.Finished9Or11Grade = finishedOnlyTextBox.Text;
                }
                if (!IsValid(Entrant))
                {
                    MessageBox.Show("Не все поля заполнены!", "Ошибка");
                    return;
                }

                DialogResult = true;
            }
            catch 
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка"); return;
            }
            
        }

        private byte[]? LoadFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Изображения (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg";
            if (dialog.ShowDialog() == true)
            {
                return File.ReadAllBytes(dialog.FileName);
            }
            else
            {
                return null;
            }
        }
        private void disabilityCertificateScan_Click(object sender, RoutedEventArgs e)
        {
            byte[]? disabilityCertificateScan = LoadFile();
            if (disabilityCertificateScan != null) 
                hasDisabilityCertificateButton.Background =
                    new SolidColorBrush(Color.FromRgb(0, 255, 0));
            Entrant.DisabilityCertificateScan = disabilityCertificateScan;
        }

        private void orphanageDocumentsScan_Click(object sender, RoutedEventArgs e)
        {
            byte[]? orphanageDocumentsScan = LoadFile();
            if (orphanageDocumentsScan_Click != null)
                hasOrphanageDocumentsButton.Background =
                    new SolidColorBrush(Color.FromRgb(0, 255, 0));
            Entrant.OrphanageDocumentsScan = orphanageDocumentsScan;
        }

        private void citizenshipComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (citizenshipComboBox.SelectedIndex == 2)
                citizenshipTextBox.IsEnabled = true;
            else
                citizenshipTextBox.IsEnabled = false;
        }

        private void finishedOnlyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (finishedOnlyComboBox.SelectedIndex == 2)
                finishedOnlyTextBox.IsEnabled = true;
            else
                finishedOnlyTextBox.IsEnabled = false;
        }
        
        private void HRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime birthDate = DatePicker.SelectedDate.Value;
            TimeSpan age = DateTime.Today - birthDate;
            int years = (int)(age.TotalDays / 365.25);
            ageLabel.Content = years.ToString();
            Entrant.Age = years;
        }

        private void hasDisabilityCertificateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hasDisabilityCertificateComboBox.SelectedIndex == 0)
            {
                hasDisabilityCertificateButton.IsEnabled = true;
                Entrant.HasDisabilityCertificate = "Да";
            }
            else
            {
                hasDisabilityCertificateButton.Background =
                    new SolidColorBrush(Color.FromRgb(128, 128, 128));
                hasDisabilityCertificateButton.IsEnabled = false;
                Entrant.DisabilityCertificateScan = null;
                Entrant.HasDisabilityCertificate = "Нет";
            }
                
        }

        private void hasOrphanageDocumentsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hasOrphanageDocumentsComboBox.SelectedIndex == 0)
            {
                hasOrphanageDocumentsButton.IsEnabled = true;
                Entrant.HasOrphanageDocuments = "Да";
            }
                
            else
            {
                hasOrphanageDocumentsButton.Background =
                    new SolidColorBrush(Color.FromRgb(128, 128, 128));
                hasOrphanageDocumentsButton.IsEnabled = false;
                Entrant.OrphanageDocumentsScan = null;
                Entrant.HasOrphanageDocuments = "Нет";
            }
                
        }

        private void subjectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (subjectComboBox.SelectedItem == null) return;

            if (subjectComboBox.SelectedItem.ToString().Contains("Костром")) 
                kostroma_districtsComboBox.IsEnabled = true;
            else kostroma_districtsComboBox.IsEnabled = false;
        }

        private bool IsValid(Entrant entrant)
        {
            return !string.IsNullOrEmpty(entrant.Name) &&
                   !string.IsNullOrEmpty(entrant.Surname) &&
                   !string.IsNullOrEmpty(entrant.Patronymic) &&
                   !string.IsNullOrEmpty(entrant.Citizenship) &&
                   !string.IsNullOrEmpty(entrant.SNILS) &&
                   !string.IsNullOrEmpty(entrant.Speciality) &&
                   !string.IsNullOrEmpty(entrant.Subject) &&
                   !string.IsNullOrEmpty(entrant.CertificateNumber) &&
                   !string.IsNullOrEmpty(entrant.Finished9Or11Grade) &&
                   !string.IsNullOrEmpty(entrant.Enlisted) &&
                   (entrant.Gender == "Мужской" || entrant.Gender == "Женский");  // проверяем корректность поля Gender
        }

        private void enlistedComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (enlistedComboBox.SelectedIndex == 0)
                yearOfEnlistedLabel.Content = DateTime.Now.Year.ToString();
            else
                yearOfEnlistedLabel.Content = "";
            
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo("http://kptc.ru/prikaz/Pravila_priema_2023.pdf")
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}
