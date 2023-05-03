using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgumentParser
{
    internal static class STDExtension
    {
        public static string Repeat(this string @string, int count)
        {
            string result = "";
            for (int i = 0; i < count; i++)
            {
                result += @string;
            }
            return result;
        }
    }
}