using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Gif;


namespace NinjaBotCore.Modules.Giphy
{
    public class ImageText
    {
        string imageSavePath = "c:\\images\\";
        string fontPath = ".\\";
        public string CreateTextInShape(string sImageText)
        {
            IImageFormat format;
            FontCollection collection = new();
            FontFamily family;
            if (File.Exists(fontPath + "BOBCAT.TTF"))
            { family = collection.Add("bobcat.ttf"); }
            else { family = collection.Add("c:\\windows\\fonts\\candara.ttf"); }
            Guid imageName = Guid.NewGuid();
            string imagePath = imageSavePath + imageName + ".gif";
            int fontLength = sImageText.Length;
            Font font = family.CreateFont(55, FontStyle.Regular);
            if (!Directory.Exists(imageSavePath))
                Directory.CreateDirectory(imageSavePath);

            Stream outputStream = new MemoryStream();
            int imageX = fontLength * 50;
            int imageY = fontLength * 35;
            var boxspace = new PointF()
            {
                X = imageX / 2,
                Y = imageX / 2,
            };
            int radius = imageX / 2;
            Image<Rgba32> image = new(imageX, imageX);
            var gifMetaData = image.Metadata.GetGifMetadata();
            gifMetaData.RepeatCount = 5;
            GifFrameMetadata metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
            int frameDelay = 1;
            DrawingOptions options = new()
            {
                GraphicsOptions = new()
                {
                    ColorBlendingMode = PixelColorBlendingMode.Multiply
                }
            };
            TextOptions textOptions = new TextOptions(font)
            {
                Origin = new PointF(boxspace.X, boxspace.Y),
                HorizontalAlignment = HorizontalAlignment.Center,
                //TextAlignment = TextAlignment.Center,
                TextJustification = TextJustification.InterCharacter,
                VerticalAlignment = VerticalAlignment.Center,
            };
            IBrush brush = Brushes.Solid(Color.Purple);

            IPen pen = Pens.Solid(Color.Green, 5);
            //image.Mutate(c => c.Resize(100, 100));
            int mandala = new Random().Next(1, 100);
            int countOfAngles = new Random().Next(25, 95);
            int angleSize = 360 / countOfAngles;
            var prongs = new Random().Next(5, 15);
            metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
            metadata.FrameDelay = frameDelay;
            for (int i = 360; i > countOfAngles; i -= angleSize)
            {

                using Image<Rgba32> image2 = new(imageX, imageX);
                IPath yourPolygon2 = new Star(boxspace, prongs: prongs, innerRadii: radius / 2, outerRadii: radius, i);
                image2.Mutate(x => x.Fill(options, brush, yourPolygon2).Draw(options, pen, yourPolygon2));
                image2.Mutate(x => x.DrawText(textOptions, sImageText, Color.White));
                GifFrameMetadata metadata2 = image2.Frames.RootFrame.Metadata.GetGifMetadata();
                if (mandala < 83) { metadata2.DisposalMethod = GifDisposalMethod.RestoreToBackground; }
                
                metadata2.FrameDelay = frameDelay;
                image.Frames.AddFrame(image2.Frames.RootFrame);
                image2.Dispose();

            }
            image.Frames.RemoveFrame(0);
            image.SaveAsGif(imageSavePath + imageName + ".gif");

            image.Dispose();
            return (imagePath);
        }
        public void ClearImagesFromStorage()
        {
            if (!Directory.Exists(imageSavePath)) return;
            else
            {
                var fileList = Directory.EnumerateFiles(imageSavePath);
                foreach (var file in fileList)
                {
                    File.Delete(file);
                }
            }

        }
    }
}
