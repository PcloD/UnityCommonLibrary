using UnityEngine;

namespace UnityCommonLibrary
{
	[ExecuteInEditMode]
	public class LookAtCamera : MonoBehaviour
	{
		[SerializeField]
		private Vector3 amount = Vector3.one;
		[SerializeField]
		private new Camera camera;
		[SerializeField]
		private bool invert;

		private void Update()
		{
			var cam = camera == null ? Camera.main : camera;
			transform.LookAt(cam.transform);
			if (invert)
			{
				transform.forward *= -1f;
			}
			transform.eulerAngles = Vector3.Scale(transform.eulerAngles, amount);
		}
	}
}
