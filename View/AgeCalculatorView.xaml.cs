using System;
using System.Collections.Generic;
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
using _04Zaporozhets.ViewModel;

namespace _04Zaporozhets.View
{

    public partial class AgeCalculatorView : Window
    {
        public AgeCalculatorViewModel ageCalculatorViewModel;
        public AgeCalculatorView()
        {
            InitializeComponent();
            ageCalculatorViewModel = new AgeCalculatorViewModel(this);
        }


        public delegate void OnButtonClicked();
        public event OnButtonClicked onPressed;

        public delegate void OnTextChanged();
        public event OnTextChanged onTextChanged;

        public delegate void OnSaveButtonClicked();
        public event OnSaveButtonClicked onSavePressed;

        public delegate void OnCancelButtonClicked();
        public event OnCancelButtonClicked onCancelPressed;

        public string FirstName
        {
            get { return FirstNameTextBox.Text; }
        }

        public string LastName
        {
            get { return LastNameTextBox.Text; }
        }

        public string Email
        {
            get { return EmailTextBox.Text; }
        }

        public DateOnly? Birthday
        {
            get { return BirthdayPicker.SelectedDate.HasValue ? DateOnly.FromDateTime(BirthdayPicker.SelectedDate.Value) : null; }
            set
            {
                if (value.HasValue && value > DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new ValidationBirthInTheFutureException("Your birth date cannot be in the future.");
                }
                BirthdayPicker.SelectedDate = value.HasValue ? new DateTime(value.Value.Year, value.Value.Month, value.Value.Day) : (DateTime?)null;

            }
        }



        private void onButtonClicked(object sender, RoutedEventArgs e)
        {
            ButtonPressed();
        }

        public void ButtonPressed()
        {
            onPressed();
        }

        private void onTextChanged_func(object sender, TextChangedEventArgs e)
        {
            onTextChanged();
        }

        private void onTextChanged_func(object sender, SelectionChangedEventArgs e)
        {
            onTextChanged();
        }

        private void onSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            onSavePressed();
        }

        private void onCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            onCancelPressed();
        }
    }
}
