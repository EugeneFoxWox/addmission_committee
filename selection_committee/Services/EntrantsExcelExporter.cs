using Microsoft.Win32;
using OfficeOpenXml;
using selection_committee.DB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace selection_committee.Services
{
    public class EntrantsExcelExporter
    {
        private readonly List<Entrant> _entrants;

        public EntrantsExcelExporter(List<Entrant> entrants)
        {
            _entrants = entrants;
        }

        public async void ExportEnlistedEntrants()
        {
            var enlistedEntrants = FilterEnlistedEntrants();
            var package = CreateExcelPackage(enlistedEntrants);
            var filePath = await GetSaveFilePathAsync();
            if (filePath != null)
            {
                try
                {
                    var file = new FileInfo(filePath);
                    package.SaveAs(file);

                    ShowSuccessMessage(file.FullName);

                    if (ShouldOpenFile())
                    {
                        OpenExcelFile(file.FullName);
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }

        private List<Entrant> FilterEnlistedEntrants()
        {
            return _entrants.Where(e => e.Enlisted == "Да").ToList();
        }

        private ExcelPackage CreateExcelPackage(List<Entrant> enlistedEntrants)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Абитуриенты");
            SetColumnHeaders(worksheet);

            for (int i = 0; i < enlistedEntrants.Count; i++)
            {
                var entrant = enlistedEntrants[i];
                FillRowData(worksheet, i + 2, entrant);
            }

            worksheet.Cells[1, 1, enlistedEntrants.Count + 1, 23].AutoFitColumns();

            return package;
        }

        private void SetColumnHeaders(ExcelWorksheet worksheet)
        {
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
        }

        private void FillRowData(ExcelWorksheet worksheet, int rowNumber, Entrant entrant)
        {
            worksheet.Cells[rowNumber, 1].Value = entrant.Id;
            worksheet.Cells[rowNumber, 2].Value = entrant.Surname;
            worksheet.Cells[rowNumber, 3].Value = entrant.Name;
            worksheet.Cells[rowNumber, 4].Value = entrant.Patronymic;
            worksheet.Cells[rowNumber, 5].Value = entrant.DateOfBirth?.ToString("dd.MM.yyyy");
            DateTime birthDate = entrant.DateOfBirth ?? new DateTime();
            TimeSpan age = DateTime.Today - birthDate;
            int years = (int)(age.TotalDays / 365.25);
            worksheet.Cells[rowNumber, 6].Value = years;
            worksheet.Cells[rowNumber, 7].Value = entrant.Gender;
            worksheet.Cells[rowNumber, 8].Value = entrant.Citizenship;
            worksheet.Cells[rowNumber, 9].Value = entrant.Subject;
            worksheet.Cells[rowNumber, 10].Value = entrant.District;
            worksheet.Cells[rowNumber, 11].Value = entrant.Finished9Or11Grade;
            worksheet.Cells[rowNumber, 12].Value = entrant.OtherSchoolsAttended;
            worksheet.Cells[rowNumber, 13].Value = entrant.AverageScore;
            worksheet.Cells[rowNumber, 14].Value = entrant.SNILS;
            worksheet.Cells[rowNumber, 15].Value = entrant.DisabilityCertificateScan != null ? "Приложен" : "";
            worksheet.Cells[rowNumber, 16].Value = entrant.OrphanageDocumentsScan != null ? "Приложен" : "";
            worksheet.Cells[rowNumber, 17].Value = entrant.Speciality;
            worksheet.Cells[rowNumber, 18].Value = entrant.CertificateNumber;
        }

        private async Task<string?> GetSaveFilePathAsync()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "Абитуриенты.xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                return await Task.FromResult(saveFileDialog.FileName);
            }

            return null;
        }

        private bool ShouldOpenFile()
        {
            var result = MessageBox.Show(
                "Вы успешно загрузили файл.\nВы хотите его открыть?", "Успешно!",
                MessageBoxButton.YesNo);

            return result == MessageBoxResult.Yes;
        }

        private void OpenExcelFile(string filePath)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(filePath)
                {
                    UseShellExecute = true
                }
            };
            process.Start();
        }

        private void ShowSuccessMessage(string filePath)
        {
            MessageBox.Show($"Вы успешно загрузили файл {filePath}", "Успешно!");
        }

        private void ShowErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Ошибка!");
        }
    }
}
