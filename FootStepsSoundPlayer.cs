using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsSoundPlayer : MonoBehaviour
{
    public AudioSource footStepAudioSource;
    public AudioClip footStepclip, waterStepClip;
    private AudioClip currentStepSound;
    float lastTime = 0;
    float duration;
    public string waterTag;

    private void Start()
    {
        duration = footStepclip.length;
        currentStepSound = footStepclip;
    }

    public void PlayFootstepSound()
    {
        if (lastTime == 0)
        {
            footStepAudioSource.PlayOneShot(currentStepSound);
        }
        if(Time.time - lastTime >= duration)
        {
            lastTime = Time.time;
            footStepAudioSource.PlayOneShot(currentStepSound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            currentStepSound = waterStepClip;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            currentStepSound = footStepclip;
        }
    }
}
