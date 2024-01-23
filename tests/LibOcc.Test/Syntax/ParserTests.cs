using LibOcc.Syntax;
using LibOcc.Text;

namespace LibOcc.Test.Syntax;

public class ParserTests
{
    [Fact]
    public void ParseLiterals()
    {
        var commonPosition = new LinearPosition(0, 0);
        
        Token[] tokens =
        [
            new Token("a", TokenKind.Identifier, commonPosition),
            new Token("1", TokenKind.Int, commonPosition),
            new Token(TokenKind.EndOfFile, commonPosition),
        ];

        var parser = new Parser(tokens);

        var result = parser.ParseProgram();
        result.IsErr().Should().Be(false);

        var expected = new Program(
        [
            new ExpressionStatement(new IdentifierExpression(tokens[0])),
            new ExpressionStatement(new IntegerExpression(tokens[1])),
        ]);
        
        result.Value!.Statements
            .Should().HaveCount(2);

        result.Value!.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ParseDefStatement()
    {
        var commonP = new LinearPosition(0, 0);
        
        Token[] tokens =
        [
            new Token(TokenKind.LeftParen, commonP),
            new Token(TokenKind.Defn, commonP),
            new Token("one", TokenKind.Identifier, commonP),
            new Token(TokenKind.LeftBracket, commonP),
            new Token("a", TokenKind.Identifier, commonP),
            new Token("b", TokenKind.Identifier, commonP),
            new Token(TokenKind.RightBracket, commonP),
            new Token("1", TokenKind.Int, commonP),
            new Token(TokenKind.RightParen, commonP),
            new Token(TokenKind.EndOfFile, commonP),
        ];

        var parser = new Parser(tokens);
        var result = parser.ParseProgram();

        result.IsErr().Should().Be(false);
        result.Value!.Statements.Should().HaveCount(1);

        var expected = new DefnStatement(
            tokens[0],
            new IdentifierExpression(tokens[2]),
            [new IdentifierExpression(tokens[4]), new IdentifierExpression(tokens[5])],
            new IntegerExpression(tokens[7])
        );

        result.Value!.Statements[0].Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ParseEmptyDefStatement()
    {
        var commonP = new LinearPosition(0, 0);
        
        Token[] tokens =
        [
            new Token(TokenKind.LeftParen, commonP),
            new Token(TokenKind.Defn, commonP),
            new Token("empty", TokenKind.Identifier, commonP),
            new Token(TokenKind.LeftBracket, commonP),
            new Token(TokenKind.RightBracket, commonP),
            new Token(TokenKind.RightParen, commonP),
            new Token(TokenKind.EndOfFile, commonP),
        ];

        var parser = new Parser(tokens);
        var result = parser.ParseProgram();

        result.IsErr().Should().Be(false);
        result.Value!.Statements.Should().HaveCount(1);

        var expected = new DefnStatement(
            tokens[0],
            new IdentifierExpression(tokens[2]),
            [],
            null
        );

        result.Value!.Statements[0].Should().BeEquivalentTo(expected);
    }
}