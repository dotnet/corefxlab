﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace System.Text.JsonLab {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("System.Text.JsonLab.ExceptionStrings", typeof(ExceptionStrings).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Depth of {0} within an array is larger than max depth of {1}..
        /// </summary>
        internal static string ArrayDepthTooLarge {
            get {
                return ResourceManager.GetString("ArrayDepthTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We are within an object but observed an &apos;]&apos;..
        /// </summary>
        internal static string ArrayEndWithinObject {
            get {
                return ResourceManager.GetString("ArrayEndWithinObject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while reading the json..
        /// </summary>
        internal static string Default {
            get {
                return ResourceManager.GetString("Default", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mismatched number of start/end objects or arrays. Depth is {0} but must be greater than 0..
        /// </summary>
        internal static string DepthMustBePositive {
            get {
                return ResourceManager.GetString("DepthMustBePositive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected a &apos;&quot;&apos; to indicate end of string, but instead reached end of data..
        /// </summary>
        internal static string EndOfStringNotFound {
            get {
                return ResourceManager.GetString("EndOfStringNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid number. Last character read: &apos;{0}&apos;. Expected a digit (&apos;0&apos;-&apos;9&apos;)..
        /// </summary>
        internal static string ExpectedDigitNotFound {
            get {
                return ResourceManager.GetString("ExpectedDigitNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid number. Last character read: &apos;{0}&apos;. Expected a digit (&apos;0&apos;-&apos;9&apos;) but reached end of data..
        /// </summary>
        internal static string ExpectedDigitNotFoundEndOfData {
            get {
                return ResourceManager.GetString("ExpectedDigitNotFoundEndOfData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected end of json after a single value but additional data found. Last character read: &apos;{0}&apos;..
        /// </summary>
        internal static string ExpectedEndAfterSingleJson {
            get {
                return ResourceManager.GetString("ExpectedEndAfterSingleJson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid end of number. Last character read: &apos;{0}&apos;. Expected a delimiter..
        /// </summary>
        internal static string ExpectedEndOfDigitNotFound {
            get {
                return ResourceManager.GetString("ExpectedEndOfDigitNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected a &apos;false&apos; value, but instead got &apos;{0}&apos;..
        /// </summary>
        internal static string ExpectedFalse {
            get {
                return ResourceManager.GetString("ExpectedFalse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid number. Last character read: &apos;{0}&apos;. Expected &apos;.&apos; or &apos;E&apos; or &apos;e&apos;..
        /// </summary>
        internal static string ExpectedNextDigitComponentNotFound {
            get {
                return ResourceManager.GetString("ExpectedNextDigitComponentNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid number. Last character read: &apos;{0}&apos;. Expected &apos;E&apos; or &apos;e&apos;..
        /// </summary>
        internal static string ExpectedNextDigitEValueNotFound {
            get {
                return ResourceManager.GetString("ExpectedNextDigitEValueNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected a &apos;null&apos; value, but instead got &apos;{0}&apos;..
        /// </summary>
        internal static string ExpectedNull {
            get {
                return ResourceManager.GetString("ExpectedNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected a &apos;:&apos; following the property name, but instead got &apos;{0}&apos;..
        /// </summary>
        internal static string ExpectedSeparaterAfterPropertyNameNotFound {
            get {
                return ResourceManager.GetString("ExpectedSeparaterAfterPropertyNameNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected &apos;&quot;&apos; for start of property name. Instead reached &apos;{0}&apos;..
        /// </summary>
        internal static string ExpectedStartOfPropertyNotFound {
            get {
                return ResourceManager.GetString("ExpectedStartOfPropertyNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected a start of a property name or value after &apos;,&apos;, but reached end of data instead..
        /// </summary>
        internal static string ExpectedStartOfPropertyOrValueNotFound {
            get {
                return ResourceManager.GetString("ExpectedStartOfPropertyOrValueNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected start of a value, but instead got &apos;{0}&apos;..
        /// </summary>
        internal static string ExpectedStartOfValueNotFound {
            get {
                return ResourceManager.GetString("ExpectedStartOfValueNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected a &apos;true&apos; value, but instead got &apos;{0}&apos;..
        /// </summary>
        internal static string ExpectedTrue {
            get {
                return ResourceManager.GetString("ExpectedTrue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected a value following the property, but instead reached end of data..
        /// </summary>
        internal static string ExpectedValueAfterPropertyNameNotFound {
            get {
                return ResourceManager.GetString("ExpectedValueAfterPropertyNameNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected either &apos;,&apos;, &apos;}}&apos;, or &apos;]&apos;, but instead got &apos;{0}&apos;..
        /// </summary>
        internal static string FoundInvalidCharacter {
            get {
                return ResourceManager.GetString("FoundInvalidCharacter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected valid end of json payload with TokenType either EndArray or EndObject. Current token type is &apos;{0}&apos;..
        /// </summary>
        internal static string InvalidEndOfJson {
            get {
                return ResourceManager.GetString("InvalidEndOfJson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Depth of {0} within an object is larger than max depth of {1}..
        /// </summary>
        internal static string ObjectDepthTooLarge {
            get {
                return ResourceManager.GetString("ObjectDepthTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We are within an array but observed an &apos;}}&apos;..
        /// </summary>
        internal static string ObjectEndWithinArray {
            get {
                return ResourceManager.GetString("ObjectEndWithinArray", resourceCulture);
            }
        }
    }
}
