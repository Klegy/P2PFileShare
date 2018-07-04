using Fileshare.Contracts.Repository;
using System;
using System.Net.PeerToPeer;

namespace Fileshare.Logics.PnrpManager
{
    public class PeerRegistrationManager : IPeerRegistrationRepository
    {
        #region field
        private PeerNameRegistration _peerNameRegistration = null;
        #endregion

        public bool IsPeerRegistered => _peerNameRegistration != null && _peerNameRegistration.IsRegistered();
        public string PeerUri => GetPeerUri();

        private string GetPeerUri()
        {
            return _peerNameRegistration?.PeerName.PeerHostName;
        }

        public PeerName PeerName { get; set; }
 
        public void StartPeerRegistration(string username, int port)
        {
            PeerName = new PeerName(username, PeerNameType.Unsecured);
            _peerNameRegistration = new PeerNameRegistration(PeerName, port, Cloud.AllLinkLocal);
            try
            {
                _peerNameRegistration.Start(); /* bug here, does recognize peername? */
            }
            catch (PeerToPeerException e)
            {
                Console.WriteLine("Peer Name Registration failed: Error {0}", e.Message);
            }        
        }

        public void StopPeerRegistration()
        {
            _peerNameRegistration?.Stop();
            _peerNameRegistration = null;
        }
    }
}
