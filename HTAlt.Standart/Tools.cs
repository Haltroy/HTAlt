//MIT License
//
//Copyright (c) 2020 Eren "Haltroy" Kanat
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HTAlt
{
    /// <summary>
    /// Custom class to handle custom actions and events.
    /// </summary>
    public class Tools
    {
        /// <summary>
        /// Resizes an <paramref name="image"/> to a certain <paramref name="height"/> and <paramref name="width"/>.
        /// </summary>
        /// <param name="image">Image to resize</param>
        /// <param name="width">Width of result.</param>
        /// <param name="height">Height of result.</param>
        /// <returns>Resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        /// <summary>
        /// Removes <paramref name="RemoveText"/> from files names in <paramref name="folder"/>.
        /// </summary>
        /// <param name="folder">Work folder</param>
        /// <param name="RemoveText">Text to remove</param>
        /// <returns><c>true</c> if successfully removes <paramref name="RemoveText"/>, otherwise <c>false</c>.</returns>
        public static bool RemoveFromFileNames(string folder, string RemoveText)
        {
            bool isSuccess = false;
            int success = 0;
            int C = 0;
            List<Exception> errors = new List<Exception>();
            try
            {
                foreach (string x in Directory.GetFiles(folder))
                {
                    string pathName = Path.GetFileName(x);
                    C++;
                    if (pathName.Contains(RemoveText))
                    {
                        string newName = folder + pathName.Replace(RemoveText, "");
                        try
                        {
                            File.Move(x, newName);
                            success++;
                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex);
                        }
                    }
                }
                Console.WriteLine("Count: " + C + " Success: " + success + " Error: " + errors.Count);
                foreach (Exception ex in errors)
                {
                    Console.WriteLine(ex.ToString());
                }
                isSuccess = true;
                return isSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Returns either <paramref name="black"/> or <paramref name="white"/> by determining with the brightess of <paramref name="color"/>.
        /// </summary>
        /// <param name="color">Color for determining.</param>
        /// <param name="white">White/Bright image to return.</param>
        /// <param name="black">Black/Dark image  to return</param>
        /// <param name="reverse"><c>true</c> to return <paramref name="black"/> on black/dark images and <paramref name="white"/> for white/bright images, otherwise <c>false</c>.</param>
        /// <returns><paramref name="black"/> or <paramref name="white"/>.</returns>
        public static Bitmap SelectImageFromColor(Color color, ref Bitmap white, ref Bitmap black, bool reverse = false)
        {
            return IsBright(color) ? (reverse ? white : black) : (reverse ? black : white);
        }

        /// <summary>
        /// Returns either <paramref name="black"/> or <paramref name="white"/> by determining with the brightess of <paramref name="color"/>.
        /// </summary>
        /// <param name="color">Color for determining.</param>
        /// <param name="white">White/Bright image to return.</param>
        /// <param name="black">Black/Dark image  to return</param>
        /// <param name="reverse"><c>true</c> to return <paramref name="black"/> on black/dark images and <paramref name="white"/> for white/bright images, otherwise <c>false</c>.</param>
        /// <returns><paramref name="black"/> or <paramref name="white"/>.</returns>
        public static Image SelectImageFromColor(Color color, ref Image white, ref Image black, bool reverse = false)
        {
            return SelectImageFromColor(color, ref white, ref black, reverse);
        }

        /// <summary>
        /// Changes <paramref name="basecolor"/> and it's different shades to <paramref name="alteringColor"/> and responding shades in <paramref name="image"/>.
        /// </summary>
        /// <param name="image">Image to work on.</param>
        /// <param name="basecolor">Color to change.</param>
        /// <param name="alteringColor">New Color.</param>
        /// <returns>Recolored <see cref="Bitmap"/></returns>
        public static Bitmap ChangeColor(Bitmap image, Color basecolor, Color alteringColor)
        {
            Bitmap nbitmap = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < 256; i++)
            {
                Color workColor = ShiftBrightnessTo(basecolor, i, false);
                Color changeColor = ShiftBrightnessTo(alteringColor, i, false);
                nbitmap = ColorReplace(nbitmap, 0, workColor, changeColor);
            }
            return nbitmap;
        }

        /// <summary>
        /// Changes <paramref name="basecolor"/> and it's different shades to <paramref name="alteringColor"/> and responding shades in <paramref name="image"/>.
        /// </summary>
        /// <param name="image">Image to work on.</param>
        /// <param name="basecolor">Color to change.</param>
        /// <param name="alteringColor">New Color.</param>
        /// <returns>Recolored <see cref="Image"/></returns>
        public static Image ChangeColor(Image image, Color basecolor, Color alteringColor)
        {
            return ChangeColor(image, basecolor, alteringColor);
        }

        /// <summary>
        /// Resizes an <paramref name="image"/> to a certain <paramref name="size"/>.
        /// </summary>
        /// <param name="image">Image to resize</param>
        /// <param name="size">Size to resize to.</param>
        /// <returns>Resized image.</returns>
        public static Bitmap ResizeImage(Image image, Size size)
        {
            Rectangle destRect = new Rectangle(0, 0, size.Width, size.Height);
            Bitmap destImage = new Bitmap(size.Width, size.Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        /// <summary>
        /// Fills <paramref name="rect"/> with a repeated pattern of <paramref name="image"/>.
        /// </summary>
        /// <param name="image">Patter image.</param>
        /// <param name="rect">Rectangle for size reference.</param>
        /// <returns>Image with the size of <paramref name="rect"/> filled with a pattern of <paramref name="image"/>.</returns>
        public static Bitmap FillPattern(Image image, Rectangle rect)
        {
            Rectangle _ImageRect;
            Rectangle drawRect;
            Bitmap result = new Bitmap(image, rect.Size);
            using (Graphics g = Graphics.FromImage(result))
            {
                for (int x = rect.X; x < rect.Right; x += image.Width)
                {
                    for (int y = rect.Y; y < rect.Bottom; y += image.Height)
                    {
                        drawRect = new Rectangle(x, y, Math.Min(image.Width, rect.Right - x),
                                       Math.Min(image.Height, rect.Bottom - y));
                        _ImageRect = new Rectangle(0, 0, drawRect.Width, drawRect.Height);

                        g.DrawImage(image, drawRect, _ImageRect, GraphicsUnit.Pixel);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Converts the image to Base 64 code.
        /// </summary>
        /// <param name="image">Image to convert.</param>
        /// <returns>String representing the base 64 code of image.</returns>
        public static string ImageToBase64(System.Drawing.Image image)
        {
            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, image.RawFormat);
                byte[] imageBytes = m.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        /// <summary>
        /// Determines if a site is an actually from Haltroy.
        /// </summary>
        /// <param name="SiteUrl">Site URL</param>
        /// <returns><c>true</c> if <paramref name="SiteUrl"/> is actually a Haltroy website, otherwise <c>false</c>.</returns>
        public static bool ValidHaltroyWebsite(string SiteUrl)
        {
            string Pattern = @"((?:http(s)?\:\/\/)?(.*\.)?haltroy\.com)";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return ValidUrl(SiteUrl) && Rgx.IsMatch(SiteUrl.Substring(0, SiteUrl.ToLower().StartsWith("http") ? SiteUrl.IndexOf(@"\", 10) : SiteUrl.IndexOf(@"\")));
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="Url">Address to determine.</param>
        /// <param name="CustomProtocols">Protocols (like <c>http</c>) to detect </param>
        /// <param name="options">Regex options to check </param>
        /// <param name="ignoreDefaults">Ignores default protocols if <c>true</c></param>
        /// <returns><c>true</c> if <paramref name="Url"/> is a valid address within <paramref name="CustomProtocols"/> rules, otherwise <c>false</c>.</returns>
        public static bool ValidUrl(string Url, string[] CustomProtocols, RegexOptions options, bool ignoreDefaults)
        {
            if (string.IsNullOrWhiteSpace(Url) || Url.Contains(" "))
            { return false; }
            else
            {
                if (!ignoreDefaults)
                {
                    int startL = CustomProtocols.Length;
                    Array.Resize<string>(ref CustomProtocols, startL + 7);
                    CustomProtocols[startL + 1] = "http";
                    CustomProtocols[startL + 2] = "https";
                    CustomProtocols[startL + 3] = "about";
                    CustomProtocols[startL + 4] = "ftp";
                    CustomProtocols[startL + 5] = "smtp";
                    CustomProtocols[startL + 6] = "pop";
                }
                string CustomProtocolPattern = @"(";
                int i = 0; int C = CustomProtocols.Length - 1;
                while (i != C)
                {
                    CustomProtocolPattern += (i % 2 == 0 ? "|" : "") + CustomProtocols[i];
                    i++;
                }
                string Pattern = @"^((" + CustomProtocolPattern + @"):(\/\/)?)|(^([\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:\/?#[\]@!\$&'\(\)\*\+,;=.]+$))|(.{1,4}\:.{1,4}\:.{1,4}\:.{1,4}\:.{1,4}\:.{1,4}\:.{1,4}\:.{1,4})|(.[1,4]\:.[1,4]\:.[1,4]\:.[1,4]\:.[1,4]\:.[1,4]\:.[1,4]\:.[1,4])";
                Regex Rgx = new Regex(Pattern, options);
                return Rgx.IsMatch(Url);
            }
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="Url">Address to determine.</param>
        /// <returns><c>true</c> if <paramref name="Url"/> is a valid address within default protocol rules, otherwise <c>false</c>.</returns>
        public static bool ValidUrl(string Url)
        {
            string[] defaults = { "http", "https", "about", "ftp", "smtp", "pop" };
            return ValidUrl(Url, defaults, false);
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="Url">Address to determine.</param>
        /// <param name="ignoreDefaults">Ignores default protocols if <c>true</c></param>
        /// <returns><c>true</c> if <paramref name="Url"/> is a valid address within default rules, otherwise <c>false</c>.</returns>
        public static bool ValidUrl(string Url, bool ignoreDefaults)
        {
            string[] empty = { };
            return ValidUrl(Url, empty, ignoreDefaults);
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="Url">Address to determine.</param>
        /// <param name="CustomProtocols">Protocols (like <c>http</c>) to detect </param>
        /// <param name="ignoreDefaults">Ignores default protocols if <c>true</c></param>
        /// <returns><c>true</c> if <paramref name="Url"/> is a valid address within <paramref name="CustomProtocols"/> rules, otherwise <c>false</c>.</returns>
        public static bool ValidUrl(string url, string[] CustomProtocols, bool ignoreDefaults)
        {
            return ValidUrl(url, CustomProtocols, RegexOptions.Compiled | RegexOptions.IgnoreCase, ignoreDefaults);
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="Url">Address to determine.</param>
        /// <param name="CustomProtocols">Protocols (like <c>http</c>) to detect </param>
        /// <param name="options">Regex options to check </param>
        /// <returns><c>true</c> if <paramref name="Url"/> is a valid address within <paramref name="CustomProtocols"/> rules, otherwise <c>false</c>.</returns>
        public static bool ValidUrl(string url, string[] CustomProtocols)
        {
            return ValidUrl(url, CustomProtocols, RegexOptions.Compiled | RegexOptions.IgnoreCase, false);
        }

        /// <summary>
        /// Converts Base 64 code to an image.
        /// </summary>
        /// <param name="base64String">Code to convert.</param>
        /// <returns>Image representing the base 64 code.</returns>
        public static System.Drawing.Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        /// <summary>
        /// Crops an image to a rectangle.
        /// </summary>
        /// <param name="img">Image to work on.</param>
        /// <param name="cropArea">Focus area to crop to.</param>
        /// <returns>Cropped image.</returns>
        public static Image CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        /// <summary>
        /// Crops a bitmap to a rectangle.
        /// </summary>
        /// <param name="bm">Bitmap to work on.</param>
        /// <param name="cropArea">Focus area to crop to.</param>
        /// <returns>Cropped bitmap.</returns>
        public static Bitmap CropBitmap(Bitmap bm, Rectangle cropArea)
        {
            return bm.Clone(cropArea, bm.PixelFormat);
        }

        /// <summary>
        /// Generates a random text with random characters with length.
        /// </summary>
        /// <param name="length">Length of random text./param>
        /// <returns>Random characters in a string.</returns>
        public static string GenerateRandomText(int length = 17)
        {
            if (length == 0) { throw new ArgumentOutOfRangeException("\"length\" must be greater than 0."); }
            if (length < 0) { length = length * -1; }
            if (length >= int.MaxValue) { throw new ArgumentOutOfRangeException("\"length\" must be smaller than the 32-bit integer limit."); }
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, length - 1).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(length)
                .ToList().ForEach(e => builder.Append(e));
            return builder.ToString();
        }

        /// <summary>
        /// Gets the base URL of an URL.
        /// </summary>
        /// <param name="url">Address for getting the base address.</param>
        /// <returns>Base URL.</returns>
        public static string GetBaseURL(string url)
        {
            Uri uri = new Uri(url);
            string baseUri = uri.GetLeftPart(System.UriPartial.Authority);
            return baseUri;
        }

        /// <summary>
        /// Generates a random color.
        /// </summary>
        /// <param name="Transparency">Value of random generated color's alpha channel. This parameter is ignored if <paramref name="RandomTransparency"/> is set to true.</param>
        /// <param name="RandomTransparency">True to randomize Alpha channel, otherwise use <paramref name="Transparency"/>.</param>
        /// <returns>Random color.</returns>
        public static Color RandomColor(int Transparency = 255, bool RandomTransparency = false)
        {
            Random rand = new Random();
            int max = 256;
            int a = Transparency;
            if (RandomTransparency)
            {
                a = rand.Next(max);
            }
            int r = rand.Next(max);
            int g = rand.Next(max);
            int b = rand.Next(max);
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Converts Hexadecimal to Color
        /// </summary>
        /// <param name="hexString">Hex Code of Color</param>
        /// <returns>Color representing the hex code.</returns>
        public static Color HexToColor(string hexString)
        {
            return ColorTranslator.FromHtml(hexString);
        }

        /// <summary>
        /// Converts Color to Hexadecimal
        /// </summary>
        /// <param name="color">Color to convert</param>
        /// <returns>String representing the hex code of color.</returns>
        public static string ColorToHex(Color color)
        {
            return ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb()));
        }

        /// <summary>
        /// Gets Image from Url
        /// </summary>
        /// <param name="url">Address of image.</param>
        /// <returns>Image located in the URL.</returns>
        public static Image GetImageFromUrl(string url)
        {
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                using (Stream stream = webClient.OpenRead(url))
                {
                    return Image.FromStream(stream);
                }
            }
        }

        /// <summary>
        /// Return <c>true</c> if path directory is empty.
        /// </summary>
        /// <param name="path">Directory path to check.</param>
        /// <returns><c>true</c> if the directory is empty, otherwise <c>false</c>.</returns>
        public static bool IsDirectoryEmpty(string path)
        {
            if (Directory.Exists(path))
            {
                if (Directory.GetDirectories(path).Length > 0) { return false; } else { return true; }
            }
            else { return true; }
        }

        /// <summary>
        /// Gets Brightness level between 0-255.
        /// </summary>
        /// <param name="c">Color for checking brightness.</param>
        /// <returns>Level of brightness between 0-255</returns>
        public static int Brightness(Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }

        /// <summary>
        /// Gets Transparency level between 0-255.
        /// </summary>
        /// <param name="c">Color for checking transparency.</param>
        /// <returns>Level of transparency between 0-255</returns>
        public static int Transparency(Color c)
        {
            return Convert.ToInt32(c.A);
        }

        /// <summary>
        /// Returns true if the color is not so opaque, owtherwise false.
        /// </summary>
        /// <param name="c">Color for checking transparency.</param>
        /// <returns>Returns true if the color is not so opaque, otherwise false.</returns>
        public static bool IsTransparencyHigh(Color c)
        {
            return Transparency(c) < 130;
        }

        /// <summary>
        /// Returns true if the color is opaque, otherwise false.
        /// </summary>
        /// <param name="c">Color for checking opacity.</param>
        /// <returns>Returns true if the color is opaque, otherwise false.</returns>
        public static bool IsOpaque(Color c)
        {
            return Transparency(c) == 255;
        }

        /// <summary>
        /// Returns true if the color is invisible due to high transparency.
        /// </summary>
        /// <param name="c"></param>
        /// <returns>Returns true if the color is invisible.</returns>
        public static bool IsInvisible(Color c)
        {
            return Transparency(c) == 0;
        }

        /// <summary>
        /// Replaces a color from an image to another color.
        /// </summary>
        /// <param name="inputImage">Image to work on.</param>
        /// <param name="tolerance">The area of ​​relationship with color equivalents.</param>
        /// <param name="oldColor">Color to change from.</param>
        /// <param name="NewColor">Color to change to.</param>
        /// <returns>Final result of the image after processing it.</returns>
        public static Bitmap ColorReplace(Bitmap inputImage, int tolerance, Color oldColor, Color NewColor)
        {
            Bitmap outputImage = new Bitmap(inputImage.Width, inputImage.Height);
            Graphics G = Graphics.FromImage(outputImage);
            G.DrawImage(inputImage, 0, 0);
            for (int y = 0; y < outputImage.Height; y++)
            {
                for (int x = 0; x < outputImage.Width; x++)
                {
                    Color PixelColor = outputImage.GetPixel(x, y);
                    if (PixelColor.R > oldColor.R - tolerance && PixelColor.R < oldColor.R + tolerance && PixelColor.G > oldColor.G - tolerance && PixelColor.G < oldColor.G + tolerance && PixelColor.B > oldColor.B - tolerance && PixelColor.B < oldColor.B + tolerance)
                    {
                        int RColorDiff = oldColor.R - PixelColor.R;
                        int GColorDiff = oldColor.G - PixelColor.G;
                        int BColorDiff = oldColor.B - PixelColor.B;

                        if (PixelColor.R > oldColor.R)
                        {
                            RColorDiff = NewColor.R + RColorDiff;
                        }
                        else
                        {
                            RColorDiff = NewColor.R - RColorDiff;
                        }

                        if (RColorDiff > 255)
                        {
                            RColorDiff = 255;
                        }

                        if (RColorDiff < 0)
                        {
                            RColorDiff = 0;
                        }

                        if (PixelColor.G > oldColor.G)
                        {
                            GColorDiff = NewColor.G + GColorDiff;
                        }
                        else
                        {
                            GColorDiff = NewColor.G - GColorDiff;
                        }

                        if (GColorDiff > 255)
                        {
                            GColorDiff = 255;
                        }

                        if (GColorDiff < 0)
                        {
                            GColorDiff = 0;
                        }

                        if (PixelColor.B > oldColor.B)
                        {
                            BColorDiff = NewColor.B + BColorDiff;
                        }
                        else
                        {
                            BColorDiff = NewColor.B - BColorDiff;
                        }

                        if (BColorDiff > 255)
                        {
                            BColorDiff = 255;
                        }

                        if (BColorDiff < 0)
                        {
                            BColorDiff = 0;
                        }

                        outputImage.SetPixel(x, y, Color.FromArgb(RColorDiff, GColorDiff, BColorDiff));
                    }
                }
            }

            return outputImage;
        }

        /// <summary>
        /// Replaces a color from an image to another color.
        /// </summary>
        /// <param name="inputImage">Image to work on.</param>
        /// <param name="tolerance">The area of ​​relationship with color equivalents.</param>
        /// <param name="oldColor">Color to change from.</param>
        /// <param name="NewColor">Color to change to.</param>
        /// <returns>Final result of the image after processing it.</returns>
        public static Image ColorReplace(Image inputImage, int tolerance, Color oldColor, Color NewColor)
        {
            Bitmap outputImage = new Bitmap(inputImage.Width, inputImage.Height);
            Graphics G = Graphics.FromImage(outputImage);
            G.DrawImage(inputImage, 0, 0);
            for (int y = 0; y < outputImage.Height; y++)
            {
                for (int x = 0; x < outputImage.Width; x++)
                {
                    Color PixelColor = outputImage.GetPixel(x, y);
                    if (PixelColor.R > oldColor.R - tolerance && PixelColor.R < oldColor.R + tolerance && PixelColor.G > oldColor.G - tolerance && PixelColor.G < oldColor.G + tolerance && PixelColor.B > oldColor.B - tolerance && PixelColor.B < oldColor.B + tolerance)
                    {
                        int RColorDiff = oldColor.R - PixelColor.R;
                        int GColorDiff = oldColor.G - PixelColor.G;
                        int BColorDiff = oldColor.B - PixelColor.B;

                        if (PixelColor.R > oldColor.R)
                        {
                            RColorDiff = NewColor.R + RColorDiff;
                        }
                        else
                        {
                            RColorDiff = NewColor.R - RColorDiff;
                        }

                        if (RColorDiff > 255)
                        {
                            RColorDiff = 255;
                        }

                        if (RColorDiff < 0)
                        {
                            RColorDiff = 0;
                        }

                        if (PixelColor.G > oldColor.G)
                        {
                            GColorDiff = NewColor.G + GColorDiff;
                        }
                        else
                        {
                            GColorDiff = NewColor.G - GColorDiff;
                        }

                        if (GColorDiff > 255)
                        {
                            GColorDiff = 255;
                        }

                        if (GColorDiff < 0)
                        {
                            GColorDiff = 0;
                        }

                        if (PixelColor.B > oldColor.B)
                        {
                            BColorDiff = NewColor.B + BColorDiff;
                        }
                        else
                        {
                            BColorDiff = NewColor.B - BColorDiff;
                        }

                        if (BColorDiff > 255)
                        {
                            BColorDiff = 255;
                        }

                        if (BColorDiff < 0)
                        {
                            BColorDiff = 0;
                        }

                        outputImage.SetPixel(x, y, Color.FromArgb(RColorDiff, GColorDiff, BColorDiff));
                    }
                }
            }

            return outputImage;
        }

        /// <summary>
        /// Applies a texture to an Image.
        /// </summary>
        /// <param name="input">Image to work on.</param>
        /// <param name="texture">Texture to apply.</param>
        /// <param name="repeatable"><c>true</c> to repeat texture like a tile. <c>false</c> to resize texture to fit to image.</param>
        /// <returns>Final result of the image after processing it.</returns>
        public static Image RepaintImage(Image input, Image texture, bool repeatable)
        {
            Bitmap inputImage = new Bitmap(input);
            Bitmap outputImage = new Bitmap(input.Width, input.Height);
            Bitmap textureImage = repeatable ? new Bitmap(texture) : new Bitmap(original: texture, newSize: input.Size);
            for (int y = 0; y < outputImage.Height; y++)
            {
                for (int x = 0; x < outputImage.Width; x++)
                {
                    Color PixelColor = textureImage.GetPixel(repeatable ? (x % textureImage.Width) : x, repeatable ? (y % textureImage.Height) : y);
                    Color PixelColor2 = inputImage.GetPixel(x, y);
                    if (PixelColor2.A < PixelColor.A)
                    {
                        outputImage.SetPixel(x, y, Color.FromArgb(PixelColor2.A, PixelColor.R, PixelColor.G, PixelColor.B));
                    }
                    else
                    {
                        outputImage.SetPixel(x, y, Color.FromArgb(PixelColor.A, PixelColor.R, PixelColor.G, PixelColor.B));
                    }
                }
            }
            return outputImage;
        }

        /// <summary>
        /// Determines which color (Black or White) to use for foreground of the color.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <returns>Returns Black if color is bright, otherwise White.</returns>
        public static Color AutoWhiteBlack(Color c)
        {
            return IsBright(c) ? Color.Black : Color.White;
        }

        /// <summary>
        /// Determies which color (Black or White) is closer to the color.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <returns>Returns White if color is bright, otherwise Black.</returns>
        public static Color WhiteOrBlack(Color c)
        {
            return IsBright(c) ? Color.White : Color.Black;
        }

        /// <summary>
        /// Returns <c>true</c> if the color is bright.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <returns><c>true</c> if color is bright, otherwise <c>false</c></returns>
        public static bool IsBright(Color c)
        {
            return Brightness(c) > 130;
        }

        /// <summary>
        /// Subtracts the number if possible.
        /// </summary>
        /// <param name="number">Integer to work on.</param>
        /// <param name="subtract">Integer to subtract.</param>
        /// <param name="limit">Integer for limit.</param>
        /// <returns>Subtracts the number if subtract is smaller than the number, otherwise returns the number untouched.</returns>
        public static int SubtractIfNeeded(int number, int subtract, int limit = 0)
        {
            return limit == 0 ? (number > subtract ? number - subtract : number) : (number - subtract < limit ? number : number - subtract);
        }

        /// <summary>
        /// Add the number if the result is going to be smaller or equal to limit.
        /// </summary>
        /// <param name="number">Integer to work on.</param>
        /// <param name="add">Integer to add.</param>
        /// <param name="limit">Integer for limit.</param>
        /// <returns>Adds the number if added number is smaller than the limit, otherwise returns the number untouched.</returns>
        public static int AddIfNeeded(int number, int add, int limit = ((2 ^ 31) - 1))
        {
            return number + add > limit ? number : number + add;
        }

        /// <summary>
        /// Reverses a color.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <param name="reverseAlpha"><c>true</c> to also reverse Alpha (Transparency) channel.</param>
        /// <returns>Opposite of the color.</returns>
        public static Color ReverseColor(Color c, bool reverseAlpha)
        {
            return Color.FromArgb(reverseAlpha ? (255 - c.A) : c.A,
                                  255 - c.R,
                                  255 - c.G,
                                  255 - c.B);
        }

        /// <summary>
        /// Shifts brightness of a color.
        /// </summary>
        /// <param name="baseColor">Color to work on.</param>
        /// <param name="value">Shift integer.</param>
        /// <param name="shiftAlpha"><c>true</c> to also shift Alpha (Transparency) channel.</param>
        /// <returns>Color with shifted brightness.</returns>
        public static Color ShiftBrightness(Color baseColor, int value, bool shiftAlpha)
        {
            return Color.FromArgb(shiftAlpha ? (IsTransparencyHigh(baseColor) ? AddIfNeeded(baseColor.A, value, 255) : SubtractIfNeeded(baseColor.A, value)) : baseColor.A,
                                  IsBright(baseColor) ? SubtractIfNeeded(baseColor.R, value) : AddIfNeeded(baseColor.R, value, 255),
                                  IsBright(baseColor) ? SubtractIfNeeded(baseColor.G, value) : AddIfNeeded(baseColor.G, value, 255),
                                  IsBright(baseColor) ? SubtractIfNeeded(baseColor.B, value) : AddIfNeeded(baseColor.B, value, 255));
        }

        /// <summary>
        /// Shifts brightness of a color to <paramref name="value"/>.
        /// </summary>
        /// <param name="baseColor">Color to work on.</param>
        /// <param name="value">New brightness value.</param>
        /// <param name="shiftAlpha"><c>true</c> to also shift Alpha (Transparency) channel.</param>
        /// <returns>Color with shifted brightness.</returns>
        public static Color ShiftBrightnessTo(Color baseColor, int value, bool shiftAlpha)
        {
            if (value == Brightness(baseColor)) { return baseColor; }
            else if (value > Brightness(baseColor))
            {
                return ShiftBrightness(baseColor, (value - Brightness(baseColor)), shiftAlpha);
            }
            else
            {
                return ShiftBrightness(baseColor, (Brightness(baseColor) - value), shiftAlpha);
            }
        }

        /// <summary>
        /// Reads a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="encode">Rules for reading the file.</param>
        /// <returns>Text inside the file.</returns>
        public static string ReadFile(string fileLocation, Encoding encode)
        {
            FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, encode);
            string result = sr.ReadToEnd();
            sr.Close();
            return result;
        }

        /// <summary>
        /// Reads a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <returns>File stream containing file information.</returns>
        public static Stream ReadFile(string fileLocation)
        {
            FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return fs;
        }

        /// <summary>
        /// Reads a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="format">Not used but required due to other overflows.</param>
        /// <returns>Image from location.</returns>
        public static Image ReadFile(string fileLocation, ImageFormat format)
        {
            if (format != null)
            {
                format = null;
            }
            Image img = Image.FromStream(ReadFile(fileLocation));
            return img;
        }

        /// <summary>
        /// Reads a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="ignored">Not used but required due to other overflows.</param>
        /// <returns>Bitmap from location.</returns>
        public static Bitmap ReadFile(string fileLocation, string ignored)
        {
            if (!string.IsNullOrWhiteSpace(ignored))
            {
                ignored = "";
            }
            return new Bitmap(ReadFile(fileLocation, format: null));
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="input">Text to write on.</param>
        /// <param name="encode">Rules to follow while writing.</param>
        /// <returns><c>true</c> if successfully writes to file, otherwise throws an exception.</returns>
        public static bool WriteFile(string fileLocation, string input, Encoding encode)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
            File.Create(fileLocation).Dispose();
            FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            writer.Write(encode.GetBytes(input), 0, encode.GetBytes(input).Length);
            writer.Close();
            return true;
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="bitmap">Bitmap to write on.</param>
        /// <param name="format">Format to use while writing.</param>
        /// <returns><c>true</c> if successfully writes to file, otherwise throws an exception.</returns>
        public static bool WriteFile(string fileLocation, Bitmap bitmap, ImageFormat format)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
            File.Create(fileLocation).Dispose();
            File.Create(fileLocation).Dispose();
            FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, format);
            //memoryStream.CopyTo(writer);
            writer.Write(memoryStream.ToArray(), 0, Convert.ToInt32(memoryStream.Length));
            memoryStream.Close();
            writer.Close();
            return true;
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="image">Image to write on.</param>
        /// <param name="format">Format to use while writing.</param>
        /// <returns><c>true</c> if successfully writes to file, otherwise throws an exception.</returns>
        public static bool WriteFile(string fileLocation, Image image, ImageFormat format)
        {
            Bitmap bitmap = new Bitmap(image);
            return WriteFile(fileLocation, bitmap, format);
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="input">Bytes to write on.</param>
        /// <returns><c>true</c> if successfully writes to file, otherwise throws an exception.</returns>
        public static bool WriteFile(string fileLocation, byte[] input)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
            File.Create(fileLocation).Dispose();
            FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            writer.Write(input, 0, input.Length);
            writer.Close();
            return true;
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="stream">Stream to write on.</param>
        /// <returns><c>true</c> if successfully writes to file, otherwise throws an exception.</returns>
        public static bool WriteFile(string fileLocation, Stream stream)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
            File.Create(fileLocation).Dispose();
            FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            stream.CopyTo(writer);
            writer.Close();
            return true;
        }
    }
}