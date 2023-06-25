using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using selection_committee.DB;
using selection_committee.DB.Models;
using selection_committee.Services;
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
            MakeComboboxItems(entrants);
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
            MakeComboboxItems(entrants);
            DataContext = entrants;
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (!(entrantsList.SelectedItem is Entrant entrant)) return;

            EntrantWindow entrantWindow = new EntrantWindow(entrant);

            var editedEntrant = entrantWindow.ShowDialog() == true ? entrantWindow.Entrant : null;
            if (editedEntrant != null)
            {
                db.Entry(entrant).CurrentValues.SetValues(editedEntrant);
                db.SaveChanges();
                ObservableCollection<Entrant> entrants = db.Entrants.Local.ToObservableCollection();
                MakeComboboxItems(entrants);
            }
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
                // Получить путь к файлу из диалога
                string filePath = saveDialog.FileName; 

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
            var exporter = new EntrantsExcelExporter(entrants);
            exporter.ExportEnlistedEntrants();
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
