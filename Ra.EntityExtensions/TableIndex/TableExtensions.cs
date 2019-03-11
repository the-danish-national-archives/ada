namespace Ra.EntityExtensions.TableIndex
{
    #region Namespace Using

    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Schema;
    using DomainEntities.TableIndex;

    #endregion

    public static class TableExtensions
    {
        #region Static

        private static readonly Regex RxValidFolderName = new Regex(@"^table\d+$");

        #endregion

        #region

        public static bool FolderNameIsValid(this Table table)
        {
            var match = RxValidFolderName.Match(table.Folder);
            return match.Success;
        }

        public static XmlSchema GetXmlSchema(this Table table)
        {
            var tableType = new XmlSchemaComplexType();
            var tableSequence = new XmlSchemaSequence();
            tableType.Particle = tableSequence;
            tableSequence.Items.Add(
                new XmlSchemaElement
                {
                    Name = "row",
                    SchemaTypeName = new XmlQualifiedName("rowType"),
                    MinOccurs = 0,
                    MaxOccursString = "unbounded"
                });
            var tableElement = new XmlSchemaElement {Name = "table", SchemaType = tableType};

            var rowType = new XmlSchemaComplexType {Name = "rowType"};
            var rowSequence = new XmlSchemaSequence();
            rowType.Particle = rowSequence;

            foreach (var column in table.Columns)
                rowSequence.Items.Add(
                    new XmlSchemaElement
                    {
                        Name = column.ColumnId,
                        MinOccurs = 1,
                        SchemaTypeName = new XmlQualifiedName("xs:" + column.GetXmlMappedType()),
                        IsNillable = column.Nullable
                    });

            var schema = new XmlSchema
            {
                TargetNamespace =
                    "http://www.sa.dk/xmlns/siard/1.0/schema0/" + string.Concat(table.Folder, ".xsd"),
                ElementFormDefault = XmlSchemaForm.Qualified,
                AttributeFormDefault = XmlSchemaForm.Unqualified
            };

            schema.Namespaces.Add(
                string.Empty,
                "http://www.sa.dk/xmlns/siard/1.0/schema0/" + string.Concat(table.Folder, ".xsd"));
            schema.Items.Add(tableElement);
            schema.Items.Add(rowType);

            return schema;
        }

        public static MemoryStream GetXmlSchemaStream(this Table table)
        {
            var schema = table.GetXmlSchema();
            var memoryStream = new MemoryStream();

            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8
            };

            using (var xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
            {
                schema.Write(xmlWriter);
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        #endregion
    }
}