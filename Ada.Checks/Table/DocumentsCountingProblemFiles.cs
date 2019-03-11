namespace Ada.Checks.Table
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChecksBase;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class DocumentsCountingProblemFiles : AdaAvViolation
    {
        #region  Constructors

        public DocumentsCountingProblemFiles(int count1, int count2, int count3)
            : base("6.C_29")
        {
            Count1 = count1;
            Count2 = count2;
            Count3 = count3;
        }

        #endregion

        #region Properties

        [AdaAvCheckNotificationTag]
        public int Count1 { get; set; }

        [AdaAvCheckNotificationTag]
        public int Count2 { get; set; }

        [AdaAvCheckNotificationTag]
        public int Count3 { get; set; }

        #endregion

        #region Nested type: DocumentsCountingProblemFilesSummery

        public class DocumentsCountingProblemFilesSummery
        {
            #region  Fields

            private int count1;
            private int count2;
            private int count3;

            #endregion

            #region  Constructors

            public DocumentsCountingProblemFilesSummery()
            {
                count1 = 0;
                count2 = 0;
                count3 = 0;
            }

            #endregion

            #region

            /// <summary>
            ///     Returns a function to be called for each row of that table, to update the summary. If not needed to be called null
            ///     is returned.
            /// </summary>
            /// <param name="table"></param>
            /// <returns></returns>
            public Action<SortedList<string, string>> GetCheckFunc(Table table)
            {
                var docIdCols = table.Columns
                    .Where(c => c.FunctionalDescriptions.Contains(FunctionalDescription.Dokumentidentifikation))
                    .ToList();

                if (docIdCols.Count == 0)
                    return null;

                return rowContents =>
                {
                    foreach (var docIdCol in docIdCols)
                    {
                        var value = rowContents[docIdCol.ColumnId];
                        switch (value)
                        {
                            case "99999991":
                                count1++;
                                break;
                            case "99999992":
                                count2++;
                                break;
                            case "99999993":
                                count3++;
                                break;
                        }
                    }
                };
            }

            public IEnumerable<AdaAvCheckNotification> Report()
            {
                yield return new DocumentsCountingProblemFiles(count1, count2, count3);
            }

            #endregion
        }

        #endregion
    }
}