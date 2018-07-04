using System.Runtime.Serialization;

namespace Fileshare.Domain.Models
{
    [DataContract]
    public partial class FileMetaData
    {
        public FileMetaData(string fileid, string filename, int filesize)
        {
            FileId = fileid;
            FileName = filename;
            FileSize = filesize;
        }

        [DataMember]
        public string FileId { get; set; }
 
        [DataMember]
        public string FileName { get; set; }
 
        [DataMember]
        public int FileSize { get; set; }
    }
}
