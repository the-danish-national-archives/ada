namespace Ada.ActionBase
{
    public interface IAdaAction<in T> : IAdaAction
    {
        #region

        ActionRunResult Run(T testSubject);

        #endregion
    }

    // Non generic interface, used for identification
    public interface IAdaAction
    {
    }
}