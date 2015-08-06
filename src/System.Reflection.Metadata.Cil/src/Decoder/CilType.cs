using System.Text;

namespace System.Reflection.Metadata.Cil.Decoder
{
    public struct CilType
    {

        private StringBuilder _name;
        private bool _isValueType;
        private bool _isClassType;

        public CilType(string name, bool isValueType, bool isClassType)
        {
            _name = new StringBuilder();
            _name.Append(name);
            _isValueType = isValueType;
            _isClassType = isClassType;
        }

        /// <summary>
        /// Name of the type (int32, string, double, etc).
        /// </summary>
        public string Name
        {
            get
            {
                return _name.ToString();
            }
        }

        /// <summary>
        /// Value to indicate if the current type inherits from System.ValueType
        /// </summary>
        public bool IsValueType
        {
            get
            {
                return _isValueType;
            }
        }

        /// <summary>
        /// Value to indicate if the current type inherits from System.Object and it doesn't from System.ValueType so it is a "class" type.
        /// </summary>
        public bool IsClassType
        {
            get
            {
                return _isClassType;
            }
        }

        /// <summary>
        /// Method that appends a string to the type while building it.
        /// </summary>
        /// <param name="str">string to append.</param>
        public void Append(string str)
        {
            _name.Append(str);
        }

#if PREFIX
        /// <summary>
        /// Method that returns the type name.
        /// </summary>
        /// <returns>string representing the type name</returns>
        public override string ToString()
        {
            return string.Format("{0}{1}", (IsValueType ? "valuetype " : (IsClassType ? "class " : string.Empty)), Name);
        }

        /// <summary>
        /// Method that returns the type name with the option to show "class" or "valutype" prefix depending on it's inheritance.
        /// </summary>
        /// <param name="showBaseType">boolean that represents if you want the prefix to be shown.</param>
        /// <returns>string that represents the type.</returns>
        public string ToString(bool showBaseType)
        {
            return ToString();
        }
#else
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        /// Method that returns the type name with the option to show "class" or "valutype" prefix depending on it's inheritance.
        /// </summary>
        /// <param name="showBaseType">boolean that represents if you want the prefix to be shown.</param>
        /// <returns>string that represents the type.</returns>
        public string ToString(bool showBaseType)
        {
            if (showBaseType)
            {
                return string.Format("{0}{1}", (IsValueType ? "valuetype " : (IsClassType ? "class " : string.Empty)), Name);
            }
            return string.Format("{0}", Name);
        }
#endif
    }
}
