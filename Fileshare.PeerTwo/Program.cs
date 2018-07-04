using Fileshare.Contracts.Repository;
using Fileshare.Contracts.Services;
using Fileshare.Domain.Models;
using Fileshare.Logics.PnrpManager;
using Fileshare.Logics.ServiceManager;
using System;
using System.Diagnostics;
using System.Linq;
using Fileshare.Test.PeerHostServices;
using System.Threading;

namespace Fileshare.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Count() <= 1)
            {
                Process.Start("Fileshare.Test.exe");
            }

            new Program().Run();
        }

        private void Run()
        {
            Console.WriteLine("Welcome to the Peer-2-Peer Fileshare Service");
            Console.WriteLine("Enter Username: ");
            string username = Console.ReadLine();

            Peer<IPingService> peer = new Peer<IPingService> {
                PeerId = Guid.NewGuid().ToString().Split('-')[4],
                UserName = username
            };

            IPeerRegistrationRepository peerRegistration = new PeerRegistrationManager();
            IPeerNameResolverRepository peerNameResolver = new PeerNameResolver(peer.PeerId);
            IPeerConfigurationService<PingService> peerConfigurationService = new PeerConfigurationService(peer);

            PeerServiceHost psh = new PeerServiceHost(peerRegistration, peerNameResolver, peerConfigurationService);

            Thread thd = new Thread(() =>
            {
                psh.RunPeerServiceHost(peer);
            }) { IsBackground = true };
            thd.Start();

            Console.ReadLine();
        }
    }
}
