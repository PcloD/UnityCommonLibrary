using UnityEngine;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public class CameraBounds2D : UCScript {
        public BoxCollider2D bounds;
        public new Camera camera;
        public bool boundedXMin = true,
                    boundedXMax = true,
                    boundedYMin = true,
                    boundedYMax = true;

        [SerializeField]
        bool runInEditor = true;

        public bool canFit { get; private set; }

        // Update is called once per frame
        void LateUpdate() {
            if(Application.isPlaying == false && !runInEditor) {
                return;
            }
            if(bounds == null || camera == null) {
                return;
            }
            var camRect = camera.OrthographicBounds();
            var lvlBoundsRect = bounds.bounds;

            canFit = lvlBoundsRect.CouldContain(camRect);

            if(!canFit || (!boundedXMin && !boundedXMax && !boundedYMin && !boundedYMax)) {
                return;
            }

            var cbl = (Vector2)camRect.min;
            var cbr = (Vector2)camRect.max;
            var lbl = (Vector2)lvlBoundsRect.min;
            var lbr = (Vector2)lvlBoundsRect.max;

            //Check and correct X differences
            if(boundedXMin || boundedXMax) {
                var lDiff = cbl.x - lbl.x;
                var rDiff = cbr.x - lbr.x;
                if(lDiff < 0f && boundedXMin) {
                    OffsetXPosition(lDiff);
                }
                else if(rDiff > 0f && boundedXMax) {
                    OffsetXPosition(rDiff);
                }
            }
            //Check and correct Y differences
            if(boundedYMin || boundedYMax) {
                var bDiff = cbl.y - lbl.y;
                var tDiff = cbr.y - lbr.y;
                if(bDiff < 0f && boundedYMin) {
                    OffsetYPosition(bDiff);
                }
                else if(tDiff > 0f && boundedYMax) {
                    OffsetYPosition(tDiff);
                }
            }
        }

        void OffsetXPosition(float offset) {
            var position = transform.position;
            position.x -= offset;
            transform.position = position;
        }

        void OffsetYPosition(float offset) {
            var position = transform.position;
            position.y -= offset;
            transform.position = position;
        }

        void OnDrawGizmosSelected() {
            if(bounds != null) {
                GizmosUtility.StoreColor(Color.green);
                GizmosUtility.DrawBounds(bounds.bounds);
                GizmosUtility.RestoreColor();
            }
        }

    }
}