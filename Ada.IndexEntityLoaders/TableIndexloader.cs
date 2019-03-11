namespace Ada.EntityLoaders
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using Ra.Common;
    using Ra.Common.Xml;
    using Ra.DomainEntities.TableIndex;

    #endregion

    public static class TableIndexloader
    {
        #region

        public static TableIndex Load(IArchivalXmlReader reader, BufferedProgressStream stream, BufferedProgressStream schema)
        {
            reader.Open(stream, schema);
            var indexFromXml = (Ra.XmlEntities.TableIndex.TableIndex) reader.Deserialize(typeof(Ra.XmlEntities.TableIndex.TableIndex));
            reader.Close();
            var index = new TableIndex
            {
                DbName = indexFromXml.DbName,
                DbProduct = indexFromXml.DbProduct,
                Version = indexFromXml.Version,
                SessionKey = Guid.NewGuid(),
                Tables = new List<Table>(),
                Views = new List<View>()
            };

            foreach (var tableFromXml in indexFromXml.Tables)
            {
                var table = new Table
                {
                    TableIndex = index,
                    Name = tableFromXml.Name,
                    Description = tableFromXml.Description,
                    Folder = tableFromXml.Folder,
                    Rows = tableFromXml.Rows,
                    Columns = new List<Column>(),
                    ForeignKeys = new List<ForeignKey>()
                };

                var primarykey = new PrimaryKey
                {
                    Name = tableFromXml.PrimaryKey.Name,
                    ParentTableName = tableFromXml.Name,
                    ParentTable = table,
                    Columns = tableFromXml.PrimaryKey.Columns
                };
                table.PrimaryKey = primarykey;

                foreach (var foreignkeyFromXml in tableFromXml.ForeignKeys)
                {
                    var foreignKey = new ForeignKey
                    {
                        Name = foreignkeyFromXml.Name,
                        ParentTableName = table.Name,
                        ParentTable = table,
                        ReferencedTable = foreignkeyFromXml.ReferencedTable,
                        References = new List<Reference>()
                    };

                    foreach (var referenceFromXml in foreignkeyFromXml.References)
                    {
                        var reference = new Reference
                        {
                            Column = referenceFromXml.Column,
                            Referenced = referenceFromXml.Referenced,
                            ParentConstraint = foreignKey
                        };
                        foreignKey.References.Add(reference);
                    }

                    table.ForeignKeys.Add(foreignKey);
                }

                foreach (var columnFromXml in tableFromXml.Columns)
                {
                    var column = new Column
                    {
                        Type = columnFromXml.Type,
                        TypeOriginal = columnFromXml.TypeOriginal,
                        ColumnId = columnFromXml.ColumnId,
                        DefaultValue = columnFromXml.DefaultValue,
                        Description = columnFromXml.Description,
                        Name = columnFromXml.Name,
                        Nullable = columnFromXml.Nullable,
                        ParentTable = table,
                        TableName = table.Name,
                        FunctionalDescriptions = new List<FunctionalDescription>()
                    };
                    foreach (var functionalDescriptionFromXml in columnFromXml.FunctionalDescriptions)
                        switch (functionalDescriptionFromXml)
                        {
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Myndighedsidentifikation:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Myndighedsidentifikation);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Dokumentidentifikation:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Dokumentidentifikation);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Lagringsform:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Lagringsform);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Afleveret:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Afleveret);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Sagsidentifikation:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Sagsidentifikation);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Sagstitel:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Sagstitel);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Dokumenttitel:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Dokumenttitel);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Dokumentdato:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Dokumentdato);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Afsender_modtager:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Afsender_modtager);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Digital_signatur:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Digital_signatur);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.FORM:
                                column.FunctionalDescriptions.Add(FunctionalDescription.FORM);
                                break;
                            case Ra.XmlEntities.TableIndex.FunctionalDescription.Kassation:
                                column.FunctionalDescriptions.Add(FunctionalDescription.Kassation);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                    table.Columns.Add(column);
                }

                index.Tables.Add(table);
            }

            foreach (var viewFromXml in indexFromXml.Views)
            {
                var view = new View
                {
                    TableIndex = index,
                    Description = viewFromXml.Description,
                    Name = viewFromXml.Name,
                    QueryOriginal = viewFromXml.OriginalQuery
                };
                index.Views.Add(view);
            }

            return index;
        }

        #endregion
    }
}