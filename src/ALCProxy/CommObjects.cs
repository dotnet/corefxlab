using System;
using System.Reflection;

namespace ALCProxy
{
    interface IServerObject
    {
        void ReturnResult(Type t, object instance);
        object CallObject(MethodInfo method, object[] args);
    }

    interface IClientObject
    {
        object SendMethod(MethodInfo method, object[] args);      
    }

    public class ClientObject : IClientObject
    {
        //Can't make this an IServerObject directly due to the type-isolation barrier
        private object _server;
        public ClientObject()
        {

        }

        public object SendMethod(MethodInfo method, object[] args)
        {
            Console.WriteLine("Sending method!");
            //return typeof(ServerObject).GetMethod("CallObject").Invoke(server, new object[] { method, args });
            return null;
        }
    }

    //Need to build DispatchBuilder to construct this seperately.
    public class ServerObject : IServerObject
    {
        private object _instance;
        public ServerObject(object instance)
        {
            _instance = instance;
        }

        public object CallObject(MethodInfo method, object[] args)
        {
            return _instance.GetType().GetMethod(method.Name).Invoke(_instance, args);
        }

        public void ReturnResult(Type t, object instance)
        {
            throw new NotImplementedException();
        }

    }
}
