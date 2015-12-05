using System.Collections.Generic;
using SB01.Core;
using SB01.Data;
using System.IO;
using System.Linq;
using SB01.Core.Logging;
using System;

namespace SB01.WPF
{
    public class Analyzer
    {
        private long CurrentAnalyzerNumber;

        public void ReadMetaData()
        {
            List<FileMetadata> analyzedFileDataList = new List<FileMetadata>();
        }

        public FileMetadata InsertAnalyzedFileData(string filePath)
        {
            FileMetadata fileMetadata = null;
            return MediaMetadataRepo.Instance().Add(fileMetadata);
        }


        // read storage directory, check/update database
        public void CheckUpdateDatabase(string sourceDirectoryPath)
        {
            IEnumerable<string> files = Directory.EnumerateFiles(sourceDirectoryPath, "*.*", SearchOption.AllDirectories); //Directory.GetFiles(sourceDirectoryPath);

            int totalFiles = files.Count();
            int i = 0;

            Log.Info(string.Format("Starting, {0} total files to process.", totalFiles));

            foreach (string filePath in files)
            {
                Log.Info(string.Format("Processing {1} of {2} - {0}", filePath, i, totalFiles));

                FileInfo fileInfo = new FileInfo(filePath);
                if (Configuration.SupportedExtensions.Contains(fileInfo.Extension, StringComparer.CurrentCultureIgnoreCase))
                {
                    FileMetadata fileMetadata = new FileMetadata(fileInfo);

                    FileMetadata masterFileMetadata = FindMatch(fileMetadata);

                    if (masterFileMetadata != null)
                    {
                        // record exists in database, touch record
                        Log.Info(string.Format("Record exists, touching"));
                        TouchRecord(masterFileMetadata);
                    }
                    else
                    {
                        // add record to database
                        Log.Info(string.Format("Adding to database"));
                        AddRecord(fileMetadata);
                    }
                }

                i++;
            }
        }

        private void AddRecord(FileMetadata fileMetadata)
        {
            MediaMetadataRepo mediaMetadataRepo = MediaMetadataRepo.Instance();
            fileMetadata.AnalyzerNumber = CurrentAnalyzerNumber;
            mediaMetadataRepo.Add(fileMetadata);
        }

        private void TouchRecord(FileMetadata masterFileMetadata)
        {
            MediaMetadataRepo mediaMetadataRepo = MediaMetadataRepo.Instance();
            mediaMetadataRepo.TouchRecord(masterFileMetadata.Id, CurrentAnalyzerNumber);
        }

        private FileMetadata FindMatch(FileMetadata fileMetadata)
        {
            // check database for match, return match if found, null otherwise
            MediaMetadataRepo mediaMetadataRepo = MediaMetadataRepo.Instance();
            return mediaMetadataRepo.FindReturnMatch(fileMetadata);
        }
    }
}
