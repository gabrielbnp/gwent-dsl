using static TokenType;

public class Lexer
{
    private string sourceCode;
    private int numLine;
    private List<Token> tokens = new List<Token>();

    public List<Token> getTokens
    {
        get { return tokens; }
    }

    public Lexer(string? sourceCode, int numLine)
    {
        this.sourceCode = "" + sourceCode;
        this.numLine = numLine;

        tokenizeCode();
    }

    // position in the code line
    private int start = 0;
    private int end = 0;

    private void tokenizeCode()
    {
        sourceCode += " ";

        while(isAtEnd == false)
        {
            string possibleToken = readToken();

            indentifyToken(possibleToken);

            start = end;
        }
    }

    private string readToken()
    {
        string token = "" + currChar;
        forward();

        if( isDigit(token[0]) )
        {
            while( (isAtEnd == false) && isDigit(currChar) )
            {
                token += currChar;
                forward();
            }
        }
        else if( isAlpha(token[0]) )
        {
            while( (isAtEnd == false) && isAlphaNumeric(currChar) )
            {
                token += currChar;
                forward();
            }
        }

        return token;
    }

    private void indentifyToken(string possibleToken)
    {
        switch(possibleToken)
        {
            case " ":
                return;
            case "\t":
                return;

            case "(":
                addToken(LEFT_PAR, "(");
                break;

            case ")":
                addToken(RIGHT_PAR, ")");
                break;

            case "+":
                addToken(PLUS, "+");
                break;

            case "-":
                addToken(MINUS, "-");
                break;

            case "*":
                addToken(STAR, "*");
                break;

            case ";":
                addToken(SEMICOLON, ";");
                break;

            case "=":
                if( currChar == '=' )
                {
                    addToken(EQUAL_EQUAL, "==");
                    forward();
                    break;
                }

                addToken(EQUAL, "=");
                break;

            case "!":
                if( currChar == '=' )
                {
                    addToken(BANG_EQUAL, "!=");
                    forward();
                    break;
                }

                addToken(BANG, "!");
                break;
            
            case "<":
                if( currChar == '=' )
                {
                    addToken(LESS_EQUAL, "<=");
                    forward();
                    break;
                }

                addToken(LESS, "<");
                break;
            
            case ">":
                if( currChar == '=' )
                {
                    addToken(GREATER_EQUAL, ">=");
                    forward();
                    break;
                }

                addToken(GREATER, ">");
                break;

            case "&":
                if( currChar == '&' )
                {
                    addToken(AND, "&&");
                    forward();
                    break;
                }

                Error.error(numLine, start, "Expected '&' character.");
                break;

            case "|":
                if( currChar == '|' )
                {
                    addToken(OR, "|");
                    forward();
                    break;
                }

                Error.error(numLine, start, "Expected '|' character.");
                break;

            case "/":
                if(currChar != '/')
                {
                    addToken(SLASH, "/");
                    break;
                }

                // it is a comment, so we will ignore the whole line
                commentDetected();
                break;

            case "\"": // it is a string
                stringDetected();
                break;

            default:
                if( isDigit(possibleToken[0]) )
                {
                    addToken(NUMBER, possibleToken, long.Parse(possibleToken) );
                    break;
                }
                else if( isAlpha(possibleToken[0]) )
                {
                    if( isKeyword(possibleToken) == false )
                        addToken(IDENTIFIER, possibleToken);

                    break;
                }

                Error.error(numLine, start, "Unexpected character.");
                break;
        }
    }

    #region Tools
    private bool isAtEnd
    {
        get
        {
            if(end > sourceCode.Length - 2)
                return true;

            return false;
        }
    }

    private char currChar
    {
        get { return sourceCode[end]; }
    }

    private void forward()
    {
        end++;
    }

    private void ignoreLine()
    {
        end = sourceCode.Length;
    }

    private void commentDetected()
    {
        ignoreLine();
    }

    private void stringDetected()
    {
        while( (isAtEnd == false) && (currChar != '\"') )
                forward();

        if( isAtEnd == false )
        {
            string str = sourceCode.Substring(start + 1, end - start - 1);
                    
            addToken(STRING, str, str);
            forward();
        }
        else
        {
            Error.error(numLine, end - 1, "Unterminated string. Expected '\"' character.");
        }
    }

    private void addToken(TokenType type, string lexeme, object? literal)
    {
        tokens.Add( new Token(type, lexeme, literal, numLine, start) );
    }

    private void addToken(TokenType type, string lexeme)
    {
        addToken(type, lexeme, null);
    }

    private bool isDigit(char c)
    {
        return ('0' <= c) && (c <= '9');
    }

    private bool isAlpha(char c)
    {
        if(c == '_')
            return true;

        if(('a' <= c) && (c <= 'z'))
            return true;

        if(('A' <= c) && (c <= 'Z'))
            return true;

        return false;
    }

    private bool isAlphaNumeric(char c)
    {
        return isAlpha(c) || isDigit(c);
    }

    private bool isKeyword(string keyword)
    {
        switch(keyword)
        {
            case "true":
                addToken(TRUE, "true", true);
                return true;

            case "false":
                addToken(FALSE, "false", false);
                return true;

            case "print":
                addToken(PRINT, "print");
                return true;

            case "int":
                addToken(INT, "int");
                return true;

            case "bool":
                addToken(BOOL, "bool");
                return true;

            case "str":
                addToken(STR, "str");
                return true;

            default:
                return false;
        }
    }

    #endregion Tools
}