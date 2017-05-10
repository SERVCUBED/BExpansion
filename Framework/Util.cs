using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace BExpansion
{
    public class Util
    {
        /// <summary>
        /// Send a WebRequest (with a timeout of 1000ms). Returns an empty string on timeout.
        /// </summary>
        /// <param name="url">The requested resource</param>
        /// <param name="quick">True to set a timeout of 1000ms</param>
        /// <param name="postData">The data to include in the POST request</param>
        /// <param name="userAgent">The useragent object of the request.</param>
        /// <param name="referrer">The referrer object of the request.</param>
        /// <returns>The requested resource</returns>
        public static string SendWebRequest(string url, bool quick = false, string postData = null, string userAgent = null, string referrer = null)
        {
            HttpWebRequest wc;
            try
            {
                var r = WebRequest.Create(new Uri(url));
                wc = r as HttpWebRequest;
            }
            catch (Exception)
            {
#if DEBUG
                if (Debugger.IsAttached)
                    throw;
#endif
                return String.Empty;
            }
            if (quick)
                wc.Timeout = 1000;
            wc.KeepAlive = false;
            byte[] buf = new byte[8192];
            StringBuilder sb = new StringBuilder();
            try
            {
                wc.UserAgent = userAgent;
                wc.Referer = referrer;

                if (postData != null)
                {
                    wc.Method = "POST";
                    wc.ContentType = "application/x-www-form-urlencoded";

                    ASCIIEncoding encoding = new ASCIIEncoding();

                    byte[] data = encoding.GetBytes(postData);
                    wc.ContentLength = data.Length;

                    var stream = wc.GetRequestStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }

                var wr = wc.GetResponse();
                Stream resStream = wr.GetResponseStream();

                int? count;
                do
                {
                    count = resStream?.Read(buf, 0, buf.Length);
                    if (count == null)
                        break;
                    if (count != 0)
                    {
                        sb.Append(Encoding.ASCII.GetString(buf, 0, (int)count));
                    }
                } while (count > 0);

                wr.Close();
                resStream?.Close();
            }
            catch (Exception ex)
            {
#if DEBUG
                if (!(ex is WebException) && Debugger.IsAttached)
                    throw;
#endif
            }

            return sb.ToString();
        }

        /// <summary>
        /// Read a file without locking it
        /// </summary>
        /// <param name="url">The URL of the file to read.</param>
        /// <param name="onError">Optional. The string to return if the file cannot be found.</param>
        /// <returns></returns>
        public static string ReadFile(string url, Encoding encoding, string onError = null)
        {
            url = url.MakeFileNameSafe();
            if (!File.Exists(url))
            {
#if DEBUG
                if (Debugger.IsAttached)
                    throw new FileNotFoundException();
#endif
                return onError;
            }

            var maxFileSize = 10 * 1024 * 1024;

            using (var str = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] fileBytes;
                int numBytes;

                // If the number of bytes to read equals the maximum file length, try to read more data from the file
                do
                {
                    fileBytes = new byte[maxFileSize];
                    numBytes = str.Read(fileBytes, 0, maxFileSize);

                    if (numBytes >= maxFileSize)
                        maxFileSize *= 2;

                } while (numBytes == maxFileSize);

                return encoding.GetString(fileBytes, 0, numBytes);
            }
        }

        /// <summary>
        /// Writes a file in another thread.
        /// </summary>
        /// <param name="uri">The path of the file to write.</param>
        /// <param name="contents">The contents of the file to write.</param>
        public static void WriteFileAsync(string uri, string contents, Encoding encoding)
        {
            var t = new Thread(() => WriteFile(uri, contents, encoding));
            t.SetApartmentState(ApartmentState.MTA);
            t.Start();
        }

        public static void WriteFile(string url, string contents, Encoding encoding)
        {
            using (var str = new FileStream(url.MakeFileNameSafe(), FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                var fileBytes = encoding.GetBytes(contents);

                str.Write(fileBytes, 0, fileBytes.Length);
            }
        }

        /// <summary>
        /// Generate a random string. Not cryptographically secure.
        /// </summary>
        /// <returns>A random string</returns>
        public static string GenerateRandomString()
        {
            Random r = new Random();
            return (char)r.Next(97, 122) + Convert.ToString(r.Next(100000));
        }
    }
}
