namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.Model
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Properties;

    #endregion

    public class FavoriteQueries : INotifyPropertyChanged
    {
        #region Static

        private static FavoriteQueries _default;

        #endregion

        #region  Constructors

        private FavoriteQueries()
        {
            var list = Settings.Default.FavoriteQueries;
            if (list == null) list = new QueryList();
            Settings.Default.FavoriteQueries = list;
            Settings.Default.Save();
            Settings.Default.FavoriteQueries.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Queries))
                    OnPropertyChanged(nameof(Queries));
            };
        }

        #endregion

        #region Properties

        public static FavoriteQueries Default => _default ?? (_default = new FavoriteQueries());

        public IEnumerable<Query> Queries => Settings.Default.FavoriteQueries.Queries;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        /// <summary>
        ///     Will add the query or if it already exist move it to the from, as determined by it's value
        /// </summary>
        /// <param name="query"></param>
        public void Add(Query query)
        {
            var queries = Settings.Default.FavoriteQueries.Queries;
            var earlierIndex = Array.FindIndex(queries, q => q?.Value == query.Value);
            if (earlierIndex < 0)
            {
                if (queries.Length < GetMaxFavoriteQueries()) Array.Resize(ref queries, queries.Length + 1);
                earlierIndex = queries.Length - 1;
            }


            while (earlierIndex > 0)
            {
                queries[earlierIndex] = queries[earlierIndex - 1];
                earlierIndex--;
            }

            queries[0] = query;

            Settings.Default.FavoriteQueries.Queries = queries;
            Settings.Default.Save();
        }

        private static int GetMaxFavoriteQueries()
        {
            return int.MaxValue;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Will remove first query in the list with same value
        /// </summary>
        /// <param name="query"></param>
        public void Remove(Query query)
        {
            var queries = Settings.Default.FavoriteQueries.Queries;
            var earlierIndex = Array.FindIndex(queries, q => q?.Value == query.Value);
            if (earlierIndex < 0) return;


            while (earlierIndex < queries.Length - 1)
            {
                queries[earlierIndex] = queries[earlierIndex + 1];
                earlierIndex++;
            }

            Array.Resize(ref queries, queries.Length - 1);

            Settings.Default.FavoriteQueries.Queries = queries;
            Settings.Default.Save();
        }

        #endregion
    }
}