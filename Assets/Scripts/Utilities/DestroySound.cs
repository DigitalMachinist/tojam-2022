using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class DestroySound : MonoBehaviour
    {
        public AudioSource Sound;

        void OnEnable()
        {
            Sound = GetComponent<AudioSource>();
            StartCoroutine(CoDelay());
        }

        IEnumerator CoDelay()
        {
            yield return new WaitUntil(() => !Sound.isPlaying);
            Destroy(gameObject);
        }
    }
}
