
//  Copyright (c) Microsoft Corporation.  All rights reserved.



.assembly extern /* 23000001 */ mscorlib
{
  .publickeytoken = ( B7 7A 5C 56 19 34 E0 89 )
  .ver 4:0:0:0
}
.assembly extern /* 23000002 */ System.Core
{
  .publickeytoken = ( B7 7A 5C 56 19 34 E0 89 )
  .ver 4:0:0:0
}
.assembly Demo1
{
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 )
  .custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78 63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )
  .custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = ( 01 00 07 01 00 00 00 00 )
  .custom instance void [mscorlib]System.Reflection.AssemblyTitleAttribute::.ctor(string) = ( 01 00 05 44 65 6D 6F 31 00 00 )
  .custom instance void [mscorlib]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = ( 01 00 00 00 00 )
  .custom instance void [mscorlib]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = ( 01 00 00 00 00 )
  .custom instance void [mscorlib]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = ( 01 00 00 00 00 )
  .custom instance void [mscorlib]System.Reflection.AssemblyProductAttribute::.ctor(string) = ( 01 00 05 44 65 6D 6F 31 00 00 )
  .custom instance void [mscorlib]System.Reflection.AssemblyCopyrightAttribute::.ctor(string) = ( 01 00 12 43 6F 70 79 72 69 67 68 74 20 C2 A9 20 20 32 30 31 35 00 00 )
  .custom instance void [mscorlib]System.Reflection.AssemblyTrademarkAttribute::.ctor(string) = ( 01 00 00 00 00 )
  .custom instance void [mscorlib]System.Runtime.InteropServices.ComVisibleAttribute::.ctor(bool) = ( 01 00 00 00 00 )
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 66 64 35 39 65 35 63 63 2D 31 64 31 36 2D 34 38 31 38 2D 62 63 63 63 2D 34 39 66 39 61 61 62 30 36 38 31 63 00 00 )
  .custom instance void [mscorlib]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )
  .custom instance void [mscorlib]System.Runtime.Versioning.TargetFrameworkAttribute::.ctor(string) = ( 01 00 1A 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B 2C 56 65 72 73 69 6F 6E 3D 76 34 2E 35 01 00 54 0E 14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79 4E 61 6D 65 12 2E 4E 45 54 20 46 72 61 6D 65 77 6F 72 6B 20 34 2E 35 )


  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module Demo1.exe
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003
.corflags 0x00020003

// =============== CLASS MEMBERS DECLARATION ===================

