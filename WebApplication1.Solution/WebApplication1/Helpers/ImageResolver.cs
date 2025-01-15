using CloudinaryDotNet;
using WebApplication1.Service;

public class ImageResolver
{
    private readonly IConfiguration _configuration;
    private readonly Cloudinary _cloudinary;

    // Inject the CloudinaryService to get the Cloudinary instance
    public ImageResolver(IConfiguration configuration, CloudinaryService cloudinaryService)
    {
        _configuration = configuration;
        _cloudinary = cloudinaryService.GetCloudinaryInstance();  // Access the Cloudinary instance here
    }

    public async Task<string> GetImageFromCloudinary(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            throw new ArgumentNullException(nameof(publicId), "Public ID cannot be null or empty.");
        }

        if (_cloudinary == null)
        {
            throw new InvalidOperationException("Cloudinary instance is not initialized.");
        }

        // Use the Cloudinary API to fetch the resource using the publicId
        var result = await _cloudinary.GetResourceAsync(publicId);

        if (result == null)
        {
            throw new Exception("Failed to fetch image from Cloudinary.");
        }

        // Return the URL of the image
        return result.Url.ToString();
    }
}
