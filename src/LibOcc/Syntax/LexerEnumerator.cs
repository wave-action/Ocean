using System.Collections;
using System.Text;
using LibOcc.Text;

namespace LibOcc.Syntax;

public partial class LexerEnumerator : IEnumerator<Token>
{
    private Token _current = new Token(TokenKind.Invalid, new LinearPosition(0, 0));
    
    public bool MoveNext()
    {
        if (Current.Kind == TokenKind.EndOfFile)
            return false;

        _current = NextToken();
        return true;
    }

    public void Reset() => _pos = 0;

    public Token Current  => _current;
    object IEnumerator.Current => Current;

    public void Dispose() { }
}

// Lexer setup
public partial class LexerEnumerator(string source)
{
    private uint _pos;

    private char Ch => Peek(0);

    private void Next() => _pos += 1;

    private char Peek(uint offset)
    {
        var peekPos = _pos + offset;
        return peekPos < source.Length ? source[(int)peekPos] : '\0';
    }

    private (char, LinearPosition) ReadChar()
    {
        var currCh = Ch;
        var pos = new LinearPosition(_pos, _pos + 1);
        
        Next();
        return (currCh, pos);
    }

    private (string, LinearPosition) ReadWhile(Func<char, bool> condition)
    {
        var builder = new StringBuilder();
        var start = _pos;
        var end = _pos + 1;

        while (condition(Ch))
        {
            var (ch, pos) = ReadChar();
            builder.Append(ch);
            end = pos.End;
        }

        return (builder.ToString(), new LinearPosition(start, end));
    }
}

public partial class LexerEnumerator
{
    private Token NextToken()
    {
        return Ch switch
        {
            _ when LexerRules.IsWhitespaceChar(Ch) => TokenizeWhitespace(),
            _ when LexerRules.IsCommentInit(Ch) => TokenizeComment(),
            _ when LexerRules.IsIdInit(Ch) => TokenizeIdentifierOrKeyword(),
            _ when LexerRules.IsNumberInit(Ch) => TokenizeNumber(),
            _ => TokenizeSingleChar()
        };
    }

    private Token TokenizeWhitespace()
    {
        var (value, pos) = ReadWhile(LexerRules.IsWhitespaceChar);
        return new Token(value, TokenKind.Whitespace, pos);
    }

    private Token TokenizeComment()
    {
        var (value, pos) = ReadWhile(
            c => !LexerRules.IsNewLineChar(c) && c != '\0');
        return new Token(value, TokenKind.Comment, pos);
    }

    private Token TokenizeIdentifierOrKeyword()
    {
        var (value, pos) = ReadWhile(LexerRules.IsIdChar);
        var kind = TokenKindExtensions.LookupKeyword(value);
        return new Token(value, kind, pos);
    }

    private Token TokenizeNumber()
    {
        // TODO: Make this generic for all numbers
        var(value, pos) = ReadWhile(LexerRules.IsNumberChar);
        return new Token(value, TokenKind.Int, pos);
    }
    
    private Token TokenizeSingleChar()
    {
        var (ch, pos) = ReadChar();
        var kind = ch switch
        {
            '(' => TokenKind.LeftParen,
            ')' => TokenKind.RightParen,
            '[' => TokenKind.LeftBracket,
            ']' => TokenKind.RightBracket,
            '{' => TokenKind.LeftBrace,
            '}' => TokenKind.RightBrace,
            ',' => TokenKind.Comma,
            '`' => TokenKind.BackQuote,
            '\'' => TokenKind.Quote,
            '\0' => TokenKind.EndOfFile,
            
            _ => TokenKind.Invalid
        };

        return new Token(ch.ToString(), kind, pos);
    }
}