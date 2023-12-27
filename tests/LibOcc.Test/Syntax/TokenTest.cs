using LibOcc.Syntax;
using LibOcc.Text;

namespace LibOcc.Test.Syntax;

public class TokenTest
{
    [Fact]
    public void ShouldHaveTheSameAsTheExplicit()
    {
        var position = new LinearPosition(0, 1);
        
        Token[] withKindValues = 
        [
            new Token(TokenKind.Comma, position),
            new Token(TokenKind.Quote, position),
            new Token(TokenKind.LeftBrace, position),
            new Token(TokenKind.Identifier, position)
        ];

        Token[] expected =
        [
            new Token(",", TokenKind.Comma, position),
            new Token("'", TokenKind.Quote, position),
            new Token("{", TokenKind.LeftBrace, position),
            new Token("Identifier", TokenKind.Identifier, position)
        ];

        withKindValues.Should()
            .HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }
}