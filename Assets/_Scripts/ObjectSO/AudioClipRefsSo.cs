using UnityEngine;

namespace _Scripts.ObjectSO
{
    [CreateAssetMenu()]
    public class AudioClipRefsSo : ScriptableObject
    {
        public AudioClip click;
        public AudioClip fail;
        public AudioClip success;
        public AudioClip finished;
    }
}