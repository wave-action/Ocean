using LibOcc.Syntax;
using LibOcc.Text;

namespace LibOcc.Test.Syntax;

public class LexerTests
{
    [Fact]
    private void ShouldTokenizeOneChar()
    {
        var input = "'";
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Token[] expected =
        [
            new Token(TokenKind.Quote, new LinearPosition(0, 1)),
            new Token(TokenKind.EndOfFile, new LinearPosition(1, 2)),
        ];

        tokens.Should()
            .HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }
    
    [Fact]
    private void ShouldTokenizeMultipleChars()
    {
        var input = "`(,[]{})";
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Token[] expected =
        [
            new Token(TokenKind.BackQuote, new LinearPosition(0, 1)),
            new Token(TokenKind.LeftParen, new LinearPosition(1, 2)),
            new Token(TokenKind.Comma, new LinearPosition(2, 3)),
            new Token(TokenKind.LeftBracket, new LinearPosition(3, 4)),
            new Token(TokenKind.RightBracket, new LinearPosition(4, 5)),
            new Token(TokenKind.LeftBrace, new LinearPosition(5, 6)),
            new Token(TokenKind.RightBrace, new LinearPosition(6, 7)),
            new Token(TokenKind.RightParen, new LinearPosition(7, 8)),
            new Token(TokenKind.EndOfFile, new LinearPosition(8, 9)),
        ];

        tokens.Should()
            .HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }

    [Fact]
    public void ShouldTokenizeIdentifier()
    {
        var input = "test-one";
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Token[] expected =
        [
            new Token(input, TokenKind.Identifier, new LinearPosition(0, 8)),
            new Token(TokenKind.EndOfFile, new LinearPosition(8, 9)),
        ];

        tokens.Should()
            .HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }
    
    [Fact]
    public void ShouldTokenizeInteger()
    {
        var input = "121";
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Token[] expected =
        [
            new Token(input, TokenKind.Int, new LinearPosition(0, 3)),
            new Token(TokenKind.EndOfFile, new LinearPosition(3, 4)),
        ];

        tokens.Should()
            .HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }
    
    [Fact]
    public void ShouldTokenizeComment()
    {
        var input = ";this is a comment";
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Token[] expected =
        [
            new Token(input, TokenKind.Comment, new LinearPosition(0, 18)),
            new Token(TokenKind.EndOfFile, new LinearPosition(18, 19)),
        ];

        tokens.Should()
            .HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }

    [Fact]
    public void ShouldTokenizeWhitespaces()
    {
        var input = "   ,   ";
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Token[] expected =
        [
            new Token("   ", TokenKind.Whitespace, new LinearPosition(0, 3)),
            new Token(TokenKind.Comma, new LinearPosition(3, 4)),
            new Token("   ", TokenKind.Whitespace, new LinearPosition(4, 7)),
            new Token(TokenKind.EndOfFile, new LinearPosition(7, 8)),
        ];

        tokens.Should()
            .HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }

    [Fact]
    public void ShouldIgnoreWhitespacesAndComments()
    {
        var input = """
                    ;; comment a
                    test-one
                    
                    test-two
                    ;; comment b
                    """;
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Tuple<string, TokenKind>[] expected =
        [
            new Tuple<string, TokenKind>("test-one", TokenKind.Identifier),
            new Tuple<string, TokenKind>("test-two", TokenKind.Identifier),
            new Tuple<string, TokenKind>("\0", TokenKind.EndOfFile)
        ];

        tokens.Where(x => x.Kind != TokenKind.Whitespace && x.Kind != TokenKind.Comment)
            .Select(x => new Tuple<string, TokenKind>(x.Value, x.Kind))
            .Should().HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }
    
    [Fact]
    public void ShouldTokenizeExpressionWithIdentifiersAndBraces()
    {
        var input = "(x y { w v })";
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Tuple<string, TokenKind>[] expected =
        [
            new Tuple<string, TokenKind>("(", TokenKind.LeftParen),
            new Tuple<string, TokenKind>("x", TokenKind.Identifier),
            new Tuple<string, TokenKind>("y", TokenKind.Identifier),
            new Tuple<string, TokenKind>("{", TokenKind.LeftBrace),
            new Tuple<string, TokenKind>("w", TokenKind.Identifier),
            new Tuple<string, TokenKind>("v", TokenKind.Identifier),
            new Tuple<string, TokenKind>("}", TokenKind.RightBrace),
            new Tuple<string, TokenKind>(")", TokenKind.RightParen),
            new Tuple<string, TokenKind>("\0", TokenKind.EndOfFile)
        ];

        tokens.Where(x => x.Kind != TokenKind.Whitespace && x.Kind != TokenKind.Comment)
            .Select(x => new Tuple<string, TokenKind>(x.Value, x.Kind))
            .Should().HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }
    
