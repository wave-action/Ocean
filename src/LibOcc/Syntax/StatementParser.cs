namespace LibOcc.Syntax;

public partial class Parser
{
    private Statement? ParseStatement()
    {
        if (Current.Kind == TokenKind.LeftParen)
        {
            var nextToken = Peek(1);
            if (_compoundStateFuncs.TryGetValue(nextToken.Kind, out var statementFunc))
            {
                return statementFunc!();
            }
        }

        return ParseExpressionStatement();
    }

    private ExpressionStatement? ParseExpressionStatement()
    {
        var expression = ParseExpression();
        return expression is null 
            ? null 
            : new ExpressionStatement(expression);
    }

    private DefnStatement ParseDefnStatement()
    {
        var startToken = ExpectToken(TokenKind.LeftParen); // Skip (
        ExpectToken(TokenKind.Defn); // Skip defn
        
        var name = ExpectToken(TokenKind.Identifier);

        var argsTokens = new List<Token>();
        ExpectToken(TokenKind.LeftBracket); // Skip [
        while (Current.Kind != TokenKind.RightBracket && Current.Kind != TokenKind.EndOfFile)
        {
            var arg = ExpectToken(TokenKind.Identifier);
            argsTokens.Add(arg);
        }
        ExpectToken(TokenKind.RightBracket); // Skip ]

        var body = ParseExpression();

        ExpectToken(TokenKind.RightParen); // Skip )

        var nameIdentifier = new IdentifierExpression(name);
        var argsIdentifiers = argsTokens.Select(x => new IdentifierExpression(x)).ToList();

        return new DefnStatement(startToken, nameIdentifier, argsIdentifiers, body);
    }
}