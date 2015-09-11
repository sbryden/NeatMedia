using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SB01.Core;
using System.Configuration;

namespace SB01.Data
{
    public class MediaMetadataRepo
    {
        private List<AnalyzedFileData> _analyzedFileDataList;
        private static MediaMetadataRepo _instance;
        private string _xmlDatabaseFilePath;
        private string XmlDatabaseFilePath
        {
            get
            {
                if (_xmlDatabaseFilePath == null)
                    _xmlDatabaseFilePath = ConfigurationManager.AppSettings["DbFilePath"];
                return _xmlDatabaseFilePath;
            }
            set
            {
                _xmlDatabaseFilePath = value;
                ConfigurationManager.AppSettings["DbFilePath"] = _xmlDatabaseFilePath;
            }
        }

        public MediaMetadataRepo()
        {
            _analyzedFileDataList = new List<AnalyzedFileData>();

            if (!string.IsNullOrEmpty(XmlDatabaseFilePath))
                ReadXmlDatabase(XmlDatabaseFilePath);
            else
            {
                XmlDatabaseFilePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) +
                                      @"\XML\cuzco.xml";

                ConfigurationManager.AppSettings["DbFilePath"] = XmlDatabaseFilePath;
            }
        }

        private void WriteAllItems(List<AnalyzedFileData> allData)
        {
            DataWrapper data = new DataWrapper();
            data.AnalyzedFileDataList = allData;
            XmlSerializer serializer = new XmlSerializer(typeof(DataWrapper));
            if (!File.Exists(XmlDatabaseFilePath))
                File.Create(XmlDatabaseFilePath);
            TextWriter writer = new StreamWriter(XmlDatabaseFilePath);
            serializer.Serialize(writer, data);
            writer.Close();
        }

        private void ReadXmlDatabase(string xmlDatabaseFilePath)
        {
            if (File.Exists(xmlDatabaseFilePath))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof (DataWrapper));
                    //FileStream stream = new FileStream(xmlDatabaseFilePath, FileMode.Open);
                    TextReader reader = new StreamReader(xmlDatabaseFilePath);
                    DataWrapper data = (DataWrapper) serializer.Deserialize(reader);
                    foreach (AnalyzedFileData fileData in data.AnalyzedFileDataList)
                        fileData.IsInDb = true;
                    _analyzedFileDataList = data.AnalyzedFileDataList;
                }
                catch
                {
                    //eat it;
                }
            }
            else
            {
                File.Create(xmlDatabaseFilePath);
            }
        }

        public static MediaMetadataRepo Instance()
        {
            if (_instance == null)
                _instance = new MediaMetadataRepo();

            return _instance;
        }

        public AnalyzedFileData Add(FileMetadata fileMetadata)
        {
            AnalyzedFileData data = new AnalyzedFileData(fileMetadata);
            data.IsInDb = true;
            _analyzedFileDataList.Add(data);
            WriteAllItems(_analyzedFileDataList);
            return data;
        }

        public List<AnalyzedFileData> GetAllAnalyzedFileData()
        {
            return _analyzedFileDataList;
        }

        public void Test()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=Database1.mdf;Integrated Security=True"))
            {
                connection.Open();
            }
        }
    }

    public class DataWrapper
    {
        public DataWrapper()
        {

        }
        public List<AnalyzedFileData> AnalyzedFileDataList { get; set; }
    }
}
