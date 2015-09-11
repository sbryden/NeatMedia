using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using SB01.Core.Logging;

namespace SB01.Core
{
    public class Renamer
    {
        private string _destinationDirectory;
        private string _archiveDirectory;
        private string[] _supportedExtensions = { ".jpg", ".jpeg", ".bmp", ".gif", ".ico", ".jpe", ".png", ".tiff", ".tif" };

        public Renamer(string destinationDirectory)
        {
            _destinationDirectory = destinationDirectory;
        }

        public Renamer(string destinationDirectory, string archiveDirectory)
        {
            _destinationDirectory = destinationDirectory;
            _archiveDirectory = archiveDirectory;
        }

        private void RenameFile(string sourceFilePath, string defaultTag = null, bool useYearFolders = true)
        {
            string targetDirectory;
            string targetFilePath;

            if (string.IsNullOrEmpty(defaultTag))
            {
                targetDirectory = _destinationDirectory;
            }
            else
            {
                targetDirectory = Path.Combine(_destinationDirectory, defaultTag);
            }

            DateTime? dateTaken = GetDateTaken(sourceFilePath);
            if (useYearFolders)
            {
                string yearStr = dateTaken.HasValue ? dateTaken.Value.ToString("yyyy") : "~";
                if (Path.GetDirectoryName(targetDirectory) != yearStr
                    && Path.GetDirectoryName(targetDirectory) != "~")
                {
                    targetDirectory = Path.Combine(targetDirectory, yearStr);
                }
            }

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            targetFilePath = DetermineFilePath(sourceFilePath, targetDirectory);
            if (targetFilePath == null) // duplicate found, skipping
            {
                Log.Info("Exact match found, skipping: " + targetFilePath);
                return;
            }

            if (useYearFolders)
            {
                targetFilePath = YearOrganizerator(targetFilePath, dateTaken);
            }

            // copy file to target
            File.Copy(sourceFilePath, targetFilePath, false);

            // archive old file
            if (!string.IsNullOrEmpty(_archiveDirectory))
            {
                if (!Directory.Exists(_archiveDirectory))
                    Directory.CreateDirectory(_archiveDirectory);

                FileInfo sourceFileInfo = new FileInfo(sourceFilePath);
                string archiveFilePath = Path.Combine(_archiveDirectory, sourceFileInfo.Name);
                int i = 0;
                while (File.Exists(archiveFilePath))
                {
                    archiveFilePath = Path.Combine(_archiveDirectory, string.Format("{0}.{1:000}", sourceFileInfo.Name, i));
                    i++;
                }
                File.Move(sourceFilePath, archiveFilePath);
            }
        }

        /// <summary>
        /// Accepts a path, checks the first directory, if matches year (YYYY) returns without change.
        /// When the first direcotry is not year, creates year directory and modifies return value to include 
        /// new directory path.
        /// </summary>
        /// <param name="targetFilePath">Full file path of file or file to be created.</param>
        /// <param name="dateTaken">Date taken nullable</param>
        /// <returns>targetFilePath with year directory</returns>
        private string YearOrganizerator(string targetFilePath, DateTime? dateTaken)
        {
            FileInfo info = new FileInfo(targetFilePath);
            DirectoryInfo firstDirectory = info.Directory;

            string yearStr;
            if (dateTaken.HasValue)
                yearStr = dateTaken.Value.ToString("yyyy");
            else
                yearStr = "~"; // here for breakpointing purposes

            if (firstDirectory != null && firstDirectory.Name.Equals(yearStr))
            {
                return targetFilePath;
            }
            else
            {
                string newDirectory;
                if (firstDirectory == null)
                    newDirectory = yearStr;
                else
                    newDirectory = Path.Combine(firstDirectory.FullName, yearStr);

                Directory.CreateDirectory(newDirectory);

                targetFilePath = Path.Combine(newDirectory, info.Name);
                return targetFilePath;
            }
        }

