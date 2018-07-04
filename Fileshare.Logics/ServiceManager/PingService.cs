using Fileshare.Contracts.Services;
using Fileshare.Domain.FileSearch;
using Fileshare.Domain.Models;
using FileShare.SampleData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

namespace Fileshare.Logics.ServiceManager
{
    public delegate void OnPeerInfo(HostInfo endPointInfo);
    public delegate void FileSearchResultDelegate(FileSearchResultModel fileSearch);

    public class PingService : IPingService
    {
        public event OnPeerInfo PeerEndPointInformation;
        public event FileSearchResultDelegate FileSearchResults;
        private readonly Random rndm;
        private int _count = new FileSample().GetFileMetaData().Count;

        public PingService()
        {
            rndm = new Random();
            ClientHostDetails = new ObservableCollection<HostInfo>();
            AvailableFileMetaData = new FileSample().GetFileMetaData().Take(rndm.Next(1, _count)).ToList();
        }

        //public pingservice(hostinfo info)
        //{
        //    fileservicehost = info;
        //    clienthostdetails = new observablecollection<hostinfo>();
        //}

        public void Ping(HostInfo info)
        {         
            var host = Dns.GetHostEntry(info.Uri);
            IPEndPointCollection ips = new IPEndPointCollection();
 
            Console.WriteLine($"Yah! Peer Entered, Peer EndPoint Details");
            host.AddressList.ToList()?.ForEach(p => { ips.Add(new IPEndPoint(p, info.Port)); });
            var peerInfo = new PeerEndPointInfo
            {
                PeerUri = info.Uri,
                PeerIpCollection = ips,
                LastUpdated = DateTime.UtcNow
            };
            PeerEndPointInformation?.Invoke(info);
        }

        public HostInfo FileServiceHost { get; set; }
        public IList<FileMetaData> AvailableFileMetaData { get; set; }
        public ObservableCollection<HostInfo> ClientHostDetails { get; set; }

        public void SearchFiles(string searchTerm, string peerId)
        {
            var result = (from file in AvailableFileMetaData
                           where searchTerm == file.FileName
                            || file.FileName.Contains(searchTerm)
                            || file.FileName.IndexOf(searchTerm, StringComparison.CurrentCulture) > 0
                           select file);

            if (result.Any())
            {
                FileSearchResultModel searchResult = new FileSearchResultModel
                {
                    PeerId = peerId,
                    Files = (ObservableCollection<FileMetaData>) result
                };
                FileSearchResults?.Invoke(searchResult);
            }
        }
    }
}
