using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MobileDbs.Infrastructure.Helpers
{
    public class ExpressionExtension : ExpressionVisitor
    {
        private Expression before, after;
        public ExpressionExtension(Expression before, Expression after)
        {
            this.before = before;
            this.after = after;
        }
        public override Expression Visit(Expression node)
        {
            return node == before ? after : base.Visit(node);
        }
    }
}
