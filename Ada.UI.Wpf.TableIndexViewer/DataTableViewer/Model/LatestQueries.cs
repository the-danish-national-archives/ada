namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.Model
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Properties;

    #endregion

    public class LatestQueries : INotifyPropertyChanged
    {
        #region Static

        private static LatestQueries _default;

        #endregion

        #region  Constructors

        private LatestQueries()
        {
            var list = Settings.Default.LatestQueries;
            if (list == null) list = new QueryList();
            Settings.Default.LatestQueries = list;
            Settings.Default.Save();
            Settings.Default.LatestQueries.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Queries))
                    OnPropertyChanged(nameof(Queries));
            };
        }

        #endregion

        #region Properties

        public static LatestQueries Default => _default ?? (_default = new LatestQueries());

        public IEnumerable<Query> Queries => Settings.Default.LatestQueries.Queries;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        public void Add(Query query)
        {
            var queries = Settings.Default.LatestQueries.Queries;
            var earlierIndex = Array.FindIndex(queries, q => q?.Value == query.Value);
            if (earlierIndex < 0)
            {
                if (queries.Length < GetMaxLatestQueries()) Array.Resize(ref queries, queries.Length + 1);
                earlierIndex = queries.Length - 1;
            }


            while (earlierIndex > 0)
            {
                queries[earlierIndex] = queries[earlierIndex - 1];
                earlierIndex--;
            }

            queries[0] = query;

            Settings.Default.LatestQueries.Queries = queries;
            Settings.Default.Save();
        }

        private static int GetMaxLatestQueries()
        {
            return Settings.Default.MaxLatestQueries;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Remove(Query query)
        {
            var queries = Settings.Default.LatestQueries.Queries;
            var earlierIndex = Array.FindIndex(queries, q => q?.Value == query.Value);
            if (earlierIndex < 0) return;


            while (earlierIndex < queries.Length - 1)
            {
                queries[earlierIndex] = queries[earlierIndex + 1];
                earlierIndex++;
            }

            Array.Resize(ref queries, queries.Length - 1);

            Settings.Default.LatestQueries.Queries = queries;
            Settings.Default.Save();
        }

        #endregion
    }
}