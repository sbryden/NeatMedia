using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SB01.Core;
using SB01.Data;

namespace SB01.WPF
{
    public class Analyzer
    {
        public void ReadMetaData()
        {
            List<AnalyzedFileData> analyzedFileDataList = new List<AnalyzedFileData>();

        }

        public AnalyzedFileData InsertAnalyzedFileData(string filePath)
        {
            FileMetadata fileMetadata = null;
            return MediaMetadataRepo.Instance().Add(fileMetadata);
        }
    }
}
