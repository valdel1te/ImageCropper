using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using ImageCropper.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageCropper.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CropController : ControllerBase
{
    private const int ImageSizeI = 1600;
    private const int ImageSizeJ = 1200;

    [HttpPost("cropped-array")]
    public async Task<ActionResult<CropResponse[]>> CropImage([FromForm] CropRequest cropRequest)
    {
        var file = cropRequest.ImageData;
        if (file == null || file.Length == 0)
            return BadRequest("error >> file is empty");

        CropResponse[] cropResponses;
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var bitmapRequestImage = new Bitmap(memoryStream);

            cropResponses =
                CropOneToManyWithPrintedCoords(bitmapRequestImage, cropRequest.I, cropRequest.J, memoryStream);
        }

        return Ok(cropResponses);
    }

    private static CropResponse[] CropOneToManyWithPrintedCoords
    (
        Bitmap bitmapOriginal,
        int sizeI,
        int sizeJ,
        MemoryStream memoryStream
    )
    {
        var resultArray = new CropResponse[sizeI * sizeJ];

        var width = ImageSizeI / sizeI;
        var height = ImageSizeJ / sizeJ;

        var index = 0;
        for (var i = 0; i < sizeJ; i++)
        {
            for (var j = 0; j < sizeI; j++)
            {
                var x = j * width;
                var y = i * height;

                var cropRectangle = new Rectangle(x, y, width, height);
                var croppedBitmap = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(croppedBitmap))
                {
                    graphics.DrawImage(
                        bitmapOriginal,
                        new Rectangle(0, 0, width, height),
                        cropRectangle,
                        GraphicsUnit.Pixel
                    );

                    var font = new Font("Courier New", 20.0f);

                    graphics.DrawString(
                        $"{x},{y}",
                        font,
                        Brushes.Blue, 
                        0,
                        0
                    );
                }

                memoryStream.SetLength(0);
                croppedBitmap.Save(memoryStream, ImageFormat.Png);

                var cropResponse = new CropResponse
                {
                    ImageData = memoryStream.ToArray(),
                    X = x,
                    Y = y,
                    Height = height,
                    Width = width
                };

                resultArray[index] = cropResponse;
                index++;
            }
        }

        return resultArray;
    }
}