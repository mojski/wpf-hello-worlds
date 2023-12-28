using System.IO;
using System.Windows.Media.Imaging;

namespace WinUI;

public static class FileHelper
{
    private const string IMAGES_FOLDER = "images";
    private const string RESOURCES_FOLDER = "Resources";
    private const string FUN_FACT_IMAGE_PLUG = "noimg.png";

    public static string GetFullFileName(string filePath) => Path.GetFileName(filePath);

    public static string GetImageJsonFilePath(string filePath)
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

    public static async Task<BitmapImage> GetBitmapImageAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var imageBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = new MemoryStream(imageBytes);
        bitmapImage.EndInit();
        return bitmapImage;
    }

    public static async Task<BitmapImage> GetImagePlugAsync(CancellationToken cancellationToken = default)
    {
        var location = AppDomain.CurrentDomain.BaseDirectory;

        var plugPath = Path.Combine(location, RESOURCES_FOLDER, FUN_FACT_IMAGE_PLUG);

        var bitmap = await GetBitmapImageAsync(plugPath, cancellationToken);

        return bitmap;
    }

    public static async Task SaveImageAndGetFileNameAsync(BitmapImage bitmap, string fileName, string jsonFilePath, CancellationToken cancellationToken)
    {
        var folder = GetBaseFolderPath(jsonFilePath);
        var saveImageFolder = Path.Combine(folder, IMAGES_FOLDER);

        if (Directory.Exists(saveImageFolder) is false)
        {
            Directory.CreateDirectory(saveImageFolder);
        }

        var fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);

        var saveImagePath = Path.Combine(saveImageFolder, fileNameOnly + extension);

        BitmapEncoder encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));

        await using var fileStream = new FileStream(saveImagePath, FileMode.Create);
        encoder.Save(fileStream);
    }
}