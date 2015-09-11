using System.IO;

namespace SB01.Core
{
    public class AnalyzedFileData : FileMetadata
    {
        public AnalyzedFileData() : base()
        {
            
        }
        public AnalyzedFileData(FileInfo fileInfo)
            : base(fileInfo)
        {

        }

        public AnalyzedFileData(FileMetadata fileMetadata) : base(fileMetadata.FileInfo)
        {
            
        }

        public string OriginalFileName { get; set; }
        public bool IsInDb { get; set; }
    }
}
