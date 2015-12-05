using System.Collections.Generic;
using System.Linq;
using SB01.Core;
using Raven.Client;
using Raven.Client.Embedded;
using System;
using Raven.Database.Server;

namespace SB01.Data
{
    public class MediaMetadataRepo
    {
        private static MediaMetadataRepo _instance = new MediaMetadataRepo();
        private IDocumentStore store;

        private MediaMetadataRepo()
        {
            try
            {
                NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8081);
                store = new EmbeddableDocumentStore { DataDirectory = @"C:\RavenSB01\", UseEmbeddedHttpServer = true };
                store.Initialize();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MediaMetadataRepo Instance()
        {
            if (_instance == null)
                _instance = new MediaMetadataRepo();

            return _instance;
        }

        public FileMetadata Add(FileMetadata fileMetadata)
        {
            using (IDocumentSession session = store.OpenSession())
            {
                session.Store(fileMetadata);
                session.SaveChanges();
            }

            return fileMetadata;
        }

        public List<FileMetadata> GetAllAnalyzedFileData()
        {
            using (IDocumentSession session = store.OpenSession())
            {
                var fileMetaDataList = session.Query<FileMetadata>().ToList();
                return fileMetaDataList;
            }
        }

        public FileMetadata FindReturnMatch(FileMetadata fileMetadata)
        {
            using (IDocumentSession session = store.OpenSession())
            {
                var fileMetaDataList = session.Query<FileMetadata>()
                                        .Where(x => x.CalculatedDateTaken == fileMetadata.CalculatedDateTaken
                                        && fileMetadata.FileName == x.FileName
                                        && fileMetadata.FileSize == x.FileSize);
                //.Where(x => fileMetadata.FileName == x.FileName);
                return fileMetaDataList.Any() ? fileMetaDataList.First() : null;
            }
        }

        public void TouchRecord(string id, long analyzerNumber)
        {
            using (IDocumentSession session = store.OpenSession())
            {
                var document = session.Load<FileMetadata>(id);
                document.AnalyzerNumber = analyzerNumber;
                session.SaveChanges();
            }
        }
    }
}
