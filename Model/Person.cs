using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using _04Zaporozhets.View;

namespace _04Zaporozhets.ViewModel
{
    public class Person : BaseBindable
    {
        private static List<Person>? _persons;
        public int AgeYears => GetAgeComponents().Years;
        public int AgeMonths => GetAgeComponents().Months;
        public int AgeDays => GetAgeComponents().Days;

        private string _firstName;
        public Person()
        {
        }
        public string FirstName
        {
            get => _firstName;
            set => UpdateProperty(ref _firstName, value, nameof(FirstName));
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => UpdateProperty(ref _lastName, value, nameof(LastName));
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                EmailValidation(value);
                UpdateProperty(ref _email, value, nameof(Email));
            }
        }

        private DateOnly _birthday;
        public DateOnly Birthday
        {
            get => _birthday;
            set
            {
                BirthdayValidation(value);
                UpdateProperty(ref _birthday, value, nameof(Birthday));
            }
        }

        public Person(string firstName, string lastName, string email, DateOnly birthdate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Birthday = birthdate;
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateOnly.MinValue) { }

        public Person(string firstName, string lastName, DateOnly birthdate)
            : this(firstName, lastName, "", birthdate) { }

        private void EmailValidation(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                throw new ValidationEmailException("", email);
            }
        }

        private void BirthdayValidation(DateOnly birthdate)
        {
            if (birthdate > DateOnly.FromDateTime(DateTime.Today))
                throw new ValidationBirthInTheFutureException("", birthdate);
            if (birthdate < DateOnly.FromDateTime(DateTime.Today.AddYears(-135)))
                throw new ValidationBirthInThePastException("", birthdate);
        }

        public bool IsAdult => Birthday < DateOnly.FromDateTime(DateTime.Today.AddYears(-18));

        public string SunSign
        {
            get
            {
                DateTime birthdayDateTime = new DateTime(Birthday.Year, Birthday.Month, Birthday.Day);
                WesternZodiac sign = birthdayDateTime.GetWesternZodiacSign();
                return sign.ToString();
            }
        }

        public string ChineseSign
        {
            get
            {
                DateTime birthdayDateTime = new DateTime(Birthday.Year, Birthday.Month, Birthday.Day);
                ChineseZodiac sign = birthdayDateTime.GetChineseZodiacSign();
                return sign.ToString();
            }
        }

        public bool IsBirthday => Birthday.Day == DateTime.Now.Day && Birthday.Month == DateTime.Now.Month;

        private (int Years, int Months, int Days) GetAgeComponents()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var birthday = Birthday;

            var years = today.Year - birthday.Year;

            if (birthday.AddYears(years) > today)
            {
                years--;
            }

            var lastBirthday = birthday.AddYears(years);
            var months = 0;
            var days = 0;

            if (today.Month < birthday.Month || (today.Month == birthday.Month && today.Day < birthday.Day))
            {
                months = 12 - birthday.Month + today.Month - 1;
                days = DateTime.DaysInMonth(today.Year, today.Month) - birthday.Day + today.Day;
            }
            else
            {
                months = today.Month - birthday.Month;
                days = today.Day - birthday.Day;
            }

            if (today.Day < birthday.Day)
            {
                months--;
            }

            if (today.CompareTo(lastBirthday) > 0)
            {
                var daysInCurrentMonth = DateTime.DaysInMonth(today.Year, today.Month);
                days += daysInCurrentMonth;

                if (days > daysInCurrentMonth)
                {
                    days -= daysInCurrentMonth;
                    months++;
                }
            }

            return (years, months, days);
        }

        public string FormattedAge
        {
            get
            {
                var ageComponents = GetAgeComponents();
                return $"{ageComponents.Years} years, {ageComponents.Months} months, {ageComponents.Days} days";
            }
        }

        public static List<Person> GenerateUsers()
        {
            if (_persons != null)
                return _persons;

            _persons = new List<Person>();
            var rand = new Random();

            for (int i = 0; i < 50; i++)
            {
                var firstName = FirstNames[rand.Next(FirstNames.Count)];
                var lastName = LastNames[rand.Next(LastNames.Count)];
                int domainIndex = rand.Next(EmailDomains.Count);
                var email = $"{firstName.ToLower()}.{lastName.ToLower()}@{EmailDomains[domainIndex]}";
                var start = DateTime.Now.AddYears(-135);
                var end = DateTime.Now;
                var range = (end - start).Days;
                var dateOfBirth = DateOnly.FromDateTime(start.AddDays(rand.Next(range)));
                var person = new Person(firstName, lastName, email, dateOfBirth);
                _persons.Add(person);
            }

            return _persons;
        }
        private static readonly List<string> EmailDomains = new List<string>
        {
            "gmail.com", "ukma.edu.ua", "hotmail.com", "outlook.com", "ukr.net"
        };


        private static readonly List<string> FirstNames = new List<string>
{
    "Dmytro", "Oleksandr", "Ivan", "Vasyl", "Andriy", "Mykola", "Serhiy", "Petro", "Oleh", "Volodymyr", "Anatoliy",
    "Yuriy", "Taras", "Bohdan", "Yaroslav", "Roman", "Maksym", "Dmytro", "Artem", "Denys", "Andriy",
    "Viktor", "Yevhen", "Hryhoriy", "Oleksiy", "Vitaliy", "Valeriy", "Anton", "Oleksiy", "Oleksandr", "Yakiv",
    "Borys", "Mykhailo", "Anatoliy", "Volodymyr", "Mykola", "Oleksandr", "Serhiy", "Hennadiy", "Oleh",
    "Yaroslav", "Vasyl", "Andriy", "Petro", "Yuriy", "Oleh", "Serhiy", "Pavlo", "Roman", "Oleksandr",
    "Viktor", "Taras", "Artem", "Mykola", "Denys", "Andriy", "Yevhen", "Vitaliy", "Valeriy", "Hryhoriy",
    "Yakiv", "Vasyl", "Borys", "Mykhailo", "Anatoliy", "Volodymyr", "Oleksiy", "Serhiy", "Trokhym", "Oleh",
    "Yaroslav", "Andriy", "Petro", "Oleksandr", "Yuriy", "Taras", "Roman", "Oleh", "Hennadiy", "Vasyl",
    "Mykola", "Anton", "Oleksandr", "Yevhen", "Andriy", "Serhiy", "Pavlo", "Oleh", "Artem", "Denys",
    "Viktor", "Valeriy", "Hryhoriy", "Mykola", "Yakiv", "Vitaliy", "Borys", "Vasyl", "Yaroslav", "Oleksandr"
};
        private static readonly List<string> LastNames = new List<string>
{
    "Zaporozhets","Kovalenko", "Bondarenko", "Tkachenko", "Kravchenko", "Omelchenko", "Kovalchuk", "Boyko", "Tkachuk", "Moroz", "Koval",
    "Zaytsev", "Pavlenko", "Polishchuk", "Shevchenko", "Romanenko", "Dubenko", "Kolesnik", "Savchenko", "Ponomarenko", "Symonenko",
    "Kucherenko", "Shcherbak", "Babych", "Golub", "Rogov", "Havrylenko", "Melnichenko", "Pavlov", "Tymoshenko", "Yakovenko",
    "Bilous", "Tkachenko", "Shvets", "Turchyn", "Gonchar", "Petriv", "Soroka", "Lysenko", "Kovalenko", "Kuzmenko",
    "Kotenko", "Sydorenko", "Makarenko", "Zakharov", "Shcherbakov", "Kravets", "Vdovychenko", "Melnik", "Kuznetsov", "Yakymenko",
    "Didenko", "Pavlov", "Doroshenko", "Shevchenko", "Nesterenko", "Lavrenko", "Prykhodko", "Zakharov", "Horban", "Borisenko",
    "Krupa", "Chernysh", "Pavliuk", "Oliynyk", "Martynenko", "Shvets", "Savchuk", "Kovalchuk", "Pavlov",
    "Dovzhenko", "Kostyuk", "Melnik", "Holub", "Kondratenko", "Petrushenko", "Rogov", "Kovalenko", "Hontar", "Ivashchenko"
};

    }
}
