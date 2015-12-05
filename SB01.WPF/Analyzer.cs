using System.Collections.Generic;
using SB01.Core;
using SB01.Data;

namespace SB01.WPF
{
    public class Analyzer
    {
        public void ReadMetaData()
        {
            List<FileMetadata> analyzedFileDataList = new List<FileMetadata>();

        }

        public FileMetadata InsertAnalyzedFileData(string filePath)
        {
            FileMetadata fileMetadata = null;
            return MediaMetadataRepo.Instance().Add(fileMetadata);
        }
    }
}
