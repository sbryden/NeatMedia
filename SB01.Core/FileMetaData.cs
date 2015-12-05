using System;
using System.IO;

namespace SB01.Core
{
    public class FileMetadata
    {
        public string Id = string.Empty;
        public string OriginalFileName { get; set; }
        public bool IsInDb { get; set; }
        public string FileName { get; set; }
        public DateTime? CalculatedDateTaken { get; set; }
        public string FullPath { get; set; }
        public long FileSize { get; set; }
        public long AnalyzerNumber { get; set; }

        public FileMetadata()
        {

        }

        public FileMetadata(FileInfo fileInfo)
        {
            FileName = fileInfo.Name;
            FullPath = fileInfo.FullName;
            FileSize = fileInfo.Length;
            CalculatedDateTaken = FileUtils.GetDateTaken(fileInfo);
        }
    }

    //public class FileMetadata
    //{
    //    public string Id = string.Empty;

    //    private FileInfo _fileInfo;
    //    public string OriginalFileName { get; set; }
    //    public bool IsInDb { get; set; }

    //    public string FileName
    //    {
    //        get { return _fileInfo.Name; }
    //    }

    //    public FileInfo FileInfo { get { return _fileInfo; } }

    //    private DateTime? _calculatedDateTaken;
    //    public DateTime? CalculatedDateTaken
    //    {
    //        get
    //        {
    //            return _calculatedDateTaken;
    //        }
    //    }

    //    public string FullPath
    //    {
    //        get { return _fileInfo.FullName; }
    //    }

    //    public long AnalyzerNumber { get; set; }

    //    public FileMetadata(FileInfo fileInfo)
    //    {
    //        _fileInfo = fileInfo;
    //        _calculatedDateTaken = FileUtils.GetDateTaken(_fileInfo);
    //    }

    //    protected FileMetadata()
    //    {
    //    }

    //    public bool IsMatch(FileMetadata fileMetadata)
    //    {
    //        if(CalculatedDateTaken == fileMetadata.CalculatedDateTaken
    //            && fileMetadata.FileName.Equals(FileName)
    //            && fileMetadata.FileInfo.Length == FileInfo.Length)
    //        {
    //            return true;
    //        }

    //        return false;
    //    }
    //}
}
