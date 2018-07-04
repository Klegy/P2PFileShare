using Fileshare.Domain.FileSearch;
using Fileshare.Domain.Models;
using System;
using System.ServiceModel;


namespace Fileshare.Contracts.FileShare
{
    [ServiceContract(CallbackContract = typeof(IFileShareServiceCallback), SessionMode = SessionMode.Required)]
    public interface IFileShareService
    {
        [OperationContract(IsOneWay = false)]
        FilePartModel GetAllFileByte(FileMetaData fileMeta);
 
        [OperationContract(IsOneWay = false)]
        FilePartModel GetFilePartBytes(FilePart filePart, FileMetaData fileMeta);

        [OperationContract(IsOneWay = false)]
        void ForwardResults(FileSearchResultModel result);

        [OperationContract(IsOneWay = true)]
        void PingHostService(HostInfo info, bool isCallback = false);
    }
}
