#region Header

// Author 
// Created 17

#endregion

namespace Ada.UI.Wpf.ProgressManagement.Properties
{
    #region Namespace Using

    using System;
    using WpfAnimatedGif;

    #endregion

    public class DummyForInclude
    {
        #region  Constructors

        public DummyForInclude()
        {
            try
            {
                var ignoreGif = ImageBehavior.AnimationCompletedEvent;
                new Ra.Common.Wpf.Properties.DummyForInclude();
                new TableIndexViewer.Properties.DummyForInclude();
                new Text.Properties.DummyForInclude();
            }
            catch (Exception)
            {
//                throw;
            }
        }

        #endregion
    }
}