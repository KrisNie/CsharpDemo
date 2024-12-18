using System;
using Services.Utilities;

namespace Services.Extensions
{
    public static class UnbelievableClassExtension
    {
        public static int GetQuarter(this DateTime dt)
        {
            return (dt.Month - 1) / 3 + 1;
        }
    }
}