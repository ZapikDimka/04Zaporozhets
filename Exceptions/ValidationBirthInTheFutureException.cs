using System;

namespace _04Zaporozhets
{
    internal class ValidationBirthInTheFutureException : Exception
    {
        private string _message;

        public ValidationBirthInTheFutureException(string message)
        {
            this._message = message;
        }
        public ValidationBirthInTheFutureException(string message, DateOnly dateTime) : this("You are tooo young") { }

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
