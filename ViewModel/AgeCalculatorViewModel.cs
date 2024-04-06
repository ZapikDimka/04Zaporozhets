using System;
using System.Threading.Tasks;
using System.Windows;
using _04Zaporozhets.View;

namespace _04Zaporozhets.ViewModel
{
    public class AgeCalculatorViewModel : BaseBindable
    {
        private AgeCalculatorView _view;
        private Person _person;
        private bool _outputDataDisplayed = false;

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

        private void updateButtonAvailability()
        {
            _view.ProceedButton.IsEnabled = isInputValid();
        }

        private async void calculate()
        {
            // Перевірка вхідних даних
            if (ValidateInputData())
            {

                string firstName = _view.FirstName;
                string lastName = _view.LastName;
                string email = _view.Email;
                DateOnly? birthDate = _view.Birthday;

                _view.OutputTextBox.Text = "";

                try
                {
                    // Використання методів валідації з класу Person
                    Person person = new Person();

                    _person = new Person(firstName, lastName, email, birthDate.Value);
                    Task<(bool isAdult, string sunSign, string chineseSign, bool isBirthday, string formattedAge)> task1 = calculateData(_person);
                    interfaceEnable(false);
                    var data = await task1;

                    string output = "FirstName: " + _person.FirstName + "\n" +
                                    "LastName: " + _person.LastName + "\n" +
                                    "Email: " + _person.Email + "\n" +
                                    "Birthdate: " + _person.Birthday.ToShortDateString() + "\n" +
                                    "Age: " + data.formattedAge + "\n" +
                                    "Is Adult: " + data.isAdult + "\n" +
                                    "Sun Sign: " + data.sunSign + "\n" +
                                    "Chinese Sign: " + data.chineseSign + "\n" +
                                    "Is Birthday: " + data.isBirthday;

                    _view.OutputTextBox.Text = output;
                    _outputDataDisplayed = true;
                    interfaceEnable(true);
                    OutputDataDisplayed = true;

                    if (data.isBirthday)
                    {
                        MessageBox.Show("Happy birthday!");
                    }
                }
                catch (ValidationBirthInTheFutureException ex)
                {
                    ex.NotifyValidationError(this, nameof(_view.Birthday));
                    MessageBox.Show(ex.Message);
                }
                catch (ValidationBirthInThePastException ex)
                {
                    ex.NotifyValidationError(this, nameof(_view.Birthday));
                    MessageBox.Show(ex.Message);
                }
                catch (ValidationEmailException ex)
                {
                    ex.NotifyValidationError(this, nameof(_view.Email));
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }


        private bool ValidateInputData()
        {
            if (!isAgeCorrect(_view.Birthday))
            {
                MessageBox.Show("Enter a valid date.");
                return false;
            }

            if (!isInputValid())
            {
                MessageBox.Show("Enter all required fields.");
                return false;
            }

            if (!IsValidEmail(_view.Email))
            {
                MessageBox.Show("Enter a valid email address.");
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async Task<(bool, string, string, bool, string)> calculateData(Person person)
        {
            Task<bool> isAdultTask = Task.Run(() => person.IsAdult);
            Task<string> sunSignTask = Task.Run(() => person.SunSign);
            Task<string> chineseSignTask = Task.Run(() => person.ChineseSign);
            Task<bool> isBirthdayTask = Task.Run(() => person.IsBirthday);

            var isAdult = await isAdultTask;
            var sunSign = await sunSignTask;
            var chineseSign = await chineseSignTask;
            var isBirthday = await isBirthdayTask;
            var formattedAge = person.FormattedAge;

            return (isAdult, sunSign, chineseSign, isBirthday, formattedAge);
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
