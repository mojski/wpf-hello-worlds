using System.IO;
using System.Windows.Media.Imaging;

namespace WinUI
{
    public static class FileHelper
    {
        private const string IMAGES_FOLDER = "images";

        public static string GetFileName(string filePath) => Path.GetFileNameWithoutExtension(filePath);

        public static string GetExtension (string filePath) => Path.GetExtension(filePath);

        public static string GetFullFileName(string filePath) => Path.GetFileName(filePath);

        public static string GetJsonFilePath(string filePath)
        {
            var fileName = GetFullFileName(filePath);
            
            return $"/{IMAGES_FOLDER}/{fileName}";
        }

        public static string GetBaseFolderPath(string loadFilePath)
        {
            var folder = Path.GetDirectoryName(loadFilePath);
            return Path.Combine(folder!);
        }

        public static string GetImagePhysicalPath(string jsonPath, string imageName)
        {
            var fileName = Path.GetFileName(imageName);
            var folder = GetBaseFolderPath(jsonPath);

            return Path.Combine(folder, IMAGES_FOLDER, fileName);
        }

        public static async Task<BitmapImage> GetBitmapImageAsync(string filePath, CancellationToken cancellation = default)
        {
            var imageBytes = await File.ReadAllBytesAsync(filePath, cancellation);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
