// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Text.JsonLab
{
    internal class PropertyInfoNode
    {
        public PropertyInfoNode Next;
        public (byte[] encodedName, PropertyInfo propertyInfo) Value;
    }

    internal class PropertyInfoLinkedList
    {
        public int Count;

        public PropertyInfoNode Head { get; private set; }

        public PropertyInfoLinkedList()
        {
            Head = new PropertyInfoNode();
        }

        public void Add((byte[], PropertyInfo) data)
        {
            var newNode = new PropertyInfoNode
            {
                Value = data
            };

            Head.Next = newNode;
            Head = newNode;
            Count++;
        }
    }
}
