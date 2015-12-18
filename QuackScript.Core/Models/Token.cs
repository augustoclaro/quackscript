using QuackScript.Core.Enums;

namespace QuackScript.Core.Models
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType tokenType, string value)
        {
            Type = tokenType;
            Value = value;
        }

        public bool Equals(TokenType tokenType, string value)
            => Type == tokenType && Value == value;
    }
}