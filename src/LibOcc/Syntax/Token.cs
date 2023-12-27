using LibOcc.Text;

namespace LibOcc.Syntax;

public readonly struct Token(string value, TokenKind kind, LinearPosition pos)
{
    public TokenKind Kind { get; init; } = kind;
    public string Value { get; init; } = value;
    public LinearPosition Position { get; init; } = pos;

    public Token(TokenKind kind, LinearPosition pos) 
        : this(kind.Value(), kind, pos) { }
}