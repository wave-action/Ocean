using LibOcc.Diagnostics;

namespace LibOcc.Syntax;

public partial class Parser
{
    private Expression? ParseExpression()
    {
        if (Current.Kind == TokenKind.LeftParen)
        {
            var nextToken = Peek(1);
            if (_compoundExprFuncs.TryGetValue(nextToken.Kind, out var compExprFunc))
            {
                return compExprFunc();
            }
        }

        if (_simpleExprFuncs.TryGetValue(Current.Kind, out var exprFunc))
        {
            return exprFunc();
        }
        
        var token = ReadToken();
        _errors.Add(new UnexpectedToken(token));
        return null;
    }

    private LetExpression ParseLetExpression()
    {
        throw new NotImplementedException();
    }

    private IfExpression ParseIfExpression()
    {
        throw new NotImplementedException();
    }
    
    private LambdaExpression ParseLambdaExpression()
    {
        throw new NotImplementedException();
    }

    private SymbolExpression ParseSymbolExpression()
    {
        throw new NotImplementedException();
    }

    private IntegerExpression ParseIntExpression()
    {
        var token = ExpectToken(TokenKind.Int);

        return new IntegerExpression(token);
    }

    private IdentifierExpression ParseIdentifierExpression()
    {
        var token = ExpectToken(TokenKind.Identifier);

        return new IdentifierExpression(token);
    }

    private CallExpression ParseCallExpression()
    {
        throw new NotImplementedException();
    }
}