        private string DetermineFilePath(string sourceFilePath, string targetDirectory)
        {
            FileInfo fileInfo = new FileInfo(sourceFilePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException(string.Format("Unable to find file at {0}.", sourceFilePath), sourceFilePath);

            DateTime dateForName = DateTime.MinValue;
            DateTime? dateTaken = GetDateTaken(fileInfo);
            dateForName = dateTaken.Value;

            if (dateForName == DateTime.MinValue)
                throw new ApplicationException("Unable to parse date taken.");

            string fileName = dateForName.ToString("yyyy-MM-dd_HH-mm-ss");

            // check for existing, append sequence
            string uniquifier = "";
            int i = 0;
            while (File.Exists(Path.Combine(targetDirectory, string.Format("{0}{1}{2}", fileName, uniquifier, fileInfo.Extension.ToLower()))))
            {
                FileInfo existingFileInfo = new FileInfo(Path.Combine(targetDirectory, string.Format("{0}{1}{2}", fileName, uniquifier, fileInfo.Extension.ToLower())));
                if (IsExactMatch(existingFileInfo, fileInfo))
                    return null;

                i++;

                if (i > 999)
                    throw new ApplicationException(string.Format("Too many files with same name {0} in {1}", fileName + fileInfo.Extension, targetDirectory));

                uniquifier = "_" + i.ToString("000");
            }

            return Path.Combine(targetDirectory, string.Format("{0}{1}{2}", fileName, uniquifier, fileInfo.Extension.ToLower()));
        }

        public bool IsExactMatch(FileInfo fileInfo1, FileInfo fileInfo2)
        {
            // check timestamp
            if (GetDateTaken(fileInfo1) != GetDateTaken(fileInfo2))
                return false;

            // check size
            if (fileInfo1.Length != fileInfo2.Length)
                return false;

            // TODO check actual bitmap data

            return true;
        }

        private DateTime? GetDateTaken(FileInfo fileInfo)
        {
            DateTime? dateTaken = null;
            using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapSource img = BitmapFrame.Create(fs);
                BitmapMetadata md = (BitmapMetadata)img.Metadata;
                if (md != null && !string.IsNullOrEmpty(md.DateTaken))
                {
                    string date = md.DateTaken;
                    dateTaken = DateTime.Parse(date);
                }
            }

            if (dateTaken.HasValue && dateTaken.Value > DateTime.MinValue)
            {
                return dateTaken.Value;
            }
            else
            {
                // use created date
                if (fileInfo.CreationTime > DateTime.MinValue && fileInfo.CreationTime < fileInfo.LastWriteTime)
                    return fileInfo.CreationTime;
                if (fileInfo.LastWriteTime > DateTime.MinValue)
                    return fileInfo.LastWriteTime;
            }

            return null;
        }

        private DateTime? GetDateTaken(string filePath)
        {
            return GetDateTaken(new FileInfo(filePath));
        }

        public void AnalyzeSource(string sourceDirectoryPath)
        {

        }

        public void Go(string sourceDirectoryPath)
        {
            // check that source and target directories are not equal or nested within eachother

            // loop through all files recursively and move/rename
            foreach (string filePath in Directory.GetFiles(sourceDirectoryPath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (_supportedExtensions.Contains(fileInfo.Extension, StringComparer.CurrentCultureIgnoreCase))
                    RenameFile(filePath);
            }
        }

        public delegate void ProgressUpdate(int progressPercent, string progressMessage);
        public void GoAsync(string sourceDirectoryPath, ProgressUpdate onProgressUpdate)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += (sender, args) =>
            {
                IEnumerable<string> files = Directory.GetFiles(sourceDirectoryPath);

                int totalFiles = files.Count();
                int i = 0;

                Log.Info(string.Format("Starting, {0} total files to process.", totalFiles));

                foreach (string filePath in files)
                {
                    Log.Info(string.Format("Processing: {0}", filePath));
                    backgroundWorker.ReportProgress((i * 100) / totalFiles, string.Format("Processing: {0}", filePath));

                    FileInfo fileInfo = new FileInfo(filePath);
                    if (_supportedExtensions.Contains(fileInfo.Extension, StringComparer.CurrentCultureIgnoreCase))
                        RenameFile(filePath);

                    i++;
                }
            };
            backgroundWorker.ProgressChanged +=
                (sender, args) => onProgressUpdate(args.ProgressPercentage, args.UserState as string);
            backgroundWorker.RunWorkerCompleted +=
                (sender, args) =>
                {
                    Log.Info("Completed");
                    onProgressUpdate(101, "Complete");
                };

            backgroundWorker.RunWorkerAsync();
        }
    }
}
