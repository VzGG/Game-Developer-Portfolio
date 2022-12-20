using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oswald.VFX
{
    public class ExplosionVFX : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip explosionSFX;
        // Start is called before the first frame update
        void Start()
        {
            audioSource.PlayOneShot(explosionSFX);
        }
    }
}