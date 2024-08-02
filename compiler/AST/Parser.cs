/*
    a program is a series of declarations
    program --> declaration* ;

    declaration     --> var_declaration
                    |   statement ;
    
    var_declaration --> ( "int" | "bool" | "str" ) IDENTIFIER ( "=" expression )? ";" ;

    statement       --> print_statement ;
    print_statement --> "print" expression ";" ;

    expression      --> comparison ;
    comparison      --> term ( ( "==" | "!=" | "<" | "<=" | ">" | ">=" ) term )* ;
    term            --> factor ( ( "+" | "-" ) factor )* ;
    factor          --> unary ( ( "*" | "/" ) unary )* ;
    unary           --> ( "-" | "!" ) unary
                    |   primary ;
    primary         --> "true" | "false" | NUMBER | STRING | IDENTIFIER | "(" expression ")" ;

*/

public class Parser
{
    private List<Token> tokens;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public List<Stmt> parse()
    {
        List<Stmt> declarations = new List<Stmt>();

        while(!isAtEnd)
        {
            Stmt decl = parseDeclaration();
            declarations.Add(decl);
        }

        return declarations;
    }

    private Stmt parseDeclaration()
    {
        return parseStatement();
    }

    private Stmt parseStatement()
    {
        if( check(TokenType.PRINT) )
        {
            advance();
            return parsePrint();
        }
        
        // report an error because only print stament (for now) are valid

        return null!;
    }

    private Stmt parsePrint()
    {
        Expr expression = parseExpression();

        // check for semicolon ";"
        advance();

        return new stmtPrint(expression);
    }

    private Expr parseExpression()
    {
        return parseComparison();
    }

    private Expr parseComparison()
    {
        Expr term = parseTerm();

        while( check( new TokenType[] {TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL, TokenType.GREATER, TokenType.GREATER_EQUAL } ) )
        {
            Token oper = currToken;
            advance();

            Expr term_ = parseTerm();

            term = new exprBinary(term, oper, term_);
        }

        return term;
    }

    private Expr parseTerm()
    {
        Expr factor = parseFactor();

        while( check( new TokenType[] {TokenType.PLUS, TokenType.MINUS} ) )
        {
            Token oper = currToken;
            advance();

            Expr factor_ = parseFactor();

            factor = new exprBinary(factor, oper, factor_);
        }

        return factor;
    }

    private Expr parseFactor()
    {
        Expr unary = parseUnary();

        while( check( new TokenType[] {TokenType.STAR, TokenType.SLASH} ) )
        {
            Token oper = currToken;
            advance();

            Expr unary_ = parseUnary();

            unary = new exprBinary(unary,oper, unary_);
        }

        return unary;
    }

    private Expr parseUnary()
    {
        if( check( new TokenType[] {TokenType.MINUS, TokenType.BANG} ) )
        {
            Token oper = currToken;
            advance();

            Expr unary = parseUnary();

            return new exprUnary(oper, unary);
        }

        return parsePrimary();
    }

    private Expr parsePrimary()
    {
        if( check(TokenType.TRUE) )
        {
            advance();
            return new exprLiteral(true);
        }
        else if( check(TokenType.FALSE) )
        {
            advance();
            return new exprLiteral(false);
        }
        else if( check(new TokenType[] {TokenType.NUMBER, TokenType.STRING} ) )
        {
            object value = currToken.literal!;
            advance();
            return new exprLiteral(value)!;
        }
        else if( check(TokenType.LEFT_PAR) )
        {
            advance();
            Expr expression = parseExpression();

            // check for the right parentheses.

            advance();

            return new exprGrouping(expression);
        }
        
        return null!;
    }

    #region Tools

        private int current = 0;
        
        private bool isAtEnd
        {
            get { return ( current >= tokens.Count() ? true : false ); }
        }

        private void advance()
        {
            if(!isAtEnd)
                current++;
        }

        private Token currToken
        {
            get { return tokens[current]; }
        }

        private bool check(TokenType type)
        {
            return currToken.type == type;
        }

        private bool check(TokenType[] types)
        {
            foreach(TokenType t in types)
            {
                if( check(t) )
                    return true;
            }

            return false;
        }

    #endregion Tools
}