using Fileshare.Domain.FileSearch;
using System.ServiceModel;

namespace Fileshare.Contracts.FileShare
{
    public interface IFileShareServiceCallback
    {
        [OperationContract(IsOneWay = false)]
        bool IsConnected(string replyMessage);

        [OperationContract(IsOneWay = false)]
        bool ForwardSearchResults(FileSearchResultModel search);
    }
}
