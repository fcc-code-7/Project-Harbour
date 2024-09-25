using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Web.Controllers
{
    public class SharedController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public SharedController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [RequestSizeLimit(104857600)] // 50MB

        [HttpPost]
        public IActionResult Upload(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "PDocs");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                // Return the correct URL for the image
                var imageUrl = Url.Content($"~/PDocs/{uniqueFileName}");
                return Json(new { success = true, imageUrl });
            }

            return Json(new { success = false, message = "Invalid image file." });
        }


    }
}
