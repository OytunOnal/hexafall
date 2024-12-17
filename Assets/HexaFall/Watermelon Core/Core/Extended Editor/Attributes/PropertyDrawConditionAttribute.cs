using System;

namespace HexFall
{
    public class PropertyDrawConditionAttribute : BaseAttribute
    {
        public PropertyDrawConditionAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
