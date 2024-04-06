using _04Zaporozhets.ViewModel;
using System;

namespace _04Zaporozhets
{
    internal class ValidationEmailException : Exception
    {
        private string _message;

        public ValidationEmailException(string message)
        {
            this._message = message;
        }

        public ValidationEmailException(string message, string email) : this("prove your email")
        {
        }

        public override string Message
        {
            get { return _message; }
        }

        internal void NotifyValidationError(AgeCalculatorViewModel ageCalculatorViewModel, string v)
        {
            throw new NotImplementedException();
        }
    }
}
