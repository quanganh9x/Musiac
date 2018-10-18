using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using T1708E_UWP.Entity;
using Windows.Storage;

namespace T1708E_UWP.Service
{
    public enum APITypes
    {
        SignIn, SignUp, CreateSong, GetSongs, GetSavedSongs, GetInfo, GetUpload
    }
    public static class APIExtensions
    {
        private static string API = "https://2-dot-backup-server-002.appspot.com/";
        private static string REGISTER_API = API + "_api/v2/members";
        private static string LOGIN_API = API + "_api/v2/members/authentication";
        private static string SONG_API = API + "_api/v2/songs";
        private static string MY_SONG_API = API + "_api/v2/songs/get-mine";
        private static string MY_INFO_API = API + "_api/v2/members/information";
        private static string UPLOAD_API = API + "get-upload-token";

        public static string getURL(this APITypes type)
        {
            switch (type)
            {
                case APITypes.SignUp:
                    return REGISTER_API;
                case APITypes.SignIn:
                    return LOGIN_API;
                case APITypes.CreateSong:
                    return SONG_API;
                case APITypes.GetSongs:
                    return SONG_API;
                case APITypes.GetSavedSongs:
                    return MY_SONG_API;
                case APITypes.GetInfo:
                    return MY_INFO_API;
                case APITypes.GetUpload:
                    return UPLOAD_API;
                default:
                    return "";
            }
        }

        public static bool isTokenNeeded(this APITypes type)
        {
            switch (type)
            {
                case APITypes.CreateSong:
                case APITypes.GetSongs:
                case APITypes.GetSavedSongs:
                case APITypes.GetInfo:
                    return true;
                default:
                    return false;
            }
        }
    }
    class ApiHandle<T>
    {
        public async static Task<string> Call(APITypes type, T item)
        {
            string URL = APIExtensions.getURL(type);
            HttpClient httpClient = new HttpClient();
            if (APIExtensions.isTokenNeeded(type)) httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + await FileHandle.GetToken());
            if (item == null)
            {
                string content = await httpClient.GetAsync(URL).Result.Content.ReadAsStringAsync();
                Debug.WriteLine("GET: " + content);
                return content; // HTTP GET
            }
            try
            {
                string data = JsonConvert.SerializeObject(item);
                string content = await httpClient.PostAsync(URL, new StringContent(data, System.Text.Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                Debug.WriteLine("POST: " + content);
                return content;
            } catch (JsonSerializationException)
            {
                ExceptionHandle.ThrowDebug("json error");
                return null;
            }
        }
        public static async Task<Uri> Upload(StorageFile file, string URL, string param, string type)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(URL);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";

            Stream rs = await wr.GetRequestStreamAsync();
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", param, "path_file", type);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);
            
            Stream fileStream = await file.OpenStreamForReadAsync();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);

            WebResponse wresp = null;
            try
            {
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                return new Uri(reader2.ReadToEnd(), UriKind.Absolute);
            }
            catch
            {
                ExceptionHandle.ThrowDebug("upload error");
                return null;
            }
        }
        public async static void ThrowException(string response)
        {
            try
            {
                Entity.Exception errorObject = JsonConvert.DeserializeObject<Entity.Exception>(response);
                if (errorObject != null && errorObject.error.Count > 0)
                {
                    ExceptionHandle.ThrowDebug(errorObject.error.Values.First().ToString());
                }
            }
            catch
            {
                ExceptionHandle.ThrowDebug("json exception parse error");
            }
            
            
        }
    }
}
