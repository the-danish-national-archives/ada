namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    #endregion

    public class TableToCsv : ICommand
    {
        #region Properties

        public bool AutoOpen { get; set; }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var table = parameter as DataTable;
            if (table == null)
                return;

            var filename = Path.GetTempFileName().Replace(".tmp", ".csv");
            var fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
                fileInfo.Delete();

            var fileArray = GetCsvText(table);

            fileInfo.Refresh();
            using (var fileStream = fileInfo.Create())
            {
                fileStream.Write(fileArray, 0, fileArray.Length);
            }


            if (AutoOpen)
                Process.Start(filename);
        }

        public event EventHandler CanExecuteChanged;

        #endregion

        #region

        private byte[] GetCsvText(DataTable table)
        {
            var seperator = ",";

            string CleanString(string input)
            {
                return input
                    .Replace(seperator, " ")
                    .Replace(Environment.NewLine, " ")
                    .Replace(@"\", @"\\")
                    .Replace("\n", @"\n");
            }

            var outputBuilder = new StringBuilder();


            var columns = table.Columns.OfType<DataColumn>()
                .Where(c => c.DataType == typeof(string))
                .Where(c => (bool?) c.ExtendedProperties[HideColumns.ExtendedPropertyForHideName] ?? true)
                .ToList();


            outputBuilder.AppendLine(string.Join(seperator, columns.Select(c => CleanString(c.ColumnName))));

            foreach (var row in table.Rows.OfType<DataRow>()) outputBuilder.AppendLine(string.Join(seperator, columns.Select(c => CleanString((string) row[c.ColumnName]))));


            byte[] _fileArray;

            using (var outputStream = new MemoryStream())
            using (var writer = new StreamWriter(outputStream))
            {
                writer.Write(outputBuilder);
                writer.Flush();
                _fileArray = outputStream.ToArray();
            }


            return _fileArray;
        }

        #endregion
    }
}