using System.Linq.Expressions;

namespace LibOcc.Syntax;

public abstract class AstNode(Token token)
{
    public Token Token { get; init; } = token;
}

public abstract class Statement(Token token) : AstNode(token);

public abstract class Expression(Token token) : AstNode(token);

public class LetExpression(Token token, List<(IdentifierExpression, Expression)> values)
    : Expression(token)
{
    public List<(IdentifierExpression, Expression)> ValuesPair { get; init; } = values;
}

public class IfExpression(Token token, Expression predicate, Expression then, Expression otherwise)
    : Expression(token)
{
    public Expression Predicate { get; init; } = predicate;
    public Expression Then { get; init; } = then;
    public Expression Otherwise { get; init; } = otherwise;
}

public class LambdaExpression(
    Token token,
    List<IdentifierExpression> args,
    Expression body) : Expression(token)
{
    public List<IdentifierExpression> Args { get; init; } = args;
    public Expression Body { get; init; } = body;
}

public class CallExpression(
    Token token,
    IdentifierExpression callee,
    List<Expression> args) : Expression(token)
{
    public IdentifierExpression Callee { get; init; } = callee;
    public List<Expression> Args { get; init; } = args;
}

public class DefnStatement(
    Token token,
    IdentifierExpression name,
    List<IdentifierExpression> args,
    Expression body) : Statement(token)
{
    public IdentifierExpression Name { get; init; } = name;
    public List<IdentifierExpression> Args { get; init; } = args;
    public Expression Body { get; init; } = body;
}

public class ExpressionStatement(Expression expression) : Statement(expression.Token)
{
    public Expression Expression { get; set; } = expression;
}

public class IdentifierExpression(Token token) : Expression(token)
{
    public string Value { get; init; } = token.Value;
}

public class IntegerExpression(Token token) : Expression(token)
{
    public string Value { get; init; } = token.Value;
}

public class SymbolExpression(Token quoteToken, Token valueToken) : Expression(quoteToken)
{
    public string Value { get; init; } = valueToken.Value;
}

public class Program(List<Statement> statements)
{
    public List<Statement> Statements { get; set; } = statements;
}