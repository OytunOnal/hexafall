using System;

namespace HexFall
{
    public class PropertyDrawerAttribute : BaseAttribute
    {
        public PropertyDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
