#region Header

// Author 
// Created 31

#endregion

namespace Ra.Common.Algorithms
{
    #region Namespace Using

    using System;

    #endregion

    public static class Algo
    {
        #region

        public static TValue FindTop<TValue>(TValue init, Func<TValue, TValue> func) where TValue : class
        {
            var temp = func(init);

            return temp == null ? init : FindTop(temp, func);
        }

        #endregion
    }
}