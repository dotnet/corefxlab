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

        public bool Contains(string optionName)
        {
            for (int i = 0; i < _options.Length; i++)
            {
                var candidate = _options[i];
                if (candidate.StartsWith(optionName)) return true;
            }
            return false;
        }

        public ReadOnlySpan<char> this[string optionName] => Get(optionName).Span;
        
        public ReadOnlyMemory<char> Get(string optionName)
        {
            if (optionName.Length < 1) throw new ArgumentOutOfRangeException(nameof(optionName));

            for (int i = 0; i < _options.Length; i++)
            {
                var candidate = _options[i];
                if (candidate.StartsWith(optionName))
                {
                    var option = candidate.AsReadOnlyMemory();
                    return option.Slice(optionName.Length);
                }
            }
            return ReadOnlyMemory<char>.Empty;
        }
    }
}
