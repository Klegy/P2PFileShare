namespace Fileshare.Domain.Models
{
    public class File
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public byte[] FileContent { get; set; }

        public FileMetaData GetFileMeta()
        {
            return new FileMetaData(FileId, FileName, FileSize);
        }
    }
}
