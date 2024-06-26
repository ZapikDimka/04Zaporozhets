﻿using System;
using System.Threading.Tasks;
using System.Windows;
using _04Zaporozhets.View;

namespace _04Zaporozhets.ViewModel
{
    public class AgeCalculatorViewModel : BaseBindable
    {
        private AgeCalculatorView _view;
        private Person? _person;
        private bool _outputDataDisplayed = false;
        public static bool _checked = false;
        public bool OutputDataDisplayed
        {
            get { return _outputDataDisplayed; }
            set
            {
                _outputDataDisplayed = value;
                OnPropertyChanged(nameof(OutputDataDisplayed));
            }
        }

        public Person GetPerson()
        {
            return _person;
        }

        public AgeCalculatorViewModel(AgeCalculatorView view)
        {
            _view = view;
            _view.onPressed += calculate;
            _view.onTextChanged += updateButtonAvailability;
        }

        private bool isAgeCorrect(DateOnly? birthday)
        {
            return (birthday != null);
        }

        private bool isInputValid()
        {
            if (string.IsNullOrEmpty(_view.FirstName) ||
                string.IsNullOrEmpty(_view.LastName) ||
                string.IsNullOrEmpty(_view.Email) ||
                !_view.Birthday.HasValue)
            {
                return false;
            }

            return true;
        }
        async Task<(bool, string, string, bool)> calculateData(Person person)
        {
            Task<bool> isAdultTask = Task.Run(() => person.IsAdult);
            Task<string> sunSignTask = Task.Run(() => person.SunSign);
            Task<string> chineseSignTask = Task.Run(() => person.ChineseSign);
            Task<bool> isBirthdayTask = Task.Run(() => person.IsBirthday);

            var isAdult = await isAdultTask;
            var sunSign = await sunSignTask;
            var chineseSign = await chineseSignTask;
            var isBirthday = await isBirthdayTask;

            return (isAdult, sunSign, chineseSign, isBirthday);
        }
        private void updateButtonAvailability()
        {
            _view.ProceedButton.IsEnabled = isInputValid();
        }

        private async void calculate()
        {
            String firstName = _view.FirstName;
            String lastName = _view.LastName;
            String email = _view.Email;
            DateOnly? birthDate = _view.Birthday;

            _view.OutputTextBox.Text = "";

            if (!isAgeCorrect(birthDate))
            {
                MessageBox.Show("Enter valid date");
                return;
            }
            try
            {
                if (birthDate.HasValue)
                    _person = new Person(firstName, lastName, email, birthDate.Value);
                Task<(bool isAdult, string sunSign, string chineseSign, bool isBirthday)> task1 = calculateData(_person);
                interfaceEnable(false);
                var data = await task1;

                string output = "FirstName: " + _person.FirstName + "\n" +
                    "LastName: " + _person.LastName + "\n" +
                    "Email: " + _person.Email + "\n" +
                    "Birthdate: " + _person.Birthday.ToShortDateString() + "\n" +
                    "Is Adult: " + data.isAdult + "\n" +
                    "Sun Sign: " + data.sunSign + "\n" +
                    "Chinese Sign: " + data.chineseSign + "\n" +
                    "Is Birthday: " + data.isBirthday;

                _view.OutputTextBox.Text = output;

                interfaceEnable(true);

                if (data.isBirthday)
                {
                    MessageBox.Show("Happy birthday");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _checked = true;
        }
        private void interfaceEnable(bool b)
        {
            if (_view.IsEnabled)
            {
                _view.ProceedButton.IsEnabled = b;
                _view.EmailTextBox.IsEnabled = b;
                _view.FirstNameTextBox.IsEnabled = b;
                _view.LastNameTextBox.IsEnabled = b;
                _view.BirthdayPicker.IsEnabled = b;
            }
        }
    }
}
