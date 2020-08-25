using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHIttable
{
    int Health { get; }
    void GetHit(WeaponItemSO weapon, Vector3 hitpoint);
}
