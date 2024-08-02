public enum TokenType
{
    // single-character tokens
    LEFT_PAR, RIGHT_PAR,
    PLUS, MINUS, STAR, SLASH,
    SEMICOLON,

    // one or two character tokens
    EQUAL, EQUAL_EQUAL,
    BANG, BANG_EQUAL,
    LESS, LESS_EQUAL,
    GREATER, GREATER_EQUAL,

    // literals
    IDENTIFIER, STRING, NUMBER,

    // keywords
    AND, OR, TRUE, FALSE, PRINT, INT, BOOL, STR
}

public class Token
{
    public TokenType type;
    public string lexeme;
    public object? literal;
    public int numLine;
    public int numColumn;

    public Token(TokenType type, string lexeme, object? literal, int numLine, int numColumn)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;

        this.numLine = numLine;
        this.numColumn = numColumn;
    }

    public override string ToString()
    {
        if(literal == null)
            return "[" + type + "] \'" + lexeme + "\'";

        return "[" + type + "] \'" + lexeme + "\' " + literal;  
    }
}