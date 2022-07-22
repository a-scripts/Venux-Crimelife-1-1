using System;
using System.Collections.Generic;
using System.Text;

namespace Venux.Module
{
    public abstract class Module<T> : BaseModule where T : Module<T>
    {
        public static T Instance { get; private set; }

        protected Module() => Venux.Module.Module<T>.Instance = (T)this;
    }
}
