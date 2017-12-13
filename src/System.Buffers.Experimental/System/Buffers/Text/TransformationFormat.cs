﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public readonly struct TransformationFormat
    {
        readonly StandardFormat _format;
        readonly IBufferTransformation _first;
        readonly IBufferTransformation[] _rest;

        public TransformationFormat(IBufferTransformation transformation)
        {
            _format = default;
            _first = transformation;
            _rest = null;
        }

        public TransformationFormat(params IBufferTransformation[] transformations)
        {
            _format = default;
            _first = null;
            _rest = transformations;
        }

        public StandardFormat Format { get => _format; }

        public bool TryTransform(Span<byte> buffer, ref int written)
        {
            int transformed;
            if (_first != null)
            {
                var status = _first.Transform(buffer, written, out transformed);
                if (status == OperationStatus.Done && _rest == null)
                {
                    written = transformed;
                    return true;
                }

                if (status == OperationStatus.DestinationTooSmall) return false;
                else if (status != OperationStatus.Done) Throw(status);
            }
            else transformed = written;

            if (_rest != null)
            {
                for (int i = 0; i < _rest.Length; i++)
                {
                    var status = _rest[i].Transform(buffer, transformed, out transformed);
                    if (status == OperationStatus.Done) continue;
                    else if (status == OperationStatus.DestinationTooSmall) return false;
                    else Throw(status);
                }
            }

            written = transformed;
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void Throw(OperationStatus status)
        {
            throw new InvalidOperationException(status.ToString());
        }
    }
}
