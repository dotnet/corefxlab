﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ALCProxy
{
    interface IServerObject
    {
        //void ReceiveMethod(MethodInfo method, object[] args);
        void ReturnResult(Type t, object instance);
    }

    interface IClientObject
    {
        object SendMethod(MethodInfo method, object[] args);      
    }

    public class ClientObject : IClientObject
    {

        public ClientObject()
        {

        }

        public object SendMethod(MethodInfo method, object[] args)
        {
            Console.WriteLine("Sending method!");
            return null;
        }
    }
}
