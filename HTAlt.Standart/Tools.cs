/*

Copyright © 2018 - 2021 haltroy

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

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
    public static class Tools
    {
        #region Image

        /// <summary>
        /// Converts <paramref name="img"/> to an <see cref="Icon"/>
        /// Thanks to Hans Passant from StackOverflow.
        /// https://stackoverflow.com/a/21389253
        /// </summary>
        /// <param name="img">Convertion <see cref="Image"/></param>
        /// <returns><seealso cref="Icon"/></returns>
        public static System.Drawing.Icon IconFromImage(System.Drawing.Image img)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);
            // Header
            bw.Write((short)0);   // 0 : reserved
            bw.Write((short)1);   // 2 : 1=ico, 2=cur
            bw.Write((short)1);   // 4 : number of images
                                  // Image directory
            int w = img.Width;
            if (w >= 256)
            {
                w = 0;
            }

            bw.Write((byte)w);    // 0 : width of image
            int h = img.Height;
            if (h >= 256)
            {
                h = 0;
            }

            bw.Write((byte)h);    // 1 : height of image
            bw.Write((byte)0);    // 2 : number of colors in palette
            bw.Write((byte)0);    // 3 : reserved
            bw.Write((short)0);   // 4 : number of color planes
            bw.Write((short)0);   // 6 : bits per pixel
            long sizeHere = ms.Position;
            bw.Write(0);     // 8 : image size
            int start = (int)ms.Position + 4;
            bw.Write(start);      // 12: offset of image data
                                  // Image data
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            int imageSize = (int)ms.Position - start;
            ms.Seek(sizeHere, System.IO.SeekOrigin.Begin);
            bw.Write(imageSize);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            // And load it
            return new System.Drawing.Icon(ms);
        }

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
        /// <param name="repeatable"><sse cref=true"/> to repeat texture like a tile. <sse cref=false"/> to resize texture to fit to image.</param>
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

        #endregion Image

        #region Internet, XML & Strings

        /// <summary>
        /// Finds the root node of <paramref name="doc"/>.
        /// </summary>
        /// <param name="doc">the <see cref="XmlNode"/> (probably <seealso cref="XmlDocument.DocumentElement"/>) to search on.</param>
        /// <returns>a <see cref="System.Xml.XmlNode"/> which represents as the root node.</returns>
        public static System.Xml.XmlNode FindRoot(System.Xml.XmlNode doc)
        {
            System.Xml.XmlNode found = null;
            if (ToLowerEnglish(doc.Name) == "root")
            {
                found = doc;
            }
            else
            {
                for (int i = 0; i < doc.ChildNodes.Count; i++)
                {
                    System.Xml.XmlNode node = doc.ChildNodes[i];
                    if (ToLowerEnglish(node.Name) == "root")
                    {
                        found = node;
                    }
                }
            }
            return found;
        }

        /// <summary>
        /// Tells if the <paramref name="node"/> is a comment node.
        /// </summary>
        /// <param name="node"><see cref="XmlNode"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool NodeIsComment(System.Xml.XmlNode node)
        {
            return node.OuterXml.StartsWith("<!--");
        }

        /// <summary>
        /// Turns all characters to lowercase, using en-US culture information to avoid language-specific ToLower() errors such as:
        /// <para>Turkish: I &lt;-&gt; ı , İ &lt;-&gt; i</para>
        /// <para>English I &lt;-&gt; i</para>
        /// </summary>
        /// <param name="s"><see cref="string"/></param>
        /// <returns><see cref="string"/></returns>
        public static string ToLowerEnglish(string s)
        {
            return s.ToLower(new System.Globalization.CultureInfo("en-US", false));
        }

        /// <summary>
        /// Finds the root node of <paramref name="doc"/>.
        /// </summary>
        /// <param name="doc">The XML document.</param>
        /// <returns>a <see cref="System.Xml.XmlNode"/> which represents as the root node.</returns>
        public static System.Xml.XmlNode FindRoot(System.Xml.XmlDocument doc)
        {
            return FindRoot(doc.DocumentElement);
        }

        /// <summary>
        /// Converts XML-formatted string to <see cref="string"/>.
        /// </summary>
        /// <param name="innerxml">XML-formatted string</param>
        /// <returns>Formatted <paramref name="s"/>.</returns>
        public static string XmlToString(string innerxml)
        {
            return innerxml.Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">");
        }

        /// <summary>
        /// Converts <paramref name="s"/> to <see cref="System.Xml"/> supported format.
        /// </summary>
        /// <param name="s"><see cref="string"/></param>
        /// <returns>Formatted <paramref name="s"/>.</returns>
        public static string ToXML(string s)
        {
            return s.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        /// <summary>
        /// Creates an Internet shortcut (Windows).
        /// </summary>
        /// <param name="Url">Location on Internet.</param>
        /// <param name="FileLocation">Location of Internet address.</param>
        public static void CreateInternetShortcut(string Url, string FileLocation)
        {
            if (!FileLocation.ToLower().EndsWith("url")) { FileLocation += ".url"; }
            WriteFile(FileLocation, "[InternetSHortcut]" + Environment.NewLine + "Url=" + Url, Encoding.UTF8);
        }

        /// <summary>
        /// Determines if a site is an actually from Haltroy.
        /// </summary>
        /// <param name="SiteUrl">Site URL</param>
        /// <returns><sse cref=true"/> if <paramref name="SiteUrl"/> is actually a Haltroy website, otherwise <sse cref=false"/>.</returns>
        public static bool ValidHaltroyWebsite(string SiteUrl)
        {
            string Pattern = @"((?:http(s)?\:\/\/)?(.*\.)?haltroy\.com)";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return ValidUrl(SiteUrl) && Rgx.IsMatch(SiteUrl.Substring(0, SiteUrl.ToLower().StartsWith("http") ? SiteUrl.IndexOf(@"\", 10) : SiteUrl.IndexOf(@"\")));
        }

        /// <summary>
        /// Prettifies XML code.
        /// Thanks to S M Kamran & Bakudan from StackOverflow
        /// https://stackoverflow.com/a/1123731
        /// </summary>
        /// <param name="xml">XML code</param>
        /// <returns>Prettified <paramref name="xml"/></returns>
        public static string BeautifyXML(string xml)
        {
            string result = "";

            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(mStream, System.Text.Encoding.Unicode);
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();

            // Load the XmlDocument with the XML.
            document.LoadXml(xml);

            writer.Formatting = System.Xml.Formatting.Indented;

            // Write the XML into a formatting XmlTextWriter
            document.WriteContentTo(writer);
            writer.Flush();
            mStream.Flush();

            // Have to rewind the MemoryStream in order to read
            // its contents.
            mStream.Position = 0;

            // Read MemoryStream contents into a StreamReader.
            System.IO.StreamReader sReader = new System.IO.StreamReader(mStream);

            // Extract the text from the StreamReader.
            string formattedXml = sReader.ReadToEnd();

            result = formattedXml;

            mStream.Close();
            writer.Close();

            return result;
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="Url">Address to determine.</param>
        /// <param name="CustomProtocols">Protocols (like <c>http</c>) to detect </param>
        /// <param name="options">Regex options to check </param>
        /// <param name="ignoreDefaults">Ignores default protocols if <sse cref=true"/></param>
        /// <returns><sse cref=true"/> if <paramref name="Url"/> is a valid address within <paramref name="CustomProtocols"/> rules, otherwise <sse cref=false"/>.</returns>
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
                string CustomProtocolPattern = string.Join("|", CustomProtocols);
                string Pattern = @"^((" + CustomProtocolPattern + @"):(\/\/)?)|(^([\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:\/?#[\]@!\$&'\(\)\*\+,;=.]+$))|(\d{1,4}(\:\d{1,4}){3,7})";
                Regex Rgx = new Regex(Pattern, options);
                return Rgx.IsMatch(Url);
            }
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="Url">Address to determine.</param>
        /// <returns><sse cref=true"/> if <paramref name="Url"/> is a valid address within default protocol rules, otherwise <sse cref=false"/>.</returns>
        public static bool ValidUrl(string Url)
        {
            string[] defaults = { "http", "https", "about", "ftp", "smtp", "pop" };
            return ValidUrl(Url, defaults, false);
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="Url">Address to determine.</param>
        /// <param name="ignoreDefaults">Ignores default protocols if <sse cref=true"/></param>
        /// <returns><sse cref=true"/> if <paramref name="Url"/> is a valid address within default rules, otherwise <sse cref=false"/>.</returns>
        public static bool ValidUrl(string Url, bool ignoreDefaults)
        {
            string[] empty = { };
            return ValidUrl(Url, empty, ignoreDefaults);
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="url">Address to determine.</param>
        /// <param name="CustomProtocols">Protocols (like <c>http</c>) to detect </param>
        /// <param name="ignoreDefaults">Ignores default protocols if <sse cref=true"/></param>
        /// <returns><sse cref=true"/> if <paramref name="Url"/> is a valid address within <paramref name="CustomProtocols"/> rules, otherwise <sse cref=false"/>.</returns>
        public static bool ValidUrl(string url, string[] CustomProtocols, bool ignoreDefaults)
        {
            return ValidUrl(url, CustomProtocols, RegexOptions.Compiled | RegexOptions.IgnoreCase, ignoreDefaults);
        }

        /// <summary>
        /// Determines if a string is an valid address to somewhere on the Internet.
        /// </summary>
        /// <param name="url">Address to determine.</param>
        /// <param name="CustomProtocols">Protocols (like <c>http</c>) to detect </param>
        /// <param name="CustomProtocols">Regex options to check </param>
        /// <returns><sse cref=true"/> if <paramref name="Url"/> is a valid address within <paramref name="CustomProtocols"/> rules, otherwise <sse cref=false"/>.</returns>
        public static bool ValidUrl(string url, string[] CustomProtocols)
        {
            return ValidUrl(url, CustomProtocols, RegexOptions.Compiled | RegexOptions.IgnoreCase, false);
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

        #endregion Internet, XML & Strings

        #region Math

        /// <summary>
        /// Trims all non-numeric chars (except ",","." and "-")
        /// </summary>
        /// <param name="input">String</param>
        /// <returns><see cref="string"/></returns>
        public static string TrimToNumbers(this string input)
        {
            return new string(input.Where(c => char.IsDigit(c) || c == ',' || c == '.' || c == '-').ToArray());
        }

        /// <summary>
        /// Subtracts the number if possible.
        /// </summary>
        /// <param name="number">Integer to work on.</param>
        /// <param name="subtract">Integer to subtract.</param>
        /// <param name="limit">Integer for limit. Default is 0.</param>
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
        /// <param name="limit">Integer for limit. Default is the 32-bit integer limit.</param>
        /// <returns>Adds the number if added number is smaller than the limit, otherwise returns the number untouched.</returns>
        public static int AddIfNeeded(int number, int add, int limit = int.MaxValue)
        {
            return number + add > limit ? number : number + add;
        }

        /// <summary>
        /// Finds the prime multiplier numbers of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"><see cref="int"/> to search in.</param>
        /// <returns>An array of <see cref="int"/> containing prime multiplier <see cref="int"/> list.</returns>
        public static int[] PrimeMultipliers(int n)
        {
            // Teşekkürler : https://www.pinareser.com/c-ile-girilen-sayinin-asal-carpanlara-ayrilmasi/
            int[] asallar = new int[] { };
            int asal = 2;
            int kontrol = 0;
            while (n > 1)
            {
                if (n % asal == 0)
                {
                    if (asal != kontrol)
                    {
                        kontrol = asal;
                        Array.Resize<int>(ref asallar, asallar.Length + 1);
                        asallar[asallar.Length - 1] = asal;
                        n = n / asal;
                    }
                    else
                    {
                        n = n / asal;
                    }
                }
                else
                {
                    asal++;
                }
            }
            return asallar;
        }

        /// <summary>
        /// Gets a <paramref name="n2"/> dividable <see cref="int"/> from <paramref name="n1"/>.
        /// </summary>
        /// <param name="n1"><see cref="int"/> representative of a main number</param>
        /// <param name="n2"><see cref="int"/> representative of a divider number</param>
        /// <param name="balancePoint">Adds remainder of divison if <paramref name="n1"/> is smaller than this paramater. Otherwise, subtracts from <paramref name="n1"/>.</param>
        /// <returns><paramref name="n2"/> dividable close number of <paramref name="n1"/>.</returns>
        public static int MakeItDividable(int n1, int n2, int balancePoint = 300)
        {
            int kalan = n1 % n2;
            return n1 < balancePoint ? n1 + (n2 - kalan) : n1 - kalan;
        }

        /// <summary>
        /// Divides <paramref name="n1"/> to <paramref name="divider"/> without remainder.
        /// </summary>
        /// <param name="n1">Main number.</param>
        /// <param name="divider">Divider number.</param>
        /// <returns>Result of devision.</returns>
        public static int Divide(int n1, int divider)
        {
            int dividable = MakeItDividable(n1, divider);
            return dividable / divider;
        }

        /// <summary>
        /// Largest Common Division between <paramref name="n1"/> and <paramref name="n2"/>.
        /// </summary>
        /// <param name="n1"><see cref="int"/> representative of a number</param>
        /// <param name="n2"><see cref="int"/> representative of a number</param>
        /// <returns><see cref="int"/> representative of Largest Common Division between <paramref name="n1"/> and <paramref name="n2"/>.</returns>
        public static int LargestCommonDivision(int n1, int n2)
        {
            // Teşekkürler: https://www.bilisimogretmeni.com/visual-studio-c/c-dersleri-obeb-okek-bulma-hesaplama.html
            while (n1 != n2)
            {
                if (n1 > n2)
                {
                    n1 = n1 - n2;
                }

                if (n2 > n1)
                {
                    n2 = n2 - n1;
                }
            }
            return n1;
        }

        /// <summary>
        /// Smallest of Common Denominator between <paramref name="n1"/> and <paramref name="n2"/>.
        /// </summary>
        /// <param name="n1"><see cref="int"/> representative of a number</param>
        /// <param name="n2"><see cref="int"/> representative of a number</param>
        /// <returns><see cref="int"/> representative of Smallest of Common Denominator between <paramref name="n1"/> and <paramref name="n2"/>.</returns>
        public static int SmallestOfCommonDenominator(int n1, int n2)
        {
            // Teşekkürler: https://www.bilisimogretmeni.com/visual-studio-c/c-dersleri-obeb-okek-bulma-hesaplama.html
            return (n1 * n2) / n1.LargestCommonDivision(n2);
        }

        /// <summary>
        /// Biggest prime multiplier number of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"><see cref="int"/> to search in.</param>
        /// <returns><see cref="int"/> as the biggest prime multiplier of <paramref name="n"/>.</returns>
        public static int BiggestPrimeMultiplier(int n)
        {
            int[] asallar = n.PrimeMultipliers();
            return asallar[asallar.Length - 1];
        }

        /// <summary>
        /// Smallest prime multiplier number of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"><see cref="int"/> to search in.</param>
        /// <returns><see cref="int"/> as the smallest prime multiplier of <paramref name="n"/>.</returns>
        public static int SmallestPrimeMultiplier(int n)
        {
            int[] asallar = n.PrimeMultipliers();
            return asallar[0];
        }

        #endregion Math

        #region Color

        /// <summary>
        /// Returns either <paramref name="black"/> or <paramref name="white"/> by determining with the brightess of <paramref name="color"/>.
        /// </summary>
        /// <param name="color">Color for determining.</param>
        /// <param name="white">White/Bright image to return.</param>
        /// <param name="black">Black/Dark image  to return</param>
        /// <param name="reverse"><sse cref=true"/> to return <paramref name="black"/> on black/dark images and <paramref name="white"/> for white/bright images, otherwise <sse cref=false"/>.</param>
        /// <returns><paramref name="black"/> or <paramref name="white"/>.</returns>
        public static Image SelectImageFromColor(Color color, ref Image white, ref Image black, bool reverse = false)
        {
            return SelectImageFromColor(color, ref white, ref black, reverse);
        }

        /// <summary>
        /// Returns either <paramref name="black"/> or <paramref name="white"/> by determining with the brightess of <paramref name="color"/>.
        /// </summary>
        /// <param name="color">Color for determining.</param>
        /// <param name="white">White/Bright image to return.</param>
        /// <param name="black">Black/Dark image  to return</param>
        /// <param name="reverse"><sse cref=true"/> to return <paramref name="black"/> on black/dark images and <paramref name="white"/> for white/bright images, otherwise <sse cref=false"/>.</param>
        /// <returns><paramref name="black"/> or <paramref name="white"/>.</returns>
        public static Bitmap SelectBitmapFromColor(Color color, ref Bitmap white, ref Bitmap black, bool reverse = false)
        {
            return IsBright(color) ? (reverse ? white : black) : (reverse ? black : white);
        }

        /// <summary>
        /// Generates a random color.
        /// </summary>
        /// <param name="Transparency">Value of random generated color's alpha channel. This parameter is ignored if <paramref name="RandomTransparency"/> is set to true.</param>
        /// <param name="RandomTransparency">True to randomize Alpha channel, otherwise use <paramref name="Transparency"/>.</param>
        /// <param name="Seed">Seed of random generator. Default is 172703.</param>
        /// <returns>Random color.</returns>
        public static Color RandomColor(int Transparency = 255, bool RandomTransparency = false, int Seed = 172703)
        {
            Random rand = new Random(Seed);
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
        /// Returns <sse cref=true"/> if the color is bright.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <returns><sse cref=true"/> if color is bright, otherwise <sse cref=false"/></returns>
        public static bool IsBright(Color c)
        {
            return Brightness(c) > 130;
        }

        /// <summary>
        /// Reverses a color.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <param name="reverseAlpha"><sse cref=true"/> to also reverse Alpha (Transparency) channel.</param>
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
        /// <param name="shiftAlpha"><sse cref=true"/> to also shift Alpha (Transparency) channel.</param>
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
        /// <param name="shiftAlpha"><sse cref=true"/> to also shift Alpha (Transparency) channel.</param>
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

        #endregion Color

        #region Files & Directories

        /// <summary>
        /// Converts a byte array to <see cref="string"/>.
        /// </summary>
        /// <param name="bytes"><see cref="byte"/> <seealso cref="Array"/>.</param>
        /// <returns><see cref="string"/></returns>
        public static string BytesToString(byte[] bytes)
        {
            string result = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                result += bytes[i].ToString("x2");
            }
            return result;
        }

        /// <summary>
        /// Verifies a file with <see cref="System.Security.Cryptography.MD5"/> and <seealso cref="System.Security.Cryptography.SHA256"/> methods.
        /// </summary>
        /// <param name="algorithm"><see cref="System.Security.Cryptography.HashAlgorithm"/></param>
        /// <param name="file">File location.</param>
        /// <param name="hash">File's supposedly hash.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool VerifyFile(System.Security.Cryptography.HashAlgorithm algorithm, string file, string hash)
        {
            return string.Equals(GetHash(algorithm, file), hash);
        }

        /// <summary>
        /// Verifies a file with <see cref="System.Security.Cryptography.MD5"/> and <seealso cref="System.Security.Cryptography.SHA256"/> methods.
        /// </summary>
        /// <param name="algorithm"><see cref="System.Security.Cryptography.HashAlgorithm"/></param>
        /// <param name="file">File location.</param>
        /// <param name="hash">File's supposedly hash.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool VerifyFile(System.Security.Cryptography.HashAlgorithm algorithm, string file, byte[] hash)
        {
            return System.Linq.Enumerable.SequenceEqual(GetHash(algorithm, file), hash);
        }

        /// <summary>
        /// Gets <see cref="System.Security.Cryptography.SHA256"/> of <paramref name="file"/>.
        /// </summary>
        /// <param name="algorithm"><see cref="System.Security.Cryptography.HashAlgorithm"/></param>
        /// <param name="file">File location.</param>
        /// <param name="ignored">Ignored value.</param>
        /// <returns><see cref="string"/></returns>
        public static string GetHash(System.Security.Cryptography.HashAlgorithm algorithm, string file, bool ignored = false)
        {
            return BytesToString(GetHash(algorithm, file));
        }

        /// <summary>
        /// Gets the file hash of <paramref name="file"/> with <paramref name="algorithm"/>.
        /// </summary>
        /// <param name="algorithm"><see cref="System.Security.Cryptography.HashAlgorithm"/></param>
        /// <param name="file"><see cref="string"/></param>
        /// <returns><see cref="byte"/> <seealso cref="Array"/>.</returns>
        public static byte[] GetHash(System.Security.Cryptography.HashAlgorithm algorithm, string file)
        {
            return algorithm.ComputeHash(ReadFile(file));
        }

        /// <summary>
        /// Return <sse cref=true"/> if path directory is empty.
        /// </summary>
        /// <param name="path">Directory path to check.</param>
        /// <returns><sse cref=true"/> if the directory is empty, otherwise <sse cref=false"/>.</returns>
        public static bool IsDirectoryEmpty(string path)
        {
            if (Directory.Exists(path))
            {
                if (Directory.GetDirectories(path).Length > 0) { return false; } else { return true; }
            }
            else { return true; }
        }

        /// <summary>
        /// Gets directory size.
        /// Thanks to hao & Alexandre Pepin from StackOverflow
        /// https://stackoverflow.com/a/468131
        /// </summary>
        /// <param name="d">Directory</param>
        /// <returns><see cref="long"/></returns>
        public static long DirectorySize(System.IO.DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            System.IO.FileInfo[] fis = d.GetFiles();
            foreach (System.IO.FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            System.IO.DirectoryInfo[] dis = d.GetDirectories();
            foreach (System.IO.DirectoryInfo di in dis)
            {
                size += DirectorySize(di);
            }
            return size;
        }

        /// <summary>
        /// Detects if user can access <paramref name="dir"/> by try{} method.
        /// </summary>
        /// <param name="dir">Directory</param>
        /// <returns><see cref="true"/> if can access to folder, <seealso cref="false"/> if user has no access to <paramref name="dir"/> and throws <see cref="Exception"/> on other scenarios.</returns>
        public static bool HasWriteAccess(string dir)
        {
            try
            {
                string random = HTAlt.Tools.GenerateRandomText(17);
                HTAlt.Tools.WriteFile(dir + "\\HTALT.TEST", random, System.Text.Encoding.Unicode);
                string file = HTAlt.Tools.ReadFile(dir + "\\HTALT.TEST", System.Text.Encoding.Unicode);
                System.IO.File.Delete(dir + "\\HTALT.TEST");
                if (file == random)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Test file \"" + dir + "\\HTALT.TEST" + "\" was altered.");
                }
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Removes <paramref name="RemoveText"/> from files names in <paramref name="folder"/>.
        /// </summary>
        /// <param name="folder">Work folder</param>
        /// <param name="RemoveText">Text to remove</param>
        /// <returns><sse cref=true"/> if successfully removes <paramref name="RemoveText"/>, otherwise <sse cref=false"/>.</returns>
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

        #region Read File

        /// <summary>
        /// Reads a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="encode">Rules for reading the file.</param>
        /// <returns>Text inside the file.</returns>
        public static string ReadFile(string fileLocation, Encoding encode)
        {
            StreamReader sr = new StreamReader(ReadFile(fileLocation), encode);
            string result = sr.ReadToEnd();
            sr.Close();
            return result;
        }

        /// <summary>
        /// Reads a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="ignored">Rules for reading the file.</param>
        /// <returns>Bytes inside the file.</returns>
        public static byte[] ReadFile(string fileLocation, bool ignored = false)
        {
            ignored = !ignored;
            using (MemoryStream ms = new MemoryStream())
            {
                ReadFile(fileLocation).CopyTo(ms);
                return ms.ToArray();
            }
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
        public static Image ReadFile(string fileLocation, ImageFormat format = null)
        {
            Image img = Image.FromStream(ReadFile(fileLocation));
            return img;
        }

        /// <summary>
        /// Reads a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="ignored">Not used but required due to other overflows.</param>
        /// <returns>Bitmap from location.</returns>
        public static Bitmap ReadFile(string fileLocation, string ignored = "")
        {
            return new Bitmap(ReadFile(fileLocation, format: null));
        }

        #endregion Read File

        #region Write File

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="input">Text to write on.</param>
        /// <param name="encode">Rules to follow while writing.</param>
        /// <returns><sse cref=true"/> if successfully writes to file, otherwise throws an exception.</returns>
        public static void WriteFile(string fileLocation, string input, Encoding encode)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
            File.Create(fileLocation).Dispose();
            if (ReadFile(fileLocation, encode) != input)
            {
                FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                writer.Write(encode.GetBytes(input), 0, encode.GetBytes(input).Length);
                writer.Close();
            }
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="bitmap">Bitmap to write on.</param>
        /// <param name="format">Format to use while writing.</param>
        /// <returns><sse cref=true"/> if successfully writes to file, otherwise throws an exception.</returns>
        public static void WriteFile(string fileLocation, Bitmap bitmap, ImageFormat format)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
            File.Create(fileLocation).Dispose();
            if (ReadFile(fileLocation, "") != bitmap)
            {
                FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                MemoryStream memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, format);
                //memoryStream.CopyTo(writer);
                writer.Write(memoryStream.ToArray(), 0, Convert.ToInt32(memoryStream.Length));
                memoryStream.Close();
                writer.Close();
            }
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="image">Image to write on.</param>
        /// <param name="format">Format to use while writing.</param>
        /// <returns><sse cref=true"/> if successfully writes to file, otherwise throws an exception.</returns>
        public static void WriteFile(string fileLocation, Image image, ImageFormat format)
        {
            Bitmap bitmap = new Bitmap(image);
            WriteFile(fileLocation, bitmap, format);
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="input">Bytes to write on.</param>
        /// <returns><sse cref=true"/> if successfully writes to file, otherwise throws an exception.</returns>
        public static void WriteFile(string fileLocation, byte[] input)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
            File.Create(fileLocation).Dispose();
            if (ReadFile(fileLocation, true) != input)
            {
                FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                writer.Write(input, 0, input.Length);
                writer.Close();
            }
        }

        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="stream">Stream to write on.</param>
        /// <returns><sse cref=true"/> if successfully writes to file, otherwise throws an exception.</returns>
        public static void WriteFile(string fileLocation, Stream stream)
        {
            if (!Directory.Exists(new FileInfo(fileLocation).DirectoryName)) { Directory.CreateDirectory(new FileInfo(fileLocation).DirectoryName); }
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
            File.Create(fileLocation).Dispose();
            if (ReadFile(fileLocation) != stream)
            {
                FileStream writer = new FileStream(fileLocation, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                stream.CopyTo(writer);
                writer.Close();
            }
        }

        #endregion Write File

        #endregion Files & Directories
    }
}