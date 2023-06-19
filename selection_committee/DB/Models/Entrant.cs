using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace selection_committee.DB.Models
{
    public class Entrant : INotifyPropertyChanged
    {

        public int Id { get; set; }

        private string? surname;
        private string? name;
        private string? patronymic;
        private string? gender;
        private DateTime? dateOfBirth;
        private int? age;
        private string? citizenship;
        private string? subject;
        private string? city;
        private string? district;
        private string? finished9Or11Grade;
        private string? otherSchoolsAttended;
        private float? averageScore;
        private string? snils;
        private string? hasDisabilityCertificate;
        private byte[]? disabilityCertificateScan;
        private string? hasOrphanageDocuments;
        private byte[]? orphanageDocumentsScan;
        private string? speciality;
        private string? admissionRulesLink;
        private string? certificateNumber;

        public string? CertificateNumber
        {
            get { return certificateNumber; }
            set { certificateNumber = value; OnPropertyChanged("CertificateNumber"); }
        }
        public string? Surname
        {
            get { return surname; }
            set { surname = value; OnPropertyChanged("Surname"); }
        }

        public string? Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        public string? Patronymic
        {
            get { return patronymic; }
            set { patronymic = value; OnPropertyChanged("Patronymic"); }
        }

        public string? Gender
        {
            get { return gender; }
            set { gender = value; OnPropertyChanged("Gender"); }
        }

        public DateTime? DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; OnPropertyChanged("DateOfBirth"); }
        }

        public int? Age
        {
            get { return age; }
            set { age = value; OnPropertyChanged("Age"); }
        }

        public string? Citizenship
        {
            get { return citizenship; }
            set { citizenship = value; OnPropertyChanged("Citizenship"); }
        }

        public string? Subject
        {
            get { return subject; }
            set { subject = value; OnPropertyChanged("Subject"); }
        }

        public string? City
        {
            get { return city; }
            set { city = value; OnPropertyChanged("City"); }
        }

        public string? District
        {
            get { return district; }
            set { district = value; OnPropertyChanged("District"); }
        }

        public string? Finished9Or11Grade
        {
            get { return finished9Or11Grade; }
            set { finished9Or11Grade = value; OnPropertyChanged("Finished9Or11Grade"); }
        }

        public string? OtherSchoolsAttended
        {
            get { return otherSchoolsAttended; }
            set { otherSchoolsAttended = value; OnPropertyChanged("OtherSchoolsAttended"); }
        }

        public float? AverageScore
        {
            get { return averageScore; }
            set { averageScore = value; OnPropertyChanged("AverageScore"); }
        }

        public string? SNILS
        {
            get { return snils; }
            set { snils = value; OnPropertyChanged("SNILS"); }
        }

        public string? HasDisabilityCertificate
        {
            get { return hasDisabilityCertificate; }
            set { hasDisabilityCertificate = value; OnPropertyChanged("HasDisabilityCertificate"); }
        }

        public byte[]? DisabilityCertificateScan
        {
            get { return disabilityCertificateScan; }
            set { disabilityCertificateScan = value; OnPropertyChanged("DisabilityCertificateScan"); }
        }

        public string? HasOrphanageDocuments
        {
            get { return hasOrphanageDocuments; }
            set { hasOrphanageDocuments = value; OnPropertyChanged("HasOrphanageDocuments"); }
        }

        public byte[]? OrphanageDocumentsScan
        {
            get { return orphanageDocumentsScan; }
            set { orphanageDocumentsScan = value; OnPropertyChanged("OrphanageDocumentsScan"); }
        }

        public string? Speciality
        {
            get { return speciality; }
            set { speciality = value; OnPropertyChanged("Speciality"); }
        }

        public string? AdmissionRulesLink
        {
            get { return admissionRulesLink; }
            set { admissionRulesLink = value; OnPropertyChanged("AdmissionRulesLink"); }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
