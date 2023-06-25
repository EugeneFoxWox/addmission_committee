using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using selection_committee.DB;
using selection_committee.DB.Models;
using selection_committee.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace selection_committee
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
            
        }

        private List<string> speciality = new List<string>()
        {
            "Все специальности",
            "Архитектура",
            "Гидрогеология и инженерная геология",
            "Информационные системы и программирование (на базе 9 классов)",
            "Строительство и эксплуатация зданий и сооружений (на базе 9 классов)",
            "Разработка электронных устройств и систем",
            "Информационные системы и программирование (на базе 11 классов)",
            "Строительство и эксплуатация зданий и сооружений (на базе 11 классов)"
        };

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            specialityComboBox.ItemsSource = speciality;
            specialityComboBox.SelectedIndex = 0;
            db.Database.EnsureCreated();
            db.Entrants.Load();
            ObservableCollection<Entrant> entrants = db.Entrants.Local.ToObservableCollection();
            DataContext = entrants;
            MakeComboboxItems(entrants);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EntrantWindow entrantWindow = new EntrantWindow(new Entrant());
            if (entrantWindow.ShowDialog() == true)
            {
                Entrant Entrant = entrantWindow.Entrant;
                db.Entrants.Add(Entrant);
            }
            db.SaveChanges();
            ObservableCollection<Entrant> entrants = db.Entrants.Local.ToObservableCollection();
            DataContext = entrants;
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            string value = findEntrantCB.Text;
            ObservableCollection<Entrant> entrants = new ObservableCollection<Entrant>();



            if (value.Trim().Equals(""))
            {
                DataContext = db.Entrants.Local.ToObservableCollection();
                return;
            }

            foreach (Entrant ent in db.Entrants)
            {
                string fullName = string.Format("{0} {1} {2}", ent.Surname, ent.Name, ent.Patronymic);
                if (fullName.Equals(value))
                {
                    entrants.Add(ent);
                }

            }

            if (entrants.Count == 0)
            {
                MessageBox.Show(string.Format("Не найден ни один работник по критерию ({0}) не найден!", value), "Ошибка!");
                return;
            }

            DataContext = entrants;



        }



        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Entrant? entrant = entrantsList.SelectedItem as Entrant;
            if (entrant is null) return;
            db.Entrants.Remove(entrant);
            db.SaveChanges();
            ObservableCollection<Entrant> entrants = db.Entrants.Local.ToObservableCollection();
            DataContext = entrants;
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Entrant? entrant = entrantsList.SelectedItem as Entrant;
            if (entrant is null) return;

            EntrantWindow EntrantWindow = new EntrantWindow(new Entrant
            {
                Id = entrant.Id,
                Name = entrant.Name,
                Surname = entrant.Surname,
                Patronymic = entrant.Patronymic,
                Gender = entrant.Gender,
                Citizenship = entrant.Citizenship,
                SNILS = entrant.SNILS,
                Finished9Or11Grade = entrant.Finished9Or11Grade,
                AverageScore = entrant.AverageScore,
                Speciality = entrant.Speciality,
                District = entrant.District,
                DateOfBirth = entrant.DateOfBirth,
                Subject = entrant.Subject,
                CertificateNumber = entrant.CertificateNumber,
                StudyBased = entrant.StudyBased,
                Enlisted = entrant.Enlisted,
                YearOfEnlisted = entrant.YearOfEnlisted,
                OrphanageDocumentsScan = entrant.OrphanageDocumentsScan,
                DisabilityCertificateScan = entrant.DisabilityCertificateScan,
                HasOrphanageDocuments = entrant.HasOrphanageDocuments,
                HasDisabilityCertificate = entrant.HasDisabilityCertificate,
            });

            if (EntrantWindow.ShowDialog() == true)
            {
                entrant = db.Entrants.Find(EntrantWindow.Entrant.Id);

                if (entrant != null)
                {
                    entrant.Id = EntrantWindow.Entrant.Id;
                    entrant.Name = EntrantWindow.Entrant.Name;
                    entrant.Patronymic = EntrantWindow.Entrant.Patronymic;
                    entrant.Surname = EntrantWindow.Entrant.Surname;
                    entrant.Gender = EntrantWindow.Entrant.Gender;
                    entrant.Citizenship = EntrantWindow.Entrant.Citizenship;
                    entrant.SNILS = EntrantWindow.Entrant.SNILS;
                    entrant.Finished9Or11Grade = EntrantWindow.Entrant.Finished9Or11Grade;
                    entrant.AverageScore = EntrantWindow.Entrant.AverageScore;
                    entrant.Speciality = EntrantWindow.Entrant.Speciality;
                    entrant.District = EntrantWindow.Entrant.District;
                    entrant.DateOfBirth = EntrantWindow.Entrant.DateOfBirth;
                    entrant.Subject = EntrantWindow.Entrant.Subject;
                    entrant.CertificateNumber = EntrantWindow.Entrant.CertificateNumber;
                    entrant.StudyBased = EntrantWindow.Entrant.StudyBased;
                    entrant.Enlisted = EntrantWindow.Entrant.Enlisted;
                    entrant.YearOfEnlisted = EntrantWindow.Entrant.YearOfEnlisted;
                    entrant.DisabilityCertificateScan = EntrantWindow.Entrant.DisabilityCertificateScan;
                    entrant.HasDisabilityCertificate = EntrantWindow.Entrant.HasDisabilityCertificate;
                    entrant.OrphanageDocumentsScan = EntrantWindow.Entrant.OrphanageDocumentsScan;
                    entrant.HasOrphanageDocuments = EntrantWindow.Entrant.HasOrphanageDocuments;

                }
            }
            db.SaveChanges();
        }

        private void SaveFile(byte[]? file, string fileName)
        {
            if (file == null)
            {
                MessageBox.Show("Файл еще не загружен", "Ошибка");
                return;
            }
            byte[] fileBytes = file;


            // Создать диалог сохранения файла
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = fileName + ".png";
            saveDialog.DefaultExt = ".png";
            saveDialog.Filter = "Изображения (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg";

            bool? result = saveDialog.ShowDialog(); // Отобразить диалог

            if (result == true)
            {
                string filePath = saveDialog.FileName; // Получить путь к файлу из диалога

                // Записать байты в файл
                File.WriteAllBytes(filePath, fileBytes);
                MessageBoxResult messageBox = MessageBox.Show(
                            "Вы успешно загрузили файл " + filePath + "\nВы хотите его открыть?", "Успешно!",
                            MessageBoxButton.YesNo);
                if (messageBox == MessageBoxResult.Yes) 
                {
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(filePath)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
            }
        }
        private void btnViewDisability_Click(object sender, RoutedEventArgs e)
        {
            Entrant? entrant = entrantsList.SelectedItem as Entrant;
            if (entrant == null) return;
            entrant = db.Entrants.Find(entrant.Id);
            SaveFile(entrant?.DisabilityCertificateScan, "Справка об инвалидности");
        }

        private void btnViewOrphanhood_Click(object sender, RoutedEventArgs e)
        {
            Entrant? entrant = entrantsList.SelectedItem as Entrant;
            if (entrant == null) return;
            entrant = db.Entrants.Find(entrant.Id);

            SaveFile(entrant?.OrphanageDocumentsScan, "Справка о сиротстве");
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            List<Entrant> entrants = db.Entrants.Local.ToList();
            SaveToExcel(entrants);
        }


        public void SaveToExcel(List<Entrant> entrants)
        {
            // Фильтруем список абитуриентов по полю Enlisted
            var enlistedEntrants = entrants.Where(e => e.Enlisted == "Да").ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Создаем новый пакет Excel
            using (var package = new ExcelPackage())
            {
                // Создаем лист
                var worksheet = package.Workbook.Worksheets.Add("Абитуриенты");

                // Задаем заголовки столбцов

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Фамилия";
                worksheet.Cells[1, 3].Value = "Имя";
                worksheet.Cells[1, 4].Value = "Отчество";
                worksheet.Cells[1, 5].Value = "Дата рождения";
                worksheet.Cells[1, 6].Value = "Возраст";
                worksheet.Cells[1, 7].Value = "Пол";
                worksheet.Cells[1, 8].Value = "Гражданство";
                worksheet.Cells[1, 9].Value = "Область";
                worksheet.Cells[1, 10].Value = "Район";
                worksheet.Cells[1, 11].Value = "Окончил 9 или 11 класс";
                worksheet.Cells[1, 12].Value = "Другие школы";
                worksheet.Cells[1, 13].Value = "Средний балл";
                worksheet.Cells[1, 14].Value = "СНИЛС";
                worksheet.Cells[1, 15].Value = "Скан свидетельства о регистрации инвалидности";
                worksheet.Cells[1, 16].Value = "Скан документов о сиротстве";
                worksheet.Cells[1, 17].Value = "Специальность";
                worksheet.Cells[1, 18].Value = "№ аттестата/диплома";

                // Заполняем ячейки данными абитуриентов
                for (int i = 0; i < enlistedEntrants.Count; i++)
                {
                    var entrant = enlistedEntrants[i];

                    worksheet.Cells[i + 2, 1].Value = entrant.Id;
                    worksheet.Cells[i + 2, 2].Value = entrant.Surname;
                    worksheet.Cells[i + 2, 3].Value = entrant.Name;
                    worksheet.Cells[i + 2, 4].Value = entrant.Patronymic;
                    worksheet.Cells[i + 2, 5].Value = entrant.DateOfBirth?.ToString("dd.MM.yyyy");
                    DateTime birthDate = entrant.DateOfBirth ?? new DateTime();
                    TimeSpan age = DateTime.Today - birthDate;
                    int years = (int)(age.TotalDays / 365.25);
                    worksheet.Cells[i + 2, 6].Value = years;
                    worksheet.Cells[i + 2, 7].Value = entrant.Gender;
                    worksheet.Cells[i + 2, 8].Value = entrant.Citizenship;
                    worksheet.Cells[i + 2, 9].Value = entrant.Subject;
                    worksheet.Cells[i + 2, 10].Value = entrant.District;
                    worksheet.Cells[i + 2, 11].Value = entrant.Finished9Or11Grade;
                    worksheet.Cells[i + 2, 12].Value = entrant.OtherSchoolsAttended;
                    worksheet.Cells[i + 2, 13].Value = entrant.AverageScore;
                    worksheet.Cells[i + 2, 14].Value = entrant.SNILS;
                    worksheet.Cells[i + 2, 15].Value = entrant.DisabilityCertificateScan != null ? "Приложен" : "";
                    worksheet.Cells[i + 2, 16].Value = entrant.OrphanageDocumentsScan != null ? "Приложен" : "";
                    worksheet.Cells[i + 2, 17].Value = entrant.Speciality;
                    worksheet.Cells[i + 2, 18].Value = entrant.CertificateNumber;
                }
                worksheet.Cells[1, 1, enlistedEntrants.Count + 1, 23].AutoFitColumns();
                // Открываем проводник для выбора пути сохранения файла
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = "Абитуриенты.xlsx";

                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        var file = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(file);

                        MessageBoxResult result = MessageBox.Show(
                            "Вы успешно загрузили файл " + file.FullName + "\nВы хотите его открыть?", "Успешно!",
                            MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            var p = new Process();
                            p.StartInfo = new ProcessStartInfo(file.FullName)
                            {
                                UseShellExecute = true
                            };
                            p.Start();
                        }
                        
                        // Сохраняем файл по выбранному пути
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка!");
                    }
                    
                }
            }
        }
        
        private void specialityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string currentSpeciality = specialityComboBox.SelectedItem.ToString() ?? "";

            DataContext = currentSpeciality == "Все специальности"
                ? db.Entrants.Local.ToObservableCollection()
                : db.Entrants.Local.Where(el => el.Speciality == currentSpeciality);

        }
    void OnComboboxTextChanged(object sender, RoutedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            // Открытие выпадающего списка
            comboBox.IsDropDownOpen = true;
            comboBox.SelectedIndex = -1;
                // Получение представления коллекции элементов комбобокса
            var collectionView = CollectionViewSource.GetDefaultView(comboBox.Items);
            if (collectionView != null)
            {
                // Фильтрация элементов коллекции
                string searchText = comboBox.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    collectionView.Filter = item =>
                        ((string)item).IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0;
                }
                else
                {
                    collectionView.Filter = null;
                }
            }
        }
    }

        private void MakeComboboxItems(ObservableCollection<Entrant> entrants)
        {
            findEntrantCB.Items.Clear();
            foreach (Entrant entrant in entrants)
            {
                if (entrant is null) continue;
                    findEntrantCB.Items.Add(entrant.Surname + " " + entrant.Name + " " + entrant.Patronymic);
            }
        }
    }
    
}
