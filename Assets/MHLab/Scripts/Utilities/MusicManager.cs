using UnityEngine;

namespace MHLab.Utilities
{
    public class MusicManager : MonoBehaviour
    {
        public AudioClip[] Musics;

        private AudioSource _audioSource;

        protected void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.clip = Musics[UnityEngine.Random.Range(0, Musics.Length)];

            _audioSource.Play();
        }
    }
}
