namespace Ra.DomainEntities
{
    public abstract class EntityBase : IAdaEntity
    {
        #region Properties

        /// <summary>
        ///     Unique key, for sql
        /// </summary>
        public virtual int Key { get; set; }

        #endregion
    }

    public interface IAdaEntity
    {
    }
}