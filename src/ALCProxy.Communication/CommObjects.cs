using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Transactions;
using Microsoft.VisualBasic.CompilerServices;

namespace ALCProxy
{
    public interface IServerObject
    {
        object CallObject(MethodInfo method, object[] args);
    }

    public interface IClientObject
    {
        object SendMethod(MethodInfo method, object[] args);
        void SetUpServer(AssemblyLoadContext alc, string typeName, string assemblyPath);
    }

    public class ClientObject : DispatchProxy, IClientObject
    {
        //Can't make this an IServerObject directly due to the type-isolation barrier
        private object _server;
        private Type _intType;
        public ClientObject(Type interfaceType)
        {
            _intType = interfaceType;
        }

        public static void Main()
        {

        }

        private Type FindType(string typeName, Assembly a)
        {
            //find the type we're looking for
            Type t = null;
            foreach (Type ty in a.GetTypes())
            {

                if (ty.Name.Equals(typeName))
                {
                    t = ty;
                    break;
                }
            }
            if (t == null)
            {
                //no type asked for in the assembly
                throw new Exception();
            }

            return t;
        }

        public void SetUpServer(AssemblyLoadContext alc, string typeName, string assemblyPath)
        {

            Assembly a = alc.LoadFromAssemblyPath(assemblyPath);

            //find the type we're looking for
            Type objType = FindType(typeName, a);
            // object instance = Activator.CreateInstance(t);

            //Load this assembly in so we can get the server into the ALC
            Assembly aa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(ClientObject)).CodeBase.Substring(8));
            Type serverType = FindType("ServerDispatch`1", aa);
            //Set up all the generics to allow for the serverDispatch to be created correctly
            Type constructedType = serverType.MakeGenericType(_intType);


            object s = Activator.CreateInstance(constructedType);

            var mm = constructedType.GetMethods();
            // MethodInfo m = constructedType.GetMethod("Create`1");
            MethodInfo m = constructedType.GetMethod(
                "Create",
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy, null, new Type[] { typeof(Type) }, null);
            m = m.MakeGenericMethod(constructedType);
            _server = m.Invoke(s, new object[] { objType });


            // aa.GetType("ServerObject");



