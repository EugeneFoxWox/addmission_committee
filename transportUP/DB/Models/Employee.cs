using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace transportUP.DB
{
    public class Employee : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string? Identificator { get; set; }

        string? name;
        string? surname;
        string? patronymic;
        string? phoneNumber;
        string? departament;
 
        public string? Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
        public string? Surname
        {
            get { return surname; }
            set { surname = value; OnPropertyChanged("Surname"); }
        }

        public string? Patronymic
        {
            get { return patronymic; }
            set { patronymic = value; OnPropertyChanged("Patronymic"); }
        }

        public string? PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; OnPropertyChanged("PhoneNumber"); }
        }

        public string? Departament
        {
            get { return departament; }
            set { departament = value; OnPropertyChanged("Departament"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
