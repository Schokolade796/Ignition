using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EW
{ 
    public class Clock : MonoBehaviour
    {
        public AudioSource audioSource;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void StopAudio()
        {
            audioSource.Stop();
        }
    }

}