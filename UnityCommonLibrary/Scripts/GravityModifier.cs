using UnityEngine;

namespace UnityCommonLibrary
{
	[RequireComponent(typeof(Rigidbody))]
	public class GravityModifier : MonoBehaviour
	{
		[SerializeField]
		private float multiplier = 0f;

		private ConstantForce force;
		private Rigidbody rb;

		private void Awake()
		{
			force = gameObject.AddComponent<ConstantForce>();
			rb = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		private void Update()
		{
			force.force = (-Physics.gravity * rb.mass) * multiplier;
		}
	}
}