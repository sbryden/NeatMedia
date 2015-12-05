using System.Collections.Generic;
using System.Linq;
using SB01.Core;
using Raven.Client;
using Raven.Client.Embedded;

namespace SB01.Data
{
    public class MediaMetadataRepo
    {
        private static MediaMetadataRepo _instance = new MediaMetadataRepo();
        private IDocumentStore store;

        private MediaMetadataRepo()
        {
            store = new EmbeddableDocumentStore { };
            store.Initialize();
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
    }
}
