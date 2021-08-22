using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace spfe
{
    public static class FileManager
    {
        public static KeyValuePair<int, string> GetFooterStrWithIdxFromFile(string filename)
        {
            
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                {
                    fs.Seek(
                        fs.Length > Constants.FooterMaxLength
                            ? -Constants.FooterMaxLength
                            : -fs.Length,
                        SeekOrigin.End);
                    byte[] buffer = new byte[fs.Length < Constants.FooterMaxLength
                        ? fs.Length 
                        : Constants.FooterMaxLength];
                    int bytesRead = fs.Read(buffer, 0, buffer.Length);
                    return bytesRead > 0
                        ? GetFooterFromChunk(buffer)
                        : new KeyValuePair<int, string>(0, String.Empty);
                }
            }
        }

        private static KeyValuePair<int, string> GetFooterFromChunk(byte[] buffer)
        {
            var chunk = Encoding.Default.GetString(buffer);
            try
            {
                var footer = Regex.Match(chunk, $"\\{Constants.FooterHeaderCorrectFormat}(.*)");
                return new KeyValuePair<int, string>(footer.Index ,footer.Groups[0].Value);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new KeyValuePair<int, string>(0, String.Empty);
            }
        }

        public static void WriteFooterToFile(string newFooter, string filename, int originalFooterLen)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                fs.Seek(-originalFooterLen, SeekOrigin.End);
                fs.Write(Encoding.ASCII.GetBytes(newFooter), 0, newFooter.Length);
                if (newFooter.Length < originalFooterLen)
                {
                    fs.SetLength(fs.Length - (originalFooterLen - newFooter.Length));
                }
            }
        }
    }
}
