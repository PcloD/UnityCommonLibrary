using System;
using UnityEngine;
using SysMath = System.Math;

namespace UnityCommonLibrary {
    public class SnapCameraBounds : UCScript {
        public float interval = 0.5f;
        Camera cam;

        void Awake() {
            cam = GetComponent<Camera>();
        }

        void LateUpdate() {
            var fixedInterval = 1 / interval;
            var camBottomLeft = cam.rect.min;
            var cblX = SysMath.Round(camBottomLeft.x * interval, MidpointRounding.AwayFromZero) / interval;
            var cblY = SysMath.Round(camBottomLeft.y * interval, MidpointRounding.AwayFromZero) / interval;
            var isAlignedX = cblX == camBottomLeft.x;
            var isAlignedY = cblY == camBottomLeft.y;
            if(!isAlignedX) {
                var position = transform.position;
                position.x -= camBottomLeft.x;
                transform.position = position;
            }
            if(!isAlignedY) {
                var position = transform.position;
                position.y -= camBottomLeft.y;
                transform.position = position;
            }
        }
    }
}
