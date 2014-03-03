using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Utilities.Graphics
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphicsHelpers
    {
        /// <summary>
        /// Resizes image by keeping aspect ratio.
        /// </summary>
        /// <param name="sourceImageFile">Source image file name</param>
        /// <param name="targetImageFile">Target image file name</param>
        /// <param name="width">New image width</param>
        /// <param name="quality">New image quality</param>
        /// <exception cref="ArgumentNullException">When <paramref name="sourceImageFile"/> or <paramref name="targetImageFile"/> are null</exception>
        /// <exception cref="FileNotFoundException">When <paramref name="sourceImageFile"/> does not exist</exception>
        public static void ResizeImage(string sourceImageFile, string targetImageFile, int width,long quality)
        {
            if (sourceImageFile == null) throw new ArgumentNullException("sourceImageFile");
            if (targetImageFile == null) throw new ArgumentNullException("targetImageFile");
            if (!File.Exists(sourceImageFile))
                throw new FileNotFoundException("File " + sourceImageFile + " could not be found");

            Image sourceImage;
            try
            {
                sourceImage = Image.FromFile(sourceImageFile);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not load image from file " + sourceImageFile, ex);
            }


            Image targetImage;
            try
            {
                targetImage = ResizeImage(sourceImage, width, quality);
            }
            catch (Exception ex)
            {
                throw new Exception("Error resizing image", ex);
            }

            try
            {
                targetImage.Save(targetImageFile, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving image to file " + targetImageFile, ex);
            }
        }

        /// <summary>
        /// Resizes image by keeping aspect ratio.
        /// </summary>
        /// <param name="sourceImage">Source image</param>
        /// <param name="width">New image width</param>
        /// <param name="quality">New image quality</param>
        /// <returns>Resized image</returns>
        public static Image ResizeImage(Image sourceImage, int width, long quality)
        {
            if (sourceImage == null) throw new ArgumentNullException("sourceImage");
            if (width <= 0) throw new ArgumentException("Invalid width " + width);
            if (quality <= 0 || quality > 100) throw new ArgumentException("Invalid quality " + quality);

            var newWidth = Math.Min(150, sourceImage.Width);
            var newHeight = newWidth * sourceImage.Height / sourceImage.Width;
            using (var bitmap = new Bitmap(newWidth, newHeight))
            {
                using (var gr = System.Drawing.Graphics.FromImage(bitmap))
                {
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(sourceImage, new Rectangle(0, 0, newWidth, newHeight));
                }

                var qualityEncoder = Encoder.Quality;
                var ratio = new EncoderParameter(qualityEncoder, quality);
                var codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;
                ImageCodecInfo jpegCodecInfo;
                try
                {
                    jpegCodecInfo = ImageCodecInfo.GetImageEncoders()
                                                  .First(x => x.MimeType.Trim().ToLower() == "image/jpeg");
                }
                catch (Exception)
                {
                    throw new Exception("Could not find any JPEG encoders in GDI+");
                }

                Image targetImage;
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, jpegCodecInfo, codecParams);
                    targetImage = Image.FromStream(ms);
                }
                return targetImage;
            }
        }
    }
}
