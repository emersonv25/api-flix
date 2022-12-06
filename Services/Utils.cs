using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api.MyFlix.Services
{
    public class Utils
    {
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public static string ReplaceSpecialChar(string str)
        {
            /** Troca os caracteres acentuados por não acentuados **/
            string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };

            for (int i = 0; i < acentos.Length; i++)
            {
                str = str.Replace(acentos[i], semAcento[i]);
            }
            /** Troca os caracteres especiais da string por "" **/
            string[] caracteresEspeciais = { "¹", "²", "³", "£", "¢", "¬", "º", "¨", "\"", "'", ".", ",", "-", ":", "(", ")", "ª", "|", "\\\\", "°", "_", "@", "#", "!", "$", "%", "&", "*", ";", "/", "<", ">", "?", "[", "]", "{", "}", "=", "+", "§", "´", "`", "^", "~" };

            for (int i = 0; i < caracteresEspeciais.Length; i++)
            {
                str = str.Replace(caracteresEspeciais[i], "");
            }

            /** Troca os caracteres especiais da string por " " **/
            str = Regex.Replace(str, @"[^\w\.@-]", " ",
                                RegexOptions.None, TimeSpan.FromSeconds(1.5));

            return str.Trim();
        }
        #region Download de arquivos
        public static string Download(string url, string fileName, string imagePath)
        {
            string ext = System.IO.Path.GetExtension(url);
            string path = Path.Combine(imagePath, fileName + ext);
            using (var client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
            return fileName + ext;
        }
        #endregion
        #region Manipulação de arquivos
        public static string GetFileFormat(string fullFileName)
        {
            var format = fullFileName.Split(".").Last();
            return "." + format;
        }

        public static List<string> SaveFiles(IFormFileCollection files, string directoryPath)
        {

            List<string> path = new List<string>();
            foreach (var file in files)
            {
                
                string fileName = (Guid.NewGuid().ToString() + GetFileFormat(file.FileName));
                string directory = CreateFilePath(fileName, directoryPath);
                #region Salva o arquivo em disco
                byte[] bytesFile = ConvertFileInByteArray(file);
                System.IO.File.WriteAllBytesAsync(directory, bytesFile);
                
                
                using (var stream = new FileStream(directory, FileMode.Create))
                {
                    file.CopyTo(stream);

                }
                #endregion



                path.Add(directory);
            }

            return path;

        }
        public static string CreateFilePath(string fileName, string directoryPath)
        {

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, fileName);

            return filePath;
        }

        public static string GetFileUrl(string filePath, string baseUrl, string imagesPath)
        {

            var fileUrl = imagesPath
                .Replace("wwwroot", "")
                .Replace("\\", "");

            return (baseUrl + "/" + fileUrl + "/" + filePath);
        }

        public static byte[] ConvertFileInByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public static void DeleteFile(string path)
        {
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }
        #endregion
    }
}

