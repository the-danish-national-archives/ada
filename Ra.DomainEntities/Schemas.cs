namespace Ra.DomainEntities
{
    public class AdaSchema : EntityBase
    {
        #region Properties

        public virtual string CheckSum { get; set; }
        public virtual string FileName { get; set; }

        #endregion
    }

    public class AdaGisSchema : AdaSchema
    {
    }

    public class AdaIndexSchema : AdaSchema
    {
        #region Properties

        public virtual string Version { get; set; }

        #endregion
    }
}