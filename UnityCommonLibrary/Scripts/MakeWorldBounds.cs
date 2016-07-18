using UnityEngine;

namespace UnityCommonLibrary
{
    public class MakeWorldBounds : MonoBehaviour
    {
        [SerializeField]
        private float thickness = 0.3f;

        private void Awake()
        {
            // Create frictionless physmaterial for walls
            var material = new PhysicMaterial("Frictionless");
            material.staticFriction = 0f;
            material.dynamicFriction = 0f;
            material.frictionCombine = PhysicMaterialCombine.Minimum;
            material.bounceCombine = PhysicMaterialCombine.Minimum;
            material.bounciness = 0f;

            var surfaces = new BoxCollider[6];
            for (var i = 0; i < 6; i++)
            {
                var obj = new GameObject();
                obj.transform.SetParent(transform, false);
                var collider = obj.AddComponent<BoxCollider>();
                collider.sharedMaterial = material;
                surfaces[i] = collider;
            }
            var size = thickness / transform.localScale.magnitude;
            var offset = 0.5f + size / 2f;
            // Floor
            surfaces[0].size = new Vector3(1f, size, 1f);
            surfaces[0].transform.localPosition = new Vector3(0f, -offset, 0f);
            surfaces[0].name = "Floor";

            // Ceiling
            surfaces[1].size = new Vector3(1f, size, 1f);
            surfaces[1].transform.localPosition = new Vector3(0f, offset, 0f);
            surfaces[1].name = "Ceiling";

            // Wall Z-
            surfaces[2].size = new Vector3(1f, 1f, size);
            surfaces[2].transform.localPosition = new Vector3(0f, 0f, -offset);
            surfaces[2].name = "Wall Z-";

            // Wall Z+
            surfaces[3].size = new Vector3(1f, 1f, size);
            surfaces[3].transform.localPosition = new Vector3(0f, 0f, offset);
            surfaces[3].name = "Wall Z+";

            // Wall X-
            surfaces[4].size = new Vector3(size, 1f, 1f);
            surfaces[4].transform.localPosition = new Vector3(-offset, 0f, 0f);
            surfaces[4].name = "Wall X-";

            // Wall X+
            surfaces[5].size = new Vector3(size, 1f, 1f);
            surfaces[5].transform.localPosition = new Vector3(offset, 0f, 0f);
            surfaces[5].name = "Wall X+";
        }

        public void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                Gizmos.DrawWireCube(transform.position, transform.localScale);
            }
        }
    }
}