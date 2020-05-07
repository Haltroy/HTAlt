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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace HTAlt
{
    /// <summary>
    /// Custom class to handle custom actions and events.
    /// </summary>
    public class Tools
    {
        private static readonly string version = "0.1.2.0";
        private static readonly string codeName = "Hiko";
        private static readonly string name = "HTAlt";
        private static readonly string link = "https://github.com/haltroy/HTAlt";
        private static readonly string developer = "Haltroy";
        public Tools()
        {
            PrintInfoToConsole();
        }
        /// <summary>
        /// Prints project info to console if not printed.
        /// If you are going to save settings for this project, please reset <c>isInfoGiven</c> before saving.
        /// </summary>
        public static bool PrintInfoToConsole()
        { 
                if (Properties.Settings.Default.isInfoGiven == false)
                {
                    Properties.Settings.Default.isInfoGiven = true;
                    Console.WriteLine(PrintInfo);
                }
                return true;
        }
        /// <summary>
        /// Returns the information text displayed in console.
        /// </summary>
        public static string PrintInfo
        {
            get => Environment.NewLine
                + "------------------"
                + Environment.NewLine
                + ProjectName
                + " v"
                + ProjectVersion.ToString()
                + (" [ " + ProjectCodeName + " ] ")
                + " by Haltroy"
                + Environment.NewLine
                + ProjectWebsite
                + Environment.NewLine
                + "------------------"
                + Environment.NewLine;
        }
        /// <summary>
        /// Returns this project's name.
        /// </summary>
        public static string ProjectName
        {
            get => name;
        }
        /// <summary>
        /// Returns the codename of this project.
        /// </summary>
        public static string ProjectCodeName
        {
            get => codeName;
        }
        /// <summary>
        /// Returns project version as Version.
        /// </summary>
        public static Version ProjectVersion
        {
            get => new Version(version);
        }
        /// <summary>
        /// Returns developer name of this project.
        /// </summary>
        public static string ProjectDeveloper
        { get => developer; }
        /// <summary>
        /// Returns project website address as Uri.
        /// </summary>
        public static Uri ProjectWebsite
        {
            get => new Uri(link);
        }
        /// <summary>
        /// Converts the image to Base 64 code.
        /// </summary>
        /// <param name="image">Image to convert.</param>
        /// <returns></returns>
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
        /// <returns></returns>
        public static System.Drawing.Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }
        /// <summary>
        /// Generates a random text with 11 random characters.
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomText
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                Enumerable
                   .Range(65, 26)
                    .Select(e => ((char)e).ToString())
                    .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                    .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                    .OrderBy(e => Guid.NewGuid())
                    .Take(11)
                    .ToList().ForEach(e => builder.Append(e));
                return builder.ToString();
            }
        }
        /// <summary>
        /// Gets the base URL of an URL.
        /// </summary>
        /// <param name="url">Address for getting the base address.</param>
        /// <returns></returns>
        public static string getBaseURL(string url)
        {
            Uri uri = new Uri(url);
            string baseUri = uri.GetLeftPart(System.UriPartial.Authority);
            return baseUri;
        }
        /// <summary>
        /// Converts Hexadecimal to Color
        /// </summary>
        /// <param name="hexString">Hex Code of Color</param>
        /// <returns></returns>
        public static Color HexToColor(string hexString)
        {
            return ColorTranslator.FromHtml(hexString);
        }
        /// <summary>
        /// Converts Color to Hexadecimal
        /// </summary>
        /// <param name="color">Color to convert</param>
        /// <returns></returns>
        public static string ColorToHex(Color color)
        {
            return ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb()));
        }
        /// <summary>
        /// Gets Image from Url
        /// </summary>
        /// <param name="url">Address of image.</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
        public static int Brightness(Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }
        /// <summary>
        /// Replaces a color from an image to another color.
        /// </summary>
        /// <param name="inputImage">Image to work on.</param>
        /// <param name="tolerance">The area of ​​relationship with color equivalents.</param>
        /// <param name="oldColor">Color to change from.</param>
        /// <param name="NewColor">Color to change to.</param>
        /// <returns></returns>
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
        /// <returns></returns>
        public static Image RepaintImage(Image input, Image texture, bool repeatable)
        {
            if (repeatable)
            {
                Bitmap inputImage = new Bitmap(input);
                Bitmap outputImage = new Bitmap(input.Width, input.Height);
                Bitmap textureImage = new Bitmap(texture);
                for (int y = 0; y < outputImage.Height; y++)
                {
                    for (int x = 0; x < outputImage.Width; x++)
                    {
                        Color PixelColor = textureImage.GetPixel(x % textureImage.Width, y % textureImage.Height);
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
            else
            {
                Bitmap inputImage = new Bitmap(input);
                Bitmap outputImage = new Bitmap(input.Width, input.Height);
                Bitmap textureImage = new Bitmap(original: texture, newSize: input.Size);
                for (int y = 0; y < outputImage.Height; y++)
                {
                    for (int x = 0; x < outputImage.Width; x++)
                    {
                        Color PixelColor = textureImage.GetPixel(x, y);
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
        }
        /// <summary>
        /// Determines which color (Black or White) to use for foreground of the color.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <returns></returns>
        public static Color AutoWhiteBlack(Color c)
        {
            return IsBright(c) ? Color.Black : Color.White;
        }
        /// <summary>
        /// Determies which color (Black or White) is closer to the color.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <returns></returns>
        public static Color WhiteOrBlack(Color c)
        {
            return IsBright(c) ? Color.White : Color.Black;
        }
        /// <summary>
        /// Returns <c>true</c> if the color is bright.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <returns></returns>
        public static bool IsBright(Color c)
        {
            return Brightness(c) > 130;
        }
        /// <summary>
        /// Subtracts the number if possible.
        /// </summary>
        /// <param name="number">Integer to work on.</param>
        /// <param name="subtract">Integer to subtract.</param>
        /// <returns></returns>
        public static int SubtractIfNeeded(int number, int subtract)
        {
            return number > subtract ? number - subtract : number;
        }
        /// <summary>
        /// Add the number if the result is going to be smaller or equal to limit.
        /// </summary>
        /// <param name="number">Integer to work on.</param>
        /// <param name="add">Integer to add.</param>
        /// <param name="limit">Integer for limit.</param>
        /// <returns></returns>
        public static int AddIfNeeded(int number, int add, int limit)
        {
            return number + add > limit ? number : number + add;
        }
        /// <summary>
        /// Reverses a color.
        /// </summary>
        /// <param name="c">Color to work on.</param>
        /// <param name="reverseAlpha"><c>true</c> to also reverse Alpha (Transparency) channel.</param>
        /// <returns></returns>
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
        /// <returns></returns>
        public static Color ShiftBrightnessIfNeeded(Color baseColor, int value, bool shiftAlpha)
        {
            if (IsBright(baseColor))
            {
                return Color.FromArgb(shiftAlpha ? SubtractIfNeeded(baseColor.A, value) : baseColor.A,
                                      SubtractIfNeeded(baseColor.R, value),
                                      SubtractIfNeeded(baseColor.G, value),
                                      SubtractIfNeeded(baseColor.B, value));
            }
            else
            {
                return Color.FromArgb(shiftAlpha ? AddIfNeeded(baseColor.A, value, 255) : baseColor.A,
                      AddIfNeeded(baseColor.R, value, 255),
                      AddIfNeeded(baseColor.G, value, 255),
                      AddIfNeeded(baseColor.B, value, 255));
            }
        }
        /// <summary>
        /// Reads a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="encode">Rules for reading the file.</param>
        /// <returns></returns>
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
        /// <returns></returns>
        public static Stream ReadFile(string fileLocation)
        {
            FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return fs;

        }
        /// <summary>
        /// Creates and writes a file without locking it.
        /// </summary>
        /// <param name="fileLocation">Location of the file.</param>
        /// <param name="input">Text to write on.</param>
        /// <param name="encode">Rules to follow while writing.</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
