#region Header

// Author 
// Created 17

#endregion

namespace Ra.Common.Wpf.ResultsList
{
    public interface IResultsList
    {
        #region Properties

        string Message { get; }
        string Text { get; }

        object Value { get; }

        #endregion
    }
}