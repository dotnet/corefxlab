// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace System.CommandLine
{
    internal sealed class ArgumentParser
    {
        private readonly IReadOnlyList<ArgumentToken> _tokens;

        public ArgumentParser(IEnumerable<string> arguments)
            : this(arguments, null)
        {
        }

        public ArgumentParser(IEnumerable<string> arguments, Func<string, IEnumerable<string>> responseFileReader)
        {
            if (arguments == null)
                throw new ArgumentNullException("arguments");

            _tokens = ArgumentLexer.Lex(arguments, responseFileReader);
        }

        public bool TryParseCommand(string name)
        {
            var token = _tokens.FirstOrDefault();

            if (token == null || token.IsOption || token.IsSeparator)
                return false;

            if (!string.Equals(token.Name, name, StringComparison.Ordinal))
                return false;

            token.MarkMatched();
            return true;
        }

        public bool TryParseOption<T>(string diagnosticName, IReadOnlyCollection<string> names, Func<string, T> valueConverter, out T value)
        {
            IReadOnlyList<T> values;
            if (!TryParseOptionList(diagnosticName, names, valueConverter, out values))
            {
                value = default(T);
                return false;
            }

            // Please note that we don't verify that the option is only specified once.
            // It's tradition on Unix to allow single options to occur more than once,
            // with 'last one wins' semantics. This simplifies scripting because you
            // can easily combine arguments.

            value = values.Last();
            return true;
        }

        public bool TryParseOptionList<T>(string diagnosticName, IReadOnlyCollection<string> names, Func<string, T> valueConverter, out IReadOnlyList<T> values)
        {
            var result = new List<T>();
            var tokenIndex = 0;
            var isFlag = typeof(T) == typeof(bool);

            while (tokenIndex < _tokens.Count)
            {
                if (TryParseOption(ref tokenIndex, names))
                {
                    string valueText;
                    if (TryParseOptionArgument(ref tokenIndex, isFlag, out valueText))
                    {
                        var value = ParseValue(diagnosticName, valueConverter, valueText);
                        result.Add(value);
                    }
                    else if (isFlag)
                    {
                        var value = (T)(object)true;
                        result.Add(value);
                    }
                    else
                    {
                        var message = string.Format(Strings.OptionRequiresValueFmt, diagnosticName);
                        throw new ArgumentSyntaxException(message);
                    }
                }

                tokenIndex++;
            }

            if (!result.Any())
            {
                values = null;
                return false;
            }

            values = result.ToArray();
            return true;
        }

        public bool TryParseParameter<T>(string diagnosticName, Func<string, T> valueConverter, out T value)
        {
            foreach (var token in _tokens)
            {
                if (token.IsMatched || token.IsOption || token.IsSeparator)
                    continue;

                token.MarkMatched();

                var valueText = token.Name;
                value = ParseValue(diagnosticName, valueConverter, valueText);
                return true;
            }

            value = default(T);
            return false;
        }

        public bool TryParseParameterList<T>(string diagnosticName, Func<string, T> valueConverter, out IReadOnlyList<T> values)
        {
            var result = new List<T>();

            T value;
            while (TryParseParameter(diagnosticName, valueConverter, out value))
            {
                result.Add(value);
            }

            if (!result.Any())
            {
                values = null;
                return false;
            }

            values = result.ToArray();
            return true;
        }

        private bool TryParseOption(ref int tokenIndex, IReadOnlyCollection<string> names)
        {
            while (tokenIndex < _tokens.Count)
            {
                var a = _tokens[tokenIndex];

                if (a.IsOption)
                {
                    if (names.Any(n => string.Equals(a.Name, n, StringComparison.Ordinal)))
                    {
                        a.MarkMatched();
                        return true;
                    }
                }

                tokenIndex++;
            }

            return false;
        }

        private bool TryParseOptionArgument(ref int tokenIndex, bool isFlag, out string argument)
        {
            argument = null;

            // Let's see whether the current token already has value, like "-o:value"

            var a = _tokens[tokenIndex];
            if (a.HasValue)
            {
                a.MarkMatched();
                argument = a.Value;
                return true;
            }

            // OK, we may need to have to advance one or two tokens. Since we don't know
            // up front, we'll take a look ahead.

            ArgumentToken lookahead;

            // So, do we have a token?

            if (!TryGetNextToken(tokenIndex, out lookahead))
                return false;

            // If it's an option, then it's not an argument and we're done.

            if (lookahead.IsOption)
                return false;

            // OK, the lookahead is either a separator or it's an argument.

            if (!lookahead.IsSeparator)
            {
                // If this is a flag, we need an explicit separator.
                // Since there is none, we don't consume this token.

                if (isFlag)
                    return false;

                lookahead.MarkMatched();
                argument = lookahead.Name;
                tokenIndex++;
                return true;
            }

            // Skip separator

            lookahead.MarkMatched();
            tokenIndex++;

            // See whether the next token is an argument.

            if (!TryGetNextToken(tokenIndex, out lookahead))
                return false;

            if (lookahead.IsOption || lookahead.IsSeparator)
                return false;

            lookahead.MarkMatched();
            argument = lookahead.Name;
            tokenIndex++;
            return true;
        }

        private bool TryGetNextToken(int tokenIndex, out ArgumentToken token)
        {
            if (++tokenIndex >= _tokens.Count)
            {
                token = null;
                return false;
            }

            token = _tokens[tokenIndex];
            return true;
        }

        private static T ParseValue<T>(string diagnosticName, Func<string, T> valueConverter, string valueText)
        {
            try
            {
                return valueConverter(valueText);
            }
            catch (FormatException ex)
            {
                var message = string.Format(Strings.CannotParseValueFmt, valueText, diagnosticName, ex.Message);
                throw new ArgumentSyntaxException(message);
            }
        }

        public string GetUnreadCommand()
        {
            return _tokens.Where(t => !t.IsOption && !t.IsSeparator).Select(t => t.Name).FirstOrDefault();
        }

        public IReadOnlyList<string> GetUnreadOptionNames()
        {
            return _tokens.Where(t => !t.IsMatched && t.IsOption).Select(t => t.Modifier + t.Name).ToArray();
        }

        public IReadOnlyList<string> GetUnreadParameters()
        {
            return _tokens.Where(t => !t.IsMatched && !t.IsOption).Select(t => t.ToString()).ToArray();
        }

        public IReadOnlyList<string> GetUnreadArguments()
        {
            return _tokens.Where(t => !t.IsMatched).Select(t => t.ToString()).ToArray();
        }
    }
}