public interface IStmt
{
    public void accept(IStmtVisitor vistor);
}

#region Statment Visitor
public interface IStmtVisitor
{
    public void visit(stmtPrint expr);
}
#endregion Statment Visitor

#region Statements
public class stmtPrint : IStmt
{
    public IExpr expr;

    public stmtPrint(IExpr expr)
    {
        this.expr = expr;
    }

    public void accept(IStmtVisitor visitor)
    {
        visitor.visit(this);
    }

    #endregion Statements
}