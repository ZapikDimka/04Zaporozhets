using System;
using System.Net.Mail; // Додано простір імен

namespace _04Zaporozhets
{
    internal class ValidationEmailException : Exception
    {
        private string _message;

        public ValidationEmailException(string message) : base(message)
        {
            this._message = message;
        }

        public ValidationEmailException(string message, string email) : this("email is no corrct")
        {
        }

        public override string Message
        {
            get { return _message; }
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public void NotifyValidationError(BaseBindable sender, string propertyName)
        {
            sender.AddPropertyError(propertyName, Message);
        }
    }
}
