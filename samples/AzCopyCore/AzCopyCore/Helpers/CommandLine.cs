// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.CommandLine
{
    // TODO (pri 3): Should I use the command line library?
    class CommandLine
    {
        readonly string[] _options;

        public CommandLine(string[] options)
        {
            _options = options;
        }

        public bool Contains(ReadOnlySpan<char> optionName)
        {
            for (int i = 0; i < _options.Length; i++)
            {
                if (_options[i].AsSpan().StartsWith(optionName)) return true;
            }
            return false;
        }

        public ReadOnlySpan<char> this[string optionName] => GetSpan(optionName);

        public ReadOnlyMemory<char> Get(string optionName)
        {
            if (optionName.Length < 1) throw new ArgumentOutOfRangeException(nameof(optionName));

            for (int i = 0; i < _options.Length; i++)
            {
                string candidate = _options[i];
                if (candidate.StartsWith(optionName))
                {
                    return candidate.AsMemory().Slice(optionName.Length);
                }
            }
            return ReadOnlyMemory<char>.Empty;
        }

        public ReadOnlySpan<char> GetSpan(ReadOnlySpan<char> optionName)
        {
            if (optionName.Length < 1) throw new ArgumentOutOfRangeException(nameof(optionName));

            for (int i = 0; i < _options.Length; i++)
            {
                ReadOnlySpan<char> candidate = _options[i].AsSpan();
                if (candidate.StartsWith(optionName))
                {
                    return candidate.Slice(optionName.Length);
                }
            }
            return ReadOnlySpan<char>.Empty;
        }

        public string GetString(string optionName)
        {
            if (optionName.Length < 1) throw new ArgumentOutOfRangeException(nameof(optionName));

            for (int i = 0; i < _options.Length; i++)
            {
                string candidate = _options[i];
                if (candidate.StartsWith(optionName))
                {
                    return candidate.Substring(optionName.Length);
                }
            }
            return "";
        }
    }
}
