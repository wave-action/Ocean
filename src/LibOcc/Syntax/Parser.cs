using LibOcc.Diagnostics;

namespace LibOcc.Syntax;

public partial class Parser(IEnumerable<Token> tokens)
{
    private List<ParserError> _errors = [];
    
    private readonly Token[] _tokens = tokens.ToArray();
    private int _pos;
    
    private Token Current => _pos <= _tokens.Length ? _tokens[_pos] : _tokens.Last();
    
    private void Next() => _pos += 1;

    private Token ReadToken()
    {
        var token = Current;
        Next();

        return token;
    }

    private Token ExpectToken(TokenKind expectKind)
    {
        var token = ReadToken();
        if (token.Kind == expectKind) return token;
        
        _errors.Add(token.Kind == TokenKind.EndOfFile 
            ? new EarlyEof(expectKind)
            : new UnexpectedToken(token, expectKind));
        
        return new Token(expectKind, token.Position);
    }
}

public partial class Parser
{
    public Result<int, ParserError> ParseProgram()
    {
        return Result<int, ParserError>.Ok(1);
    }
}