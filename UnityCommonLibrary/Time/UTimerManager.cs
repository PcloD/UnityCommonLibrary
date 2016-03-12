using UnityCommonLibrary.Attributes;

namespace UnityCommonLibrary.Time {
	[AutoInstantiate]
	public sealed class UTimerManager : MonoSingleton<UTimerManager> {
		private void Update() {
			foreach(var t in UTimer.allReadonly) {
				t.Tick();
			}
		}
	}
}
