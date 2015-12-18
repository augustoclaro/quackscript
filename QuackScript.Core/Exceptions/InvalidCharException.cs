using System.Resources;

namespace QuackScript.Core.Exceptions
{
    public class InvalidCharException : QuackException
    {
        public InvalidCharException(char ch, int line)
            : base($"Unexpected character '{ch}' at line {line}.")
        {
        }
        public InvalidCharException(char ch, string code, int line)
            : base($"Unexpected character '{ch}' after {code} at line {line}.")
        {
        }
    }
}