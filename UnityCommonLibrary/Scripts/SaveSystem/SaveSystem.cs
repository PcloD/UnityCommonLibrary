using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace UnityCommonLibrary {
    public static class SaveSystem {
        public static Encoding ENCODING = Encoding.UTF8;
        public static string SAVE_FOLDER = Application.persistentDataPath + "/SaveData";

        public static AbstractSaveData data;

        public static T LoadData<T>(int slot) where T : AbstractSaveData {
            try {
                var path = string.Format("{0}/{1}{2}", SAVE_FOLDER, "save", slot);
                AbstractSaveData data;
                var bf = new BinaryFormatter();
                using(var fs = File.OpenRead(path)) {
                    data = (AbstractSaveData)bf.Deserialize(fs);
                }
                return (T)data;
            }
            catch(Exception e) {
                throw e;
            }
        }

        public static void WriteData() {
            if(!data.writeable) {
                return;
            }
            var path = GetSavePath(data);
            var bf = new BinaryFormatter();
            Directory.CreateDirectory(SAVE_FOLDER);
            using(var fs = File.OpenWrite(path)) {
                bf.Serialize(fs, data);
            }
        }

        public static string GetSavePath(AbstractSaveData data) {
            return string.Format("{0}/{1}{2}", SAVE_FOLDER, "save", data.slot);
        }

    }
}