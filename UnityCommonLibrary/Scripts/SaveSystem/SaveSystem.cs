using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace UnityCommonLibrary {
    public static class SaveSystem<T> where T : AbstractSaveData<T> {
        public static Encoding ENCODING = Encoding.UTF8;
        public static string SAVE_FOLDER = Application.persistentDataPath + "/SaveData";
        public static string SAVE_PREFIX = "save";

        public static T data;

        public static T LoadData(int slot) {
            var path = string.Format("{0}/{1}{2}", SAVE_FOLDER, SAVE_PREFIX, slot);
            T data;
            var bf = new BinaryFormatter();
            using(var fs = File.OpenRead(path)) {
                data = (T)bf.Deserialize(fs);
            }
            return data;
        }

        public static void WriteData() {
            if(!data.writeable.value) {
                return;
            }
            var path = GetSavePath(data);
            var bf = new BinaryFormatter();
            Directory.CreateDirectory(SAVE_FOLDER);
            using(var fs = File.OpenWrite(path)) {
                bf.Serialize(fs, data);
            }
        }

        public static string GetSavePath(T data) {
            return string.Format("{0}/{1}{2}", SAVE_FOLDER, SAVE_PREFIX, data.slot);
        }

    }
}