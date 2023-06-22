using Microsoft.EntityFrameworkCore;
using selection_committee.DB;
using selection_committee.DB.Models;
using selection_committee.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            db.Database.EnsureCreated();
            db.Entrants.Load();
            ObservableCollection<Entrant> entrants = db.Entrants.Local.ToObservableCollection();
            DataContext = entrants;
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
                    entrant.OrphanageDocumentsScan = EntrantWindow.Entrant.OrphanageDocumentsScan;
                }
            }
            db.SaveChanges();
        }

        private void SaveFile(byte[]? file)
        {
            if (file == null)
            {
                MessageBox.Show("Файл еще не загружен", "Ошибка");
                return;
            }
            byte[] fileBytes = file;

            // Создать диалог сохранения файла
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.FileName = "скан.png";
            saveDialog.DefaultExt = ".png";

            bool? result = saveDialog.ShowDialog(); // Отобразить диалог

            if (result == true)
            {
                string filePath = saveDialog.FileName; // Получить путь к файлу из диалога

                // Записать байты в файл
                File.WriteAllBytes(filePath, fileBytes);
            }
        }
        private void btnViewDisability_Click(object sender, RoutedEventArgs e)
        {
            Entrant? entrant = entrantsList.SelectedItem as Entrant;
            if (entrant == null) return;
            entrant = db.Entrants.Find(entrant.Id);
            SaveFile(entrant?.DisabilityCertificateScan);
        }

        private void btnViewOrphanhood_Click(object sender, RoutedEventArgs e)
        {
            Entrant? entrant = entrantsList.SelectedItem as Entrant;
            if (entrant == null) return;
            entrant = db.Entrants.Find(entrant.Id);
            SaveFile(entrant?.OrphanageDocumentsScan);
        }
    }
}
