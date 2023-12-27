namespace LibOcc.Syntax;

public static class LexerRules
{
    public static bool IsNewLineChar(char c) =>
        c == '\u000D' || // \r
        c == '\u000A' || // \n
        c == '\u000B' || // vertical tab
        c == '\u000C' || // form feed

        // NEXT LINE from latin1
        c == '\u0085' ||

        // Dedicated whitespace characters from Unicode
        c == '\u2028' || // LINE SEPARATOR
        c == '\u2029'; // PARAGRAPH SEPARATOR

    public static bool IsWhitespaceChar(char c) =>
        IsNewLineChar(c) ||
        c == '\u0009' || // \t
        c == '\u0020' || // space
        c == '\u200E' || // LEFT-TO-RIGHT MARK
        c == '\u200F'; // RIGHT-TO-LEFT MARK

    public static bool IsCommentInit(char c) =>
        c == ';';

    public static bool IsIdInit(char c) =>
        char.IsLetter(c) ||
        c == '_' ||
        c == '<' ||
        c == '>' ||
        c == '=' ||
        c == '+' ||
        c == '-' ||
        c == '*' ||
        c == '/' ||
        c == '%' ||
        c == '?' ||
        c == '!';

    public static bool IsNumberInit(char c) =>
        char.IsDigit(c);

    public static bool IsNumberChar(char c) =>
        char.IsDigit(c);

    public static bool IsIdChar(char c) =>
        IsIdInit(c) || char.IsDigit(c);
}
