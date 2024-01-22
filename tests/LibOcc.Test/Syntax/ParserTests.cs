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
        var lexer = new Lexer("(defn one [a b] 1)");

        var parser = new Parser(lexer);
        var result = parser.ParseProgram();

        result.IsErr().Should().Be(false);

        result.Value!.Statements.Should().HaveCount(1);
    }
}