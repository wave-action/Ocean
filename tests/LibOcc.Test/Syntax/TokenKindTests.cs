using LibOcc.Syntax;

namespace LibOcc.Test.Syntax;

public class TokenKindTests
{
    [Fact]
    public void ShouldGetAllKeywordKinds()
    {
        string[] literals =
        [
            "defn",
            "lambda",
            "let",
            "if"
        ];

        TokenKind[] expected =
        [
            TokenKind.Defn,
            TokenKind.Lambda,
            TokenKind.Let,
            TokenKind.If
        ];

        literals.Select(TokenKindExtensions.LookupKeyword)
            .Should().HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }

    [Fact]
    public void ShouldGetTheCorrectValuesFromKind()
    {
        TokenKind[] kinds =
        [
            TokenKind.Defn,
            TokenKind.Lambda,
            TokenKind.Let,
            TokenKind.If,
            TokenKind.LeftParen,
            TokenKind.RightParen,
            TokenKind.LeftBracket,
            TokenKind.RightBracket,
            TokenKind.LeftBrace,
            TokenKind.RightBrace,
            TokenKind.Comma,
            TokenKind.Quote,
            TokenKind.BackQuote,
            TokenKind.EndOfFile,
        ];

        string[] expected =
        [
            "defn",
            "lambda",
            "let",
            "if",
            "(",
            ")",
            "[",
            "]",
            "{",
            "}",
            ",",
            "'",
            "`",
            "\u0000",
        ];

        kinds.Select(kind => kind.Value())
            .Should().HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }
}