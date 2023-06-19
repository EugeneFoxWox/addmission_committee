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

namespace selection_committee.Windows
{
    /// <summary>
    /// Логика взаимодействия для EntrantWindow.xaml
    /// </summary>
    public partial class EntrantWindow : Window
    {

        public string? TitleAb = "Абитуриент ";

        public Entrant Entrant { get; set; }

        private List<string> yesNo = new List<string>() { "Да", "Нет"};
        private List<string> genders = new List<string>() { "Мужской", "Женский" };
        private List<string> citizenship = new List<string>() { "Россия", "Страны СНГ", "Другое" };
        private List<string> variants9Or11Years = new List<string>()
        {
            "Только 9 классов",
            "Только 11 классов",
            "Другое"
        };
        private List<string> speciality = new List<string>()
        {
            "Архитектура",
            "Гидрогеология и инженерная геология",
            "Информационные системы и программирование (на базе 9 классов)",
            "Строительство и эксплуатация зданий и сооружений (на базе 9 классов)",
            "Разработка электронных устройств и систем",
            "Информационные системы и программирование (на базе 11 классов)",
            "Строительство и эксплуатация зданий и сооружений (на базе 11 классов)"
        };
        List<string> subject = new List<string>
        {
            "Республика Адыгея", "Республика Башкортостан", "Республика Бурятия",
            "Республика Алтай", "Республика Дагестан", "Республика Ингушетия",
            "Кабардино-Балкарская Республика", "Республика Калмыкия",
            "Карачаево-Черкесская Республика", "Республика Карелия",
            "Республика Коми", "Республика Крым", "Республика Марий Эл",
            "Республика Мордовия",
            "Республика Саха (Якутия)",
            "Республика Северная Осетия - Алания",
            "Республика Татарстан", "Республика Тыва",
            "Удмуртская Республика", "Республика Хакасия",
            "Чеченская Республика", "Чувашская Республика",
            "Алтайский край", "Краснодарский край",
            "Красноярский край", "Приморский край",
            "Ставропольский край", "Хабаровский край",
            "Амурская область", "Архангельская область",
            "Астраханская область","Белгородская область",
            "Брянская область","Владимирская область","Волгоградская область",
            "Вологодская область","Воронежская область",
            "Ивановская область","Иркутская область",
            "Калининградская область","Калужская область",
            "Кемеровская область","Кировская область",
            "Костромская область","Курганская область",
            "Курская область","Ленинградская область",
            "Липецкая область","Магаданская область",
            "Московская область","Мурманская область",
            "Нижегородская область","Новгородская область",
            "Новосибирская область","Омская область",
            "Оренбургская область","Орловская область",
            "Пензенская область","Пермский край",
            "Псковская область","Ростовская область",
            "Рязанская область","Самарская область",
            "Саратовская область","Сахалинская область",
            "Свердловская область","Смоленская область",
            "Тамбовская область","Тверская область","Томская область",
            "Тульская область",
            "Тюменская область","Ульяновская область","Челябинская область",
            "Ярославская область","г. Москва","г. Санкт-Петербург","Еврейская автономная область",
            "Ненецкий автономный округ","Ханты-Мансийский автономный округ - Югра",
            "Чукотский автономный округ","Ямало-Ненецкий автономный округ"
        };
        private List<string> district = new List<string>()
        {
            "Антроповский район", "Буйский район", "Вохомский район",
            "Галичский район", "Кадыйский район",
            "Костромской район","Красносельский район", "Макарьевский район", 
            "Нейский округ","Нерехтский район","Октябрьский район", "Островский район",
            "Павинский район", "Поназыревский район",
            "Пыщугский район", "Солигаличский район", "Судиславский район", "Сусанинский район",
            "Чухломский район", "Шарьинский район","г. Буй","г. Волгореченск",
            "г. Галич","г. Кострома", "г. Мантурово",  "город Шарья", "Кологривский округ",
            "Межевской округ", "Парфеньевский округ"
        };
        private List<string> studyBasedList = new List<string>() { 
            "Бюджет", 
            "На основе договора об оказании плантых услуг" 
        };

        public EntrantWindow(Entrant entrant)
        {
            InitializeComponent();
            Entrant = entrant;
            DataContext = Entrant;

            FillGenderComboBox();
            FillStudyBasedComboBox();
            FillSpecialityComboBox();
            FillKostromaDistrictsComboBox();
            FillSubjectComboBox();
            FillFinishedOnlyComboBox();
            FillCitizenshipComboBox();
            FillHasDisabilityCertificateComboBox();
            SetHasDisabilityCertificateButtonEnabled();
            FillHasOrphanageDocumentsComboBox();
            SetHasOrphanageDocumentsButtonEnabled();
        }

        private void FillGenderComboBox()
        {
            genderComboBox.ItemsSource = genders;
            genderComboBox.SelectedIndex = genders.IndexOf(Entrant.Gender ?? "");
        }

        private void FillStudyBasedComboBox()
        {
            studyBasedComboBox.ItemsSource = studyBasedList;
            studyBasedComboBox.SelectedIndex = studyBasedList.IndexOf(Entrant.StudyBased ?? "");
        }

        private void FillSpecialityComboBox()
        {
            specialityComboBox.ItemsSource = speciality;
            specialityComboBox.SelectedIndex = speciality.IndexOf(Entrant.Speciality ?? "");
        }

        private void FillKostromaDistrictsComboBox()
        {
            kostroma_districtsComboBox.ItemsSource = district;
            if (Entrant.District != null && Entrant.District.Contains("Костром"))
                kostroma_districtsComboBox.IsEnabled = true;
            else
                kostroma_districtsComboBox.IsEnabled = false;
            kostroma_districtsComboBox.SelectedIndex = district.IndexOf(Entrant.District ?? "");
        }

