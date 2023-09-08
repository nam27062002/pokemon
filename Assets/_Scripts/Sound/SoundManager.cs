using System;
using _Scripts.ObjectSO;
using UnityEngine;

namespace _Scripts.Sound
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;
        public static SoundManager Instance => instance;

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        [SerializeField] AudioClipRefsSo  audioClipRefsSo;
        
        public void PlaySoundClick()
        {
            PlaySound(audioClipRefsSo.click,Vector3.zero);
        }

        public void PlaySoundSuccess()
        {
            PlaySound(audioClipRefsSo.success,Vector3.zero);
        }

        public void PlaySoundFail()
        {
            PlaySound(audioClipRefsSo.fail,Vector3.zero);
        }
        
        private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip,position,volume);
        }
    }
    
}
