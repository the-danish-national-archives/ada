namespace Ada.Checks.Gml
{
    #region Namespace Using

    using ChecksBase;

    #endregion

    public class GmlSchemaNotFound : AdaAvViolation
    {
        #region  Constructors

        public GmlSchemaNotFound(string name)
            : base("5.G_2")
        {
            Name = name;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public string Name { get; set; }

        #endregion
    }
}