using System.Collections;

namespace LibOcc.Syntax;

public class Lexer(string source) : IEnumerable<Token>
{
    private readonly LexerEnumerator _enumerator = new(source);

    public IEnumerator<Token> GetEnumerator() => _enumerator;
    IEnumerator IEnumerable.GetEnumerator() => _enumerator;
}