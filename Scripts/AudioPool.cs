using UnityEngine;
using UnityEngine.Audio;

namespace UnityCommonLibrary {
    public sealed class AudioPool : BehaviourPool<AudioSource> {
        public AudioMixerGroup group { get; private set; }

        public AudioPool(AudioMixerGroup group, int initialCount) : base(initialCount) {
            this.group = group;
        }

        protected override void PostGetItem(ref AudioSource item) {
            item.outputAudioMixerGroup = group;
        }

        protected override bool PreReturnItem(ref AudioSource item) {
            base.PreReturnItem(ref item);
            item.Stop();
            item.clip = null;
            item.outputAudioMixerGroup = group;
            return true;
        }
    }
}