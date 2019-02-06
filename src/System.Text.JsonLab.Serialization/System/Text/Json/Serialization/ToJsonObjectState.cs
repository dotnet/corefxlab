// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;

namespace System.Text.Json.Serialization
{
    internal struct ToJsonObjectState
    {
        // The object (POCO or IEnumerable) that is being populated
        public object CurrentValue;
        public JsonClassInfo ClassInfo;

        public IEnumerator Enumerator;

        // Current property values
        public JsonPropertyInfo PropertyInfo;

        // The current property.
        public int PropertyIndex;

        // Has the Start tag been written
        public bool StartObjectWritten;

        // Has the Start tag been written
        public bool StartArrayWritten;

        public void Reset()
        {
            CurrentValue = null;
            ClassInfo = null;
            StartObjectWritten = false;
            PropertyIndex = 0;
            Enumerator = null;
            ResetProperty();
        }

        public void ResetProperty()
        {
            PropertyInfo = null;
            StartArrayWritten = false;
        }
    }
}
