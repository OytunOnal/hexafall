using System;

namespace HexFall
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : DrawConditionAttribute
    {
        private string conditionName;

        public ShowIfAttribute(string conditionName)
        {
            this.conditionName = conditionName;
        }

        public string ConditionName
        {
            get
            {
                return this.conditionName;
            }
        }
    }
}