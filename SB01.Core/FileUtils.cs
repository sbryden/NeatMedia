using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SB01.Core
{
    class FileUtils
    {
        public static DateTime? GetDateTaken(FileInfo fileInfo)
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
    }
}
