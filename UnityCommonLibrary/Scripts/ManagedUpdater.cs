namespace UnityCommonLibrary
{
    using UnityEngine;

    /// <summary>
    /// An experimental class to see if doing all updates in managed land is faster
    /// http://blogs.unity3d.com/2015/12/23/1k-update-calls/
    /// </summary>
    public class ManagedUpdater : MonoSingleton<ManagedUpdater>
    {
        private IUpdateable[] updateables = new IUpdateable[1000];
        private int active;
        private int inactive;
        private int empty;

        private int nextEstimatedSlot = 0;
        private int highestFilledSlot = 0;

        private void Update()
        {
            active = 0;
            inactive = 0;
            empty = 0;
            int highestExisting = 0;
            int lastEmptySlot = -1;
            for (int i = 0; i <= highestFilledSlot; i++)
            {
                var updatable = updateables[i];
                // Do null check once
                var isNull = updatable == null;
                if (!isNull)
                {
                    highestExisting = i;
                    if (updatable.enabled)
                    {
                        active++;
                        updatable.ManagedUpdate();
                    }
                    else
                    {
                        inactive++;
                    }
                    if (lastEmptySlot > 0)
                    {
                        // Move this updatable backwards
                        updateables[lastEmptySlot] = updatable;
                        lastEmptySlot = i;
                        updateables[lastEmptySlot] = null;
                    }
                }
                else if (isNull)
                {
                    lastEmptySlot = i;
                    empty++;
                    if (i == highestFilledSlot)
                    {
                        highestFilledSlot = highestExisting;
                    }
                }
            }
        }
        public int AddUpdatable(IUpdateable updateable)
        {
            int index = -1;
            // First try to assign to known empty slot
            if (updateables[nextEstimatedSlot] == null)
            {
                updateables[nextEstimatedSlot] = updateable;
                index = nextEstimatedSlot;
                nextEstimatedSlot++;
            }
            else
            {
                // Otherwise check for first empty
                for (var i = 0; i < updateables.Length; i++)
                {
                    var u = updateables[i];
                    if (u != null)
                    {
                        updateables[i] = updateable;
                        index = i;
                        break;
                    }
                }
            }
            if (index == -1)
            {
                Debug.LogError("No slots available for updateable");
            }
            else
            {
                highestFilledSlot = Mathf.Max(highestFilledSlot, index);
            }
            return index;
        }
        public void RemoveUpdatable(IUpdateable updatable, int updaterIndex)
        {
            if (updaterIndex > updateables.Length || updaterIndex < 0)
            {
                Debug.LogError("updaterIndex out of bounds: " + updaterIndex);
                return;
            }
            // Eliminate multiple bounds checks
            var current = updateables[updaterIndex];
            if (current == null || current == updatable)
            {
                /*
				 * We actually want to keep the largest, not the smallest.
				 * If we kept the smallest we would need to search much farther
				 * next time we need to add an updateable.
				 */
                nextEstimatedSlot = Mathf.Max(updaterIndex, nextEstimatedSlot);
                updateables[updaterIndex] = null;
            }
            else
            {
                Debug.LogError("updatable not located at expected slot");
            }
        }
    }
}