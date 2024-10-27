using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

namespace Portfolio_Website.Pages
{
    public class GalleryModel : PageModel
    {
        [BindProperty]
        public IFormFile file { get; set; }

        public string UploadResult { get; set; }
        public string PhotoFolder { get; set; }

        public List<string> imagesFilePath { get; set; } = new List<string>();

        public void OnGet()
        {
            LoadPhotos();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            System.Diagnostics.Debug.WriteLine("it went off");

            if (file == null)
            {
                UploadResult = "Please select a Photo to upload.";
                LoadPhotos();
                return Page();
            }

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!extension.Contains(".jpg") || !extension.Contains(".jpeg") || !extension.Contains(".png"))
            {
                UploadResult = "Invalid file type. Please upload an image (jpg, jpeg, png).";
                LoadPhotos();
                return Page();
            }

            try
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", file.FileName);
                PhotoFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos");

                int nmbr = 1;

                if (System.IO.File.Exists(filepath))
                {
                    string newFileName = "{fileName}_{count++}{extension}";
                    filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", newFileName);
                }

                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    imagesFilePath = new List<string>();
                    imagesFilePath = Directory.GetFiles(PhotoFolder, "*.*").Where(f => f.EndsWith(".jpg") || f.EndsWith(".png")).Select(f => $"/Photos/{Path.GetFileName(f)}").ToList();
                    
                }

                UploadResult = "file uploaded successfully";
                LoadPhotos();
                return Page();
            }

            catch (Exception ex)
            {
                UploadResult += ex.ToString();
                return null;
            }
        }

        private void LoadPhotos()
        {
            PhotoFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos");
            imagesFilePath = new List<string>();
            imagesFilePath = Directory.GetFiles(PhotoFolder, "*.*").Where(f => f.EndsWith(".jpg") || f.EndsWith(".png")).Select(f => $"/Photos/{Path.GetFileName(f)}").ToList();
        }
    }


}
