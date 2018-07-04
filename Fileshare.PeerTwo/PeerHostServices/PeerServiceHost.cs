using Fileshare.Contracts.FileShare;
using Fileshare.Contracts.Repository;
using Fileshare.Contracts.Services;
using Fileshare.Domain.Models;
using Fileshare.Logics.FileShareManager;
using Fileshare.Logics.ServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fileshare.Test.PeerHostServices
{
    public class PeerServiceHost
    {
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private bool IsStarted = false;
        private int _port = 0;
        FileShareManager _file = new FileShareManager();
        Dictionary<string, HostInfo> _currentHost = new Dictionary<string, HostInfo>();

        public PeerServiceHost(IPeerRegistrationRepository peerRegistration, IPeerNameResolverRepository peerNameResolver, IPeerConfigurationService<PingService> peerConfigurationService)
        {
            RegisterPeer = peerRegistration;
            ResolvePeer = peerNameResolver;
            ConfigurePeer = peerConfigurationService;
            _port = ConfigurePeer.Port;
        }

        public IPeerRegistrationRepository RegisterPeer { get; set; }
        public IPeerNameResolverRepository ResolvePeer { get; set; }
        public IPeerConfigurationService<PingService> ConfigurePeer { get; set; }

        public void RunPeerServiceHost(Peer<IPingService> peer)
        {
            if (peer == null)
                throw new ArgumentNullException(nameof(peer));

            RegisterPeer.StartPeerRegistration(peer.PeerId, _port);
            if (RegisterPeer.IsPeerRegistered)
            {
                Console.WriteLine($"{peer.UserName} Registration completed");
                Console.WriteLine($"Peer Uri: {RegisterPeer.PeerUri} \t\t {_port}");
            }

            if (ResolvePeer != null)
            {
                Console.WriteLine($"Resolving {peer.UserName}");
                ResolvePeer.ResolvePeerName(peer.PeerId);
                var result = ResolvePeer.PeerEndPointCollection;
                Console.WriteLine($"\t\t\t EndPoints for {RegisterPeer.PeerUri}");
                if (ConfigurePeer.StartPeerService())
                {
                    Console.WriteLine("Peer services started");
                    peer.Channel.Ping(new HostInfo
                    {
                        Id = peer.PeerId,
                        Port = _port,
                        Uri = RegisterPeer.PeerUri
                    });
 
                    if (ConfigurePeer.PingService != null)
                    {
                        ConfigurePeer.PingService.PeerEndPointInformation += PingServiceOnPeerEndPointInformation;
                    }

                    Thread thd = new Thread(new ThreadStart((() =>
                    {
                        if (StartFileShareService(_port, RegisterPeer.PeerUri))
                        {
                            Console.WriteLine("Files service host started ");
                            var files = ConfigurePeer.PingService.AvailableFileMetaData;
                            if (files.Any())
                            {
                                Console.WriteLine($"\n Available files: {files.Count}");

                            }
                            files.ToList().ForEach(fp =>
                            {
                                Console.WriteLine($"Filename: {fp.FileName}      Size: {fp.FileSize}");
                            });
                        }
                    })));
                    thd.Start();

                }
                else
                {
                    Console.WriteLine($"Error starting up peer services");
                }
            }
        }

        private void PingServiceOnPeerEndPointInformation(HostInfo endPointInfo)
        {
            Console.WriteLine("\n");
            if (endPointInfo.Callback == null)
            {
                Console.WriteLine($"Testing {endPointInfo.Uri}");
                var uri = $"net.tcp://{endPointInfo.Uri}:{endPointInfo.Port}/FileShare";
                var callback = new InstanceContext(new FileShareCallback());
                var binding = new NetTcpBinding(SecurityMode.None);
                var channel = new DuplexChannelFactory<IFileShareService>(callback, binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                if (proxy != null)
                {
                    var hinfo = new HostInfo
                    {
                        Id = ConfigurePeer.Peer.PeerId,
                        Port = _port,
                        Uri = RegisterPeer.PeerUri
                    };
                    proxy.PingHostService(hinfo, true);
                }
            }
            else
            {
                if (_currentHost.Any())
                {
                    _currentHost.Add(endPointInfo.Id, endPointInfo);
                    Console.WriteLine($"Testing {endPointInfo.Uri}");
                    var uri = $"net.tcp://{endPointInfo.Uri}:{endPointInfo.Port}/FileShare";
                    var callback = new InstanceContext(new FileShareCallback());
                    var binding = new NetTcpBinding(SecurityMode.None);
                    var channel = new DuplexChannelFactory<IFileShareService>(callback, binding);
                    var endPoint = new EndpointAddress(uri);
                    var proxy = channel.CreateChannel(endPoint);
                    if (proxy != null)
                    {
                        HostInfo info = new HostInfo
                        {
                            Id = ConfigurePeer.Peer.PeerId,
                            Port = _port,
                            Uri = RegisterPeer.PeerUri
                        };
                        proxy.PingHostService(info);
                        Console.WriteLine($"{_currentHost.Count} Host currently connected");
                        _currentHost.ToList().ForEach(p =>
                        {
                            Console.WriteLine($"Host ID: {p.Key}");
                            Console.WriteLine($"EndPoint: {p.Value.Uri}:{p.Value.Port}");
                        });
                    }
                }
                else
                {
                    if (_currentHost.Any(p => p.Key == endPointInfo.Id))
                    {
                        Console.WriteLine("Host already exists");
                    }
                    else
                    {
                        var uri = $"net.tcp://{endPointInfo.Uri}:{endPointInfo.Port}/FileShare";
                        var callback = new InstanceContext(new FileShareCallback());
                        var binding = new NetTcpBinding(SecurityMode.None);
                        var channel = new DuplexChannelFactory<IFileShareService>(callback, binding);
                        var endPoint = new EndpointAddress(uri);
                        var proxy = channel.CreateChannel(endPoint);
                        if (proxy != null)
                        {
                            HostInfo info = new HostInfo
                            {
                                Id = ConfigurePeer.Peer.PeerId,
                                Port = _port,
                                Uri = RegisterPeer.PeerUri
                            };
                            proxy.PingHostService(info);
                            Console.WriteLine($"{_currentHost.Count} Host currently connected");
                            _currentHost.ToList().ForEach(p =>
                            {
                                Console.WriteLine($"Host ID: {p.Key}");
                                Console.WriteLine($"EndPoint: {p.Value.Uri}:{p.Value.Port}");
                            });
                        }
                    }
                }
            }
        }

        public bool StartFileShareService(int port, string uri)
        {
            if (uri.Any() && _port > 0)
            {
                Uri[] uris = new Uri[1];
                var address = $"net.tcp://{uri}:{port}/FileShare";
                uris[0] = new Uri(address);
                IFileShareService filehsare = _file;
                var host = new ServiceHost(filehsare, uris);
                var binding = new NetTcpBinding(SecurityMode.None);
                host.AddServiceEndpoint(typeof(IFileShareService), binding, string.Empty);
                host.Opened += HostOnOpened;
                _file.CurrentHostUpdate += FileOnCurrentUpdate;
                host.Open();
                return IsStarted;
            }
            return IsStarted;
        }

        private void FileOnCurrentUpdate(HostInfo info, bool isCallback)
        {
            if (isCallback)
            {
                var uri = $"net.tcp://{info.Uri}:{info.Port}/FileShare";
                var callback = new InstanceContext(new FileShareCallback());
                var binding = new NetTcpBinding(SecurityMode.None);
                var channel = new DuplexChannelFactory<IFileShareService>(callback, binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                if (proxy != null)
                {
                    HostInfo hinfo = new HostInfo
                    {
                        Id = ConfigurePeer.Peer.PeerId,
                        Port = _port,
                        Uri = RegisterPeer.PeerUri
                    };
                    proxy.PingHostService(hinfo);
                    _currentHost.Add(info.Id, hinfo);
                    Console.WriteLine($"{_currentHost.Count(p => p.Value.Callback != null)} Host with direct connection");
                    Console.WriteLine($"{_currentHost.Count} Host available");
                    _currentHost.Distinct().ToList().ForEach(p =>
                    {
                        Console.WriteLine($"Host Info: ID: {p.Key}      Host: {p.Value.Uri}:{p.Value.Port}");
                    });
                }
            }
            else
            {
                if (info != null && _currentHost.All(p => p.Key != info.Id))
                {
                    _currentHost.Add(info.Id, info);
                    Console.WriteLine($"{_currentHost.Count} Host currently available");
                    _currentHost.ToList().ForEach(p =>
                    {
                        Console.WriteLine($"Host ID: {p.Key} EndPoint: {p.Value.Uri}:{p.Value.Port}");
                    });
                }
                else if (!_currentHost.Any())
                {
                    _currentHost.Add(info.Id, info);
                    Console.WriteLine($"{_currentHost.Count} Host currently available");
                    _currentHost.ToList().ForEach(p =>
                    {
                        Console.WriteLine($"Host ID: {p.Key}");
                        Console.WriteLine($"EndPoint: {p.Value.Uri}:{p.Value.Port}");
                    });
                }
            }
        }

        private void HostOnOpened(object sender, EventArgs e)
        {
            IsStarted = true;
        }
    }
}
