namespace Ra.EntityExtensions.TableIndex
{
    #region Namespace Using

    using DomainEntities.TableIndex;

    #endregion

    public static class ViewExtensions
    {
        #region

        public static bool IsAvQuery(this View view)
        {
            return view.Name.StartsWith("AV");
        }

        #endregion
    }
}