using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace IDFCR.Shared;

internal class LinkExpressionVisitor : ExpressionVisitor
{
    public string? MemberName { get; private set; }
    public MemberInfo? Member { get; private set; }
    [return: NotNullIfNotNull("node")]
    public override Expression? Visit(Expression? node)
    {
        return base.Visit(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        Member = node.Member;
        MemberName = node.Member.Name;
        return base.VisitMember(node);
    }
}
