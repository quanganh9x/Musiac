using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public static class FileHandle
    {
        private static StorageFolder folder = ApplicationData.Current.LocalFolder;
        public static async Task<StorageFile> Create(string filename)
        {
            try
            {
                return await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            } catch (System.Exception)
            {
                throw new System.Exception();
            }
        }
        public static async Task<StorageFile> Get(string filename)
        {
            try
            {
                return await folder.GetFileAsync(filename);
            } catch (System.Exception)
            {
                throw new System.Exception();
            }
        }
        public static async Task Save(string filename, string value)
        {
            await FileIO.WriteTextAsync(await Create(filename), value);
        }
        public static async Task<string> Read(string filename)
        {
            try
            {
                return await FileIO.ReadTextAsync(await Get(filename));
            } catch (System.Exception)
            {
                return null;
            }
        }
        public static async Task<string> GetToken()
        {
            try
            {
                return await FileIO.ReadTextAsync(await Get("token.ini"));
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
