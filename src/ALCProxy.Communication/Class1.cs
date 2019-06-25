using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ALCProxy.Communication
{
    public class ExampleDispatch<I> : DispatchProxy
    {
        public object instance;
        internal static I Create<Dispatch>(Type type)
        {
            object proxy = Create<I, ExampleDispatch<I>>(); //Error throws here
            ((ExampleDispatch<I>)proxy).SetParameters(type, new Type[] { }, new object[] { });
            return (I)proxy;
        }

        private void SetParameters(Type instanceType, Type[] argTypes, object[] constructorArgs)
        {
            instance = instanceType.GetConstructor(argTypes).Invoke(constructorArgs);
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return targetMethod.Invoke(instance, args);
        }
    }
}
