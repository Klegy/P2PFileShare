using Fileshare.Domain.Models;
using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Fileshare.Domain.FileSearch
{
    [DataContract]
    public class FileSearchResultModel
    {
        [DataMember]
        public string PeerId { get; set; }

        [DataMember]
        public ICollection<FileMetaData> Files { get; set; }
    }
}
