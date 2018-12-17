using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FaceAuth.ViewModel
{
    static class StorageFileProvider
    {

        private const string CachedCaptureKey = "InSessionCapture";


        public static async Task<Byte[]> SaveToFileAsync(StorageFile file)
        {
            Byte[] bytes = null;

            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }

            return bytes;
        }

        
        public static async void CacheCaptureAsync(StorageFile file)
        {
            //Create dataFile.txt in LocalFolder and write “My text” to it 
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            await file.CopyAsync(ApplicationData.Current.LocalFolder, CachedCaptureKey, NameCollisionOption.ReplaceExisting);
        }

        public static async void PurgeLocalCacheAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            await localFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }

        public static async Task<StorageFile> GetCachedFileAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            return await localFolder.GetFileAsync(CachedCaptureKey);
        }
    }
}
