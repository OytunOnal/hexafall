﻿using System;

namespace HexFall
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumFlagsAttribute : DrawerAttribute
    {
        public EnumFlagsAttribute()
        {

        }
    }
}