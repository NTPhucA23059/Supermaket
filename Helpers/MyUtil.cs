using System.Text;

namespace Supermaket.Helpers
{
    public class MyUtil
    {
        public static string UploadHinh(IFormFile file, string folder)
        {
            try
            {
                var directoryPath = Path.Combine("wwwroot", "Image", folder);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var filePath = Path.Combine(directoryPath, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return file.FileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tải hình ảnh: {ex.Message}");
                return string.Empty;
            }
        }


        public static string GenerateRamdomKey(int length = 5)
        {
            var pattern = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}
