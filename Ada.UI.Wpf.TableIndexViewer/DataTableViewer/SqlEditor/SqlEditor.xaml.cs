namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.SqlEditor
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Xml;
    using FluentNHibernate.Conventions;
    using GalaSoft.MvvmLight.CommandWpf;
    using ICSharpCode.AvalonEdit.CodeCompletion;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;
    using NHibernate.Util;
    using Ra.DomainEntities.TableIndex;

    #endregion

    /// <summary>
    ///     Interaction logic for SqlEditor.xaml
    /// </summary>
    public partial class SqlEditor : UserControl
    {
        #region Static

        private static SqlSuggester sqlSuggester;


        public static readonly DependencyProperty TableIndexProperty = DependencyProperty.Register(
            "TableIndex", typeof(TableIndex), typeof(SqlEditor), new PropertyMetadata(default(TableIndex), PropertyChangedCallback));


        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(SqlEditor), new PropertyMetadata(default(string)) {PropertyChangedCallback = PropertyChangedCallback});

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(SqlEditor), new PropertyMetadata(ScrollBarVisibility.Auto));

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(SqlEditor), new PropertyMetadata(ScrollBarVisibility.Auto));

        #endregion

        #region  Fields

        private bool _changingText;

        private CompletionWindow _completionWindow;


        private RelayCommand _openSuggestionsCommand;

        #endregion

        #region  Constructors

        static SqlEditor()
        {
            var sqlHighlighting = LoadHighlightingDefinitionFromResource();

            HighlightingManager.Instance.RegisterHighlighting("SQL", new[] {".sql"}, sqlHighlighting);
        }

        public SqlEditor()
        {
            InitializeComponent();
            textEditor.TextChanged += (s, e) => AvoidSetTextRecursivly(() => Text = textEditor.Text);
            textEditor.TextArea.TextEntering += TextEditor_TextArea_TextEntering;
            textEditor.TextArea.TextEntered += TextEditor_TextArea_TextEntered;
        }

        #endregion

        #region Properties

        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get => (ScrollBarVisibility) GetValue(HorizontalScrollBarVisibilityProperty);
            set => SetValue(HorizontalScrollBarVisibilityProperty, value);
        }

        public RelayCommand OpenSuggestionsCommand
        {
            get
            {
                return _openSuggestionsCommand
                       ?? (_openSuggestionsCommand = new RelayCommand(
                           () => ShowMenu(true)));
            }
        }

        public TableIndex TableIndex
        {
            get => (TableIndex) GetValue(TableIndexProperty);
            set => SetValue(TableIndexProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get => (ScrollBarVisibility) GetValue(VerticalScrollBarVisibilityProperty);
            set => SetValue(VerticalScrollBarVisibilityProperty, value);
        }

        #endregion

        #region

        private void AddHighlightingColorToResources(string colorName, ResourceDictionary resourceDictionary)
        {
            resourceDictionary.Add(colorName, GetStyleFromHighlightingColor(textEditor.SyntaxHighlighting.GetNamedColor(colorName)));
        }

        private void AvoidSetTextRecursivly(Action a)
        {
            if (_changingText) return;
            _changingText = true;
            a();
            _changingText = false;
        }

        private static Style GetStyleFromHighlightingColor(HighlightingColor hColor)
        {
            var res = new Style(typeof(TextBlock));
            if (hColor.Foreground != null)
                res.Setters.Add(new Setter(TextBlock.ForegroundProperty, hColor.Foreground.GetBrush(null)));
            if (hColor.Background != null)
                res.Setters.Add(new Setter(TextBlock.BackgroundProperty, hColor.Background.GetBrush(null)));
            if (hColor.FontStyle != null)
                res.Setters.Add(new Setter(TextBlock.FontStyleProperty, hColor.FontStyle));
            if (hColor.FontWeight != null)
                res.Setters.Add(new Setter(TextBlock.FontWeightProperty, hColor.FontWeight));
            if (hColor.Underline ?? false)
                res.Setters.Add(new Setter(TextBlock.TextDecorationsProperty, TextDecorations.Underline));

            return res;
        }

        private static IHighlightingDefinition LoadHighlightingDefinitionFromResource()
        {
            IHighlightingDefinition sqlHighlighting;
            using (
                var s =
                    typeof(SqlEditor).Assembly.GetManifestResourceStream(
                        "Ada.UI.Wpf.TableIndexViewer.DataTableViewer.SqlEditor.SqlHighlighting.xshd"))
            {
                if (s == null) throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    sqlHighlighting = HighlightingLoader.Load(
                        reader,
                        HighlightingManager.Instance);
                }
            }

            return sqlHighlighting;
        }


        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            var sqlEdit = sender as SqlEditor;
            if (sqlEdit == null)
                return;
            switch (eventArgs.Property.Name)
            {
                case "TableIndex":
                    var tableIndex = eventArgs.NewValue as TableIndex;
                    if (tableIndex == null) return;
                    if (sqlSuggester?.TableIndex != tableIndex) sqlSuggester = new SqlSuggester(tableIndex);
                    sqlEdit.SetSyntaxHighlighting();
                    break;
                case "Text":
                    var s = eventArgs.NewValue as string;
                    if (s == null) return;
                    sqlEdit.AvoidSetTextRecursivly(
                        () =>
                        {
                            try
                            {
                                sqlEdit.textEditor.Text = s;
                                sqlEdit.textEditor.SelectionStart = s.Length;
                            }
                            catch (Exception)
                            {
                            }
                        });
                    break;
#if DEBUG
                default:
                    throw new InvalidOperationException($"DependencyProperty {eventArgs.Property.Name} called back, but not handled");
#endif
            }
        }

        private static void SetHighlightingRuleWords(IHighlightingDefinition highlightingDefinition, string colorName, IEnumerable<string> words)
        {
            var color = highlightingDefinition.GetNamedColor(colorName);
            var rule = highlightingDefinition.MainRuleSet.Rules.FirstOrDefault(hr => hr.Color.Equals(color));
            if (rule == null)
                throw new InvalidOperationException("No rule with color '{colorName}' found");

            // More or less taken from XmlHighlightingDefinition.cs: 213 VisitKeyWords
            // Beware: 
            // We can use "\b" only where the keyword starts/ends with a letter or digit, otherwise we don't
            // highlight correctly. (example: ILAsm-Mode.xshd with ".maxstack" keyword)
            //            ColumnName
            var keyWordRegex = new StringBuilder();

            keyWordRegex.Append(@"\b(?>");
            // (?> = atomic group
            // atomic groups increase matching performance, but we
            // must ensure that the keywords are sorted correctly.
            // "\b(?>in|int)\b" does not match "int" because the atomic group captures "in".
            // To solve this, we are sorting the keywords by descending length.
            var i = 0;
            foreach (var word in words.OrderByDescending(w => w.Length))
            {
                if (i++ > 0) keyWordRegex.Append('|');
                keyWordRegex.Append(Regex.Escape(word));
            }

            keyWordRegex.Append(@")\b");

            var options = RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase;

            rule.Regex = new Regex(keyWordRegex.ToString(), options);
        }

        private void SetSyntaxHighlighting()
        {
            var sqlHighlighting = LoadHighlightingDefinitionFromResource();

            SetHighlightingRuleWords(sqlHighlighting, "TableName", TableIndex.Tables.Select(t => t.Name));
            SetHighlightingRuleWords(sqlHighlighting, "ColumnName", TableIndex.Tables.SelectMany(t => t.Columns).Select(c => c.Name));

            textEditor.SyntaxHighlighting = sqlHighlighting;
        }


        private void ShowMenu(bool showWhenEmpty)
        {
            int startOffset;
            var endOffset = textEditor.CaretOffset;
            var tempSuggestions = sqlSuggester.GetSuggestions(textEditor.Document.GetText(0, endOffset), out startOffset);
            var suggestions = tempSuggestions as IList<SqlSuggestion> ?? tempSuggestions.ToList();

            if (suggestions.IsEmpty() && !showWhenEmpty)
                return;


            _completionWindow = new CompletionWindow(textEditor.TextArea);
            AddHighlightingColorToResources("TableName", _completionWindow.Resources);
            AddHighlightingColorToResources("ColumnName", _completionWindow.Resources);

            _completionWindow.StartOffset = startOffset;
            _completionWindow.HorizontalAlignment = HorizontalAlignment.Stretch;
            _completionWindow.SizeToContent = SizeToContent.WidthAndHeight;

            var data = _completionWindow.CompletionList.CompletionData;
            suggestions.ForEach(s => data.Add(s));


            _completionWindow.CompletionList.SelectItem(textEditor.Document.GetText(startOffset, endOffset - startOffset));

            // switching between view for the completion window when it has suggestions, or it does not
            var emptyView = new SqlSuggestionEmptyView();
            var correctView = _completionWindow.Content;

            Action updateView = () =>
            {
                var box = _completionWindow.CompletionList.ListBox;
                if (box.Items.Count == 0)
                {
                    if (!ReferenceEquals(_completionWindow.Content, emptyView)) _completionWindow.Content = emptyView;
                }
                else
                {
                    if (!ReferenceEquals(_completionWindow.Content, correctView)) _completionWindow.Content = correctView;
                }
            };

            updateView();

            _completionWindow.CompletionList.SelectionChanged += (sender, args) => updateView();

            _completionWindow.Closed += delegate { _completionWindow = null; };

            _completionWindow.Show();
        }

        private void TextEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ".") ShowMenu(false);
        }

        private void TextEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && _completionWindow != null)
                if (!char.IsLetterOrDigit(e.Text[0]))
                    _completionWindow.CompletionList.RequestInsertion(e);
            // e.Handled not set true, as symbol should still be entered
        }

        #endregion
    }
}