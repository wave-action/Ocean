using System.Collections.Immutable;
using System.Linq.Expressions;
using LibOcc.Diagnostics;

namespace LibOcc.Syntax;

public partial class Parser
{
    private readonly Token[] _tokens;
    private List<ParserError> _errors = [];
    
    private Dictionary<TokenKind, Func<Expression>> _compoundExprFuncs;
    private Dictionary<TokenKind, Func<Expression>> _quotedExprFuncs;
    private Dictionary<TokenKind, Func<Expression>> _simpleExprFuncs;
    private Dictionary<TokenKind, Func<Statement>> _compoundStateFuncs;
    
    public Parser(IEnumerable<Token> tokens)
    {
        _tokens = tokens.Where(t => !t.Kind.IsSkippable()).ToArray();

        _compoundExprFuncs = new Dictionary<TokenKind, Func<Expression>>
        {
            [TokenKind.Lambda] = ParseLambdaExpression,
            [TokenKind.If] = ParseIfExpression,
            [TokenKind.Let] = ParseLetExpression,
            [TokenKind.Identifier] = ParseCallExpression
        };

        _quotedExprFuncs = new Dictionary<TokenKind, Func<Expression>>
        {
            [TokenKind.Identifier] = ParseSymbolExpression,
        };

        _simpleExprFuncs = new Dictionary<TokenKind, Func<Expression>>
        {
            [TokenKind.Identifier] = ParseIdentifierExpression,
            [TokenKind.Int] = ParseIntExpression,
        };
        
        _compoundStateFuncs = new Dictionary<TokenKind, Func<Statement>>
        {
            [TokenKind.Defn] = ParseDefnStatement,
        };
    }
    
    private int _pos;
    
    private Token Current => Peek(0);

    private Token Peek(int offset)
    {
        var pos = _pos + offset;
        return pos < _tokens.Length ? _tokens[pos] : _tokens.Last();
    }
    
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
    
    public Result<Program, ImmutableArray<ParserError>> ParseProgram()
    {
        List<Statement> statements = [];
        while (Current.Kind != TokenKind.EndOfFile)
        {
            var statement = ParseStatement();
            if (statement is not null)
                statements.Add(statement);
        }
        
        var program = new Program(statements);
        return _errors.Count > 0
            ? Result<Program, ImmutableArray<ParserError>>.Err(_errors.ToImmutableArray())
            : Result<Program, ImmutableArray<ParserError>>.Ok(program);
    }
}