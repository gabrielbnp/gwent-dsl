public interface Stmt
{
    public void accept(StmtVisitor visitor);
}

public interface StmtVisitor
{
    public void visitPrint(stmtPrint stmt);
}

public class stmtPrint : Stmt // print expression;
{
    public Expr expression;

    public stmtPrint(Expr expression)
    {
        this.expression = expression;
    }

    public void accept(StmtVisitor visitor)
    {
        visitor.visitPrint(this);
    }
}