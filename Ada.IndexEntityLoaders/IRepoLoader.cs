namespace IndexEntityLoaders
{
    #region Namespace Using

    using System.Collections.Generic;
    using Ra.Common;
    using Ra.Common.Xml;

    #endregion

    public interface IRepoLoader<out TEntity>
    {
        #region

        IEnumerable<TEntity> Load();

        #endregion
    }

    public interface IXmlRepoLoader<out TEntity>
    {
        #region

        IEnumerable<TEntity> Load(IArchivalXmlReader reader, BufferedProgressStream stream, BufferedProgressStream schema);

        #endregion
    }
}