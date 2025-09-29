using ABCRetail.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Web.Controllers
{
    public class FilesController : Controller
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var files = await _fileService.GetLogFilesAsync();
            return View(files);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                await _fileService.UploadLogFileAsync(file);
                return RedirectToAction(nameof(Index));
            }
            
            ModelState.AddModelError("", "Please select a file to upload.");
            return View();
        }

        public async Task<IActionResult> Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            var stream = await _fileService.DownloadLogFileAsync(fileName);
            return File(stream, "application/octet-stream", fileName);
        }

        public IActionResult Details(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            ViewBag.FileName = fileName;
            return View();
        }

        public IActionResult Edit(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            ViewBag.OriginalFileName = fileName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string originalFileName, string newFileName)
        {
            if (string.IsNullOrEmpty(originalFileName) || string.IsNullOrEmpty(newFileName))
            {
                ModelState.AddModelError("", "Both original and new file names are required.");
                ViewBag.OriginalFileName = originalFileName;
                return View();
            }

            // File rename logic would go here
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string fileName)
        {
            ViewBag.FileName = fileName;
            ViewData["Title"] = "Delete File";
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string fileName, bool confirmed = false)
        {
            if (confirmed && !string.IsNullOrEmpty(fileName))
            {
                // Add your file deletion logic here
                // Example: System.IO.File.Delete(Path.Combine("uploads", fileName));
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
