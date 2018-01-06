using System;

namespace Butterfly.Client
{
    public class DispatchEventArgs<T> : EventArgs
    {
        public T Data { get; }

        public DispatchEventArgs(T data)
        {
            Data = data;
        }
    }
}