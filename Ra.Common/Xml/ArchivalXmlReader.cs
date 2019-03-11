// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchivalXmlReader.cs" company="Rigsarkivet">
//   
// </copyright>
// <summary>
//   Generic xml reader with custom error handling.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Xml
{
    #region Namespace Using

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using Serialization;

    #endregion

    /// <summary>
    ///     Generic xml reader with custom error handling.
    /// </summary>
    public class ArchivalXmlReader : IArchivalXmlReader
    {
        #region  Fields

        /// <summary>
        ///     The validation filter.
        /// </summary>
        protected readonly IXmlEventFilter eventFilter;

        protected readonly ArchivalXmlReaderSettings settings;

        protected readonly XmlReaderSettings xmlReaderSettings;

        protected BufferedProgressStream currentSchemaStream;

        protected BufferedProgressStream currentXmlStream;


        /// <summary>
        ///     The reader.
        /// </summary>
        protected XmlReader reader;

        #endregion

        #region  Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArchivalXmlReader" /> class.
        /// </summary>
        /// <param name="schemas">
        ///     The schemas.
        /// </param>
        /// <param name="eventFilter">
        ///     The validation filter.
        /// </param>
        public ArchivalXmlReader(IXmlEventFilter eventFilter, ArchivalXmlReaderSettings settings = null)
        {
            this.eventFilter = eventFilter;
            this.settings = settings ?? new ArchivalXmlReaderSettings();

            xmlReaderSettings = new XmlReaderSettings
            {
                CloseInput = true,
                ValidationType = ValidationType.Schema,
                IgnoreComments = true,
                IgnoreWhitespace = false,
                ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings
            };
            xmlReaderSettings.ValidationEventHandler += XmlValidationEventHandler;
        }

        #endregion

        #region IArchivalXmlReader Members

        /// <summary>
        ///     The xml event.
        /// </summary>
        public event XmlEventHandler XmlEvent;

        /// <summary>
        ///     The close.
        /// </summary>
        public void Close()
        {
            currentXmlStream = null;
            reader.Close();
            reader.Dispose();
        }

        /// <summary>
        ///     The deserialize.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public object Deserialize(Type type)
        {
            object result = null;
            try
            {
                result = Serializer.XMLDeserialize(reader, type);
            }
            catch (Exception xe)
            {
                XmlExceptionHandler(xe);
            }

            return result;
        }

        public void Open(BufferedProgressStream stream, BufferedProgressStream schemaStream)
        {
            PrepareReader(stream, PrepareSettings(schemaStream));
        }


        /// <summary>
        ///     The read.
        /// </summary>
        public void ReadToEnd()
        {
            try
            {
                ReadAll();
            }
            catch (Exception xe)
            {
                XmlExceptionHandler(xe);
            }
        }

        #endregion

        #region

        private void CheckDeclaration(XmlReader reader, XmlEventType prologErrorType, XmlEventType encodingErrorType, FileInfo sourceFile)
        {
            var declaration = GetDeclaration(reader);
            if (declaration == null)
            {
                ProcessEvent(new XmlHandlerEventArgs(prologErrorType, sourceFile));
            }
            else
            {
                if (string.IsNullOrEmpty(declaration.Encoding)
                    || !declaration.Encoding.Equals(settings.Encoding.WebName, StringComparison.OrdinalIgnoreCase))
                    ProcessEvent(
                        new XmlHandlerEventArgs(encodingErrorType, sourceFile));
            }
        }


        /// <summary>
        ///     The element stream.
        /// </summary>
        /// <param name="elementName">
        ///     The element name.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public IEnumerable<XElement> ElementStream(string elementName)
        {
            var reading = true;
            XElement el = null;
            while (reading)
            {
                try
                {
                    el = null;
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == elementName && reader.IsStartElement(elementName))
                        el = XNode.ReadFrom(reader) as XElement;
                    else
                        reading = reader.ReadToFollowing(elementName);
                }
                catch (Exception e)
                {
                    if (e is XmlException || e is XmlSchemaException)
                        XmlExceptionHandler(e);
                    else
                        throw;
                }

                if (el != null) yield return el;
            }
        }

        private XDeclaration GetDeclaration(XmlReader reader)
        {
            try
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.XmlDeclaration)
                    return new XDeclaration(
                        reader.GetAttribute("version"),
                        reader.GetAttribute("encoding"),
                        null);
            }
            catch (Exception e)
            {
                XmlExceptionHandler(e);
            }

            return null;
        }

        public XDocument GetXDocument()
        {
            var xDoc = new XDocument();
            try
            {
                XDocument.Load(reader);
            }
            catch (Exception xe)
            {
                XmlExceptionHandler(xe);
                xDoc = null;
            }

            return xDoc;
        }


        /// <summary>
        ///     The get xml doc.
        /// </summary>
        /// <returns>
        ///     The <see cref="XmlDocument" />.
        /// </returns>
        public XmlDocument GetXmlDoc()
        {
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(reader);
            }
            catch (Exception xe)
            {
                XmlExceptionHandler(xe);
                xmlDoc = null;
            }

            return xmlDoc;
        }

        protected void LoadOfflineSchema(XmlSchemaSet schemaSet, Stream stream)
        {
            var readerSettings = new XmlReaderSettings();
            readerSettings.DtdProcessing = DtdProcessing.Ignore;
            readerSettings.CloseInput = true;

            var schemaReader = XmlReader.Create(stream, readerSettings);

            CheckDeclaration(
                schemaReader,
                XmlEventType.SchemaMissingProlog,
                XmlEventType.SchemaDeclaredEncodingIllegal,
                currentSchemaStream.file);


            schemaSet.Add(null, schemaReader);
        }

        protected virtual XmlSchemaSet LoadSchemas(BufferedProgressStream schemaStream)
        {
            var schemas = new XmlSchemaSet();
            if (schemaStream != null)
            {
                currentSchemaStream = schemaStream;
//                schemas = 
                LoadOfflineSchema(schemas, schemaStream);
            }

            schemas.Compile();
            return schemas;
        }

        /// <summary>
        ///     The on xml event.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void OnXmlEvent(XmlHandlerEventArgs e)
        {
            var eventHandler = XmlEvent;
            if (eventHandler != null) eventHandler(this, e);
        }

        private void PrepareReader(BufferedProgressStream stream, XmlReaderSettings settings)
        {
            try
            {
                reader = XmlReader.Create(stream, settings);
                currentXmlStream = stream;
                CheckDeclaration(
                    reader,
                    XmlEventType.XmlMissingProlog,
                    XmlEventType.XmlDeclaredEncodingIllegal,
                    currentXmlStream.file);
            }
            catch (Exception xe)
            {
                XmlExceptionHandler(xe);
            }
        }

        private XmlReaderSettings PrepareSettings(BufferedProgressStream schemaStream)
        {
            var settings = xmlReaderSettings.Clone();
            try
            {
                var schemas = LoadSchemas(schemaStream);

                settings.Schemas = schemas;
            }
            catch (XmlSchemaException e)
            {
                SchemaExceptionHandler(e);
            }

            return settings;
        }

        private void ProcessEvent(XmlHandlerEventArgs args)
        {
            if (args.OriginalException != null)
                if (eventFilter.ShouldRethrow(args.OriginalException))
                    throw args.OriginalException;

            var filteredArgs = eventFilter.ProcessEventArgs(args);
            OnXmlEvent(filteredArgs);
        }


        protected virtual void ReadAll()
        {
            while (reader.Read())
            {
            }
        }

        private void SchemaExceptionHandler(Exception e)
        {
            var args = new XmlHandlerEventArgs(XmlEventType.SchemaWellFormedNessError, currentSchemaStream.file, e);
            ProcessEvent(args);
        }

        protected void SchemaValidationEventHandler(object sender, ValidationEventArgs e)
        {
            var eventType = new XmlEventType();
            if (e.Severity == XmlSeverityType.Error) eventType = XmlEventType.SchemaValidationError;

            if (e.Severity == XmlSeverityType.Warning) eventType = XmlEventType.SchemaValidationWarning;

            var args = new XmlHandlerEventArgs(eventType, currentSchemaStream.file, e.Exception);
            ProcessEvent(args);
        }

        private void XmlExceptionHandler(Exception e)
        {
            var args = new XmlHandlerEventArgs(XmlEventType.XmlWellFormednessError, currentXmlStream.file, e);
            ProcessEvent(args);
        }


        /// <summary>
        ///     The xml validation event reader.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void XmlValidationEventHandler(object sender, ValidationEventArgs e)
        {
            var eventType = new XmlEventType();
            if (e.Severity == XmlSeverityType.Error) eventType = XmlEventType.XmlValidationError;

            if (e.Severity == XmlSeverityType.Warning) eventType = XmlEventType.XmlValidationWarning;

            var args = new XmlHandlerEventArgs(eventType, currentXmlStream.file, e.Exception);
            ProcessEvent(args);
        }

        #endregion
    }
}