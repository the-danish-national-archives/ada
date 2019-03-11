namespace Ra.DomainEntities.TableIndex
{
    public class View : EntityBase, IAnnotatedEntity
    {
        #region Properties

        public virtual string Name { get; set; }

        public virtual string QueryOriginal { get; set; }
        public virtual TableIndex TableIndex { get; set; }

        public virtual string NotRunnableQuery
        {

            //TODO skal testes /KNA
            get
            {
                if (QueryOriginal.ToUpper().Contains("WHERE"))
                {
                    return QueryOriginal + " AND 1 = 0";
                }

                return QueryOriginal + " WHERE 1 = 0";
            }
        }

        #endregion

        #region IAnnotatedEntity Members

        public virtual string Description { get; set; }

        #endregion

        #region

        public virtual bool IsAvView()
        {
            return Name.StartsWith("AV");
        }

        #endregion
    }
}