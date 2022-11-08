namespace SIEL_1836109025062022.Services
{
    public interface IFilesManagement
    {
        void DeleteExistingFile(string db_path);
        Task<string> UploadFile(string path, IFormFile file, string file_name, string file_location);
    }
    public class FilesManagement : IFilesManagement


    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FilesManagement(IWebHostEnvironment webHostEnvironment )
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFile(string path, IFormFile file, string file_name, string file_location)
        {
            var fileName = System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                path, file_name + Path.GetExtension(file.FileName));
            var file_name_db = System.IO.Path.
                Combine("/", file_location,
                file_name + Path.GetExtension(file.FileName));

            await file.CopyToAsync(new System.IO.FileStream(
                fileName, System.IO.FileMode.Create));
            return file_name_db;

        }
        public void DeleteExistingFile(string db_path)
        {
            string path = System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                "wwwroot/" + db_path);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

    }
}
