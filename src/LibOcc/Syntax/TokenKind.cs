namespace LibOcc.Syntax;

public enum TokenKind
{
    // Internal
    Invalid,
    EndOfFile,
    
    // Keywords
    Defn,    // defn
    Lambda,  // lambda
    Let,     // let
    If,      // if
    
    // Ignore
    Comment,
    Whitespace,
    
    // Literals
    Identifier,
    Int,
    
    // Special Chars
    LeftParen,    // (
    RightParen,   // )
    LeftBracket,  // [
    RightBracket, // ]
    LeftBrace,    // {
    RightBrace,   // }
    
    Comma,     // ,
    Quote,     // '
    BackQuote, // `
}

public static class TokenKindExtensions
{
    public static TokenKind LookupKeyword(string value)
    {
        return value switch
        {
            "defn" => TokenKind.Defn,
            "lambda" => TokenKind.Lambda,
            "let" => TokenKind.Let,
            "if" => TokenKind.If,
            
            _ => TokenKind.Identifier
        };
    }

    public static string Value(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.Defn => "defn",
            TokenKind.Lambda => "lambda",
            TokenKind.Let => "let",
            TokenKind.If => "if",
            TokenKind.LeftParen => "(",
            TokenKind.RightParen => ")",
            TokenKind.LeftBracket => "[",
            TokenKind.RightBracket => "]",
            TokenKind.LeftBrace => "{",
            TokenKind.RightBrace => "}",
            TokenKind.Comma => ",",
            TokenKind.Quote => "'",
            TokenKind.BackQuote => "`",
            TokenKind.EndOfFile => "\u0000",
            
            _ => kind.ToString(),
        };
    }

    public static bool IsSkippable(this TokenKind kind)
    {
        switch (kind)
        {
            case TokenKind.Whitespace:
            case TokenKind.Comment:
                return true;
            
            default: return false;
        }
    }
}