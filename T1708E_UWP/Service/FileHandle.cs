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
        public static async Task SaveToFile(string value)
        {
            Service.ProgressBar.SetProgress(50, true);
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "quanganh9x";
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            
            if (file != null)
            {
                Service.ProgressBar.SetProgress(80, true);
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                await Windows.Storage.FileIO.WriteTextAsync(file, value);
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    await Dialog.Show("success!");
                }
                else
                {
                    await Dialog.Show("Failed");
                }
            }
            Service.ProgressBar.Hide();
        }
    }
}
