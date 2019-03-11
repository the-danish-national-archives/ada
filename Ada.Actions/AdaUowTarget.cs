namespace Ada.Actions
{
    #region Namespace Using

    using Repositories;

    #endregion

    public class AdaUowTarget
    {
        #region  Fields

        #endregion

        #region  Constructors

        public AdaUowTarget(IAdaUowFactory adaUowFactory)
        {
            AdaUowFactory = adaUowFactory;
        }

        #endregion

        #region Properties

        public IAdaUowFactory AdaUowFactory { get; }

        #endregion
    }
}