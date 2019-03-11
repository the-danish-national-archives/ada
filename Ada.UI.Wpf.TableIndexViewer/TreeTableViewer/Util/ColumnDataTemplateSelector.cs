namespace Ada.UI.Wpf.TableIndexViewer.TreeTableViewer.Util
{
    #region Namespace Using

    using System.Windows;
    using System.Windows.Controls;
    using FluentNHibernate.Conventions;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public class ColumnDataTemplateSelector : DataTemplateSelector
    {
        #region

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null)
            {
                var resource = "";
                if (item is Column)
                    resource = "ColumnDataTemplate";
                if (item is ForeignKey)
                    resource = "ForeignKeyDataTemplate";
                if (item is Reference)
                    resource = "ReferenceDataTemplate";

                if (resource.IsNotEmpty())
                    return
                        element.FindResource(resource) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }

        #endregion
    }
}