using static TokenType;

public class Parser
{
    private List<Token> tokens;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public List<IStmt> parse()
    {
        List<IStmt> statements = new List<IStmt>();

        while( isAtEnd == false )
        {
            IStmt stmt = statement();
            statements.Add( stmt );
        }

        return statements;
    }

    #region Expr Grammar

    private IExpr primary()
    {
        if( check(TRUE) )
        {
            forward();
            return new exprLiteral(true);
        }

        if( check(FALSE) )
        {
            forward();
            return new exprLiteral(false);
        }

        if( check( new TokenType[] {NUMBER, STRING} ) )
        {
            IExpr expr = new exprLiteral( currToken.literal! );
            forward();

            return expr;
        }

        if( check(LEFT_PAR) )
        {
            forward();
            IExpr expr = expression();

            if( !check(RIGHT_PAR) )
            {
                // throw an error
            }

            forward();

            return new exprGrouping(expr);

        }

        // at this point throws an error with message "Expect expression"
        return null!;
    }

    private IExpr unary()
    {
        if( check( new TokenType[] {BANG, MINUS} ) )
        {
            Token oper = currToken;
            forward();

            IExpr expr = unary();

            return new exprUnary(oper, expr);
        }

        return primary();
    }

    private IExpr factor()
    {
        IExpr left = unary();

        while( check( new TokenType[] {STAR, SLASH} ) )
        {
            Token oper = currToken;
            forward();

            IExpr right = unary();

            left = new exprBinary(left, oper, right);
        }

        return left;
    }

    private IExpr term()
    {
        IExpr left = factor();

        while( check( new TokenType[] {PLUS, MINUS} ) )
        {
            Token oper = currToken;
            forward();

            IExpr right = factor();

            left = new exprBinary(left, oper, right);
        }

        return left;
    }

    private IExpr comparison()
    {
        IExpr left = term();

        while( check( new TokenType[] {EQUAL_EQUAL, BANG_EQUAL, LESS, LESS_EQUAL, GREATER, GREATER_EQUAL} ) )
        {
            Token oper = currToken;
            forward();

            IExpr right = term();

            left = new exprBinary(left, oper, right);
        }

        return left;
    }

    private IExpr expression()
    {
        return comparison();
    }

    #endregion Expr Grammar

    #region Stmt Grammar

    private IStmt stmtPrint()
    {
        IExpr value = expression();

        if( check(SEMICOLON) )
        {
            forward();
        }
        else
        {
            // throw an error here
        }

        return new stmtPrint(value);
    }

    private IStmt statement()
    {
        if( check(PRINT) )
        {
            forward();
            return stmtPrint();
        }

        return null!;
    }

    #endregion Stmt Grammar

    #region Tools

    private int current = 0;

    private bool isAtEnd
    {
        get { return current >= tokens.Count; }
    }

    private Token currToken
    {
        get { return tokens[current]; }
    }

    private void forward()
    {
        if( isAtEnd == false )
            current++;
    }

    private bool check(TokenType type)
    {
        if( isAtEnd )
            return false;

        return currToken.type == type;
    }

    private bool check(TokenType[] types)
    {
        if( isAtEnd )
            return false;

        foreach(TokenType type in types)
        {
            if( currToken.type == type )
                return true;
        }

        return false;
    }

    #endregion Tools
}