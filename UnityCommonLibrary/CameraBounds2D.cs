using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary
{
    [ExecuteInEditMode]
    public class CameraBounds2D : MonoBehaviour
    {
        public bool BoundedXMin = true,
            BoundedXMax = true,
            BoundedYMin = true,
            BoundedYMax = true;

        public BoxCollider2D Bounds;
        public Camera Camera;

        [SerializeField]
        private bool _runInEditor = true;

        public bool CanFit { get; private set; }

        private void LateUpdate()
        {
            if (Application.isPlaying == false && !_runInEditor)
            {
                return;
            }
            if (Bounds == null || Camera == null)
            {
                return;
            }
            var camRect = Camera.OrthographicBounds();
            var lvlBoundsRect = Bounds.bounds;

            CanFit = lvlBoundsRect.CouldContain(camRect);

            if (!CanFit || !BoundedXMin && !BoundedXMax && !BoundedYMin && !BoundedYMax)
            {
                return;
            }

            var cbl = (Vector2) camRect.min;
            var cbr = (Vector2) camRect.max;
            var lbl = (Vector2) lvlBoundsRect.min;
            var lbr = (Vector2) lvlBoundsRect.max;

            //Check and correct X differences
            if (BoundedXMin || BoundedXMax)
            {
                var lDiff = cbl.x - lbl.x;
                var rDiff = cbr.x - lbr.x;
                if (lDiff < 0f && BoundedXMin)
                {
                    OffsetXPosition(lDiff);
                }
                else if (rDiff > 0f && BoundedXMax)
                {
                    OffsetXPosition(rDiff);
                }
            }
            //Check and correct Y differences
            if (BoundedYMin || BoundedYMax)
            {
                var bDiff = cbl.y - lbl.y;
                var tDiff = cbr.y - lbr.y;
                if (bDiff < 0f && BoundedYMin)
                {
                    OffsetYPosition(bDiff);
                }
                else if (tDiff > 0f && BoundedYMax)
                {
                    OffsetYPosition(tDiff);
                }
            }
        }

        private void OffsetXPosition(float offset)
        {
            var position = transform.position;
            position.x -= offset;
            transform.position = position;
        }

        private void OffsetYPosition(float offset)
        {
            var position = transform.position;
            position.y -= offset;
            transform.position = position;
        }

        private void OnDrawGizmosSelected()
        {
            if (Bounds != null)
            {
                GizmosUtility.StoreColor(Color.green);
                GizmosUtility.DrawBounds(Bounds.bounds);
                GizmosUtility.RestoreColor();
            }
        }
    }
}