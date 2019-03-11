// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Serializer.cs" company="">
//   
// </copyright>
// <summary>
//   The serializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ra.Common.Serialization
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    ///     The serializer.
    /// </summary>
    public static class Serializer
    {
        #region

        /// <summary>
        ///     The binary deserialize.
        /// </summary>
        /// <param name="filepath">
        ///     The filepath.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public static object BinaryDeserialize(string filepath)
        {
            var fs = getFileStream(filepath, FileMode.OpenOrCreate);
            var bf = new BinaryFormatter();
            var obj = bf.Deserialize(fs);
            fs.Close();
            return obj;
        }

        /// <summary>
        ///     The binary serialize.
        /// </summary>
        /// <param name="filepath">
        ///     The filepath.
        /// </param>
        /// <param name="obj">
        ///     The obj.
        /// </param>
        public static void BinarySerialize(string filepath, object obj)
        {
            var fs = getFileStream(filepath, FileMode.Create);
            var bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }

        /// <summary>
        ///     The get file stream.
        /// </summary>
        /// <param name="filepath">
        ///     The filepath.
        /// </param>
        /// <param name="mode">
        ///     The mode.
        /// </param>
        /// <returns>
        ///     The <see cref="FileStream" />.
        /// </returns>
        private static FileStream getFileStream(string filepath, FileMode mode)
        {
            var dirInfo = new DirectoryInfo(Path.GetDirectoryName(filepath));
            if (!dirInfo.Exists) dirInfo.Create();

            return new FileStream(filepath, mode);
        }

        /// <summary>
        ///     The xml deserialize.
        /// </summary>
        /// <param name="filepath">
        ///     The filepath.
        /// </param>
        /// <param name="T">
        ///     The t.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public static object XMLDeserialize(string filepath, Type T)
        {
            var fs = getFileStream(filepath, FileMode.OpenOrCreate);
            var xs = new XmlSerializer(T);
            var obj = xs.Deserialize(fs);
            fs.Close();
            return obj;
        }

        /// <summary>
        ///     The xml deserialize.
        /// </summary>
        /// <param name="reader">
        ///     The reader.
        /// </param>
        /// <param name="T">
        ///     The t.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public static object XMLDeserialize(XmlReader reader, Type T)
        {
            var xs = new XmlSerializer(T);
            return xs.Deserialize(reader);
        }

        /// <summary>
        ///     The xml serialize.
        /// </summary>
        /// <param name="filepath">
        ///     The filepath.
        /// </param>
        /// <param name="obj">
        ///     The obj.
        /// </param>
        public static void XMLSerialize(string filepath, object obj)
        {
            var fs = getFileStream(filepath, FileMode.Create);
            var xs = new XmlSerializer(obj.GetType());
            xs.Serialize(fs, obj);
            fs.Close();
        }

        #endregion
    }

    /// <summary>
    ///     The SerializableRa interface.
    /// </summary>
    public interface ISerializableRa
    {
    }

    /// <summary>
    ///     The serializable ra extender.
    /// </summary>
    public static class SerializableRaExtender
    {
        #region

        /// <summary>
        ///     The deserialize from binary.
        /// </summary>
        /// <param name="serializable">
        ///     The serializable.
        /// </param>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public static object DeserializeFromBinary(this ISerializableRa serializable, string filePath)
        {
            return Serializer.BinaryDeserialize(filePath);
        }

        /// <summary>
        ///     The deserialize from xml.
        /// </summary>
        /// <param name="serializable">
        ///     The serializable.
        /// </param>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public static object DeserializeFromXml(this ISerializableRa serializable, string filePath)
        {
            return Serializer.XMLDeserialize(filePath, serializable.GetType());
        }

        /// <summary>
        ///     The serialize to binary.
        /// </summary>
        /// <param name="serializable">
        ///     The serializable.
        /// </param>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        public static void SerializeToBinary(this ISerializableRa serializable, string filePath)
        {
            Serializer.BinarySerialize(filePath, serializable);
        }

        /// <summary>
        ///     The serialize to xml.
        /// </summary>
        /// <param name="serializable">
        ///     The serializable.
        /// </param>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        public static void SerializeToXml(this ISerializableRa serializable, string filePath)
        {
            Serializer.XMLSerialize(filePath, serializable);
        }

        #endregion
    }
}