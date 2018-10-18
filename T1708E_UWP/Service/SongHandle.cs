using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1708E_UWP.Entity;
using Windows.Media.Core;

namespace T1708E_UWP.Service
{
    public static class SongHandle
    {
        private static ObservableCollection<Song> _songs = new ObservableCollection<Song>();
        private static ObservableCollection<Song> _mysongs = new ObservableCollection<Song>();
        public async static Task<ObservableCollection<Song>> Get(bool isRequired)
        {
            if (_songs.Count != 0 && !isRequired) return _songs;
            try
            {
                _songs = JsonConvert.DeserializeObject<ObservableCollection<Song>>(await ApiHandle<string>.Call(APITypes.GetSongs, null));
                return _songs;
            }
            catch
            {
                ExceptionHandle.ThrowDebug("json error");
                return null;
            }
        }
        public async static Task<ObservableCollection<Song>> GetMine(bool isRequired)
        {
            if (_mysongs.Count != 0 && !isRequired) return _mysongs;
            try
            {
                _mysongs = JsonConvert.DeserializeObject<ObservableCollection<Song>>(await ApiHandle<string>.Call(APITypes.GetSavedSongs, null));
                return _mysongs;
            } catch
            {
                ExceptionHandle.ThrowDebug("json error");
                return null;
            }
        }
        
    }
}
