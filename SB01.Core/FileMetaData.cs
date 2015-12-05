using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SB01.Core
{
    public class FileMetadata
    {
        private FileInfo _fileInfo;
        public string OriginalFileName { get; set; }
        public bool IsInDb { get; set; }

        public string FileName
        {
            get { return _fileInfo.Name; }
        }

        public FileInfo FileInfo { get { return _fileInfo; } }

        private DateTime? _calculatedDateTaken;
        public DateTime? CalculatedDateTaken
        {
            get
            {
                return _calculatedDateTaken;
            }
        }

        public string FullPath
        {
            get { return _fileInfo.FullName; }
        }

        public FileMetadata(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
            _calculatedDateTaken = FileUtils.GetDateTaken(_fileInfo);
        }

        protected FileMetadata()
        {
        }
    }
}
