using Fileshare.Domain.Models;

namespace Fileshare.Contracts.Repository
{
    public interface IPeerNameResolverRepository
    {
        void ResolvePeerName(string peerId);
        PeerEndPointsCollection PeerEndPointCollection { get; set; }
    }
}
