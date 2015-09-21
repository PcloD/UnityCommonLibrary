using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary {
    [Serializable]
    public abstract class SaveField<T, S> where S : SaveField<T, S> {
        protected static EqualityComparer<T> comparer = EqualityComparer<T>.Default;

        protected T _value;
        public virtual T value {
            get {
                return _value;
            }
            set {
                var changed = !comparer.Equals(_value, value);
                if(changed && !AllowChange(value)) {
                    return;
                }
                _value = value;
                if(changed && Application.isPlaying) {
                    OnValueChanged();
                }
            }
        }

        public SaveField() {
            this._value = default(T);
        }

        public SaveField(T initialVal) {
            this._value = initialVal;
        }

        protected virtual bool AllowChange(T newValue) { return true; }

        protected abstract void OnValueChanged();

        public static implicit operator T(SaveField<T, S> field) {
            return field.value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    [Serializable]
    public class SaveBool : SaveField<bool, SaveBool> {
        public SaveBool() { }
        public SaveBool(bool initVal) : base(initVal) { }

        protected override void OnValueChanged() {
            SaveBoolChanged.Notify(this, this);
        }
    }

    [Serializable]
    public class SaveInt : SaveField<int, SaveInt> {
        public int minVal, maxVal;
        public bool isClamped;

        public SaveInt() { }
        public SaveInt(int initVal) : base(initVal) { }
        public SaveInt(int minVal, int maxVal) {
            this.minVal = minVal;
            this.maxVal = maxVal;
        }
        public SaveInt(int initVal, int minVal, int maxVal) : base(initVal) {
            this.minVal = minVal;
            this.maxVal = maxVal;
            isClamped = true;
            ClampVal();
        }

        void ClampVal() {
            if(isClamped) {
                _value = Mathf.Clamp(_value, minVal, maxVal);
            }
        }

        protected override void OnValueChanged() {
            ClampVal();
            SaveIntChanged.Notify(this, this);
        }
    }

    [Serializable]
    public class SaveByte : SaveField<byte, SaveByte> {
        public byte minVal, maxVal;
        public bool isClamped;

        public SaveByte() { }
        public SaveByte(byte initVal) : base(initVal) { }
        public SaveByte(byte minVal, byte maxVal) {
            this.minVal = minVal;
            this.maxVal = maxVal;
        }
        public SaveByte(byte initVal, byte minVal, byte maxVal) : base(initVal) {
            this.minVal = minVal;
            this.maxVal = maxVal;
            isClamped = true;
            ClampVal();
        }

        void ClampVal() {
            if(isClamped) {
                _value = Math.ClampByte(_value, minVal, maxVal);
            }
        }

        protected override void OnValueChanged() {
            ClampVal();
            SaveByteChanged.Notify(this, this);
        }
    }

    [Serializable]
    public class SaveSByte : SaveField<sbyte, SaveSByte> {
        public sbyte minVal, maxVal;
        public bool isClamped;

        public SaveSByte() { }
        public SaveSByte(sbyte initVal) : base(initVal) { }
        public SaveSByte(sbyte minVal, sbyte maxVal) {
            this.minVal = minVal;
            this.maxVal = maxVal;
        }
        public SaveSByte(sbyte initVal, sbyte minVal, sbyte maxVal) : base(initVal) {
            this.minVal = minVal;
            this.maxVal = maxVal;
            isClamped = true;
            ClampVal();
        }

        void ClampVal() {
            if(isClamped) {
                _value = Math.ClampSByte(_value, minVal, maxVal);
            }
        }

        protected override void OnValueChanged() {
            ClampVal();
            SaveSByteChanged.Notify(this, this);
        }
    }

    [Serializable]
    public class SaveFloat : SaveField<float, SaveFloat> {
        public float minVal, maxVal;
        public bool isClamped;

        public SaveFloat() { }
        public SaveFloat(float initVal) : base(initVal) { }
        public SaveFloat(float minVal, float maxVal) {
            this.minVal = minVal;
            this.maxVal = maxVal;
        }
        public SaveFloat(float initVal, float minVal, float maxVal) : base(initVal) {
            this.minVal = minVal;
            this.maxVal = maxVal;
            isClamped = true;
            ClampValue();
        }

        void ClampValue() {
            if(isClamped) {
                _value = Mathf.Clamp(_value, minVal, maxVal);
            }
        }

        protected override void OnValueChanged() {
            if(isClamped) {
                _value = Mathf.Clamp(_value, minVal, maxVal);
            }
            SaveFloatChanged.Notify(this, this);
        }
    }

    [Serializable]
    public class SaveString : SaveField<string, SaveString> {
        public int maxLength;
        public bool isClamped;

        public SaveString() {
            _value = string.Empty;
        }
        public SaveString(string initVal) : base(initVal) { }
        public SaveString(string initVal, int maxLength) : base(initVal) {
            this.maxLength = maxLength;
            isClamped = true;
            ClampValue();
        }

        void ClampValue() {
            if(isClamped) {
                _value = _value.Substring(0, maxLength);
            }
        }

        protected override void OnValueChanged() {
            ClampValue();
            SaveStringChanged.Notify(this, this);
        }

        protected override bool AllowChange(string newValue) {
            return newValue != null;
        }

    }
}