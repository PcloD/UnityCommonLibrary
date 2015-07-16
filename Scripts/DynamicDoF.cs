#if IMAGE_EFFECTS
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(DepthOfField))]
    [ExecuteInEditMode]
    public class DynamicDoF : UCScript {
        public float AdaptationSpeed = 10.0f;
        public float MaxDistance = 40.0f;

        DepthOfField dof;
        float distance;
        float targetDistance;

        void Start() {
            dof = GetComponent<DepthOfField>();
        }

        void Update() {
            if(!dof.enabled) {
                return;
            }

            //Find focal distance
            targetDistance = -1f;
            var hits = Physics.RaycastAll(transform.position, transform.forward);
            foreach(var s in hits) {
                if(s.collider.isTrigger) //ignore all triggers
                    continue;
                if(s.collider.name == "Sight Block") //technically not a trigger, just a collider
                    continue;

                targetDistance = Mathf.Min(s.distance + 0.5f, MaxDistance);
                break;
            }
            if(targetDistance < 0f) {
                targetDistance = MaxDistance;
            }

            //Move focal point
            float d = targetDistance - distance;
            float step = Mathf.Min(AdaptationSpeed * Time.deltaTime, Mathf.Abs(d));
            distance += Mathf.Sign(d) * step;

            //do JS update
            dof.focalLength = distance;
        }
    }
}
#endif