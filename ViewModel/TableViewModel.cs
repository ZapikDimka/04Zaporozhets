using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using _04Zaporozhets.View;

namespace _04Zaporozhets.ViewModel
{
    internal class TableViewModel
    {
        private TableView _view;
        private static List<Person> _persons = new List<Person>();
        private static bool _dataLoaded = false; // Прапорець, що показує, чи вже були завантажені дані з файлу

        static TableViewModel()
        {
            string filePath = "data.json"; // Шлях до файлу JSON
            if (File.Exists(filePath) && !_dataLoaded)
            {
                LoadDataFromFile(filePath);
                _dataLoaded = true; // Помічаємо, що дані були завантажені з файлу
            }
            else
            {
                _persons = Person.GenerateUsers(); // Якщо файл не існує або дані вже завантажені, створити новий список
            }
        }

        public TableViewModel(TableView view)
        {
            _view = view;
            _view.userGrid.ItemsSource = _persons;
        }

        public void SaveDataToFile(string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(_persons);
                File.WriteAllText(filePath, json);
                MessageBox.Show("Data saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving data: {ex.Message}");
            }
        }

        public static void LoadDataFromFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                _persons = JsonConvert.DeserializeObject<List<Person>>(json);
                MessageBox.Show("Data loaded successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading data: {ex.Message}");
            }
        }
    }
}
