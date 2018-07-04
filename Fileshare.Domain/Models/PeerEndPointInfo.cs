using System;
using System.Net;

namespace Fileshare.Domain.Models
{
    public class PeerEndPointInfo
    {
        public string PeerUri { get; set; }
        public IPEndPointCollection PeerIpCollection { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
