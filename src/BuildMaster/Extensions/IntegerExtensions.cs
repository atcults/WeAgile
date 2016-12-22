using System;

namespace BuildMaster.Extensions
{
    public static class IntegerExtensions
    {
        public static void Times(this int n, Action<int> action)
        {
            for (var i = 1; i <= n; i++)
                action(i);
        }
    }
}