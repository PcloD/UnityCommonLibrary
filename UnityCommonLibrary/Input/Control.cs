namespace UnityCommonLibrary.Input
{
    public abstract class Control {
        public string name { get; private set; }

        public Control(string name) {
            this.name = name;
        }

        public abstract void Reset();
        internal abstract void Update();
    }
}
