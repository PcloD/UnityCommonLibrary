//
//		Path following particle system script
//		Set up 6 empty objects for the particles to follow
//     Copyright (c) Vincent DeLuca 2014.  All rights reserved.
//

using UnityEngine;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlePath : MonoBehaviour {

        //setting up your 6 targets
        public Transform target1;

        public Transform target2;
        public Transform target3;
        public Transform target4;
        public Transform target5;
        public Transform target6;

        ParticleSystem pSystem;

        void Awake() {
            pSystem = GetComponent<ParticleSystem>();
        }

        void Update() {
            Trail();
        }

        void Trail() {
            ParticleSystem.Particle[] p = new ParticleSystem.Particle[pSystem.particleCount + 1];
            int l = pSystem.GetParticles(p);

            var D1 = target1.position - transform.position;
            var D2 = target2.position - target1.position;
            var D3 = target3.position - target2.position;
            var D4 = target4.position - target3.position;
            var D5 = target5.position - target4.position;
            var D6 = target6.position - target5.position;

            int i = 0;
            while(i < l) {
                //setting the velocity of each particle from target to target
                if(p[i].lifetime < (p[i].startLifetime / 12)) {
                    p[i].velocity = 6f / p[i].startLifetime * D6;
                }
                else if(p[i].lifetime < ((3 * p[i].startLifetime) / 12)) {
                    var t = ((p[i].startLifetime / 6) - (p[i].lifetime - (p[i].startLifetime / 12))) / (p[i].startLifetime / 6);
                    p[i].velocity = 6f / p[i].startLifetime * Bezier(D5, D6, t);
                }
                else if(p[i].lifetime < ((5 * p[i].startLifetime) / 12)) {
                    var t = ((p[i].startLifetime / 6) - (p[i].lifetime - ((3 * p[i].startLifetime) / 12))) / (p[i].startLifetime / 6);
                    p[i].velocity = 6f / p[i].startLifetime * Bezier(D4, D5, t);
                }
                else if(p[i].lifetime < ((7 * p[i].startLifetime) / 12)) {
                    var t = ((p[i].startLifetime / 6) - (p[i].lifetime - ((5 * p[i].startLifetime) / 12))) / (p[i].startLifetime / 6);
                    p[i].velocity = 6f / p[i].startLifetime * Bezier(D3, D4, t);
                }
                else if(p[i].lifetime < ((9 * p[i].startLifetime) / 12)) {
                    var t = ((p[i].startLifetime / 6) - (p[i].lifetime - ((7 * p[i].startLifetime) / 12))) / (p[i].startLifetime / 6);
                    p[i].velocity = 6f / p[i].startLifetime * Bezier(D2, D3, t);
                }
                else if(p[i].lifetime < ((11 * p[i].startLifetime) / 12)) {
                    var t = ((p[i].startLifetime / 6) - (p[i].lifetime - ((9 * p[i].startLifetime) / 12))) / (p[i].startLifetime / 6);
                    p[i].velocity = 6f / p[i].startLifetime * Bezier(D1, D2, t);
                }
                else {
                    p[i].velocity = 6f / p[i].startLifetime * D1;
                }
                i++;
            }

            pSystem.SetParticles(p, l);
        }

        //this is the math to smooth out the path, known as bezier curves
        private Vector3 Bezier(Vector3 P0, Vector3 P2, float t) {
            var P1 = (P0 + P2) / 2f;
            return (1f - t) * ((1f - t) * P0 + t * P1) + t * ((1f - t) * P1 + t * P2);
        }
    }
}