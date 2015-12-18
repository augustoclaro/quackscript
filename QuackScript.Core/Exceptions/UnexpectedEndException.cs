using System.Resources;

namespace QuackScript.Core.Exceptions
{
    public class UnexpectedEndException : QuackException
    {
        public UnexpectedEndException(int line)
            : base($"Unexpected end of code at line {line}.")
        {
        }
    }
}