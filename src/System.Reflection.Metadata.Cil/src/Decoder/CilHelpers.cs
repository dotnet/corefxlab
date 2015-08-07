using System.Collections.Generic;
using System.Reflection.Emit;

namespace System.Reflection.Metadata.Cil.Decoder
{
    internal class CilDecoderHelpers
    {
        internal static CilDecoderHelpers Instance = new CilDecoderHelpers();

        internal readonly OpCode[] oneByteOpCodes;
        internal readonly OpCode[] twoByteOpCodes;
        
        /*Should add .ctor and .cctor are they commonly used as variable names? */
        private readonly HashSet<string> msilKeywords = new HashSet<string> { "#line", ".addon", ".assembly", ".class", ".corflags", ".custom", ".data", ".emitbyte", ".entrypoint",
            ".event",".export", ".field", ".file", ".fire", ".get", ".hash", ".imagebase", ".import", ".language", ".line", ".locale", ".localized", ".locals", ".manifestres", ".maxstack",
            ".method", ".module", ".mresource", ".namespace", ".other", ".override", ".pack", ".param", ".pdirect", ".permission", ".permissionset", ".property", ".publickey",
            ".publickeytoken", ".removeon", ".set", ".size", ".subsystem", ".try", ".ver", ".vtable", ".vtentry", ".vtfixup", ".zeroinit", "^THE_END^", "abstract", "add", "add.ovf",
            "add.ovf.un", "algorithm", "alignment", "and", "ansi", "any", "arglist", "array", "as", "assembly", "assert", "at", "auto", "autochar", "beforefieldinit", "beq", "beq.s",
            "bge", "bge.s", "bge.un", "bge.un.s", "bgt", "bgt.s", "bgt.un", "bgt.un.s", "ble", "ble.s", "ble.un", "ble.un.s", "blob", "blob_object", "blt", "blt.s", "blt.un", "blt.un.s",
            "bne.un", "bne.un.s", "bool", "box", "br", "br.s", "break", "brfalse", "brfalse.s", "brinst", "brinst.s", "brnull", "brnull.s", "brtrue", "brtrue.s", "brzero", "brzero.s",
            "bstr", "bytearray", "byvalstr", "call", "calli", "callmostderived", "callvirt", "carray", "castclass", "catch", "cdecl", "ceq", "cf", "cgt", "cgt.un", "char", "cil",
            "ckfinite", "class", "clsid", "clt", "clt.un", "const", "constrained.", "conv.i", "conv.i1", "conv.i2", "conv.i4", "conv.i8", "conv.ovf.i", "conv.ovf.i.un", "conv.ovf.i1",
            "conv.ovf.i1.un", "conv.ovf.i2", "conv.ovf.i2.un", "conv.ovf.i4", "conv.ovf.i4.un", "conv.ovf.i8", "conv.ovf.i8.un", "conv.ovf.u", "conv.ovf.u.un", "conv.ovf.u1",
            "conv.ovf.u1.un", "conv.ovf.u2", "conv.ovf.u2.un", "conv.ovf.u4", "conv.ovf.u4.un", "conv.ovf.u8", "conv.ovf.u8.un", "conv.r.un", "conv.r4", "conv.r8", "conv.u", "conv.u1",
            "conv.u2", "conv.u4", "conv.u8", "cpblk", "cpobj", "currency", "custom", "date", "decimal", "default", "default", "demand", "deny", "div", "div.un", "dup", "endfault",
            "endfilter", "endfinally", "endmac", "enum", "error", "explicit", "extends", "extern", "FALSE", "famandassem", "family", "famorassem", "fastcall", "fastcall", "fault",
            "field", "filetime", "filter", "final", "finally", "fixed", "flags", "float", "float32", "float64", "forwardref", "fromunmanaged", "handler", "hidebysig", "hresult", "idispatch",
            "il", "illegal", "implements", "implicitcom", "implicitres", "import", "in", "inheritcheck", "init", "initblk", "initobj", "initonly", "instance", "int", "int16", "int32",
            "int64", "int8", "interface", "internalcall", "isinst", "iunknown", "jmp", "lasterr", "lcid", "ldarg", "ldarg.0", "ldarg.1", "ldarg.2", "ldarg.3", "ldarg.s", "ldarga",
            "ldarga.s", "ldc.i4", "ldc.i4.0", "ldc.i4.1", "ldc.i4.2", "ldc.i4.3", "ldc.i4.4", "ldc.i4.5", "ldc.i4.6", "ldc.i4.7", "ldc.i4.8", "ldc.i4.M1", "ldc.i4.m1", "ldc.i4.s",
            "ldc.i8", "ldc.r4", "ldc.r8", "ldelem", "ldelem.i", "ldelem.i1", "ldelem.i2", "ldelem.i4", "ldelem.i8", "ldelem.r4", "ldelem.r8", "ldelem.ref", "ldelem.u1", "ldelem.u2",
            "ldelem.u4", "ldelem.u8", "ldelema", "ldfld", "ldflda", "ldftn", "ldind.i", "ldind.i1", "ldind.i2", "ldind.i4", "ldind.i8", "ldind.r4", "ldind.r8", "ldind.ref", "ldind.u1",
            "ldind.u2", "ldind.u4", "ldind.u8", "ldlen", "ldloc", "ldloc.0", "ldloc.1", "ldloc.2", "ldloc.3", "ldloc.s", "ldloca", "ldloca.s", "ldnull", "ldobj", "ldsfld", "ldsflda",
            "ldstr", "ldtoken", "ldvirtftn", "leave", "leave.s", "linkcheck", "literal", "localloc", "lpstr", "lpstruct", "lptstr", "lpvoid", "lpwstr", "managed", "marshal",
            "method", "mkrefany", "modopt", "modreq", "mul", "mul.ovf", "mul.ovf.un", "native", "neg", "nested", "newarr", "newobj", "newslot", "noappdomain", "no.", "noinlining",
            "nomachine", "nomangle", "nometadata", "noncasdemand", "noncasinheritance", "noncaslinkdemand", "nop", "noprocess", "not", "not_in_gc_heap", "notremotable", "notserialized",
            "null", "nullref", "object", "objectref", "opt", "optil", "or", "out", "permitonly", "pinned", "pinvokeimpl", "pop", "prefix1", "prefix2", "prefix3", "prefix4", "prefix5",
            "prefix6", "prefix7", "prefixref", "prejitdeny", "prejitgrant", "preservesig", "private", "privatescope", "protected", "public", "readonly.", "record", "refany", "refanytype",
            "refanyval", "rem", "rem.un", "reqmin", "reqopt", "reqrefuse", "reqsecobj", "request", "ret", "rethrow", "retval", "rtspecialname", "runtime", "safearray", "sealed",
            "sequential", "serializable", "shl", "shr", "shr.un", "sizeof", "special", "specialname", "starg", "starg.s", "static", "stdcall", "stdcall", "stelem", "stelem.i", "stelem.i1",
            "stelem.i2", "stelem.i4", "stelem.i8", "stelem.r4", "stelem.r8", "stelem.ref", "stfld", "stind.i", "stind.i1", "stind.i2", "stind.i4", "stind.i8", "stind.r4", "stind.r8",
            "stind.ref", "stloc", "stloc.0", "stloc.1", "stloc.2", "stloc.3", "stloc.s", "stobj", "storage", "stored_object", "stream", "streamed_object", "string", "struct", "stsfld",
            "sub", "sub.ovf", "sub.ovf.un", "switch", "synchronized", "syschar", "sysstring", "tail.", "tbstr", "thiscall", "thiscall", "throw", "tls", "to", "TRUE", "type", "typedref",
            "unaligned.", "unbox", "unbox.any", "unicode", "unmanaged", "unmanagedexp", "unsigned", "unused", "userdefined", "value", "valuetype", "vararg", "variant", "vector",
            "virtual", "void", "volatile.", "wchar", "winapi", "with", "wrapper", "xor" };

        private CilDecoderHelpers()
        {
            oneByteOpCodes = new OpCode[0x100];
            twoByteOpCodes = new OpCode[0x100];

            var opCodeType = typeof(OpCode);
            var fields = typeof(OpCodes).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach(var field in fields)
            {
                if (field.FieldType != opCodeType) continue;

                OpCode opCode = (OpCode)field.GetValue(null);
                var value = unchecked((ushort)opCode.Value);
                if(value < 0x100)
                {
                    oneByteOpCodes[value] = opCode;
                }
                else if((value & 0xff00) == 0xfe00)
                {
                    twoByteOpCodes[value & 0xff] = opCode;
                }
            }
        }

        internal string NormalizeString(string str)
        {
            if (str == ".ctor" || str == ".cctor") return str;
            if (!msilKeywords.Contains(str))
            {
                return str;
            }
            return string.Format("'{0}'", str);
        }
    }
}
