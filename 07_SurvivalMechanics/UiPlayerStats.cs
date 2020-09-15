using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayerStats : MonoBehaviour
{
    public Image healthBar, staminaBar;
    
    public void SetStamina(float value)
    {
        staminaBar.transform.localScale = new Vector3(Mathf.Clamp01(value), 1, 1);
    }

    public void SetHealth(float value)
    {
        healthBar.transform.localScale = new Vector3(Mathf.Clamp01(value), 1, 1);
    }
}
