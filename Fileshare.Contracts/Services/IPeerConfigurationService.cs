using Fileshare.Domain.Models;

namespace Fileshare.Contracts.Services
{
    public interface IPeerConfigurationService<T>
    {
        int Port { get; }
        Peer<IPingService> Peer { get; }
        T PingService { get; set; }
        bool StartPeerService();
        bool StopPeerService();
    }
}
