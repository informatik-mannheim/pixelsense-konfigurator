using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

using WPFImage = System.Windows.Controls.Image;
using Image = System.Drawing.Image;

namespace GrabTheScreen
{
    class ImageHelper
    {
        /// <summary>
        /// Converts Image in a WPF Image Control to a System.Drawing.Image.
        /// </summary>
        /// <param name="image">Image to be converted.</param>
        /// <returns>Converted image.</returns>
        public static Image ConvertWpfImageToImage(WPFImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image", "Image can not be null.");
            }
                
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            MemoryStream ms = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
            encoder.Save(ms);
            return Image.FromStream(ms);
        }

        /// <summary>
        /// Creates Base64 string representing the bytes of an image.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <returns>The Base64 string representing the image.</returns>
        public static string ToBase64String(Image image)
        {
            if (image != null)
            {
                ImageConverter imageConverter = new ImageConverter();
                byte[] buffer = (byte[]) imageConverter.ConvertTo(image, typeof(byte[]));
                return Convert.ToBase64String(buffer, Base64FormattingOptions.InsertLineBreaks);
            }
            else
            {
                return null;
            }
        }
    }
}
