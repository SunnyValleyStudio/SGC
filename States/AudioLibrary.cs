using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    public static AudioLibrary instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public AudioClip woodHitClip, stoneHitClip;

    public AudioClip weaponWoosh;

}
