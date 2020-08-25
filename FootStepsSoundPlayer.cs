using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsSoundPlayer : MonoBehaviour
{
    public AudioSource footStepAudioSource;
    public AudioClip footStepclip;
    float lastTime = 0;
    float duration;

    private void Start()
    {
        duration = footStepclip.length;
    }

    public void PlayFootstepSound()
    {
        if (lastTime == 0)
        {
            footStepAudioSource.PlayOneShot(footStepclip);
        }
        if(Time.time - lastTime >= duration)
        {
            lastTime = Time.time;
            footStepAudioSource.PlayOneShot(footStepclip);
        }
    }
}
