using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ResourceNode : MonoBehaviour, IHIttable
{
    [SerializeField]
    private int health = 3;
    public int Health => health;

    AudioSource audioSource;

    public MaterialSO itemToSpawn;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetAudio();
    }

    private void SetAudio()
    {
        switch (itemToSpawn.resourceType)
        {
            case ResourceType.None:
                throw new Exception("Resource cant be of type None");
            case ResourceType.Wood:
                audioSource.clip = AudioLibrary.instance.woodHitClip;
                return;
            case ResourceType.Stone:
                audioSource.clip = AudioLibrary.instance.stoneHitClip;
                return;
            default:
                break;
        }
    }

    public void GetHit(WeaponItemSO weapon, Vector3 hitpoint)
    {
        int resourceCountToSpawn = 1;
        if (weapon.GetType().Equals(typeof(ToolSO)))
        {
            resourceCountToSpawn = ((ToolSO)weapon).GetResourceHarvested(itemToSpawn.resourceType);
        }
        ItemSpawnManager.instance.CreateItemInPlace(hitpoint, itemToSpawn, resourceCountToSpawn);

        audioSource.Play();

        health--;
        if (health <= 0)
        {
            StartCoroutine(DestroyObject(audioSource.clip.length));
        }
    }

    private IEnumerator DestroyObject(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
