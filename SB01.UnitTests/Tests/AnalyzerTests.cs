using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SB01.WPF;

namespace SB01.UnitTests.Tests
{
    [TestClass]
    public class AnalyzerTests
    {
        [TestMethod]
        public void CheckUpdateDatabase()
        {
            Analyzer analyzer = new Analyzer();
            analyzer.CheckUpdateDatabase("");
        }
    }
}
