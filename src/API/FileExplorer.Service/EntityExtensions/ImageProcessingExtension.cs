using System.IO;
using FileExplorer.DataModel;
using FileExplorer.Service;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

public static class ImageProccessingExtension
{
    public static byte[] CompressImage(this FileModel file, int quality)
    {
        using (MemoryStream inputStream = new MemoryStream(file.Content))
        {
            using (Image image = Image.Load(inputStream))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    // Perform image compression using ImageSharp
                    image.Save(outputStream, new JpegEncoder { Quality = quality });

                    return outputStream.ToArray();
                    
                }
            }
        }
    }

    public static bool isImage(this FileModel file)
    {
        return file.ContentType.Contains("image");
    }
}