    [Fact]
    public void ShouldTokenizeKeywords()
    {
        var input = "(defn if lambda let)";
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Tuple<string, TokenKind>[] expected =
        [
            new Tuple<string, TokenKind>("(", TokenKind.LeftParen),
            new Tuple<string, TokenKind>("defn", TokenKind.Defn),
            new Tuple<string, TokenKind>("if", TokenKind.If),
            new Tuple<string, TokenKind>("lambda", TokenKind.Lambda),
            new Tuple<string, TokenKind>("let", TokenKind.Let),
            new Tuple<string, TokenKind>(")", TokenKind.RightParen),
            new Tuple<string, TokenKind>("\0", TokenKind.EndOfFile)
        ];

        tokens.Where(x => x.Kind != TokenKind.Whitespace && x.Kind != TokenKind.Comment)
            .Select(x => new Tuple<string, TokenKind>(x.Value, x.Kind))
            .Should().HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }
    
    [Fact]
    public void ShouldAHoleProgram()
    {
        var input = """
                    ;; file: test.oc
                    (defn sum [a b]
                        (+ a b))
                        
                    (if (= a b)
                        (println a)
                        (println b))
                    """;
        var lexer = new Lexer(input);
        var tokens = lexer.ToList();

        Tuple<string, TokenKind>[] expected =
        [
            new Tuple<string, TokenKind>("(", TokenKind.LeftParen),
            new Tuple<string, TokenKind>("defn", TokenKind.Defn),
            new Tuple<string, TokenKind>("sum", TokenKind.Identifier),
            new Tuple<string, TokenKind>("[", TokenKind.LeftBracket),
            new Tuple<string, TokenKind>("a", TokenKind.Identifier),
            new Tuple<string, TokenKind>("b", TokenKind.Identifier),
            new Tuple<string, TokenKind>("]", TokenKind.RightBracket),
            new Tuple<string, TokenKind>("(", TokenKind.LeftParen),
            new Tuple<string, TokenKind>("+", TokenKind.Identifier),
            new Tuple<string, TokenKind>("a", TokenKind.Identifier),
            new Tuple<string, TokenKind>("b", TokenKind.Identifier),
            new Tuple<string, TokenKind>(")", TokenKind.RightParen),
            new Tuple<string, TokenKind>(")", TokenKind.RightParen),
            new Tuple<string, TokenKind>("(", TokenKind.LeftParen),
            new Tuple<string, TokenKind>("if", TokenKind.If),
            new Tuple<string, TokenKind>("(", TokenKind.LeftParen),
            new Tuple<string, TokenKind>("=", TokenKind.Identifier),
            new Tuple<string, TokenKind>("a", TokenKind.Identifier),
            new Tuple<string, TokenKind>("b", TokenKind.Identifier),
            new Tuple<string, TokenKind>(")", TokenKind.RightParen),
            new Tuple<string, TokenKind>("(", TokenKind.LeftParen),
            new Tuple<string, TokenKind>("println", TokenKind.Identifier),
            new Tuple<string, TokenKind>("a", TokenKind.Identifier),
            new Tuple<string, TokenKind>(")", TokenKind.RightParen),
            new Tuple<string, TokenKind>("(", TokenKind.LeftParen),
            new Tuple<string, TokenKind>("println", TokenKind.Identifier),
            new Tuple<string, TokenKind>("b", TokenKind.Identifier),
            new Tuple<string, TokenKind>(")", TokenKind.RightParen),
            new Tuple<string, TokenKind>(")", TokenKind.RightParen),
            new Tuple<string, TokenKind>("\0", TokenKind.EndOfFile)
        ];

        tokens.Where(x => x.Kind != TokenKind.Whitespace && x.Kind != TokenKind.Comment)
            .Select(x => new Tuple<string, TokenKind>(x.Value, x.Kind))
            .Should().HaveCount(expected.Length)
            .And.ContainInOrder(expected);
    }
}