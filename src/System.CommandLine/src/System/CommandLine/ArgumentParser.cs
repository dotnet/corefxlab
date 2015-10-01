// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

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
            var a = _tokens[tokenIndex];
            if (a.HasValue)
            {
                a.MarkMatched();
                argument = a.Value;
                return true;
            }

            tokenIndex++;
            if (tokenIndex < _tokens.Count)
            {
                var b = _tokens[tokenIndex];
                if (!b.IsOption)
                {
                    // This might be an argument or a separator
                    if (!b.IsSeparator)
                    {
                        // If we require a separator we can't consume this.
                        if (isFlag)
                            goto noResult;

                        b.MarkMatched();
                        argument = b.Name;
                        return true;
                    }

                    // Skip separator
                    b.MarkMatched();
                    tokenIndex++;

                    if (tokenIndex < _tokens.Count)
                    {
                        var c = _tokens[tokenIndex];
                        if (!c.IsOption)
                        {
                            c.MarkMatched();
                            argument = c.Name;
                            return true;
                        }
                    }
                }
            }

        noResult:
            argument = null;
            return false;
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