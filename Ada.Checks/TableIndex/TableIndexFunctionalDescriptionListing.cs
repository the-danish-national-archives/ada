namespace Ada.Checks.TableIndex
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.Common.ExtensionMethods;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class TableIndexFunctionalDescriptionListing : AdaAvCheckNotification
    {
        #region  Constructors

        public TableIndexFunctionalDescriptionListing(IEnumerable<string> functionalDescriptionUsageList)
            : base("6.C_14_2")
        {
            ListOfNames = functionalDescriptionUsageList;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTagSmartToString(Seperator = "\n\t\t")]
        //"¤¤")]
        public IEnumerable<string> ListOfNames { get; set; }

        #endregion

        #region

        public static IEnumerable<AdaAvCheckNotification> Create(ILookup<FunctionalDescription, Column> colFuncDesc)
        {
            var functionalDescriptionUsageList = new List<string>();
            foreach (FunctionalDescription funcDesc in Enum.GetValues(typeof(FunctionalDescription)))
            {
                string ColToString(Column c)
                {
                    return $"{c.ParentTable.Name}.{c.Name} ({c.ParentTable.Folder}.{c.ColumnId})";
                }

                var colGroup = colFuncDesc[funcDesc];

                functionalDescriptionUsageList.Add(
                    $"{funcDesc}: {string.Join(", ", colGroup.Count().ToString().Yield().Union(colGroup.Select(ColToString)))}");
            }

            yield return new TableIndexFunctionalDescriptionListing(functionalDescriptionUsageList);
        }

        #endregion
    }
}