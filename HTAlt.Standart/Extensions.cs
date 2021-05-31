/*

Copyright © 2021 Eren "Haltroy" Kanat

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

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
        /// Converts <paramref name="bytes"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="bytes"><see cref="byte"/> <seealso cref="Array"/>.</param>
        /// <returns><see cref="string"/></returns>
        public static string ToString(this byte[] bytes)
        {
            return Tools.BytesToString(bytes);
        }

        /// <summary>
        /// Verifies file located in <paramref name="file"/> with <see cref="System.Security.Cryptography.MD5"/> and <seealso cref="System.Security.Cryptography.SHA256"/> methods.
        /// </summary>
        /// <param name="file">File location.</param>
        /// <param name="md5"><see cref="System.Security.Cryptography.MD5"/></param>
        /// <param name="sha256"><see cref="System.Security.Cryptography.SHA256"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool VerifyFile(this string file, string md5, string sha256)
        {
            return Tools.VerifyFile(file, md5, sha256);
        }

        /// <summary>
        /// Verifies file located in <paramref name="file"/> with <see cref="System.Security.Cryptography.MD5"/> and <seealso cref="System.Security.Cryptography.SHA256"/> methods.
        /// </summary>
        /// <param name="file">File location.</param>
        /// <param name="hash">File's supposedly hash.</param>
        /// <param name="preferSha256"><c>true</c> to use SHA256 over MD5, otherwise <c>false</c>.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool VerifyFile(this string file, string hash, bool preferSha256 = false)
        {
            return Tools.VerifyFile(file, hash, preferSha256);
        }

        /// <summary>
        /// Verifies file located in <paramref name="file"/> with <see cref="System.Security.Cryptography.MD5"/> and <seealso cref="System.Security.Cryptography.SHA256"/> methods.
        /// </summary>
        /// <param name="file">File location.</param>
        /// <param name="md5"><see cref="System.Security.Cryptography.MD5"/></param>
        /// <param name="sha256"><see cref="System.Security.Cryptography.SHA256"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool VerifyFile(this string file, byte[] md5, byte[] sha256)
        {
            return Tools.VerifyFile(file, md5, sha256);
        }

        /// <summary>
        /// Verifies file located in <paramref name="file"/> with <see cref="System.Security.Cryptography.MD5"/> and <seealso cref="System.Security.Cryptography.SHA256"/> methods.
        /// </summary>
        /// <param name="file">File location.</param>
        /// <param name="filehash">File's supposedly hash.</param>
        /// <param name="preferSha256"><c>true</c> to use SHA256 over MD5, otherwise <c>false</c>.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool VerifyFile(this string file, byte[] filehash, bool preferSha256 = false)
        {
            return Tools.VerifyFile(file, filehash, preferSha256);
        }

        /// <summary>
        /// Gets <see cref="System.Security.Cryptography.MD5"/> of <paramref name="file"/>.
        /// </summary>
        /// <param name="file">File location.</param>
        /// <param name="ignored">Ignored value.</param>
        /// <returns><see cref="string"/></returns>
        public static string GetMd5Hash(this string file, bool ignored = false)
        {
            return Tools.GetMd5Hash(file, ignored);
        }

        /// <summary>
        /// Gets <see cref="System.Security.Cryptography.SHA256"/> of <paramref name="file"/>.
        /// </summary>
        /// <param name="file">File location.</param>
        /// <param name="ignored">Ignored value.</param>
        /// <returns><see cref="string"/></returns>
        public static string GetSha256Hash(this string file, bool ignored = false)
        {
            return Tools.GetSha256Hash(file, ignored);
        }

        /// <summary>
        /// Gets <see cref="System.Security.Cryptography.MD5"/> of <paramref name="file"/>.
        /// </summary>
        /// <param name="file">File location.</param>
        /// <returns><see cref="byte"/> <seealso cref="Array"/></returns>
        public static byte[] GetMd5Hash(this string file)
        {
            return Tools.GetMd5Hash(file);
        }

        /// <summary>
        /// Gets <see cref="System.Security.Cryptography.SHA256"/> of <paramref name="file"/>.
        /// </summary>
        /// <param name="file">File location.</param>
        /// <param name="ignored">Ignored value.</param>
        /// <returns><see cref="byte"/> <seealso cref="Array"/></returns>
        public static byte[] GetSha256Hash(string file)
        {
            return Tools.GetSha256Hash(file);
        }

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

        /// <summary>
        /// Gets size of the directory located in this string.
        /// </summary>
        /// <param name="dir">Directory</param>
        /// <returns><see cref="long"/></returns>
        public static long GetDirectorySize(this string dir)
        {
            return Tools.DirectorySize(new DirectoryInfo(dir));
        }

        /// <summary>
        /// Gets size of this directory.
        /// </summary>
        /// <param name="dir">Directory</param>
        /// <returns><see cref="long"/></returns>
        public static long Size(this DirectoryInfo dir)
        {
            return Tools.DirectorySize(dir);
        }

        /// <summary>
        /// Detects if user can access <paramref name="dir"/> with try {} method.
        /// </summary>
        /// <param name="dir">Directory</param>
        /// <returns><see cref="true"/> if can access to folder, <seealso cref="false"/> if user has no access to <paramref name="dir"/> and throws <see cref="Exception"/> on other scenarios.</returns>
        public static bool HasWriteAccess(this string dir)
        {
            return Tools.HasWriteAccess(dir);
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
        /// Finds the prime multiplier numbers of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"><see cref="int"/> to search in.</param>
        /// <returns>An array of <see cref="int"/> containing prime multiplier <see cref="int"/> list.</returns>
        public static int[] PrimeMultipliers(this int n)
        {
            return Tools.PrimeMultipliers(n);
        }

        /// <summary>
        /// Gets a <paramref name="n2"/> dividable <see cref="int"/> from <paramref name="n1"/>.
        /// </summary>
        /// <param name="n1"><see cref="int"/> representative of a main number</param>
        /// <param name="n2"><see cref="int"/> representative of a divider number</param>
        /// <param name="balancePoint">Adds remainder of divison if <paramref name="n1"/> is smaller than this paramater. Otherwise, subtracts from <paramref name="n1"/>.</param>
        /// <returns><paramref name="n2"/> dividable close number of <paramref name="n1"/>.</returns>
        public static int MakeItDividable(this int n1, int n2, int balancePoint = 300)
        {
            return Tools.MakeItDividable(n1, n2, balancePoint);
        }

        /// <summary>
        /// Divides <paramref name="n1"/> to <paramref name="divider"/> without remainder.
        /// </summary>
        /// <param name="n1">Main number.</param>
        /// <param name="divider">Divider number.</param>
        /// <returns>Result of devision.</returns>
        public static int Divide(this int n1, int divider)
        {
            return Tools.Divide(n1, divider);
        }

        /// <summary>
        /// Largest Common Division between <paramref name="n1"/> and <paramref name="n2"/>.
        /// </summary>
        /// <param name="n1"><see cref="int"/> representative of a number</param>
        /// <param name="n2"><see cref="int"/> representative of a number</param>
        /// <returns><see cref="int"/> representative of Largest Common Division between <paramref name="n1"/> and <paramref name="n2"/>.</returns>
        public static int LargestCommonDivision(this int n1, int n2)
        {
            return Tools.LargestCommonDivision(n1, n2);
        }

        /// <summary>
        /// Smallest of Common Denominator between <paramref name="n1"/> and <paramref name="n2"/>.
        /// </summary>
        /// <param name="n1"><see cref="int"/> representative of a number</param>
        /// <param name="n2"><see cref="int"/> representative of a number</param>
        /// <returns><see cref="int"/> representative of Smallest of Common Denominator between <paramref name="n1"/> and <paramref name="n2"/>.</returns>
        public static int SmallestOfCommonDenominator(this int n1, int n2)
        {
            return Tools.SmallestOfCommonDenominator(n1, n2);
        }

        /// <summary>
        /// Biggest prime multiplier number of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"><see cref="int"/> to search in.</param>
        /// <returns><see cref="int"/> as the biggest prime multiplier of <paramref name="n"/>.</returns>
        public static int BiggestPrimeMultiplier(this int n)
        {
            return Tools.BiggestPrimeMultiplier(n);
        }

        /// <summary>
        /// Smallest prime multiplier number of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"><see cref="int"/> to search in.</param>
        /// <returns><see cref="int"/> as the smallest prime multiplier of <paramref name="n"/>.</returns>
        public static int SmallestPrimeMultiplier(this int n)
        {
            return Tools.SmallestPrimeMultiplier(n);
        }

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
        /// Prettifies XML code.
        /// Thanks to S M Kamran & Bakudan from StackOverflow
        /// </summary>
        /// <param name="xml">XML code</param>
        /// <returns>Prettified <paramref name="xml"/></returns>
        public static string BeautifyXML(this string xml)
        {
            return Tools.BeautifyXML(xml);
        }

        /// <summary>
        /// Prettifies XML code.
        /// </summary>
        /// <param name="node">XML node</param>
        /// <returns>Prettified <paramref name="node"/></returns>
        public static string BeautifyXML(this XmlNode node)
        {
            return Tools.BeautifyXML(node.OuterXml);
        }

        /// <summary>
        /// Prettifies XML code.
        /// </summary>
        /// <param name="doc">XML Document</param>
        /// <returns>Prettified <paramref name="doc"/></returns>
        public static string BeautifyXML(this XmlDocument doc)
        {
            return Tools.BeautifyXML(doc.DocumentElement.OuterXml);
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
        /// Converts <paramref name="img"/> to an <see cref="Icon"/>
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns><see cref="Icon"/></returns>
        public static Icon ToIcon(this Image image)
        {
            return Tools.IconFromImage(image);
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

        #region Text & XML

        /// <summary>
        /// Finds the root node of <paramref name="doc"/>.
        /// </summary>
        /// <param name="doc">the <see cref="XmlNode"/> (probably <seealso cref="XmlDocument.DocumentElement"/>) to search on.</param>
        /// <returns>a <see cref="System.Xml.XmlNode"/> which represents as the root node.</returns>
        public static System.Xml.XmlNode FindRoot(this System.Xml.XmlNode doc)
        {
            return Tools.FindRoot(doc);
        }

        /// <summary>
        /// Tells if the <paramref name="node"/> is a comment node.
        /// </summary>
        /// <param name="node"><see cref="XmlNode"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool IsComment(this System.Xml.XmlNode node)
        {
            return Tools.NodeIsComment(node);
        }

        /// <summary>
        /// Turns all characters to lowercase, using en-US culture information to avoid language-specific ToLower() errors such as:
        /// <para>Turkish: I &lt;-&gt; ı , İ &lt;-&gt; i</para>
        /// <para>English I &lt;-&gt; i</para>
        /// </summary>
        /// <param name="s"><see cref="string"/></param>
        /// <returns><see cref="string"/></returns>
        public static string ToLowerEnglish(this string s)
        {
            return Tools.ToLowerEnglish(s);
        }

        /// <summary>
        /// Finds the root node of <paramref name="doc"/>.
        /// </summary>
        /// <param name="doc">The XML document.</param>
        /// <returns>a <see cref="System.Xml.XmlNode"/> which represents as the root node.</returns>
        public static System.Xml.XmlNode FindRoot(this System.Xml.XmlDocument doc)
        {
            return Tools.FindRoot(doc);
        }

        /// <summary>
        /// Converts XML-formatted string to <see cref="string"/>.
        /// </summary>
        /// <param name="innerxml">XML-formatted string</param>
        /// <returns>Formatted <paramref name="s"/>.</returns>
        public static string XmlToString(this string innerxml)
        {
            return Tools.XmlToString(innerxml);
        }

        /// <summary>
        /// Converts <paramref name="s"/> to <see cref="System.Xml"/> supported format.
        /// </summary>
        /// <param name="s"><see cref="string"/></param>
        /// <returns>Formatted <paramref name="s"/>.</returns>
        public static string ToXML(this string s)
        {
            return Tools.ToXML(s);
        }

        #endregion Text & XML
    }
}