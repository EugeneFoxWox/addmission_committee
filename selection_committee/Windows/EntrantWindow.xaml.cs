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
using System.Diagnostics;
using System.Windows.Navigation;

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
            "Чукотский автономный округ","Ямало-Ненецкий автономный округ"};
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

        public EntrantWindow(Entrant entrant)
        {
            InitializeComponent();
            Entrant = entrant;
            DataContext = Entrant;

            genderComboBox.ItemsSource = genders;
            genderComboBox.SelectedIndex = genders.IndexOf(entrant.Gender ?? "");

            specialityComboBox.ItemsSource = speciality;
            specialityComboBox.SelectedIndex = speciality.IndexOf(entrant.Speciality ?? "");

            kostroma_districtsComboBox.ItemsSource = district;
            kostroma_districtsComboBox.SelectedIndex = district.IndexOf(entrant.District ?? "");

            subjectComboBox.ItemsSource = subject;
            subjectComboBox.SelectedIndex = subject.IndexOf(entrant.Subject ?? "");


            finishedOnlyTextBox.IsEnabled = false;
            finishedOnlyComboBox.ItemsSource = variants9Or11Years;
            int finishedOnlyIndex = variants9Or11Years.IndexOf(entrant.Finished9Or11Grade ?? "");
            finishedOnlyComboBox.SelectedIndex = finishedOnlyIndex == -1 ? 2 : finishedOnlyIndex;

            if (finishedOnlyComboBox.SelectedIndex == 2 && entrant.Finished9Or11Grade != null)
            {
                finishedOnlyTextBox.Text = entrant.Finished9Or11Grade;
                finishedOnlyTextBox.IsEnabled = true;
            }


            citizenshipTextBox.IsEnabled= false;
            citizenshipComboBox.ItemsSource = citizenship;
            int citizenshipIndex = citizenship.IndexOf(entrant.Citizenship ?? "");
            citizenshipComboBox.SelectedIndex = finishedOnlyIndex == -1 ? 2 : finishedOnlyIndex;

            if (citizenshipComboBox.SelectedIndex == 2 && entrant.Citizenship != null)
            {
                citizenshipTextBox.Text = entrant.Citizenship;
                citizenshipTextBox.IsEnabled = true;
            }

            hasDisabilityCertificateComboBox.ItemsSource = yesNo;
            int hasDisabilityCertificateIndex = yesNo.IndexOf(entrant.HasDisabilityCertificate ?? "");
            hasDisabilityCertificateComboBox.SelectedIndex = hasDisabilityCertificateIndex == -1 
                ? 0
                : finishedOnlyIndex;

            hasDisabilityCertificateButton.IsEnabled = false;
            if (hasDisabilityCertificateComboBox.SelectedIndex == 0)
            {
                hasDisabilityCertificateButton.IsEnabled = true;
            }
            

            hasOrphanageDocumentsComboBox.ItemsSource = yesNo;
            int hasOrphanageDocumentsIndex = yesNo.IndexOf(entrant.HasOrphanageDocuments ?? "");
            hasOrphanageDocumentsComboBox.SelectedIndex = hasOrphanageDocumentsIndex == -1 
                ? 0
                : finishedOnlyIndex;

            hasOrphanageDocumentsButton.IsEnabled = false;
            if (hasDisabilityCertificateComboBox.SelectedIndex == 0)
            {
                hasOrphanageDocumentsButton.IsEnabled = true;
            }

        }
        
        

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Entrant.Gender = genderComboBox.SelectedItem.ToString();
            Entrant.Citizenship = citizenshipComboBox.SelectedItem.ToString();
            Entrant.Finished9Or11Grade = finishedOnlyComboBox.SelectedItem.ToString();
            Entrant.Speciality = specialityComboBox.SelectedItem.ToString();
            Entrant.District = kostroma_districtsComboBox.SelectedItem.ToString();
            Entrant.Subject = subjectComboBox.SelectedItem.ToString();


            if (citizenshipComboBox.SelectedIndex == 2)
            {
                Entrant.Citizenship = citizenshipTextBox.Text;
            }

            if (finishedOnlyComboBox.SelectedIndex == 2)
            {
                Entrant.Finished9Or11Grade = finishedOnlyTextBox.Text;
            }

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
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
