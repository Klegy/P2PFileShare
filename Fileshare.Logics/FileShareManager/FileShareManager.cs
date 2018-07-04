using Fileshare.Domain.Models;
using Fileshare.Contracts.FileShare;
using Fileshare.Domain.FileSearch;
using System;
using System.ServiceModel;
using System.Collections.Generic;

namespace Fileshare.Logics.FileShareManager
{
    public delegate void CurrentHostInfo(HostInfo info, bool isCallback = false);
    public delegate void CurrentClientInfo(string peerId, IFileShareServiceCallback callback);

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class FileShareManager : IFileShareService
    {
        private Dictionary<string, HostInfo> _currentHost = new Dictionary<string, HostInfo>();

        public event CurrentHostInfo CurrentHostUpdate;
 
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

        public void PingHostService(HostInfo info, bool isCallback)
        {
            Console.WriteLine($"Peer: {info.Id}     Server: {info.Uri}:{info.Port}\n");

            var callback = OperationContext.Current.GetCallbackChannel<IFileShareServiceCallback>();
            if (callback != null)
            {
                if (isCallback)
                {
                    if (callback.IsConnected($"Ping back direct connection: {DateTime.UtcNow:T}"))
                    {
                        info.Callback = callback;
                        CurrentHostUpdate?.Invoke(info, true);
                    }
                }
                else
                {
                    if (callback.IsConnected($"Direct Peer connection established at: {DateTime.UtcNow:D}"))
                    {
                        _currentHost.Add(info.Id, info);
                        info.Callback = callback;
                        CurrentHostUpdate?.Invoke(info);
                    }
                }
            }
        }
    }
}
