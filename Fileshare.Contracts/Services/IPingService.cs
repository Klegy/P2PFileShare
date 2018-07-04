using Fileshare.Domain.Models;
using System;
using System.ServiceModel;


namespace Fileshare.Contracts.Services
{
    [ServiceContract(CallbackContract = typeof(IPingService))]
    public interface IPingService
    {
        [OperationContract(IsOneWay = true)]
        void Ping(HostInfo host);

        [OperationContract(IsOneWay = true)]
        void SearchFiles(string searchTerm, string peerId);
    }
}