            //TODO: Actually get the server into the ALC because while this does work, it's currently just the exact same stuff as it was before but with even more reflection.
            //_server = (IServerObject)ServerDispatch<IServerObject>.Create<ServerDispatch<IServerObject>>(t);
        }

        public object SendMethod(MethodInfo method, object[] args)
        {

            MethodInfo m = _server.GetType().GetMethod("CallObject");
            return m.Invoke(_server, new object[] { method, args });
            //MethodInfo m = _server.GetType().GetMethod(((MethodInfo)args[0]).Name);
            //return m.Invoke(_server, (object[])args[1]);

        }

        //This should always be a "SendMethod" option, with 2 args, the methodinfo of what needs to be sent and the args for said method
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return SendMethod(targetMethod, args);
        }
    }
    /// <summary>
    /// Currently using a DispatchProxy directly here leads to an error when calling the "Create" method: System.NotSupportedException: 'A non-collectible assembly may not reference a collectible assembly.'
    /// So for now I'm doing some custom IL generation until I can figure out how to get around that problem, which seems to be sorting things out since it's all dynamic and manually set to be collectible.
    /// </summary>
    /// <typeparam name="I"></typeparam>

    //public class ServerDispatch<I> : DispatchProxy
    //{
    //    public object instance;
    //    internal static I Create<Dispatch>(Type type)
    //    {
    //        Console.WriteLine("Hello from somewhere in the middle of nowhere!");
    //        var a = Assembly.GetExecutingAssembly();
    //        Console.WriteLine(AssemblyLoadContext.GetLoadContext(a).Name);
    //        Console.WriteLine(a.IsCollectible);
    //        using (AssemblyLoadContext.GetLoadContext(a).EnterContextualReflection())
    //        {            //TODO System.NotSupportedException: 'A non-collectible assembly may not reference a collectible assembly.'
    //            Console.WriteLine(AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()).IsCollectible);
    //            Console.WriteLine(typeof(ServerDispatch<I>).IsCollectible);
    //            object proxy = ServerDispatch<I>.Create<I, ServerDispatch<I>>();
    //            ((ServerDispatch<I>)proxy).SetParameters(type, new Type[] { }, new object[] { });
    //            return (I)proxy;
    //        }
    //        //return (I)proxy;
    //    }

    //    internal new static I Create<I, Dispatch>()
    //    {
    //        return DispatchProxy.Create<I, ServerDispatch<I>>();
    //    }

    //    private void SetParameters(Type instanceType, Type[] argTypes, object[] constructorArgs)
    //    {
    //        instance = instanceType.GetConstructor(argTypes).Invoke(constructorArgs);
    //    }

    //    protected override object Invoke(MethodInfo targetMethod, object[] args)
    //    {
    //        //The first arg is the methodinfo, the second is the list of args
    //        MethodInfo m = instance.GetType().GetMethod(((MethodInfo)args[0]).Name);
    //        return m.Invoke(instance, (object[])args[1]);
    //    }
    //}

    //I is the interface type, while the passed in type is the type of the object we want to create
    public class ServerDispatch<I> : IServerObject
    {


        public object instance;

        void InstantiateObject(Type classType)
        {
            //Creates the proxied object and saves it so we can slot it into the proxy later
            instance = (classType.GetConstructor(Type.EmptyTypes).Invoke(new object[] { }));

        }


        internal static IServerObject Create<Dispatch>(Type type)
        {
            ServerDispatch<I> proxy = new ServerDispatch<I>();
            proxy.InstantiateObject(type);



            return proxy;

            //return (I)proxy;
        }

        public object CreateObject(Type classType)
        {
            //Creates the targeted object that we're trying to proxy
            InstantiateObject(classType);

            //Generate a name for our new assembly
            var aName = Guid.NewGuid().ToString();

            //Build the assembly and type
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(aName),
                AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder mb =
                ab.DefineDynamicModule(aName);
            TypeBuilder tb = mb.DefineType(
                aName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit,
                typeof(object),
                new Type[] { typeof(I) });


            //Basic constructor that instantiates the object, doesn't do anything here but we'll insert stuff into the fields later
            ConstructorBuilder cb = tb.DefineDefaultConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName);

            //Creates the field in the class, but we don't set the field until after the proxy is instantiated
            FieldBuilder fb = tb.DefineField("InstanceToCall", typeof(object), FieldAttributes.Public | FieldAttributes.RTSpecialName);
            //Field that holds the type of the object, serves to increase performance by not having to call it every time in the methods
            FieldBuilder fb2 = tb.DefineField("InstanceType", typeof(Type), FieldAttributes.Public | FieldAttributes.RTSpecialName);

            //Build Methods
            MethodInfo[] methods = classType.GetMethods();

            //This stuff isn't being used yet, but it could be useful later on
            //IEnumerable<MethodInfo> methodsToImplement = GetMethodsToImplement(classType);
            //IEnumerable<string> methodNames =
            //    methodsToImplement.Select(x => ((MethodInfo)x).Name);

            //Run through each method, create, and insert the IL
            foreach (MethodInfo m in methods)
            {
                MethodBody body = m.GetMethodBody();
                if (m.IsStatic) //TODO make this available to more method types
                {
                    continue;
                }

                //TODO actually get args working
                IEnumerable<ParameterInfo> methodArgs = m.GetParameters();

                Type[] methodTypes =
                    methodArgs.Select(x => x.ParameterType).ToArray<Type>();
                int numParams = methodTypes.Length;


                //To Work on for getting parameters and return types going
                foreach (Type ty in methodTypes)
                {

                }


                MethodBuilder callMethod = tb.DefineMethod(
                    m.Name,
                    m.Attributes,
                    m.ReturnType,
                    methodTypes); //TODO needs to change later to allow for parameters

                ILGenerator il = callMethod.GetILGenerator();

                SetupMethodIL(il, fb, fb2, m.Name, numParams);
            }

            //Creates the type now that the methods are set up
            Type t = tb.CreateType();
            ConstructorInfo c = t.GetConstructor(Type.EmptyTypes);

            //Instantiate the proxy, so we can slot the object into the field
            //TODO remove the prog requirements
            object obj = c.Invoke(new object[] { });

            //slotting object and type here...
            t.InvokeMember("InstanceToCall", BindingFlags.DeclaredOnly |
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.SetField, null, obj, new object[] { instance });
            t.InvokeMember("InstanceType", BindingFlags.DeclaredOnly |
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.SetField, null, obj, new object[] { instance.GetType() });

            //and we're done!
            return obj;
        }

        private void SetupMethodIL(ILGenerator il, FieldBuilder objField, FieldBuilder typeField, string methodName, int numParams)

        //We Get the 
        {
            //Currently calls obj.GetType().GetMethod(methodName).Invoke(obj, args)
            //TODO simplify this to reduce the amount of reflection needed at runtime of the proxy

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            //il.Emit(OpCodes.Ldfld, fb);
            //il.EmitCall(OpCodes.Callvirt, typeof(object).GetMethod("GetType"), null);
            il.Emit(OpCodes.Ldfld, typeField);
            il.Emit(OpCodes.Ldstr, methodName);
            il.EmitCall(OpCodes.Callvirt, typeof(Type).GetMethod("GetMethod", new Type[] { typeof(string) }), null);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, objField); //The instance of the object that is being called

            //TODO swap null args with the list of given parameters

            //il.Emit(OpCodes.ldc_I)

            il.Emit(OpCodes.Ldnull);
            //il.Emit(OpCodes.Ldarg_0);
            //il.Emit(OpCodes.Ldc_I4_0);
            //il.Emit(OpCodes.Newarr, typeof(object));
            il.EmitCall(OpCodes.Callvirt, typeof(MethodInfo).GetMethod("Invoke", new Type[] { typeof(object), typeof(object[]) }), null);

            //TODO figure out how to give a return type
            il.Emit(OpCodes.Pop);

            il.Emit(OpCodes.Ret);
        }



        public ServerDispatch() {

        }

        public object CallObject(MethodInfo method, object[] args)
        {
            MethodInfo m = instance.GetType().GetMethod(method.Name);
            //return m.Invoke(instance, (object[])args[1]);
            return m.Invoke(instance, args);
        }
    }

}
