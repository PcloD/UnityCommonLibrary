namespace UnityCommonLibrary {
    [System.Serializable]
    public abstract class AbstractSaveData<T> where T : AbstractSaveData<T> {
        public static T data {
            get {
                return SaveSystem<T>.data;
            }
            set {
                SaveSystem<T>.data = value;
            }
        }

        public SaveBool writeable = new SaveBool(true);
        public SaveInt slot = new SaveInt(byte.MaxValue);
    }
}