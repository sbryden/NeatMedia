using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SB01.Core;
using SB01.Data;

namespace SB01.UnitTests.Tests
{
    [TestClass]
    public class DataTests
    {
        [ClassInitializeAttribute]
        public  static void init(TestContext context)
        {
            ConfigurationManager.AppSettings["DbFilePath"] = @"C:\test\db.xml";
        }

        [TestMethod]
        public void TestGetAllData()
        {
            MediaMetadataRepo repo = MediaMetadataRepo.Instance();
            List<FileMetadata> allAnalyzedFileData = repo.GetAllAnalyzedFileData();
        }

        [TestMethod]
        public void TestInsertData()
        {
            MediaMetadataRepo repo = MediaMetadataRepo.Instance();
            repo.Add(new FileMetadata(new FileInfo(@"c:\test\test.JPG")));
        }
    }
}
