using Fileshare.Domain.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FileShare.SampleData
{
    public class FileSample
    {
        private ObservableCollection<File> _availableFiles = new ObservableCollection<File>();
        private ObservableCollection<FileMetaData> _metaData = new ObservableCollection<FileMetaData>();

        public ObservableCollection<File> GetAvailableFiles()
        {
            if (!_availableFiles.Any())
            {
                _availableFiles.Add(new Task<File>(() =>
                {
                    var file = new File
                    {
                        FileId = Guid.NewGuid().ToString().Split('-')[4],
                        FileContent = new byte[2347],
                        FileName = "Max Payne",
                        FileSize = 2347,
                        FileType = "video/mp4"
                    };
                    _metaData.Add(file.GetFileMeta());
                    return file;
                }).Result);

                _availableFiles.Add(new Task<File>(() =>
                {
                    var file = new File
                    {
                        FileId = Guid.NewGuid().ToString().Split('-')[4],
                        FileContent = new byte[4325],
                        FileName = "Star Wars, A New Hope",
                        FileSize = 4325,
                        FileType = "video/mp4"
                    };
                    _metaData.Add(file.GetFileMeta());
                    return file;
                }).Result);

                _availableFiles.Add(new Task<File>(() =>
                {
                    var file = new File
                    {
                        FileId = Guid.NewGuid().ToString().Split('-')[4],
                        FileContent = new byte[5672],
                        FileName = "Star Wars, The Empire Strikes Back",
                        FileSize = 5672,
                        FileType = "video/mp4"
                    };
                    _metaData.Add(file.GetFileMeta());
                    return file;
                }).Result);

                _availableFiles.Add(new Task<File>(() =>
                {
                    var file = new File
                    {
                        FileId = Guid.NewGuid().ToString().Split('-')[4],
                        FileContent = new byte[3425],
                        FileName = "Star Wars, Return Of The Jedi",
                        FileSize = 3425,
                        FileType = "video/mp4"
                    };
                    _metaData.Add(file.GetFileMeta());
                    return file;
                }).Result);
            }
            return _availableFiles;
        }

        public ObservableCollection<FileMetaData> GetFileMetaData()
        {
            if (!_metaData.Any())
            {
                GetAvailableFiles().ToList().ForEach(p =>
                {
                    _metaData.Add(new FileMetaData(p.FileId, p.FileName, p.FileSize));
                });
                return _metaData;
            }
            return _metaData;
        }


    }
}
