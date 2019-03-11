namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.SqlEditor
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class SqlSuggester
    {
        #region  Constructors

        public SqlSuggester(TableIndex tableIndex)
        {
            TableIndex = tableIndex;
        }

        #endregion

        #region Properties

        public TableIndex TableIndex { get; }

        #endregion

        #region

        private IEnumerable<SqlSuggestion> GenerateColumnNameOnly(string text)
        {
            var lastWord = GetLastWord(text);

            var table = TableIndex.Tables.FirstOrDefault(t =>
                string.Compare(t.Name, lastWord, StringComparison.OrdinalIgnoreCase) == 0);

            return GenerateFromTable(table);
        }

        protected IEnumerable<SqlSuggestion> GenerateFromTable(Table table)
        {
            if (table == null)
                yield break;
            foreach (var column in table.Columns) yield return new SqlSuggestion(column);
        }

        protected IEnumerable<SqlSuggestion> GenerateFromTableIndex(bool includeColumn = false)
        {
            foreach (var table in TableIndex.Tables)
            {
                yield return new SqlSuggestion(table);

                if (!includeColumn) continue;

                foreach (var column in table.Columns) yield return new SqlSuggestion(column);
            }
        }

        public static string GetLastWord(string text)
        {
            var (start, end) = GetLastWordPos(text);
            return text.Substring(start, end - start);
        }

        public static (int start, int end) GetLastWordPos(string text)
        {
            var end = text.Length - 1;
            for (; end >= 0; end--)
                if (char.IsLetterOrDigit(text[end]))
                    break;
            end++;
            var start = end - 1;
            for (; start >= 0; start--)
                if (!char.IsLetterOrDigit(text[start]))
                    break;
            start++;
            return (start, end);
        }

        public static int GetLastWordStart(string text)
        {
            var (start, _) = GetLastWordPos(text);
            return start;
        }

        /// <summary>
        ///     Creates suggestions for the next word to be used in a SQL query
        /// </summary>
        /// <param name="text">
        ///     Text ending where the suggestion should come
        /// </param>
        /// <param name="startOffset"></param>
        /// <returns>
        ///     An <see cref="IEnumerable{T}" />, with the generated suggestions
        /// </returns>
        public IEnumerable<SqlSuggestion> GetSuggestions(string text, out int startOffset)
        {
            if (text.Length == 0 || !char.IsLetterOrDigit(text[text.Length - 1]))
                startOffset = text.Length;
            else
                startOffset = GetLastWordStart(text);

            text = text.Substring(0, startOffset);

            var textTrimmed = text.TrimEnd();
            return textTrimmed.EndsWith(".") ? GenerateColumnNameOnly(text) : GenerateFromTableIndex(true);
        }

        #endregion
    }
}