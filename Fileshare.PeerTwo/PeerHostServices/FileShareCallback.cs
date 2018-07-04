using Fileshare.Contracts.FileShare;
using Fileshare.Domain.FileSearch;
using System;

namespace Fileshare.Test.PeerHostServices
{
    public class FileShareCallback : IFileShareServiceCallback
    {
        public bool IsConnected(string replyMessage)
        {
            if (!string.IsNullOrEmpty(replyMessage))
            {
                Console.WriteLine(replyMessage);
                return true;
            }
            return false;
        }

        public bool ForwardSearchResults(FileSearchResultModel search)
        {
            throw new NotImplementedException();
        }
    }
}
