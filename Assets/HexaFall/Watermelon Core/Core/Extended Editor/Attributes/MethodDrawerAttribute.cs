using System;

namespace HexFall
{
    public class MethodDrawerAttribute : BaseAttribute
    {
        public MethodDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}