namespace Ada.Checks.Gml
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ChecksBase;
    using Repositories;

    #endregion

    public class GmlOgcSchemasMissing : AdaAvDynamicFromSql
    {
        #region  Constructors

        public GmlOgcSchemasMissing()
            : base("5.G_1")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(IAdaUowFactory testFactory)
        {
            return CheckInstance(typeof(GmlOgcSchemasMissing), testFactory);
        }

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT name AS Name FROM gisSchemas EXCEPT SELECT name FROM sharedSchemasOnDisk";
        }

        #endregion
    }
}