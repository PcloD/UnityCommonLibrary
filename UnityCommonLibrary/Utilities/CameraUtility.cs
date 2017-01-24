using UnityEngine;

namespace UnityCommonLibrary.Utility
{
	public static class CameraUtility
	{
		public static Bounds OrthographicBounds(this Camera camera)
		{
			var camHeight = camera.orthographicSize * 2f;
			return new Bounds(camera.transform.position, new Vector3(camHeight * camera.aspect, camHeight, 0));
		}
	}
}