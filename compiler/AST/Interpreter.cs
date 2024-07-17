using static TokenType;

public interface IVisitor
{
    public object visit(exprLiteral expr);
    public object visit(exprUnary expr);
    public object visit(exprBinary expr);
    public object visit(exprGrouping expr);
}

public class Interpreter : IVisitor
{

    public void interpret(IExpr expr)
    {
        object value = evaluate(expr);

        System.Console.WriteLine(value);
    }

    #region Visitor

    public object visit(exprLiteral expr)
    {
        return expr.value;
    }

    public object visit(exprGrouping expr)
    {
        return evaluate(expr.expr);
    }

    public object visit(exprUnary expr)
    {
        object right = evaluate(expr.expr);

        switch( expr.oper.type )
        {
            case BANG:
                return !isTrue(right);
            case MINUS:
                return -(Int64) right;
        }

        return null!;
    }

    public object visit(exprBinary expr)
    {
        object left = evaluate(expr.left);
        object right = evaluate(expr.right);

        switch( expr.oper.type )
        {
            case PLUS:
                return (Int64) left + (Int64) right;
            case MINUS:
                return (Int64) left - (Int64) right;
            case STAR:
                return (Int64) left * (Int64) right;
            case SLASH:
                return (Int64) left / (Int64) right;

            case EQUAL_EQUAL:
                return left == right;
            case BANG_EQUAL:
                return left != right;
            case LESS:
                return (Int64) left < (Int64) right;
            case LESS_EQUAL:
                return (Int64) left <= (Int64) right;
            case GREATER:
                return (Int64) left > (Int64) right;
            case GREATER_EQUAL:
                return (Int64) left >= (Int64) right;
        }

        return null!;
    }

    #endregion Visitor

    #region Tools
    
    private object evaluate(IExpr expr)
    {
        return expr.accept(this);
    }

    private bool isTrue(object value)
    {
        if( (value is Int64) && ((Int64) value == 0) )
            return false;

        if( (value is string) && ((string) value == "") )
            return false;

        if( value is bool )
            return (bool) value;

        return true;
    }

    #endregion Tools
}