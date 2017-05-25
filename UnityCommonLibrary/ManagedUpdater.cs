using UnityEngine;

namespace UnityCommonLibrary
{
    /// <summary>
    ///     An experimental class to see if doing all updates in managed land is faster
    ///     http://blogs.unity3d.com/2015/12/23/1k-update-calls/
    /// </summary>
    public class ManagedUpdater : MonoSingleton<ManagedUpdater>
    {
        private readonly IUpdateable[] _updateables = new IUpdateable[1000];
        private int _active;
        private int _empty;
        private int _highestFilledSlot;
        private int _inactive;

        private int _nextEstimatedSlot;

        public int AddUpdatable(IUpdateable updateable)
        {
            var index = -1;
            // First try to assign to known empty slot
            if (_updateables[_nextEstimatedSlot] == null)
            {
                _updateables[_nextEstimatedSlot] = updateable;
                index = _nextEstimatedSlot;
                _nextEstimatedSlot++;
            }
            else
            {
                // Otherwise check for first empty
                for (var i = 0; i < _updateables.Length; i++)
                {
                    var u = _updateables[i];
                    if (u != null)
                    {
                        _updateables[i] = updateable;
                        index = i;
                        break;
                    }
                }
            }
            if (index == -1)
            {
                UCLCore.Logger.LogError("", "No slots available for updateable");
            }
            else
            {
                _highestFilledSlot = Mathf.Max(_highestFilledSlot, index);
            }
            return index;
        }

        public void RemoveUpdatable(IUpdateable updatable, int updaterIndex)
        {
            if (updaterIndex > _updateables.Length || updaterIndex < 0)
            {
                UCLCore.Logger.LogError("", "updaterIndex out of bounds: " + updaterIndex);
                return;
            }
            // Eliminate multiple bounds checks
            var current = _updateables[updaterIndex];
            if (current == null || current == updatable)
            {
                /*
                 * We actually want to keep the largest, not the smallest.
                 * If we kept the smallest we would need to search much farther
                 * next time we need to add an updateable.
                 */
                _nextEstimatedSlot = Mathf.Max(updaterIndex, _nextEstimatedSlot);
                _updateables[updaterIndex] = null;
            }
            else
            {
                UCLCore.Logger.LogError("", "updatable not located at expected slot");
            }
        }

        private void Update()
        {
            _active = 0;
            _inactive = 0;
            _empty = 0;
            var highestExisting = 0;
            var lastEmptySlot = -1;
            for (var i = 0; i <= _highestFilledSlot; i++)
            {
                var updatable = _updateables[i];
                // Do null check once
                var isNull = updatable == null;
                if (!isNull)
                {
                    highestExisting = i;
                    if (updatable.Enabled)
                    {
                        _active++;
                        updatable.ManagedUpdate();
                    }
                    else
                    {
                        _inactive++;
                    }
                    if (lastEmptySlot > 0)
                    {
                        // Move this updatable backwards
                        _updateables[lastEmptySlot] = updatable;
                        lastEmptySlot = i;
                        _updateables[lastEmptySlot] = null;
                    }
                }
                else
                {
                    lastEmptySlot = i;
                    _empty++;
                    if (i == _highestFilledSlot)
                    {
                        _highestFilledSlot = highestExisting;
                    }
                }
            }
        }
    }
}