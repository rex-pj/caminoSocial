﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Coco.Common.Helper
{
    public static class ImageHelper
    {
        public static Image Base64ToImage(string base64String)
        {
            string base64 = base64String.Substring(base64String.IndexOf(',') + 1);
            byte[] imageBytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static string ToBase64String(this Bitmap bmp, ImageFormat imageFormat)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                string base64String = string.Empty;
                bmp.Save(memoryStream, imageFormat);

                memoryStream.Position = 0;
                byte[] byteBuffer = memoryStream.ToArray();

                memoryStream.Close();

                base64String = Convert.ToBase64String(byteBuffer);
                byteBuffer = null;

                return base64String;
            }
        }

        public static string CropBase64Image(string base64String, int x, int y, int width, int height)
        {
            var image = Base64ToImage(base64String);
            var cropRect = new Rectangle(x, y, width, height);
            var src = image as Bitmap;
            var target = new Bitmap(cropRect.Width, cropRect.Height);

            using (var graphic = Graphics.FromImage(target))
            {
                graphic.DrawImage(src, cropRect,
                    new Rectangle(0, 0, src.Width, src.Height),
                    GraphicsUnit.Pixel);

                var targetImage = ToBase64String(target, target.RawFormat);
                return targetImage;
            }
        }
    }
}
