using System;
using System.Configuration;
using SB01.Core;

namespace SB01.Console
{
    class Program
    {
        //private static string testSource = @"F:\test\source";
        //private static string testDestination = @"F:\test\destination";
        //private static string testProcessed = @"F:\test\processed";

        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string source = appSettings["SourceDirectory"];
            string destination = appSettings["DestinationDirectory"];
            string archive = appSettings["ArchiveDirectory"];

            Renamer renamer = new Renamer(destination, archive);
            renamer.Go(source);
        }
    }
}
