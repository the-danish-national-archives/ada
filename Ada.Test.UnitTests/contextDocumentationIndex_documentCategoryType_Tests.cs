namespace Ada.Test.UnitTests
{
    #region Namespace Using

    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using NUnit.Framework;
    using Ra.XmlEntities.ContextDocumentationIndex;

    #endregion

    [TestFixture]
    public class contextDocumentationIndex_documentCategoryType_Tests
    {
        private static void CheckXmlType
        (
            XmlSchema schema,
            XmlSchemaType schemaType,
            XmlQualifiedName schemaTypeName,
            Type type)
        {
            if (schemaType == null)
            {
                var elementType = GetElement(schema, schemaTypeName.Name);
                var complexType = GetComplexType(schema, schemaTypeName.Name);
                var simpleType = GetSimpleType(schema, schemaTypeName.Name);

                if (elementType != null)
                {
                    CheckXmlType(schema, elementType, type);
                }
                else if (complexType != null)
                {
                    CheckXmlType(schema, complexType, type);
                }
                else if (simpleType != null)
                {
                    // ignore
                }
                else if (schemaTypeName.Namespace != schema.TargetNamespace)
                {
                    // ignore
                }
                else
                {
                    Assert.Fail("Cannot find schemaType");
                }
            }
            else
            {
                if (schemaType is XmlSchemaComplexType)
                {
                    CheckXmlType(schema, (XmlSchemaComplexType) schemaType, type);
                }
                else if (schemaType is XmlSchemaSimpleType)
                {
//                  Not checked
// 
                }
                else
                {
                    Assert.Fail("Unexpected XmlSchemaType");
                }
            }
        }

        private static void CheckXmlType(XmlSchema schema, XmlSchemaComplexType schemaType, Type type)
        {
            if (type.GenericTypeArguments.Length != 0) type = type.GenericTypeArguments[0];

            var categoryTypes = type.GetProperties();
            var sequence = schemaType?.Particle as XmlSchemaSequence;
            foreach (var item in sequence?.Items.Cast<XmlSchemaObject>() ?? Enumerable.Empty<XmlSchemaObject>())
            {
                var element = item as XmlSchemaElement;
                Assert.NotNull(element);

                var prop =
                    categoryTypes.FirstOrDefault(
                        m => m.GetCustomAttribute<XmlElementAttribute>()?.ElementName == element.Name);
                Assert.NotNull(prop, $"element type from xsd not found: {element.Name}, in {type.FullName}");
                CheckXmlType(schema, element.SchemaType, element.SchemaTypeName, prop.PropertyType);
            }
        }

        private static void CheckXmlType(XmlSchema schema, XmlSchemaElement schemaElement, Type type)

        {
            var categoryTypes = type.GetProperties();

            var complexType = schemaElement?.SchemaType as XmlSchemaComplexType;
            var sequence = complexType?.Particle as XmlSchemaSequence; // Assuming type is list
            foreach (var item in sequence?.Items.Cast<XmlSchemaObject>() ?? Enumerable.Empty<XmlSchemaObject>())
            {
                var element = item as XmlSchemaElement;
                Assert.NotNull(element);

                var prop = categoryTypes.First(m => m.GetCustomAttribute<XmlElementAttribute>()?.ElementName == element.Name);

                Assert.NotNull(prop, $"element type from xsd not found: {element.Name}, in {type.FullName}");
                CheckXmlType(schema, element.SchemaType, element.SchemaTypeName, prop.PropertyType.GenericTypeArguments[0]);
            }
        }

        private static XmlSchemaElement GetElement(XmlSchema schema, string schemaTypeName)
        {
            // Element
            return
                schema.Items.Cast<XmlSchemaObject>()
                    .OfType<XmlSchemaElement>()
                    .FirstOrDefault(t => t.Name == schemaTypeName);
        }

        private static XmlSchemaComplexType GetComplexType(XmlSchema schema, string schemaTypeName)
        {
            return schema.Items.Cast<XmlSchemaObject>()
                .OfType<XmlSchemaComplexType>()
                .FirstOrDefault(t => t.Name == schemaTypeName);
        }

        private static XmlSchemaSimpleType GetSimpleType(XmlSchema schema, string schemaTypeName)
        {
            return schema.Items.Cast<XmlSchemaObject>()
                .OfType<XmlSchemaSimpleType>()
                .FirstOrDefault(t => t.Name == schemaTypeName);
        }

        [Test]
        public void Xml_mapping()
        {
            var schemaStream =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("Ada.Test.UnitTests.contextDocumentationIndex.xsd");

            var schema = XmlSchema.Read(
                schemaStream,
                (sender, args) => Assert.Fail("Initialization error, loading xml-schema"));


            var type = typeof(ContextDocumentationIndex);
            var schemaTypeName = "contextDocumentationIndex";


            CheckXmlType(schema, null, new XmlQualifiedName(schemaTypeName), type);
        }
    }
}