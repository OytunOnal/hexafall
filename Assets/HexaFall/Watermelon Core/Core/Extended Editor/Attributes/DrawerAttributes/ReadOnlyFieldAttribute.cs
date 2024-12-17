using System;

namespace HexFall
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyFieldAttribute : DrawerAttribute
    {
    }
}
