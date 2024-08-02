public class Interpreter : ExprVisitor, StmtVisitor
{
    private List<Stmt> declarations;

    public Interpreter(List<Stmt> declarations)
    {
        this.declarations = declarations;
    }

    public void interpret()
    {
        foreach(Stmt stmt in declarations)
            executeStmt(stmt);
    }

    #region Expr Visitor

        public object visitBinary(exprBinary expression)
        {
            Token oper = expression.oper;
            object left = evaluateExpr(expression.left);
            object right = evaluateExpr(expression.right);

            switch(oper.type)
            {
                case TokenType.PLUS:
                case TokenType.MINUS:
                case TokenType.STAR:
                case TokenType.SLASH:
                    return arithmetic(left, oper.type, right);

                case TokenType.EQUAL_EQUAL:
                case TokenType.BANG_EQUAL:
                    return equality(left, oper.type, right);

                case TokenType.LESS:
                case TokenType.LESS_EQUAL:
                case TokenType.GREATER:
                case TokenType.GREATER_EQUAL:
                    return comparison(left, oper.type, right);

                default:
                    return null!;
            }
        }

        public object visitGrouping(exprGrouping group)
        {
            return evaluateExpr(group.expression);
        }

        public object visitLiteral(exprLiteral literal)
        {
            return literal.value;
        }

        public object visitUnary(exprUnary unary)
        {
            object value = evaluateExpr(unary.expression);

            if(unary.oper.type == TokenType.MINUS)
                value = -(long) value;
            else if(unary.oper.type == TokenType.BANG)
                value = !isTrue(value);

            return value;
        }

    #endregion

    #region Stmt Visitor

        public void visitPrint(stmtPrint print)
        {
            object value = evaluateExpr(print.expression);
            System.Console.WriteLine(value);
        }

    #endregion

    #region Tools

        private object evaluateExpr(Expr expr)
        {
            return expr.accept(this);
        }

        private void executeStmt(Stmt stmt)
        {
            stmt.accept(this);
        }

        private long arithmetic(object left, TokenType operType, object right)
        {
            switch(operType)
            {
                case TokenType.PLUS:
                    return (long) left + (long) right;

                case TokenType.MINUS:
                    return (long) left - (long) right;

                case TokenType.STAR:
                    return (long) left * (long) right;

                case TokenType.SLASH: // what happend if right_ is zero?
                    return (long) left / (long) right;

                default:
                    return 0; // this line is actually unreachable
            }
        }

        private bool equality(object left, TokenType operType, object right)
        {
            switch(operType)
            {
                case TokenType.EQUAL_EQUAL:
                    return isEqual(left, right);
                case TokenType.BANG_EQUAL:
                    return !isEqual(left, right);

                default:
                    return false; // unreacheable
            }
        }

        private bool comparison(object left, TokenType operType, object right)
        {
            switch(operType)
            {
                case TokenType.LESS:
                    return isLess(left, right);
                case TokenType.LESS_EQUAL:
                    return isLessEqual(left, right);
                case TokenType.GREATER:
                    return !isLess(left, right);
                case TokenType.GREATER_EQUAL:
                    return !isLessEqual(left, right);

                default:
                    return false; // actually unreachable
            }
        }

        private bool isEqual(object left, object right)
        {
            return left == right;
        }

        private bool isLess(object left, object right)
        {
            if(left is string)
            {
                string left_ = (string) left;
                string right_ = (string) right;

                int n = System.Math.Min(left_.Length, right_.Length);

                for(int i = 0; i < n; i++)
                {
                    if(left_[i] < right_[i])
                        return true;
                    else if(left_[i] > right_[i])
                        return false;
                }

                return (left_.Length < right_.Length ? true : false);
            }

            if(left is bool)
                left = (long) ((bool) left ? 1 : 0);

            if(right is bool)
                right = (long) ((bool) right ? 1 : 0);

            return (long) left < (long) right;
        }

        private bool isLessEqual(object left, object right)
        {
            return isLess(left, right) || isEqual(left, right);
        }

        private bool isTrue(object value)
        {
            if(value is bool)
                return (bool) value;

            if( (value is long) && ((long) value == 0) )
                return false;
            if( (value is string) && ((string) value == "") )
                return false;

            return true;
        }
    
    #endregion
}