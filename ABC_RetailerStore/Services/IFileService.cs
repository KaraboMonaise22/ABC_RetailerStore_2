namespace ABCRetail.Web.Services
{
    public interface IFileService
    {
        Task<string> UploadLogFileAsync(IFormFile file);
        Task<IEnumerable<string>> GetLogFilesAsync();
        Task<Stream> DownloadLogFileAsync(string fileName);
    }
}
