using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SB01.Core;

namespace SB01.UnitTests.Tests
{
    [TestClass]
    public class CoreTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string outputDirectory = @"C:\temp\sb01output";
            if (Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, true);
            Renamer renamer = new Renamer(outputDirectory);
            renamer.Go(@"C:\temp\sb01testfiles");
        }

        [TestMethod]
        public void TestFileInfo()
        {
            FileInfo fileInfo = new FileInfo(@"F:\test\test\test.jpg");
            Console.Write(fileInfo.Name);
        }

        [TestMethod]
        public void TestFileMatch()
        {
            string outputDirectory = @"C:\temp\sb01output";
            if (Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, true);
            Renamer renamer = new Renamer(outputDirectory);

            FileInfo fileInfo1 = new FileInfo(@"C:\test\copy1.jpg");
            FileInfo fileInfo2 = new FileInfo(@"C:\test\copy2.jpg");

            bool isMatch = renamer.IsExactMatch(fileInfo1, fileInfo2);

            Console.Write("Match? " + isMatch);
        }
    }
}
