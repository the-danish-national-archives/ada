namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.SqlEditor
{
    #region Namespace Using

    using System;
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit.CodeCompletion;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Editing;
    using Ra.DomainEntities.TableIndex;

    #endregion

    /// <summary>
    ///     Implements AvalonEdit ICompletionData interface to provide the entries in the completion drop down.
    /// </summary>
    public class SqlSuggestion : ICompletionData
    {
        #region  Constructors

        public SqlSuggestion(string text, string type = "not set", object data = null)
        {
            Type = type;
            Data = data;
            Text = text;
            Content = new SqlSuggestionEmptyView {DataContext = this};
        }

        public SqlSuggestion(Table table) : this(table.Name, "Table", table)
        {
            Content = new SqlSuggestionTableView {DataContext = this};
        }

        public SqlSuggestion(Column column) : this(column.Name, "Column", column)
        {
            Content = new SqlSuggestionColumnView {DataContext = this};
        }

        #endregion

        #region Properties

        public object Data { get; }

        public string Type { get; }

        #endregion

        #region ICompletionData Members

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, Text);
        }

        public ImageSource Image => null;

        public string Text { get; }

        public object Content { get; }

        public object Description => $"{Type}";

        public double Priority => 0;

        #endregion
    }
}