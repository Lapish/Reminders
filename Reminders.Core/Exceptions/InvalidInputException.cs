using System;

namespace Reminders.Core.Exceptions
{
    [Serializable]
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) :base(message)
        {
        }
    }
}