using Azure.Storage.Files.Shares;

namespace ABCRetail.Web.Services
{
    public class FileService : IFileService
    {
        private readonly ShareClient _shareClient;
        private readonly ShareDirectoryClient _directoryClient;

        public FileService(ShareServiceClient shareServiceClient)
        {
            _shareClient = shareServiceClient.GetShareClient("log-files");
            _shareClient.CreateIfNotExists();
            
            _directoryClient = _shareClient.GetRootDirectoryClient();
        }

        public async Task<string> UploadLogFileAsync(IFormFile file)
        {
            var fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{file.FileName}";
            var fileClient = _directoryClient.GetFileClient(fileName);
            
            using var stream = file.OpenReadStream();
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadAsync(stream);
            
            return fileName;
        }

        public async Task<IEnumerable<string>> GetLogFilesAsync()
        {
            var files = new List<string>();
            await foreach (var item in _directoryClient.GetFilesAndDirectoriesAsync())
            {
                if (!item.IsDirectory)
                {
                    files.Add(item.Name);
                }
            }
            return files;
        }

        public async Task<Stream> DownloadLogFileAsync(string fileName)
        {
            var fileClient = _directoryClient.GetFileClient(fileName);
            var response = await fileClient.DownloadAsync();
            return response.Value.Content;
        }
    }
}
