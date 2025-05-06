using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

internal class LinkExpressionVisitor : ExpressionVisitor
{
    public string? MemberName { get; private set; }
    [return: NotNullIfNotNull("node")]
    public override Expression? Visit(Expression? node)
    {
        return base.Visit(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        MemberName = node.Member.Name;
        return base.VisitMember(node);
    }
}
