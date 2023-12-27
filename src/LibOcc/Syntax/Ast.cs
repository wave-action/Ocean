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

public class IdentifierExpression(Token token) : Expression(token)
{
    public string Value { get; init; } = token.Value;
}

public class IntegerExpression(Token token) : Expression(token)
{
    public string Value { get; init; } = token.Value;
}
