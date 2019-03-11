namespace Ada.Checks.Gml
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using ChecksBase;
    using Repositories;

    #endregion

    public class GmlOgcSchemasInvalid : AdaAvDynamicFromSql
    {
        #region  Constructors

        public GmlOgcSchemasInvalid()
            : base("5.G_3")
        {
        }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Check(IAdaUowFactory testFactory)
        {
            return CheckInstance(typeof(GmlOgcSchemasInvalid), testFactory);
        }

        protected override string GetTestQuery(Type type)
        {
            return
                @"SELECT gisSchemas.name AS Name FROM gisSchemas INNER JOIN sharedSchemasOnDisk ON gisSchemas.name = sharedSchemasOnDisk.name AND gisSchemas.MD5 != sharedSchemasOnDisk.checksum";
        }

        #endregion
    }
}