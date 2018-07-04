using System.ServiceModel;

namespace Fileshare.Contracts.FileShare
{
    [ServiceContract(CallbackContract = typeof(IFileShareServiceCallback), SessionMode = SessionMode.Required)]
    public interface IFileShareHostService
    {
        bool StopHost();
        bool StartHost();
    }
}