.class /* 02000002 */ Demo1.Program extends [mscorlib]System.Object
{
  .field /* 04000001 */ public static string str
  .field /* 04000002 */ public static valuetype [mscorlib]System.DateTime dt
  .field /* 04000003 */ public static valuetype Demo1.StructTest st
  .field /* 04000004 */ private static string <Str>k__BackingField
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 )
  .custom instance void [mscorlib]System.Diagnostics.DebuggerBrowsableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggerBrowsableState) = ( 01 00 00 00 00 00 00 00 )
  .method /* 06000001 */ public hidebysig static specialname string get_Str() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 )
    // code size       10 (0xa)
    .maxstack  1
    .locals init([0] string V_0)
    IL_0000:  /* 7E   | (04)000004       */ ldsfld       string Demo1.Program::<Str>k__BackingField
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 2B   | 00               */ br.s       IL_0008
    IL_0008:  /* 06   |                  */ ldloc.0
    IL_0009:  /* 2A   |                  */ ret
  } // end of method get_Str

  .method /* 06000002 */ public hidebysig static specialname void set_Str(string 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 )
    // code size        9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 80   | (04)000004       */ stsfld       string Demo1.Program::<Str>k__BackingField
    IL_0006:  /* 2B   | 00               */ br.s       IL_0008
    IL_0008:  /* 2A   |                  */ ret
  } // end of method set_Str

  .method /* 06000003 */ private hidebysig static void Main(string[] args) cil managed
  {
    .entrypoint
    // code size      184 (0xb8)
    .maxstack  6
    .locals init([0] float64 V_0,
                 [1] int64 V_1,
                 [2] int32 V_2,
                 [3] float32 V_3,
                 [4] bool V_4)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 23   | 0000000000002440 */ ldc.r8     10.
    IL_000a:  /* 0A   |                  */ stloc.0
    IL_000b:  /* 21   | C701BDDE19000000 */ ldc.i8     0x19debd01c7
    IL_0014:  /* 0B   |                  */ stloc.1
    IL_0015:  /* 1F   | FFFFFFFF         */ ldc.i4.s   25
    IL_0017:  /* 0C   |                  */ stloc.2
    IL_0018:  /* 22   | BA493A41         */ ldc.r4     11.643
    IL_001d:  /* 0D   |                  */ stloc.3
    IL_001e:  /* 7F   | (04)000002       */ ldsflda      valuetype [mscorlib]System.DateTime Demo1.Program::dt
    IL_0023:  /* 28   | (0A)000011       */ call         instance int32 [mscorlib]System.DateTime::get_Day()
    IL_0028:  /* 28   | (0A)000012       */ call         void [mscorlib]System.Console::WriteLine(int32)
    IL_002d:  /* 00   |                  */ nop
    IL_002e:  /* 7F   | (04)000003       */ ldsflda      valuetype Demo1.StructTest Demo1.Program::st
    IL_0033:  /* 28   | (06)00000A       */ call         instance string Demo1.StructTest::get_foo()
    IL_0038:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_003d:  /* 00   |                  */ nop
    IL_003e:  /* 28   | (06)000001       */ call         string Demo1.Program::get_Str()
    IL_0043:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_0048:  /* 00   |                  */ nop
    IL_0049:  /* 72   | (70)000001       */ ldstr        "World"
    IL_004e:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_0053:  /* 00   |                  */ nop
    IL_0054:  /* 72   | (70)00000D       */ ldstr        "Goodbye"
    IL_0059:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_005e:  /* 00   |                  */ nop
    IL_005f:  /* 12   | 01               */ ldloca.s   V_1
    IL_0061:  /* 28   | (0A)000014       */ call         instance string [mscorlib]System.Int64::ToString()
    IL_0066:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_006b:  /* 00   |                  */ nop
    IL_006c:  /* 08   |                  */ ldloc.2
    IL_006d:  /* 1F   | FFFFFFFF         */ ldc.i4.s   20
    IL_006f:  /* FE02 |                  */ cgt
    IL_0071:  /* 13   | 04               */ stloc.s    V_4
    IL_0073:  /* 11   | 04               */ ldloc.s    V_4
    IL_0075:  /* 2C   | 11               */ brfalse.s  IL_0088
    IL_0077:  /* 00   |                  */ nop
    IL_0078:  /* 12   | 00               */ ldloca.s   V_0
    IL_007a:  /* 28   | (0A)000015       */ call         instance string [mscorlib]System.Double::ToString()
    IL_007f:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_0084:  /* 00   |                  */ nop
    IL_0085:  /* 00   |                  */ nop
    IL_0086:  /* 2B   | 0D               */ br.s       IL_0095
    IL_0088:  /* 12   | 02               */ ldloca.s   V_2
    IL_008a:  /* 28   | (0A)000016       */ call         instance string [mscorlib]System.Int32::ToString()
    IL_008f:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_0094:  /* 00   |                  */ nop
    IL_0095:  /* 12   | 03               */ ldloca.s   V_3
    IL_0097:  /* 28   | (0A)000017       */ call         instance string [mscorlib]System.Single::ToString()
    IL_009c:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_00a1:  /* 00   |                  */ nop
    IL_00a2:  /* 72   | (70)00001D       */ ldstr        "Test"
    IL_00a7:  /* 16   |                  */ ldc.i4.0
    IL_00a8:  /* 16   |                  */ ldc.i4.0
    IL_00a9:  /* 16   |                  */ ldc.i4.0
    IL_00aa:  /* 1F   | FFFFFFFF         */ ldc.i4.s   32
    IL_00ac:  /* 73   | (0A)000018       */ newobj       instance void class [mscorlib]System.Collections.Generic.List`1<string>::.ctor()
    IL_00b1:  /* 28   | (06)000004       */ call         void Demo1.Program::Hola(string,int32,int32,int32,int32,class [mscorlib]System.Collections.Generic.IList`1<string>)
    IL_00b6:  /* 00   |                  */ nop
    IL_00b7:  /* 2A   |                  */ ret
  } // end of method Main

  .method /* 06000004 */ public hidebysig static void Hola(string x, int32 d, int32 z, int32 w, int32 test, class [mscorlib]System.Collections.Generic.IList`1<string> list) cil managed
  {
    // code size      117 (0x75)
    .maxstack  2
    .locals init([0] int32 V_0,
                 [1] class [mscorlib]System.FormatException V_1,
                 [2] class [mscorlib]System.BadImageFormatException V_2)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 1F   | FFFFFFFF         */ ldc.i4.s   20
    IL_0003:  /* 0A   |                  */ stloc.0
    .try
    {
      .try
      {
        IL_0004:  /* 00   |                  */ nop
        .try
        {
          IL_0005:  /* 00   |                  */ nop
          IL_0006:  /* 1F   | FFFFFFFF         */ ldc.i4.s   10
          IL_0008:  /* 0A   |                  */ stloc.0
          IL_0009:  /* 00   |                  */ nop
          IL_000a:  /* DE   | 03               */ leave.s    IL_000f
        } // end .try
        finally
        {
          IL_000c:  /* 00   |                  */ nop
          IL_000d:  /* 00   |                  */ nop
          IL_000e:  /* DC   |                  */ endfinally
        } // end handler
        IL_000f:  /* 72   | (70)000027       */ ldstr        "1"
        IL_0014:  /* 73   | (0A)000019       */ newobj       instance void [mscorlib]System.ApplicationException::.ctor(string)
        IL_0019:  /* 7A   |                  */ throw
      } // end .try
      filter
      {
        IL_001a:  /* 75   | (01)000016       */ isinst       [mscorlib]System.FormatException
        IL_001f:  /* 25   |                  */ dup
        IL_0020:  /* 2D   | 04               */ brtrue.s   IL_0026
        IL_0022:  /* 26   |                  */ pop
        IL_0023:  /* 16   |                  */ ldc.i4.0
        IL_0024:  /* 2B   | 14               */ br.s       IL_003a
        IL_0026:  /* 0B   |                  */ stloc.1
        IL_0027:  /* 07   |                  */ ldloc.1
        IL_0028:  /* 6F   | (0A)00001A       */ callvirt     instance string [mscorlib]System.Exception::get_Message()
        IL_002d:  /* 72   | (70)000027       */ ldstr        "1"
        IL_0032:  /* 28   | (0A)00001B       */ call         bool [mscorlib]System.String::op_Equality(string,string)
        IL_0037:  /* 16   |                  */ ldc.i4.0
        IL_0038:  /* FE03 |                  */ cgt.un
        IL_003a:  /* FE11 |                  */ endfilter
      } // end filter
      { // handler
        IL_003c:  /* 26   |                  */ pop
        IL_003d:  /* 00   |                  */ nop
        IL_003e:  /* 00   |                  */ nop
        IL_003f:  /* DE   | 05               */ leave.s    IL_0046
      } // end handler
      catch [mscorlib]System.BadImageFormatException
      {
        IL_0041:  /* 0C   |                  */ stloc.2
        IL_0042:  /* 00   |                  */ nop
        IL_0043:  /* 00   |                  */ nop
        IL_0044:  /* DE   | 00               */ leave.s    IL_0046
      } // end handler
      IL_0046:  /* DE   | 03               */ leave.s    IL_004b
    } // end .try
    finally
    {
      IL_0048:  /* 00   |                  */ nop
      IL_0049:  /* 00   |                  */ nop
      IL_004a:  /* DC   |                  */ endfinally
    } // end handler
    IL_004b:  /* 02   |                  */ ldarg.0
    IL_004c:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_0051:  /* 00   |                  */ nop
    IL_0052:  /* 12   | 00               */ ldloca.s   V_0
    IL_0054:  /* 28   | (0A)000016       */ call         instance string [mscorlib]System.Int32::ToString()
    IL_0059:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_005e:  /* 00   |                  */ nop
    IL_005f:  /* 0E   | 04               */ ldarg.s    test
    IL_0061:  /* 28   | (0A)000012       */ call         void [mscorlib]System.Console::WriteLine(int32)
    IL_0066:  /* 00   |                  */ nop
    IL_0067:  /* 0E   | 05               */ ldarg.s    list
    IL_0069:  /* 6F   | (0A)00001C       */ callvirt     instance int32 class [mscorlib]System.Collections.Generic.ICollection`1<string>::get_Count()
    IL_006e:  /* 28   | (0A)000012       */ call         void [mscorlib]System.Console::WriteLine(int32)
    IL_0073:  /* 00   |                  */ nop
    IL_0074:  /* 2A   |                  */ ret
  } // end of method Hola

  .method /* 06000005 */ public hidebysig static int32 Switch(int32 y) cil managed
  {
    // code size      100 (0x64)
    .maxstack  2
    .locals init([0] int32 V_0,
                 [1] int32 V_1)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 0A   |                  */ stloc.0
    IL_0003:  /* 06   |                  */ ldloc.0
    IL_0004:  /* 17   |                  */ ldc.i4.1
    IL_0005:  /* 59   |                  */ sub
    IL_0006:  /* 45   | 04000000         */ switch    (
              /*      | 02000000         */            IL_001d,
              /*      | 0F000000         */            IL_002a,
              /*      | 1C000000         */            IL_0037,
              /*      | 29000000         */            IL_0044)
    IL_001b:  /* 2B   | 34               */ br.s       IL_0051
    IL_001d:  /* 72   | (70)00002B       */ ldstr        "Fail"
    IL_0022:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_0027:  /* 00   |                  */ nop
    IL_0028:  /* 2B   | 29               */ br.s       IL_0053
    IL_002a:  /* 72   | (70)000035       */ ldstr        "Pass"
    IL_002f:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_0034:  /* 00   |                  */ nop
    IL_0035:  /* 2B   | 1C               */ br.s       IL_0053
    IL_0037:  /* 72   | (70)00003F       */ ldstr        "Good"
    IL_003c:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_0041:  /* 00   |                  */ nop
    IL_0042:  /* 2B   | 0F               */ br.s       IL_0053
    IL_0044:  /* 72   | (70)000049       */ ldstr        "Excelent"
    IL_0049:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_004e:  /* 00   |                  */ nop
    IL_004f:  /* 2B   | 02               */ br.s       IL_0053
    IL_0051:  /* 2B   | 00               */ br.s       IL_0053
    IL_0053:  /* 72   | (70)00005B       */ ldstr        "End of switch"
    IL_0058:  /* 28   | (0A)000013       */ call         void [mscorlib]System.Console::WriteLine(string)
    IL_005d:  /* 00   |                  */ nop
    IL_005e:  /* 02   |                  */ ldarg.0
    IL_005f:  /* 0B   |                  */ stloc.1
    IL_0060:  /* 2B   | 00               */ br.s       IL_0062
    IL_0062:  /* 07   |                  */ ldloc.1
    IL_0063:  /* 2A   |                  */ ret
  } // end of method Switch

  .method /* 06000006 */ public hidebysig static void LoadToken() cil managed
  {
    // code size       81 (0x51)
    .maxstack  6
    .locals init([0] class [System.Core]System.Linq.Expressions.Expression`1<class [mscorlib]System.Func`2<int32,int32>> V_0,
                 [1] class [mscorlib]System.Type V_1,
                 [2] class [System.Core]System.Linq.Expressions.ParameterExpression V_2)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* D0   | (01)000020       */ ldtoken      [mscorlib]System.Int32
    IL_0006:  /* 28   | (0A)00001D       */ call         class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
    IL_000b:  /* 72   | (70)000077       */ ldstr        "x"
    IL_0010:  /* 28   | (0A)00001E       */ call         class [System.Core]System.Linq.Expressions.ParameterExpression [System.Core]System.Linq.Expressions.Expression::Parameter(class [mscorlib]System.Type,string)
    IL_0015:  /* 0C   |                  */ stloc.2
    IL_0016:  /* 14   |                  */ ldnull
    IL_0017:  /* D0   | (06)000005       */ ldtoken      method int32 Demo1.Program::Switch(int32)
    IL_001c:  /* 28   | (0A)00001F       */ call         class [mscorlib]System.Reflection.MethodBase [mscorlib]System.Reflection.MethodBase::GetMethodFromHandle(valuetype [mscorlib]System.RuntimeMethodHandle)
    IL_0021:  /* 74   | (01)00002B       */ castclass    [mscorlib]System.Reflection.MethodInfo
    IL_0026:  /* 17   |                  */ ldc.i4.1
    IL_0027:  /* 8D   | (01)000028       */ newarr       [System.Core]System.Linq.Expressions.Expression
    IL_002c:  /* 25   |                  */ dup
    IL_002d:  /* 16   |                  */ ldc.i4.0
    IL_002e:  /* 08   |                  */ ldloc.2
    IL_002f:  /* A2   |                  */ stelem.ref
    IL_0030:  /* 28   | (0A)000020       */ call         class [System.Core]System.Linq.Expressions.MethodCallExpression [System.Core]System.Linq.Expressions.Expression::Call(class [System.Core]System.Linq.Expressions.Expression,class [mscorlib]System.Reflection.MethodInfo,class [System.Core]System.Linq.Expressions.Expression[])
    IL_0035:  /* 17   |                  */ ldc.i4.1
    IL_0036:  /* 8D   | (01)00001B       */ newarr       [System.Core]System.Linq.Expressions.ParameterExpression
    IL_003b:  /* 25   |                  */ dup
    IL_003c:  /* 16   |                  */ ldc.i4.0
    IL_003d:  /* 08   |                  */ ldloc.2
    IL_003e:  /* A2   |                  */ stelem.ref
    IL_003f:  /* 28   | (2B)000001       */ call         class [System.Core]System.Linq.Expressions.Expression`1<!!0> [System.Core]System.Linq.Expressions.Expression::Lambda<class [mscorlib]System.Func`2<int32,int32>>(class [System.Core]System.Linq.Expressions.Expression,class [System.Core]System.Linq.Expressions.ParameterExpression[])
    IL_0044:  /* 0A   |                  */ stloc.0
    IL_0045:  /* D0   | (01)00002D       */ ldtoken      [mscorlib]System.Reflection.Emit.OpCodes
    IL_004a:  /* 28   | (0A)00001D       */ call         class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
    IL_004f:  /* 0B   |                  */ stloc.1
    IL_0050:  /* 2A   |                  */ ret
  } // end of method LoadToken

  .method /* 06000007 */ public hidebysig virtual instance string ToString() cil managed
  {
    // code size       25 (0x19)
    .maxstack  1
    .locals init([0] bool V_0,
                 [1] string V_1)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (0A)000022       */ call         instance class [mscorlib]System.Type [mscorlib]System.Object::GetType()
    IL_0007:  /* 6F   | (0A)000023       */ callvirt     instance bool [mscorlib]System.Type::get_IsAbstract()
    IL_000c:  /* 0A   |                  */ stloc.0
    IL_000d:  /* 12   | 00               */ ldloca.s   V_0
    IL_000f:  /* 28   | (0A)000024       */ call         instance string [mscorlib]System.Boolean::ToString()
    IL_0014:  /* 0B   |                  */ stloc.1
    IL_0015:  /* 2B   | 00               */ br.s       IL_0017
    IL_0017:  /* 07   |                  */ ldloc.1
    IL_0018:  /* 2A   |                  */ ret
  } // end of method ToString

  .method /* 06000008 */ public hidebysig specialname rtspecialname instance void .ctor() cil managed
  {
    // code size        8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call         instance void [mscorlib]System.Object::.ctor()
    IL_0006:  /* 00   |                  */ nop
    IL_0007:  /* 2A   |                  */ ret
  } // end of method .ctor

  .method /* 06000009 */ private hidebysig static specialname rtspecialname void .cctor() cil managed
  {
    // code size       43 (0x2b)
    .maxstack  8
    IL_0000:  /* 72   | (70)00007B       */ ldstr        "Hello"
    IL_0005:  /* 80   | (04)000001       */ stsfld       string Demo1.Program::str
    IL_000a:  /* 7F   | (04)000002       */ ldsflda      valuetype [mscorlib]System.DateTime Demo1.Program::dt
    IL_000f:  /* FE15 | (01)000011       */ initobj      [mscorlib]System.DateTime
    IL_0015:  /* 7F   | (04)000003       */ ldsflda      valuetype Demo1.StructTest Demo1.Program::st
    IL_001a:  /* FE15 | (02)000003       */ initobj      Demo1.StructTest
    IL_0020:  /* 72   | (70)000087       */ ldstr        "Default"
    IL_0025:  /* 80   | (04)000004       */ stsfld       string Demo1.Program::<Str>k__BackingField
    IL_002a:  /* 2A   |                  */ ret
  } // end of method .cctor

  .property /* 17000001 */ string Str()
  {
    .get string Demo1.Program::get_Str()
    .set void Demo1.Program::set_Str(string)
  }

} // end of class Demo1.Program

.class /* 02000003 */ Demo1.StructTest extends [mscorlib]System.ValueType
{
  .pack 0
  .size 1
  .method /* 0600000A */ public hidebysig specialname instance string get_foo() cil managed
  {
    // code size       11 (0xb)
    .maxstack  1
    .locals init([0] string V_0)
    IL_0000:  /* 00   |                  */ nop
    IL_0001:  /* 7E   | (0A)000026       */ ldsfld       string [mscorlib]System.String::Empty
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 2B   | 00               */ br.s       IL_0009
    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 2A   |                  */ ret
  } // end of method get_foo

  .property /* 17000002 */ instance string foo()
  {
    .get instance string Demo1.StructTest::get_foo()
  }

} // end of class Demo1.StructTest

//Time elapsed: 00:00:00.1095462
