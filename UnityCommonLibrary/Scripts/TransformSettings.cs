using UnityEngine;

namespace UnityCommonLibrary
{
    public struct TransformSettings
    {
        public Vector3 LocalScale { get; }
        public Transform Parent { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public int SiblingIndex { get; }

        public TransformSettings(Transform transform)
        {
            Parent = transform.parent;
            SiblingIndex = transform.GetSiblingIndex();
            Position = transform.position;
            Rotation = transform.rotation;
            LocalScale = transform.localScale;
        }

        public void ApplyTo(Transform transform)
        {
            transform.parent = Parent;
            transform.SetSiblingIndex(SiblingIndex);
            transform.position = Position;
            transform.rotation = Rotation;
            transform.localScale = LocalScale;
        }
    }

    public struct RectTransformSettings
    {
        private TransformSettings _transformSettings;

        public Vector3 AnchoredPosition3D { get; }
        public Vector2 AnchorMax { get; }
        public Vector2 AnchorMin { get; }
        public Vector2 Pivot { get; }
        public Vector2 SizeDelta { get; }


        public RectTransformSettings(RectTransform rect)
        {
            AnchoredPosition3D = rect.anchoredPosition3D;
            AnchorMin = rect.anchorMin;
            AnchorMax = rect.anchorMax;
            SizeDelta = rect.sizeDelta;
            Pivot = rect.pivot;
            _transformSettings = new TransformSettings(rect);
        }

        public void ApplyAll(RectTransform rect)
        {
            _transformSettings.ApplyTo(rect);
            rect.anchorMin = AnchorMin;
            rect.anchorMax = AnchorMax;
            rect.pivot = Pivot;
            rect.anchoredPosition3D = AnchoredPosition3D;
            rect.sizeDelta = SizeDelta;
        }
    }
}