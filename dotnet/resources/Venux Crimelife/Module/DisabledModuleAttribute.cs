using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisabledModuleAttribute : Attribute
    {
    }
}
