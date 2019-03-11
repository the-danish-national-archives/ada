namespace Ra.DomainEntities.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     foreignKey element in siardDiark Archive
    /// </summary>
    public class ForeignKey : Constraint
    {
        #region Properties

        /// <summary>
        ///     Den tabel, som fremmednøglen referer til
        /// </summary>
        public virtual string ReferencedTable { get; set; }

        /// <summary>
        ///     Reference (list of columns and referenced columns)
        /// </summary>
        public virtual IList<Reference> References { get; set; }

        #endregion


        //public virtual void UpdateIntegrity()
        //{
        //    var referencedTable = this.ParentTable.TableIndex.Tables.Find(x => x.Name.Equals(this.ReferencedTable));
        //    if (referencedTable != null)
        //    {
        //        this.ReferencedTableExists = true;
        //        var pkColumns = referencedTable.PrimaryKey.Columns;
        //        var refColumns = this.References.Select(x => x.Referenced).ToList();
        //        var missingRef = pkColumns.Except(refColumns).Any();
        //        var invalidRef = refColumns.Except(pkColumns).Any();

        //        if (!missingRef && !invalidRef)
        //        {
        //            ReferencesFullKey = true;
        //        }

        //        if (!missingRef && invalidRef)
        //        {
        //            ReferencesPartialKey = true;
        //        }
        //    }
        //}

        //[XmlIgnore]
        //public virtual bool ReferencedTableExists { get; protected set; }
        //[XmlIgnore]
        //public virtual bool ReferencesFullKey { get; protected set; }
        //[XmlIgnore]
        //public virtual bool ReferencesPartialKey { get; protected set; }
        //[XmlIgnore]
        //public virtual bool MatchingDataTypes { get; protected set; }
    }
}