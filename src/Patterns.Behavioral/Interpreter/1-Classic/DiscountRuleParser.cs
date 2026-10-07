using System.Globalization;

namespace Patterns.Behavioral.Interpreter.Classic;

/// <summary>
/// Turns the text of a discount rule into a tree of <see cref="IRuleExpression"/>. A recursive descent parser:
/// one method per grammar rule. GoF leaves parsing out of the pattern; you always need one. Guide: §6.3.
/// <code>
/// rule      := condition ("AND" condition)*
/// condition := "total" (">" | ">=") number | "category" "=" "'" text "'"
/// </code>
/// </summary>
public static class DiscountRuleParser
{
    public static IRuleExpression Parse(string rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        var tokens = new Tokenizer(rule);

        var expression = Condition(tokens);
        while (tokens.Peek() is { } next && next.IsWord("AND"))
        {
            tokens.Take();
            expression = new AndExpression(expression, Condition(tokens));
        }
        if (tokens.Peek() is { } extra) { throw Unexpected(extra); }
        return expression;
    }

    private static IRuleExpression Condition(Tokenizer tokens)
    {
        var subject = tokens.Take();
        if (subject.IsWord("total"))
        {
            var comparison = tokens.Take();
            if (comparison.Text is not (">" or ">=")) { throw Unexpected(comparison); }
            var number = tokens.Take();
            if (number.Kind != TokenKind.Number
                || number.Text.EndsWith('.') // "5." is not a number of this language
                || !decimal.TryParse(number.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var amount))
            {
                throw Unexpected(number);
            }
            return new TotalGreaterThan(amount, OrEqual: comparison.Text == ">=");
        }
        if (subject.IsWord("category"))
        {
            var equals = tokens.Take();
            if (equals.Text != "=") { throw Unexpected(equals); }
            var name = tokens.Take();
            if (name.Kind != TokenKind.Quoted || name.Text.Length == 2) { throw Unexpected(name); } // '' names no category
            return new HasCategory(name.Text[1..^1]); // without the quotes
        }
        throw Unexpected(subject);
    }

    private static FormatException Unexpected(Token token) =>
        new(string.Create(CultureInfo.InvariantCulture, $"Unexpected '{token.Text}' at position {token.Position}."));

    private static FormatException EndOfRule() => new("Unexpected end of rule.");

    private enum TokenKind { Word, Number, Symbol, Quoted }

    /// <summary>A piece of the text; <c>Position</c> is the 0-based index of its first character.</summary>
    private sealed record Token(TokenKind Kind, string Text, int Position)
    {
        public bool IsWord(string word) =>
            Kind == TokenKind.Word && string.Equals(Text, word, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Reads tokens on demand, so the parser reports the first problem from the left
    /// ("price > 5!" complains about "price", not about "!").
    /// </summary>
    private sealed class Tokenizer(string text)
    {
        private int _position;
        private Token? _peeked;

        public Token? Peek() => _peeked ??= Read();

        public Token Take()
        {
            var token = Peek() ?? throw EndOfRule();
            _peeked = null;
            return token;
        }

        private Token? Read()
        {
            while (_position < text.Length && char.IsWhiteSpace(text[_position])) { _position++; }
            if (_position == text.Length) { return null; }

            var start = _position;
            var c = text[start];
            TokenKind kind;
            if (char.IsAsciiLetter(c))
            {
                while (_position < text.Length && char.IsAsciiLetter(text[_position])) { _position++; }
                kind = TokenKind.Word;
            }
            else if (char.IsAsciiDigit(c))
            {
                while (_position < text.Length && (char.IsAsciiDigit(text[_position]) || text[_position] == '.')) { _position++; }
                kind = TokenKind.Number;
            }
            else if (c == '>')
            {
                _position += _position + 1 < text.Length && text[_position + 1] == '=' ? 2 : 1;
                kind = TokenKind.Symbol;
            }
            else if (c == '=')
            {
                _position++;
                kind = TokenKind.Symbol;
            }
            else if (c == '\'')
            {
                var end = text.IndexOf('\'', start + 1);
                if (end < 0) { throw EndOfRule(); } // the closing quote never came
                _position = end + 1;
                kind = TokenKind.Quoted;
            }
            else
            {
                throw Unexpected(new Token(TokenKind.Symbol, c.ToString(), start));
            }
            return new Token(kind, text[start.._position], start);
        }
    }
}
