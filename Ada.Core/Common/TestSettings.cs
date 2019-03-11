namespace Ada.Common
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ra.Common.Reflection;
    using Ra.Common.Serialization;

    #endregion

    [UILabels("Indstillinger", "", "", "")]
    [Serializable]
    public class TestSettings
    {
        #region Static

        public static bool Internal_Viewer;

        #endregion

        #region  Fields

        public DocumentationTestSettings Documentation;
        public DocumentTestSettings Documents;
        public DriveSettings Drives;
        public FileIndexTestSettings FileIndex;

        public string[] InputDictionaries = new string[0];
        public TableTestSettings TableTest;

        #endregion

        #region  Constructors

        //        public ContextFlag DocumentsOrDocumentationContext;

        public TestSettings()
        {
            // OBS !!!! rækkefølge er betydende 
            Documents = new DocumentTestSettings();
            Documentation = new DocumentationTestSettings();
            TableTest = new TableTestSettings();
            FileIndex = new FileIndexTestSettings();
            Drives = new DriveSettings();
        }

        #endregion

        #region

        public static TestSettings Deserialize(FileInfo path)
        {
            return (TestSettings) Serializer.XMLDeserialize(path.FullName, typeof(TestSettings));
        }

        #endregion
    }


    public abstract class DocumentBaseSettings
    {
        #region  Constructors

        public DocumentBaseSettings()
        {
            Compression = true;
            Private_Tags = false;
            Active = true;
            Blank_First_Page = true;
            Blank_All_Documents = true;
            Internal_Viewer = true;
        }

        #endregion

        #region Properties

        [UILabels("Aktiv testgruppe", "", "Master Switch", "")]
        public bool Active { get; set; }

        [UILabels("Blanke dokumenter", "", "", "")]
        public bool Blank_All_Documents { get; set; }

        [UILabels("Blank første side", "", "", "")]
        public bool Blank_First_Page { get; set; }

        [UILabels("Kompression", "", "", "")]
        public bool Compression { get; set; }

        [UILabels("Internal doc. viewer", "", "", "")]
        public bool Internal_Viewer
        {
            get => TestSettings.Internal_Viewer;
            set => TestSettings.Internal_Viewer = value;
        }

        [UILabels("Page Count Alert level", "", "", "")]
        public int PageCount_Alert_Level { get; set; }

        [UILabels("Page Count High", "", "", "")]
        public bool PageCount_High { get; set; }

        [UILabels("Private Tags", "", "", "")]
        public bool Private_Tags { get; set; }

        #endregion

        //
        //        public bool tiff_xyRes_Uneven { get; set; }
        //        public bool tiff_FillorderLegal { get; set; }
    }

    public abstract class DocumentationBaseSettings
    {
        #region  Constructors

        public DocumentationBaseSettings()
        {
            Compression = true;
            Private_Tags = false;
            Active = true;
            Blank_First_Page = true;
            Blank_All_Documents = true;
        }

        #endregion

        #region Properties

        [UILabels("Aktiv testgruppe", "", "Master Switch", "")]
        public bool Active { get; set; }

        [UILabels("Blanke dokumenter", "", "", "")]
        public bool Blank_All_Documents { get; set; }

        [UILabels("Blank første side", "", "", "")]
        public bool Blank_First_Page { get; set; }

        [UILabels("Kompression", "", "", "")]
        public bool Compression { get; set; }

        [UILabels("Page Count Alert level", "", "", "")]
        public int PageCount_Alert_Level { get; set; }

        [UILabels("Page Count High", "", "", "")]
        public bool PageCount_High { get; set; }

        [UILabels("Private Tags", "", "", "")]
        public bool Private_Tags { get; set; }

        #endregion

        //
        //        public bool tiff_xyRes_Uneven { get; set; }
        //        public bool tiff_FillorderLegal { get; set; }
    }

    [UILabels("Dokumenter", "", "", "")]
    public class DocumentTestSettings : DocumentBaseSettings
    {
    }

    [UILabels("KontekstDokumentation", "", "", "")]
    public class DocumentationTestSettings : DocumentationBaseSettings
    {
    }


    [UILabels("Struktur og filer", "", "", "")]
    public class FileIndexTestSettings
    {
        #region  Constructors

        public FileIndexTestSettings()
        {
            MD5Test = true;
        }

        #endregion

        #region Properties

        [UILabels("MD5 Test", "", "", "")]
        public bool MD5Test { get; set; }

        #endregion
    }


    [UILabels("Tabeller", "", "", "")]
    public class TableTestSettings
    {
        #region  Constructors

        public TableTestSettings()
        {
            PrimaryKeyTest = true;
            ForeignKeyTest = true;
            Active = true;
            //this.RewriteFileIndex = true;
        }

        #endregion

        #region Properties

        [UILabels("Aktiv testgruppe", "", "Master Switch", "")]
        public bool Active { get; set; }

        [UILabels("Fremmednøgletest", "", "", "")]
        public bool ForeignKeyTest { get; set; }

        [UILabels("Primærnøgletest", "", "", "")]
        public bool PrimaryKeyTest { get; set; }

        #endregion
    }

    [UILabels("Aktive Drev", "", "", "")]
    [Serializable]
    public class DriveSettings
    {
        #region  Constructors

        public DriveSettings()
        {
            DriveList = new List<DriveStatus>();
            UpdateDrives();
        }

        #endregion

        #region Properties

        [UILabels("Drev", "", "", "")]
        public List<DriveStatus> DriveList { get; set; }

        #endregion

        #region

        public IEnumerable<string> GetActiveDrives()
        {
            return DriveList
                .Where(d => d.Status)
                .Select(d => d.Drive);
        }

        public void UpdateDrives()
        {
            var newDrives = new List<DriveStatus>();
            foreach (var d in DriveInfo.GetDrives()) newDrives.Add(new DriveStatus(d.Name, DriveList.FirstOrDefault(o => o.Drive == d.Name)?.Status ?? false));
            DriveList.Clear();
            DriveList.AddRange(newDrives);
        }

        #endregion
    }

    [Serializable]
    public class DriveStatus
    {
        #region  Constructors

        public DriveStatus(string drive, bool status)
        {
            Drive = drive;
            Status = status;
        }

        public DriveStatus()
        {
            Drive = "";
            Status = false;
        }

        #endregion

        #region Properties

        public string Drive { get; set; }
        public bool Status { get; set; }

        #endregion
    }
}