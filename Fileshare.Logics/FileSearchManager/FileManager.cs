using Fileshare.Domain.Models;
using Fileshare.Contracts.FileShare;
using Fileshare.Domain.FileSearch;
using System;

namespace Fileshare.Logics.FileShareManager
{
    public class FileManager : IFileShareService
    {
        public void ForwardResults(FileSearchResultModel result)
        {
            throw new NotImplementedException();
        }

        public FilePartModel GetAllFileByte(FileMetaData fileMeta)
        {
            throw new NotImplementedException();
        }

        public FilePartModel GetFilePartBytes(FilePart filePart, FileMetaData fileMeta)
        {
            throw new NotImplementedException();
        }
    }
}