        private void FillSubjectComboBox()
        {
            subjectComboBox.ItemsSource = subject;
            subjectComboBox.SelectedIndex = subject.IndexOf(Entrant.Subject ?? "");
        }

        private void FillFinishedOnlyComboBox()
        {
            finishedOnlyTextBox.IsEnabled = false;
            finishedOnlyComboBox.ItemsSource = variants9Or11Years;
            int finishedOnlyIndex = variants9Or11Years.IndexOf(Entrant.Finished9Or11Grade ?? "");
            finishedOnlyComboBox.SelectedIndex = finishedOnlyIndex == -1 ? 2 : finishedOnlyIndex;

            if (finishedOnlyComboBox.SelectedIndex == 2 && Entrant.Finished9Or11Grade != null)
            {
                finishedOnlyTextBox.Text = Entrant.Finished9Or11Grade;
                finishedOnlyTextBox.IsEnabled = true;
            }
        }

        private void FillCitizenshipComboBox()
        {
            citizenshipTextBox.IsEnabled = false;
            citizenshipComboBox.ItemsSource = citizenship;
            int citizenshipIndex = citizenship.IndexOf(Entrant.Citizenship ?? "");
            citizenshipComboBox.SelectedIndex = citizenshipIndex == -1 ? 2 : citizenshipIndex;

            if (citizenshipComboBox.SelectedIndex == 2 && Entrant.Citizenship != null)
            {
                citizenshipTextBox.Text = Entrant.Citizenship;
                citizenshipTextBox.IsEnabled = true;
            }
        }

        private void FillHasDisabilityCertificateComboBox()
        {
            hasDisabilityCertificateComboBox.ItemsSource = yesNo;
            int hasDisabilityCertificateIndex = yesNo.IndexOf(Entrant.HasDisabilityCertificate ?? "");
            hasDisabilityCertificateComboBox.SelectedIndex = hasDisabilityCertificateIndex == -1
                ? 0
                : hasDisabilityCertificateIndex;
        }

        private void SetHasDisabilityCertificateButtonEnabled()
        {
            hasDisabilityCertificateButton.IsEnabled = false;
            if (hasDisabilityCertificateComboBox.SelectedIndex == 0)
            {
                hasDisabilityCertificateButton.IsEnabled = true;
            }
        }

        private void FillHasOrphanageDocumentsComboBox()
        {
            hasOrphanageDocumentsComboBox.ItemsSource = yesNo;
            int hasOrphanageDocumentsIndex = yesNo.IndexOf(Entrant.HasOrphanageDocuments ?? "");
            hasOrphanageDocumentsComboBox.SelectedIndex = hasOrphanageDocumentsIndex == -1
                ? 0
                : hasOrphanageDocumentsIndex;
        }

        private void SetHasOrphanageDocumentsButtonEnabled()
        {
            hasOrphanageDocumentsButton.IsEnabled = false;
            if (hasDisabilityCertificateComboBox.SelectedIndex == 0)
            {
                hasOrphanageDocumentsButton.IsEnabled = true;
            }
        }


        private void FillEntrantData()
        {
            Entrant.Gender = genderComboBox.SelectedItem.ToString();
            Entrant.Citizenship = citizenshipComboBox.SelectedItem.ToString();
            Entrant.Finished9Or11Grade = finishedOnlyComboBox.SelectedItem.ToString();
            Entrant.Speciality = specialityComboBox.SelectedItem.ToString();
            Entrant.District = kostroma_districtsComboBox.SelectedItem.ToString();
            Entrant.Subject = subjectComboBox.SelectedItem.ToString();
            Entrant.StudyBased = studyBasedComboBox.SelectedItem.ToString();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValid(Entrant))
                {
                    MessageBox.Show("Не все поля заполнены!", "Ошибка");
                    return;
                }
                FillEntrantData();


                if (citizenshipComboBox.SelectedIndex == 2)
                {
                    Entrant.Citizenship = citizenshipTextBox.Text;
                }

                if (finishedOnlyComboBox.SelectedIndex == 2)
                {
                    Entrant.Finished9Or11Grade = finishedOnlyTextBox.Text;
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
        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            byte[]? file = LoadFile();
            MessageBox.Show(file?.ToString(), "Файл не загружен");
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
        }

        private void hasDisabilityCertificateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hasDisabilityCertificateComboBox.SelectedIndex == 0)
                hasDisabilityCertificateButton.IsEnabled = true;
            else
                hasDisabilityCertificateButton.IsEnabled = false;
        }

        private void hasOrphanageDocumentsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hasOrphanageDocumentsComboBox.SelectedIndex == 0)
                hasOrphanageDocumentsButton.IsEnabled = true;
            else
                hasOrphanageDocumentsButton.IsEnabled = false;
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
                   !string.IsNullOrEmpty(entrant.District) &&
                   !string.IsNullOrEmpty(entrant.Subject) &&
                   !string.IsNullOrEmpty(entrant.CertificateNumber) &&
                   !string.IsNullOrEmpty(entrant.Finished9Or11Grade) &&
                   (entrant.Gender == "Мужской" || entrant.Gender == "Женский") && // проверяем корректность поля Gender
                   (entrant.DateOfBirth <= DateTime.Today.AddYears(-16) && entrant.DateOfBirth >= DateTime.Today.AddYears(-100)) && // проверяем корректность даты рождения
                   (entrant.AverageScore >= 0 && entrant.AverageScore <= 10); // проверяем корректность среднего балла
        }
    }
}
