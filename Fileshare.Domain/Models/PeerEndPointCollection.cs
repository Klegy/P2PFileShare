using System.Collections.ObjectModel;
using System.Net;
using System.Net.PeerToPeer;

namespace Fileshare.Domain.Models
{
    public class PeerEndPointsCollection
    {
        public PeerEndPointsCollection(PeerName peer)
        {
            PeerHostName = peer;
            PeerEndPoints = new ObservableCollection<PeerEndPointInfo>();
        }

        public PeerName PeerHostName { get; set; }
        public ObservableCollection<PeerEndPointInfo> PeerEndPoints { get; set; }
    }
}
