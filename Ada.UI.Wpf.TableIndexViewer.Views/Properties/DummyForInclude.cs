#region Header

// Author 
// Created 17

#endregion

namespace Ra.Common.Wpf.Properties
{
    #region Namespace Using

    using System;
    using Microsoft.Expression.Interactivity;

    #endregion

    public class DummyForInclude
    {
        #region  Constructors

        // add inter

        public DummyForInclude()
        {
            try
            {
                var temp = typeof(VisualStateUtilities).Assembly;
            }
            catch (Exception)
            {
                //                throw;
            }
        }

        #endregion
    }
}