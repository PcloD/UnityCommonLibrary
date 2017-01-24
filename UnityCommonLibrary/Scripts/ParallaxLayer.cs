using UnityEngine;

namespace UnityCommonLibrary
{
	[ExecuteInEditMode]
	public class ParallaxLayer : MonoBehaviour
	{
		public new ParallaxCamera camera;
		public float speedX;
		public float speedY;
		public bool reverseDirection;

		private Vector3 prevCamPosition;
		private bool prevMoveParallax;

		private void OnEnable()
		{
			if(camera == null)
			{
				camera = FindObjectOfType<ParallaxCamera>();
			}
			if(camera != null)
			{
				prevCamPosition = camera.transform.position;
			}
		}

		private void Update()
		{
			if(!camera)
			{
				return;
			}
			if(camera.moveParallax && !prevMoveParallax)
			{
				prevCamPosition = camera.transform.position;
			}
			prevMoveParallax = camera.moveParallax;
			if(!Application.isPlaying && !camera.moveParallax)
			{
				return;
			}
			var dist = camera.transform.position - prevCamPosition;
			var dir = (reverseDirection) ? -1f : 1f;
			transform.position += Vector3.Scale(dist, new Vector3(speedX, speedY)) * dir;

			prevCamPosition = camera.transform.position;
		}
	}
}