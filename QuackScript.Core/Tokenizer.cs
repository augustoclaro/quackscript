using QuackScript.Core.Enums;
using QuackScript.Core.Exceptions;
using QuackScript.Core.Models;
using System;
using System.IO;
using System.Text;

namespace QuackScript.Core
{
    public class Tokenizer
    {
        private readonly TextReader _inputCode;
        private readonly StringBuilder _buffer = new StringBuilder();
        private char _currentChar;
        private int _currentCharCode;
        public int Line { get; private set; } = 1;
        private bool AtEndOfSource => _currentChar == '\0';

        public Tokenizer(TextReader inputCode)
        {
            if (inputCode == null) throw new ArgumentNullException(nameof(inputCode));
            _inputCode = inputCode;
        }

        private void ReadNextChar()
            => _currentChar = ((_currentCharCode = _inputCode.Read()) > 0 ? (char)_currentCharCode : '\0');

        private void SkipWhitespace()
        {
            while (char.IsWhiteSpace(_currentChar))
            {
                if (_currentChar.ToString() == "\n") Line++;
                ReadNextChar();
            }
        }

        private void StoreCurrentCharAndReadNext()
        {
            _buffer.Append(_currentChar);
            ReadNextChar();
        }

        private string ExtractStoredChars()
        {
            var bufferValue = _buffer.ToString();
            _buffer.Length = 0;
            return bufferValue;
        }

        private void CheckForUnexpectedEndOfSource()
        {
            if (AtEndOfSource)
                throw new UnexpectedEndException(Line);
        }

        private void ThrowInvalidCharException()
        {
            if (_buffer.Length == 0)
                throw new InvalidCharException(_currentChar, Line);
            throw new InvalidCharException(_currentChar, _buffer.ToString(), Line);
        }

        public Token ReadNextToken()
        {
            SkipWhitespace();

            if (AtEndOfSource)
                return null;

            if (char.IsLetter(_currentChar))
                return ReadWord();

            if (char.IsDigit(_currentChar))
                return ReadIntegerConstant();

            if (_currentChar == '\'')
                return ReadStringConstant();

            return ReadSymbol();
        }

        private Token ReadWord()
        {
            do StoreCurrentCharAndReadNext();
            while (char.IsLetterOrDigit(_currentChar));
            return new Token(TokenType.Word, ExtractStoredChars());
        }

        private Token ReadIntegerConstant()
        {
            do
                StoreCurrentCharAndReadNext();
            while (char.IsDigit(_currentChar));
            return new Token(TokenType.Number, ExtractStoredChars());
        }

        private Token ReadStringConstant()
        {
            ReadNextChar();
            while (!AtEndOfSource && _currentChar != '\'')
                StoreCurrentCharAndReadNext();
            CheckForUnexpectedEndOfSource();
            ReadNextChar();
            return new Token(TokenType.String, ExtractStoredChars());
        }

        private Token ReadSymbol()
        {
            switch (_currentChar)
            {
                case '*':
                case '/':
                case '(':
                case ')':
                case ',':
                case '=':
                case ';':
                case ':':
                case '~':
                case '.':
                    StoreCurrentCharAndReadNext();
                    return new Token(TokenType.Symbol, ExtractStoredChars());

                case '<':
                    StoreCurrentCharAndReadNext();
                    if (_currentChar == '>' || _currentChar == '=')
                        StoreCurrentCharAndReadNext();
                    return new Token(TokenType.Symbol, ExtractStoredChars());

                case '>':
                    StoreCurrentCharAndReadNext();
                    if (_currentChar == '=')
                        StoreCurrentCharAndReadNext();
                    return new Token(TokenType.Symbol, ExtractStoredChars());

                case '+':
                    StoreCurrentCharAndReadNext();
                    if (_currentChar == '+')
                        StoreCurrentCharAndReadNext();
                    return new Token(TokenType.Symbol, ExtractStoredChars());

                case '-':
                    StoreCurrentCharAndReadNext();
                    if (_currentChar == '-')
                        StoreCurrentCharAndReadNext();
                    return new Token(TokenType.Symbol, ExtractStoredChars());

                case '_':
                    return ReadWord();

                default:
                    CheckForUnexpectedEndOfSource();
                    ThrowInvalidCharException();
                    break;
            }

            return null;
        }
    }
}