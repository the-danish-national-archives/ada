// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableIndexExtensions.cs" company="Rigsarkivet">
// </copyright>
// <summary>
//   The table index extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.EntityExtensions.TableIndex
{
    #region Namespace Using

    using System.Collections.Generic;
    using System.Linq;
    using DomainEntities.TableIndex;

    #endregion

    /// <summary>
    ///     The table index extensions.
    /// </summary>
    public static class TableIndexExtensions
    {
        #region

        public static IEnumerable<ForeignKey> AllForeignKeys(this TableIndex index)
        {
            return index.Tables.SelectMany(t => t.ForeignKeys);
        }

        public static IEnumerable<PrimaryKey> AllPrimaryKeys(this TableIndex index)
        {
            return index.Tables.Select(t => t.PrimaryKey);
        }

        #endregion
    }
}