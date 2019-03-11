namespace Ada.UI.Wpf.TableIndexViewer.DataTableViewer.ViewModel
{
    #region Namespace Using

    using System.ComponentModel;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public interface IQueryViewModel : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        ///     Gets a short description of what the query should return
        /// </summary>
        string Description { get; }

        bool HasDescription { get; }

        bool HasName { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether the query is a favorite query, setting it will change the values in
        ///     <c>FavoriteQueries</c> and <c>LatestQueries</c>.
        /// </summary>
        bool IsFavorite { get; set; }

        /// <summary>
        ///     Gets the name of the query
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the query, assumedly in SQL, also known as queryOriginal
        /// </summary>
        string Query { get; }

        /// <summary>
        ///     Gets the table index for which the Query was designed, this is also used to help with the highlighting of the query
        /// </summary>
        TableIndex TableIndex { get; }

        #endregion
    }
}