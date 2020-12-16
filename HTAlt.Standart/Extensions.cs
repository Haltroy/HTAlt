//MIT License
//
//Copyright (c) 2020-2021 Eren "Haltroy" Kanat
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

using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace HTAlt
{
    /// <summary>
    /// This class contains extensions.
    /// </summary>
    public static class Extensions
    {
        #region Files & Directories

        #region Write File

        /// <summary>
        /// Writes <paramref name="bytes"/> to <paramref name="location"/>.
        /// </summary>
        /// <param name="bytes">Bytes to write.</param>
        /// <param name="location">File location.</param>
        public static void WriteToFile(this byte[] bytes, string location)
        {
            Tools.WriteFile(location, bytes);
        }

        /// <summary>
        /// Writes <paramref name="stream"/> to <paramref name="location"/>.
        /// </summary>
        /// <param name="stream">Stream to write.</param>
        /// <param name="location">File location.</param>
        public static void WriteToFile(this Stream stream, string location)
        {
            Tools.WriteFile(location, stream);
        }

        /// <summary>
        /// Writes <paramref name="img"/> to <paramref name="location"/> with <paramref name="format"/>.
        /// </summary>
        /// <param name="img">Image to write.</param>
        /// <param name="location">File location.</param>
        /// <param name="format">Format of image.</param>
        public static void WriteToFile(this Image img, string location, System.Drawing.Imaging.ImageFormat format)
        {
            Tools.WriteFile(location, img, format);
        }

        /// <summary>
        /// Writes <paramref name="bmap"/> to <paramref name="location"/> with <paramref name="format"/>.
        /// </summary>
        /// <param name="bmap">Bitmap to write.</param>
        /// <param name="location">File location.</param>
        /// <param name="format">Format of image.</param>
        public static void WriteToFile(this Bitmap bmap, string location, System.Drawing.Imaging.ImageFormat format)
        {
            Tools.WriteFile(location, bmap, format);
        }

        /// <summary>
        /// Writes string to <paramref name="location"/> with <see cref="Encoding.Unicode"/> as default encoding.
        /// </summary>
        /// <param name="str">String to write.</param>
        /// <param name="location">File location.</param>
        public static void WriteToFile(this string str, string location)
        {
            str.WriteToFile(location, Encoding.Unicode);
        }

        /// <summary>
        /// Writes string to <paramref name="location"/> with <paramref name="encoding"/>.
        /// </summary>
        /// <param name="str">String to write.</param>
        /// <param name="location">File location.</param>
        /// <param name="encoding">Encoding of file.</param>
        public static void WriteToFile(this string str, string location, Encoding encoding)
        {
            Tools.WriteFile(location, str, encoding);
        }

        #endregion Write File

        /// <summary>
        /// Checks if file exists by assuming this string is a path to a file.
        /// </summary>
        /// <param name="str">File location</param>
        /// <returns><sse cref=true"/> if file exists, otherwise <sse cref=false"/>.</returns>
        public static bool FileExists(this string str)
        {
            return System.IO.File.Exists(str);
        }

        /// <summary>
        /// Checks if directory exists by assuming this string is a path to a directory.
        /// </summary>
        /// <param name="str">Directory location</param>
        /// <returns><sse cref=true"/> if directory exists, otherwise <sse cref=false"/>.</returns>
        public static bool DirectoryExists(this string str)
        {
            return System.IO.Directory.Exists(str);
        }

        /// <summary>
        /// Checks if directory is empty by assuming this string is a path to a directory.
        /// </summary>
        /// <param name="str">Directory location.</param>
        /// <returns><sse cref=true"/> if directory is empty, otherwise <sse cref=false"/>.</returns>
        public static bool IsDirectoryEmpty(this string str)
        {
            return Tools.IsDirectoryEmpty(str);
        }

        /// <summary>
        /// Deletes a string from every files' name in a directory by assuming this string is a path to a directory.
        /// </summary>
        /// <param name="str">Directory location.</param>
        /// <param name="stringToRemove">String to remove from file names.</param>
        public static void RemoveStringFromFileNames(this string str, string stringToRemove)
        {
            Tools.RemoveFromFileNames(str, stringToRemove);
        }

        #endregion Files & Directories

        #region Color

        /// <summary>
        /// Selects an image (<paramref name="white"/> or <paramref name="black"/>) based on <paramref name="clr"/>'s brightness.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="white">Image to pick if <paramref name="clr"/> is dark.</param>
        /// <param name="black">Image to pick if <paramref name="clr"/> is bright.</param>
        /// <returns><paramref name="white"/> if <paramref name="clr"/> is dark, otherwise <paramref name="black"/>.</returns>
        public static Image SelectImage(this Color clr, ref Image white, ref Image black)
        {
            return Tools.SelectImageFromColor(clr, ref white, ref black, false);
        }

        /// <summary>
        /// Selects an image (<paramref name="white"/> or <paramref name="black"/>) based on <paramref name="clr"/>'s brightness and <paramref name="reverse"/> mode.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="white">Image to pick if <paramref name="clr"/> is dark (in non-reverse mode) or bright (in reverse mode).</param>
        /// <param name="black">Image to pick if <paramref name="clr"/> is bright (in non-reverse mode) or dark (in reverse mode).</param>
        /// <param name="reverse">Reverses the output.</param>
        /// <returns>In non-reverse mode: <paramref name="white"/> if <paramref name="clr"/> is dark, otherwise <paramref name="black"/>.<para/> In reverse mode: <paramref name="black"/> if <paramref name="clr"/> is dark, otherwise <paramref name="white"/>.</returns>
        public static Image SelectImage(this Color clr, ref Image white, ref Image black, bool reverse)
        {
            return Tools.SelectImageFromColor(clr, ref white, ref black, reverse);
        }

        /// <summary>
        /// Selects a bitmap (<paramref name="white"/> or <paramref name="black"/>) based on <paramref name="clr"/>'s brightness.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="white">Bitmap to pick if <paramref name="clr"/> is dark.</param>
        /// <param name="black">Bitmap to pick if <paramref name="clr"/> is bright.</param>
        /// <returns><paramref name="white"/> if <paramref name="clr"/> is dark, otherwise <paramref name="black"/>.</returns>
        public static Bitmap SelectBitmap(this Color clr, ref Bitmap white, ref Bitmap black)
        {
            return Tools.SelectBitmapFromColor(clr, ref white, ref black, false);
        }

        /// <summary>
        /// Selects a bitmap (<paramref name="white"/> or <paramref name="black"/>) based on <paramref name="clr"/>'s brightness and <paramref name="reverse"/> mode.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="white">Bitmap to pick if <paramref name="clr"/> is dark (in non-reverse mode) or bright (in reverse mode).</param>
        /// <param name="black">Bitmap to pick if <paramref name="clr"/> is bright (in non-reverse mode) or dark (in reverse mode).</param>
        /// <param name="reverse">Reverses the output.</param>
        /// <returns>In non-reverse mode: <paramref name="white"/> if <paramref name="clr"/> is dark, otherwise <paramref name="black"/>. <para/> In reverse mode: <paramref name="black"/> if <paramref name="clr"/> is dark, otherwise <paramref name="white"/>.</returns>
        public static Bitmap SelectBitmap(this Color clr, ref Bitmap white, ref Bitmap black, bool reverse)
        {
            return Tools.SelectBitmapFromColor(clr, ref white, ref black, reverse);
        }

        /// <summary>
        /// Reverses <paramref name="clr"/>.
        /// </summary>
        /// <param name="clr">Color to reverse.</param>
        /// <returns>Reversed <paramref name="clr"/>.</returns>
        public static Color Reverse(this Color clr)
        {
            return Tools.ReverseColor(clr, false);
        }

        /// <summary>
        /// Reverses <paramref name="clr"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="reverseAlpha"><see cref="true"/> to reverse transparency, otherwise <see cref="false"/>.</param>
        /// <returns>Reversed <paramref name="clr"/>.</returns>
        public static Color Reverse(this Color clr, bool reverseAlpha)
        {
            return Tools.ReverseColor(clr, reverseAlpha);
        }

        /// <summary>
        /// Random color.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <returns>A random color.</returns>
        public static Color Random(this Color clr)
        {
            return Tools.RandomColor();
        }

        /// <summary>
        /// Random color.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="Seed"></param>
        /// <returns>A random color.</returns>
        public static Color Random(this Color clr, int Seed)
        {
            return Tools.RandomColor(255, false, Seed);
        }

        /// <summary>
        /// Random color.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="Transparency">Transparency of random color.</param>
        /// <param name="ignoreThisParam">Set this parameter to whatever you want. Ignored.</param>
        /// <returns>A random color.</returns>
        public static Color Random(this Color clr, int Transparency, bool ignoreThisParam = true)
        {
            return Tools.RandomColor(Transparency);
        }

        /// <summary>
        /// Random color.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="randomTransparency"><sse cref=true"/> for random transparency, otherwise <sse cref=false"/>.</param>
        /// <returns>A random color.</returns>
        public static Color Random(this Color clr, bool randomTransparency)
        {
            return Tools.RandomColor(255, randomTransparency);
        }

        /// <summary>
        /// Random color.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="Transparency">Transparency of random color.</param>
        /// <param name="Seed">Seed for random number generator.</param>
        /// <returns>A random color.</returns>
        public static Color Random(this Color clr, int Transparency, int Seed)
        {
            return Tools.RandomColor(Transparency, false, Seed);
        }

        /// <summary>
        /// Random color.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="randomTransparency"><sse cref=true"/> for random transparency, otherwise <sse cref=false"/>.</param>
        /// <param name="Seed">Seed for random number generator.</param>
        /// <returns>A random color.</returns>
        public static Color Random(this Color clr, bool randomTransparency, int Seed)
        {
            return Tools.RandomColor(255, randomTransparency, Seed);
        }

        /// <summary>
        /// Determines if <paramref name="clr"/> is bright.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <returns><sse cref=true"/> if <paramref name="clr"/> is bright, otherwise <sse cref=false"/>.</returns>
        public static bool IsBright(this Color clr)
        {
            return Tools.IsBright(clr);
        }

        /// <summary>
        /// Brightness level of <paramref name="clr"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <returns>The brightness level of <paramref name="clr"/>.</returns>
        public static int Brightness(this Color clr)
        {
            return Tools.Brightness(clr);
        }

        /// <summary>
        /// Determines if <paramref name="clr"/> is invisible.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <returns><sse cref=true"/> if color is invisible, otherwise <sse cref=false"/>.</returns>
        public static bool IsInvisible(this Color clr)
        {
            return Tools.IsInvisible(clr);
        }

        /// <summary>
        /// Determines if <paramref name="clr"/> is opaque.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <returns><see cref="true"/> if <paramref name="clr"/> is opaque, otherwise <see cref="false"/>.</returns>
        public static bool IsOpaque(this Color clr)
        {
            return Tools.IsOpaque(clr);
        }

        /// <summary>
        /// Determines  if the transparency level of <paramref name="clr"/> is high.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <returns><see cref="true"/> if the transparency level of <paramref name="clr"/> is high, otherwise <see cref="false"/>.</returns>
        public static bool IsTransparencyHigh(this Color clr)
        {
            return Tools.IsTransparencyHigh(clr);
        }

        /// <summary>
        /// Determines the transparency level of <paramref name="clr"/>. Same as <see cref="Color.A"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <returns>The transparency level of <paramref name="clr"/>.</returns>
        public static int Transparency(this Color clr)
        {
            return clr.A;
        }

        /// <summary>
        /// Gives hexadecimal color code of <paramref name="clr"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <returns>Hex code of <paramref name="clr"/>.</returns>
        public static string ToHex(this Color clr)
        {
            return Tools.ColorToHex(clr);
        }

        /// <summary>
        /// Color from hexadecimal color code.
        /// </summary>
        /// <param name="hex">Hexadecimal string.</param>
        /// <returns><see cref="Color"/> from <paramref name="hex"/> color code.</returns>
        public static Color HexToColor(this string hex)
        {
            return Tools.HexToColor(hex);
        }

        /// <summary>
        /// Picks <see cref="Color.White"/> if <paramref name="clr"/> is dark, otherwise picks <see cref="Color.Black"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <returns><see cref="Color.White"/> if <paramref name="clr"/> is dark, otherwise picks <see cref="Color.Black"/>.</returns>
        public static Color AutoWhiteBlack(this Color clr)
        {
            return clr.AutoWhiteBlack(false);
        }

        /// <summary>
        /// Picks <see cref="Color.White"/> if <paramref name="clr"/> is dark (in non-reverse mode) or bright (in reverse mode), otherwise picks <see cref="Color.Black"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="reverse"></param>
        /// <returns>In non-reverse mode: <see cref="Color.White"/> if <paramref name="clr"/> is dark, otherwise picks <see cref="Color.Black"/>. <para/> In reverse mode: <see cref="Color.Black"/> if <paramref name="clr"/> is dark, otherwise picks <see cref="Color.White"/>. </returns>
        public static Color AutoWhiteBlack(this Color clr, bool reverse)
        {
            return reverse ? Tools.WhiteOrBlack(clr) : Tools.AutoWhiteBlack(clr);
        }

        /// <summary>
        /// Shifts brightness of <paramref name="clr"/> to <paramref name="shiftTo"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="shiftTo">New brightness level.</param>
        /// <returns>A <see cref="Color"/>.</returns>
        public static Color ShiftBrightnessTo(this Color clr, int shiftTo)
        {
            return Tools.ShiftBrightnessTo(clr, shiftTo, false);
        }

        /// <summary>
        /// Shifts brightness of <paramref name="clr"/> to <paramref name="shiftTo"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="shiftTo">New brightness level.</param>
        /// <param name="shiftAlpha"><see cref="true"/> to shift transparency, otherwise <see cref="false"/>.</param>
        /// <returns>A <see cref="Color"/>.</returns>
        public static Color ShiftBrightnessTo(this Color clr, int shiftTo, bool shiftAlpha)
        {
            return Tools.ShiftBrightnessTo(clr, shiftTo, shiftAlpha);
        }

        /// <summary>
        /// Shifts brightness of <paramref name="clr"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="shift">Shift level.</param>
        /// <returns>A <see cref="Color"/>.</returns>
        public static Color ShiftBrightness(this Color clr, int shift)
        {
            return Tools.ShiftBrightness(clr, shift, false);
        }

        /// <summary>
        /// Shifts brightness of <paramref name="clr"/>.
        /// </summary>
        /// <param name="clr">Color</param>
        /// <param name="shift">Shift level.</param>
        /// <param name="shiftAlpha"><see cref="true"/> to shift transparency, otherwise <see cref="false"/>.</param>
        /// <returns>A <see cref="Color"/>.</returns>
        public static Color ShiftBrightness(this Color clr, int shift, bool shiftAlpha)
        {
            return Tools.ShiftBrightness(clr, shift, shiftAlpha);
        }

        #endregion Color

        #region Math

        /// <summary>
        /// Adds if 32-bit integer limit is unreached after calculation.
        /// </summary>
        /// <param name="n">Integer</param>
        /// <param name="add">Add number.</param>
        /// <returns><see cref="int"/></returns>
        public static int AddWithLimit(this int n, int add)
        {
            return Tools.AddIfNeeded(n, add, int.MaxValue);
        }

        /// <summary>
        /// Adds if <paramref name="limit"/> is unreached after calculation.
        /// </summary>
        /// <param name="n">Integer</param>
        /// <param name="add">Add number.</param>
        /// <param name="limit">Limit number</param>
        /// <returns><see cref="int"/></returns>
        public static int AddWithLimit(this int n, int add, int limit)
        {
            return Tools.AddIfNeeded(n, add, limit);
        }

        /// <summary>
        /// Subtracts <paramref name="st"/> from <paramref name="n"/> if <paramref name="limit"/> is unreached after calculation.
        /// </summary>
        /// <param name="n">Integer</param>
        /// <param name="st">Subtract number</param>
        /// <param name="limit">Limit number</param>
        /// <returns><see cref="int"/></returns>
        public static int SubtractWithlimit(this int n, int st, int limit)
        {
            return Tools.SubtractIfNeeded(n, st, limit);
        }

        /// <summary>
        /// Subtracts <paramref name="st"/> from <paramref name="n"/> if 0 is unreached after calculation.
        /// </summary>
        /// <param name="n">Integer</param>
        /// <param name="st">Subtract number</param>
        /// <returns><see cref="int"/></returns>
        public static int SubtractWithlimit(this int n, int st)
        {
            return Tools.SubtractIfNeeded(n, st, 0);
        }

        #endregion Math

        #region Internet

        /// <summary>
        /// Gets the base URL of <paramref name="str"/>.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Base URL of <paramref name="str"/>.</returns>
        public static string GetBaseUrl(this string str)
        {
            return Tools.GetBaseURL(str);
        }

        /// <summary>
        /// Determines if <paramref name="str"/> is a valid Haltroy website URL.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns><see cref=">true"/> if <paramref name="str"/> is a valid Haltroy website URL, otherwise <see cref="false"/>.</returns>
        public static bool IsHaltroyWebsite(this string str)
        {
            return Tools.ValidHaltroyWebsite(str);
        }

        /// <summary>
        /// Checks if <paramref name="str"/> is a valid URL.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns><see cref="true"/> if <paramref name="str"/> is a valid URL, otherwise <see cref="false"/>.</returns>
        public static bool ValidUrl(this string str)
        {
            return Tools.ValidUrl(str);
        }

        /// <summary>
        /// Checks if <paramref name="str"/> is a valid URL.
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="ignoreDefaults"><see cref="true"/> to ignore default protocols, otherwise <see cref="false"/>.</param>
        /// <returns><see cref="true"/> if <paramref name="str"/> is a valid URL, otherwise <see cref="false"/>.</returns>
        public static bool ValidUrl(this string str, bool ignoreDefaults)
        {
            return Tools.ValidUrl(str, ignoreDefaults);
        }

        /// <summary>
        /// Checks if <paramref name="str"/> is a valid URL.
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="CustomProtocols">Custom protocols to check alongside with defaults.</param>
        /// <returns><see cref="true"/> if <paramref name="str"/> is a valid URL, otherwise <see cref="false"/>.</returns>
        public static bool ValidUrl(this string str, string[] CustomProtocols)
        {
            return Tools.ValidUrl(str, CustomProtocols);
        }

        /// <summary>
        /// Checks if <paramref name="str"/> is a valid URL.
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="ignoreDefaults"><see cref="true"/> to ignore default protocols, otherwise <see cref="false"/>.</param>
        /// <param name="CustomProtocols">Custom protocols to check alongside with defaults (if <paramref name="ignoreDefaults"/> is <see cref="false"/>) .</param>
        /// <returns><see cref="true"/> if <paramref name="str"/> is a valid URL, otherwise <see cref="false"/>.</returns>
        public static bool ValidUrl(this string str, bool ignoreDefaults, string[] CustomProtocols)
        {
            return Tools.ValidUrl(str, CustomProtocols, ignoreDefaults);
        }

        /// <summary>
        /// Checks if <paramref name="str"/> is a valid URL.
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="ignoreDefaults"><see cref="true"/> to ignore default protocols, otherwise <see cref="false"/>.</param>
        /// <param name="options">Regex options.</param>
        /// <param name="CustomProtocols">Custom protocols to check alongside with defaults (if <paramref name="ignoreDefaults"/> is <see cref="false"/>) .</param>
        /// <returns><see cref="true"/> if <paramref name="str"/> is a valid URL, otherwise <see cref="false"/>.</returns>
        public static bool ValidUrl(this string str, bool ignoreDefaults, RegexOptions options, string[] CustomProtocols)
        {
            return Tools.ValidUrl(str, CustomProtocols, options, ignoreDefaults);
        }

        /// <summary>
        /// Creates Internet Shortcut (Windows) to <paramref name="location"/>.
        /// </summary>
        /// <param name="str">URL</param>
        /// <param name="location">Location of shortcut.</param>
        public static void CreateInternetShortcutTo(this string str, string location)
        {
            Tools.CreateInternetShortcut(str, location);
        }

        /// <summary>
        /// Creates Internet Shortcut (Windows) to <paramref name="str"/>.
        /// </summary>
        /// <param name="str">Location of shortcut.</param>
        /// <param name="url">URL of shortcut.</param>
        public static void CreateInternetShortcut(this string str, string url)
        {
            Tools.CreateInternetShortcut(url, str);
        }

        #endregion Internet

        #region Image

        /// <summary>
        /// Resizes <paramref name="image"/>.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="width">New width of <paramref name="image"/>.</param>
        /// <param name="height">New height of <paramref name="image"/>.</param>
        /// <returns><see cref="Bitmap"/></returns>
        public static Bitmap Resize(this Image image, int width, int height)
        {
            return Tools.ResizeImage(image, width, height);
        }

        /// <summary>
        /// Resizes <paramref name="image"/>.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="size">New size of <paramref name="image"/>.</param>
        /// <returns><see cref="Bitmap"/></returns>
        public static Bitmap Resize(this Image image, Size size)
        {
            return Tools.ResizeImage(image, size);
        }

        /// <summary>
        /// Changes <paramref name="basecolor"/> to <paramref name="alteringColor"/> of <paramref name="image"/>.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="basecolor">Color to change.</param>
        /// <param name="alteringColor">New color.</param>
        /// <returns><see cref="Bitmap"/></returns>
        public static Bitmap ChangeColor(this Bitmap image, Color basecolor, Color alteringColor)
        {
            return Tools.ChangeColor(image, basecolor, alteringColor);
        }

        /// <summary>
        /// Changes <paramref name="basecolor"/> to <paramref name="alteringColor"/> of <paramref name="image"/>.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="basecolor">Color to change.</param>
        /// <param name="alteringColor">New color.</param>
        /// <returns><see cref="Image"/></returns>
        public static Image ChangeColor(this Image image, Color basecolor, Color alteringColor)
        {
            return Tools.ChangeColor(image, basecolor, alteringColor);
        }

        /// <summary>
        /// Fills <paramref name="rect"/> with <paramref name="image"/>.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="rect">Rectangle</param>
        /// <returns><see cref="Image"/></returns>
        public static Bitmap FillWithPattern(this Image image, Rectangle rect)
        {
            return Tools.FillPattern(image, rect);
        }

        /// <summary>
        /// Converts <paramref name="image"/> to base64 <see cref="string"/>.
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns><see cref="string"/></returns>
        public static string ToBase64(this Image image)
        {
            return Tools.ImageToBase64(image);
        }

        /// <summary>
        /// Converts base64 <see cref="string"/> to <see cref="Image"/>.
        /// </summary>
        /// <param name="base64String">Base64 string.</param>
        /// <returns><see cref="Image"/></returns>
        public static Image FromBase64(this string base64String)
        {
            return Tools.Base64ToImage(base64String);
        }

        /// <summary>
        /// Crops <paramref name="img"/>.
        /// </summary>
        /// <param name="img">Image</param>
        /// <param name="cropArea">Crop Area.</param>
        /// <returns><see cref="Image"/></returns>
        public static Image Crop(this Image img, Rectangle cropArea)
        {
            return Tools.CropImage(img, cropArea);
        }

        /// <summary>
        /// Crops <paramref name="bm"/>.
        /// </summary>
        /// <param name="bm">Image</param>
        /// <param name="cropArea">Crop Area.</param>
        /// <returns><see cref="Bitmap"/></returns>
        public static Bitmap Crop(this Bitmap bm, Rectangle cropArea)
        {
            return Tools.CropBitmap(bm, cropArea);
        }

        /// <summary>
        /// Gets <see cref="Image"/> from <paramref name="url"/>.
        /// </summary>
        /// <param name="url">URL of Image.</param>
        /// <returns><see cref="Image"/></returns>
        public static Image GetImage(this string url)
        {
            return Tools.GetImageFromUrl(url);
        }

        /// <summary>
        /// Replaces <paramref name="oldColor"/> (with <paramref name="tolerance"/>) to <paramref name="NewColor"/> in <paramref name="inputImage"/>.
        /// </summary>
        /// <param name="inputImage">Image</param>
        /// <param name="tolerance">Tolerance of color.</param>
        /// <param name="oldColor">Old Color</param>
        /// <param name="NewColor">New Color</param>
        /// <returns><see cref="Bitmap"/></returns>
        public static Bitmap ReplaceColor(this Bitmap inputImage, int tolerance, Color oldColor, Color NewColor)
        {
            return Tools.ColorReplace(inputImage, tolerance, oldColor, NewColor);
        }

        /// <summary>
        /// Replaces <paramref name="oldColor"/> (with <paramref name="tolerance"/>) to <paramref name="NewColor"/> in <paramref name="inputImage"/>.
        /// </summary>
        /// <param name="inputImage">Image</param>
        /// <param name="tolerance">Tolerance of color.</param>
        /// <param name="oldColor">Old Color</param>
        /// <param name="NewColor">New Color</param>
        /// <returns><see cref="Image"/></returns>
        public static Image ReplaceColor(this Image inputImage, int tolerance, Color oldColor, Color NewColor)
        {
            return Tools.ColorReplace(inputImage, tolerance, oldColor, NewColor);
        }

        /// <summary>
        /// Repaints <paramref name="input"/> with <paramref name="texture"/>.
        /// </summary>
        /// <param name="input">Image</param>
        /// <param name="texture"></param>
        /// <param name="repeatable"><see cref="true"/> to repeat <paramref name="texture"/>, otherwise <see cref="false"/>.</param>
        /// <returns><see cref="Image"/></returns>
        public static Image Repaint(this Image input, Image texture, bool repeatable)
        {
            return Tools.RepaintImage(input, texture, repeatable);
        }

        #endregion Image
    }
}