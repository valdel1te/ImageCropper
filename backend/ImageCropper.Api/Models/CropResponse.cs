using System.Drawing;

namespace ImageCropper.Api.Models;

public class CropResponse
{
    public byte[] ImageData { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
}