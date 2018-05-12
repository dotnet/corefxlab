// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
