namespace Fileshare.Domain.Models
{
    public class HostInfo
    {
        public string Id { get; set; }
        public string Uri { get; set; }
        public int Port { get; set; }
        public object Callback { get; set; }
    }
}
