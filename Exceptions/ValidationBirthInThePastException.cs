using System;

namespace _04Zaporozhets
{
    internal class ValidationBirthInThePastException : Exception
    {
        private string _message;

        public ValidationBirthInThePastException(string message)
        {
            this._message = message;
        }

        public ValidationBirthInThePastException(string message, DateOnly dateTime) : this(("You are tooo old")){ }

        public override string Message
        {
            get { return _message; }
        }

        public void NotifyValidationError(BaseBindable sender, string propertyName)
        {
            sender.AddPropertyError(propertyName, Message);
        }
    }
}
