public interface Expr
{
    public object accept(ExprVisitor visitor);
}

public interface ExprVisitor
{
    public object visitBinary(exprBinary expr);
    public object visitGrouping(exprGrouping expr);
    public object visitLiteral(exprLiteral expr);
    public object visitUnary(exprUnary expr);
}

public class exprBinary : Expr // left oper right
{
    public Expr left;
    public Expr right;
    public Token oper;

    public exprBinary(Expr left, Token oper, Expr right)
    {
        this.left = left;
        this.right = right;
        this.oper = oper;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitBinary(this);
    }
}

public class exprGrouping : Expr // ( expression )
{
    public Expr expression;

    public exprGrouping(Expr expression)
    {
        this.expression = expression;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitGrouping(this);
    }
}

public class exprLiteral : Expr
{
    public object value;

    public exprLiteral(object value)
    {
        this.value = value!;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitLiteral(this);
    }
}

public class exprUnary : Expr // ( "!" | "-" ) expression
{
    public Token oper;
    public Expr expression;

    public exprUnary(Token oper, Expr expression)
    {
        this.oper = oper;
        this.expression = expression;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitUnary(this);
    }
}