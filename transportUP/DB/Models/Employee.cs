using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace transportUP.DB
{
    public class Entrant : INotifyPropertyChanged
    {
        public int Id { get; set; }
        

        string? name;
        string? surname;
        string? patronymic;
        string? gender;
        int? full_age;
        string? birthday;
        string? nationality;
        string? place_of_residence;
        bool? completed_nine_classes;
        bool? completed_eleven_classes;
        double? avg_sertificate;
        int? snils;
        string? speciality;
        string? budget;
        bool? enlisted;
        string? date_of_income;



        private string? name;
        public string? Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        private string? surname;
        public string? Surname
        {
            get { return surname; }
            set { surname = value; OnPropertyChanged("Surname"); }
        }

        private string? patronymic;
        public string? Patronymic
        {
            get { return patronymic; }
            set { patronymic = value; OnPropertyChanged("Patronymic"); }
        }

        private string? gender;
        public string? Gender
        {
            get { return gender; }
            set { gender = value; OnPropertyChanged("Gender"); }
        }

        private int? fullAge;
        public int? FullAge
        {
            get { return fullAge; }
            set { fullAge = value; OnPropertyChanged("FullAge"); }
        }

        private string? birthday;
        public string? Birthday
        {
            get { return birthday; }
            set { birthday = value; OnPropertyChanged("Birthday"); }
        }

        private string? nationality;
        public string? Nationality
        {
            get { return nationality; }
            set { nationality = value; OnPropertyChanged("Nationality"); }
        }

        private string? placeOfResidence;
        public string? PlaceOfResidence
        {
            get { return placeOfResidence; }
            set { placeOfResidence = value; OnPropertyChanged("PlaceOfResidence"); }
        }

        private bool? completedNineClasses;
        public bool? CompletedNineClasses
        {
            get { return completedNineClasses; }
            set { completedNineClasses = value; OnPropertyChanged("CompletedNineClasses"); }
        }

        private bool? completedElevenClasses;
        public bool? CompletedElevenClasses
        {
            get { return completedElevenClasses; }
            set { completedElevenClasses = value; OnPropertyChanged("CompletedElevenClasses"); }
        }

        private double? avgSertificate;
        public double? AvgSertificate
        {
            get { return avgSertificate; }
            set { avgSertificate = value; OnPropertyChanged("AvgSertificate"); }
        }

        private int? snils;
        public int? Snils
        {
            get { return snils; }
            set { snils = value; OnPropertyChanged("Snils"); }
        }

        private string? speciality;
        public string? Speciality
        {
            get { return speciality; }
            set { speciality = value; OnPropertyChanged("Speciality"); }
        }

        private string? budget;
        public string? Budget
        {
            get { return budget; }
            set { budget = value; OnPropertyChanged("Budget"); }
        }

        private bool? enlisted;
        public bool? Enlisted
        {
            get { return enlisted; }
            set { enlisted = value; OnPropertyChanged("Enlisted"); }
        }

        private string? dateOfIncome;
        public string? DateOfIncome
        {
            get { return dateOfIncome; }
            set { dateOfIncome = value; OnPropertyChanged("DateOfIncome"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
