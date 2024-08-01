public interface IExpr
{
    public object accept(IExprVisitor visitor);
}

#region Expression Visitor
public interface IExprVisitor
{
    public object visit(exprLiteral expr);
    public object visit(exprUnary expr);
    public object visit(exprBinary expr);
    public object visit(exprGrouping expr);
}
#endregion Expression Visitor

#region Expressions
public class exprLiteral : IExpr
{
    public object value;

    public exprLiteral(object value)
    {
        this.value = value;
    }

    public object accept(IExprVisitor visitor)
    {
        return visitor.visit(this);
    }

    public override string ToString()
    {
        if(value == null)
            return "";

        return (string) value;
    }
}

public class exprUnary : IExpr
{
    public Token oper;
    public IExpr expr;

    public exprUnary(Token oper, IExpr expr)
    {
        this.oper = oper;
        this.expr = expr;
    }

    public object accept(IExprVisitor visitor)
    {
        return visitor.visit(this);
    }
}

public class exprBinary : IExpr
{
    public IExpr left, right;
    public Token oper;

    public exprBinary(IExpr left, Token oper, IExpr right)
    {
        this.left = left;
        this.oper = oper;
        this.right = right;
    }

    public object accept(IExprVisitor visitor)
    {
        return visitor.visit(this);
    }
}

public class exprGrouping : IExpr
{
    public IExpr expr;

    public exprGrouping(IExpr expr)
    {
        this.expr = expr;
    }

    public object accept(IExprVisitor visitor)
    {
        return visitor.visit(this);
    }

    #endregion Expressions
}