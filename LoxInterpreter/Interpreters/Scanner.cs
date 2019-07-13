using System.Collections.Generic;

namespace LoxInterpreter.Interpreters
{
    public class Scanner
    {
        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();

        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> ScanTokents()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }
            tokens.Add(new Token(TokenType.EOF,"",null,line));
            return tokens;
        }

        private bool IsAtEnd()
        {
            return current >= source.Length;
        }

        /// <summary>
        /// We move one character at a time via the source code
        /// and try to do a pattern maching with the character
        /// </summary>
        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    AddToken(TokenType.STAR);
                    break;
                case '!':
                    // ! or !=
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    // = or ==
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    // < or <=
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    //> or >= 
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GRETER);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        //comment with //
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        // or the / operator
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ': 
                case '\r':
                case '\t':
                    //ignore whitespaces
                    break;
                case '\n':
                    //new line
                    line++;
                    break;
                case '"':
                    String();
                    break;
                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else
                    {
                        Program.Error(line, $"Unexpected Character {c}");
                    }
                    break;
            }
        }

        /// <summary>
        /// numbers can be 12.34, .1234 or 1234
        /// </summary>
        private void Number()
        {
            //go one char at a time if we are getting numbers
            while (IsDigit(Peek())) Advance();

            //for fraction
            if(Peek()=='.' && IsDigit(PeekNext()))
            {
                Advance();
                while (IsDigit(Peek())) Advance();
            }
            AddToken(TokenType.NUMBER, double.Parse(source.Substring(start, current)));
        }

        private void String()
        {
            //go one char at a time till we find the closing " for a string literal
            while(Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++; //lox supports multiline text
                Advance();
            }

            //unterminated text
            if (IsAtEnd())
            {
                Program.Error(line, "Unterminated Text");
                return;
            }

            //the closing "
            Advance();

            //trim the saraounding quotes in a text literal
            var value = source.Substring(start + 1, current - 1);
            AddToken(TokenType.STRING, value);
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[current];
        }

        private char PeekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[current] != expected) return false;

            current++;
            return true;
        }

        private char Advance()
        {
            current++;
            return source[current - 1];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type,object literal)
        {
            var text = source.Substring(start, current);
            tokens.Add(new Token(type, text, literal, line));
        }

    }
}
