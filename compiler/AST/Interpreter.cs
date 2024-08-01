using System;
using static TokenType;

public class Interpreter : IExprVisitor, IStmtVisitor
{

    public void interpret(List<IStmt> statements)
    {
        foreach(IStmt stmt in statements)
        {
            execute(stmt);
        }
    }

    private void execute(IStmt stmt)
    {
        stmt.accept(this);
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
                return (long) left + (long) right;
            case MINUS:
                return (long) left - (long) right;
            case STAR:
                return (long) left * (long) right;
            case SLASH:
                return (long) left / (long) right;

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

    public void visit(stmtPrint stmt)
    {
        object value = evaluate( stmt.expr );
        Console.WriteLine(value.ToString());
    }

    #endregion Visitor

    #region Tools
    
    private object evaluate(IExpr expr)
    {
        return expr.accept(this);
    }

    private bool isTrue(object value)
    {
        if( (value is long) && ((long) value == 0) )
            return false;

        if( (value is string) && ((string) value == "") )
            return false;

        if( value is bool )
            return (bool) value;

        return true;
    }

    #endregion Tools
}