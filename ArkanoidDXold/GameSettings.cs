using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using Windows.System.Threading;
using ArkanoidDX.Levels;


namespace ArkanoidDX
{

    [DataContract]
    public class GameSettings
    {
        public Dictionary<string, WadScore> Unlocks
        {
            get {
                if (_unlocks == null)
                {
                    _result.Wait();
                    if (_result.Result != null)
                    {
                        _unlocks = _result.Result;
                    }
                    else
                    {
                        ResetToDefault();
                    }
                }
                return _unlocks;
            }
        }

        [DataMember]
        public Dictionary<string, WadScore> _unlocks;

        private Task<Dictionary<string, WadScore>> _result;

        public ArkanoidDX ArkanoidGame;

        public GameSettings(ArkanoidDX arkanoidGame)
        {
            ArkanoidGame = arkanoidGame;
            if (Utilities.DoesFileExistAsync(ApplicationData.Current.RoamingFolder, "Unlocks"))
            {
                _result = Load<Dictionary<string, WadScore>>(ApplicationData.Current.RoamingFolder, "Unlocks");
            }
            else
            {
                _unlocks = new Dictionary<string, WadScore>();
                Save();
            }           
        }

        public async void Save()
        {
            await Save<Dictionary<string, WadScore>>(ApplicationData.Current.RoamingFolder, "Unlocks", Unlocks);
        }

        public void ResetToDefault()
        {
            _unlocks = new Dictionary<string, WadScore>();
            Save();
        }

        

        public static async Task Save<T>(StorageFolder folder, string fileName, object instance)
        {
            var newFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var newFileStream = await newFile.OpenStreamForWriteAsync();
            var ser = new DataContractSerializer(typeof(T));
            ser.WriteObject(newFileStream, instance);
            newFileStream.Dispose();
        }
        public static async Task<T> Load<T>(StorageFolder folder, string fileName)
        {
            try
            {
                var newFile = await folder.GetFileAsync(fileName);
                var newFileStream = await newFile.OpenStreamForReadAsync();
                var ser = new DataContractSerializer(typeof(T));
                var b = (T)ser.ReadObject(newFileStream);
                newFileStream.Dispose();
                return b;
            }
            catch
            {
                return default(T);
            }
        }
    }
}