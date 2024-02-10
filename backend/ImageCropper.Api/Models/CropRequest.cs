namespace ImageCropper.Api.Models;

public class CropRequest
{
    public IFormFile ImageData { get; set; }
    public int I { get; set; }
    public int J { get; set; }
}