public interface IVisitor
{
    public object visit(exprLiteral expr);
    public object visit(exprUnary expr);
    public object visit(exprBinary expr);
    public object visit(exprGrouping expr);
}