﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary.Utility
{
    public static class TransformUtility
    {
        private static readonly Queue<Transform> _bfsSearchQueue = new Queue<Transform>();

        public static void SetPosition(this Transform transform, Space space,
            float? x = null, float? y = null, float? z = null)
        {
            var newV3 = transform.position;
            VectorUtility.SetXyz(ref newV3, x, y, z);
            if (space == Space.World)
            {
                transform.position = newV3;
            }
            else
            {
                transform.localPosition = newV3;
            }
        }

        public static void SetLocalScale(this Transform transform, float? x = null,
            float? y = null, float? z = null)
        {
            var newV3 = transform.localScale;
            VectorUtility.SetXyz(ref newV3, x, y, z);
            transform.localScale = newV3;
        }

        public static void SetEulerAngles(this Transform transform, Space space,
            float? x = null, float? y = null, float? z = null)
        {
            var newV3 = transform.eulerAngles;
            VectorUtility.SetXyz(ref newV3, x, y, z);
            if (space == Space.World)
            {
                transform.rotation = Quaternion.Euler(newV3);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(newV3);
            }
        }

        public static void Reset(this Transform t)
        {
            Reset(t, TransformElement.All, Space.World);
        }

        public static void Reset(this Transform t, TransformElement elements)
        {
            Reset(t, elements, Space.World);
        }

        public static void Reset(this Transform t, Space space)
        {
            Reset(t, TransformElement.All, space);
        }

        public static void Reset(this Transform t, TransformElement elements, Space space)
        {
            if ((elements & TransformElement.Position) != 0)
            {
                if (space == Space.World)
                {
                    t.position = Vector3.zero;
                }
                else
                {
                    t.localPosition = Vector3.zero;
                }
            }
            if ((elements & TransformElement.Rotation) != 0)
            {
                if (space == Space.World)
                {
                    t.rotation = Quaternion.identity;
                }
                else
                {
                    t.localRotation = Quaternion.identity;
                }
            }
            if ((elements & TransformElement.Scale) != 0)
            {
                t.localScale = Vector3.one;
            }
        }

        public static void Match(this Transform t, Transform other,
            TransformElement elements)
        {
            if ((elements & TransformElement.Position) != 0)
            {
                t.position = other.position;
            }
            if ((elements & TransformElement.Rotation) != 0)
            {
                t.rotation = other.rotation;
            }
            if ((elements & TransformElement.Scale) != 0)
            {
                t.localScale = other.localScale;
            }
        }

        public static Vector3 DirectionTo(this Transform t, Transform other)
        {
            return (other.position - t.position).normalized;
        }

        public static Vector3 DirectionTo(this Transform t, Vector3 other)
        {
            return (other - t.position).normalized;
        }

        public static Vector3 DirectionTo(this Vector3 v, Vector3 other)
        {
            return (other - v).normalized;
        }

        public static Vector3 DirectionTo(this Vector3 v, Transform other)
        {
            return (other.position - v).normalized;
        }

        public static Transform FindChildBfs(this Transform t, string search,
            StringComparison comparison = StringComparison.CurrentCulture,
            bool tag = false)
        {
            _bfsSearchQueue.Clear();
            for (var i = 0; i < t.childCount; i++)
            {
                _bfsSearchQueue.Enqueue(t.GetChild(i));
            }
            Transform found = null;
            while (_bfsSearchQueue.Count > 0)
            {
                var child = _bfsSearchQueue.Dequeue();
                if (tag && child.tag.Equals(search, comparison) ||
                    child.name.Equals(search, comparison))
                {
                    found = child;
                    break;
                }
                for (var i = 0; i < child.childCount; i++)
                {
                    _bfsSearchQueue.Enqueue(child.GetChild(i));
                }
            }
            _bfsSearchQueue.Clear();
            return found;
        }

        public static Transform FindChildDfs(this Transform t, string search,
            StringComparison comparison = StringComparison.CurrentCulture,
            bool tag = false)
        {
            for (var i = 0; i < t.childCount; i++)
            {
                var child = t.GetChild(i);
                if (tag && child.tag.Equals(search, comparison) ||
                    child.name.Equals(search, comparison))
                {
                    return child;
                }
                child = FindChildDfs(t.GetChild(i), search);
                if (child)
                {
                    return child;
                }
            }
            return null;
        }

        public static Transform FindParent(this Transform t, string search,
            StringComparison comparison = StringComparison.CurrentCulture,
            bool tag = false)
        {
            var transform = t;
            while (transform)
            {
                if (tag && transform.tag.Equals(search, comparison) ||
                    transform.name.Equals(search, comparison))
                {
                    return transform;
                }
                transform = transform.parent;
            }
            return null;
        }

        public static Transform[] GetChildren(this Transform transform)
        {
            var array = new Transform[transform.childCount];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = transform.GetChild(i);
            }
            return array;
        }
    }
}