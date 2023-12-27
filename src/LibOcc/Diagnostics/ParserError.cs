using LibOcc.Syntax;

namespace LibOcc.Diagnostics;

public abstract class ParserError : Exception;

public class UnexpectedToken(Token found, TokenKind? expected = null) : ParserError
{
    public Token Found { get; init; } = found;
    public TokenKind? Expected { get; init; } = expected;
}

public class EarlyEof(TokenKind? expected = null) : ParserError
{
    public TokenKind? Expected { get; init; } = expected;
}
