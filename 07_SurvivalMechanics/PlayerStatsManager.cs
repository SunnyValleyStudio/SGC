using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatsManager : MonoBehaviour
{
	[SerializeField]
	public UnityEvent OnDeath;

    [SerializeField]
    private int healthInitialValue;
    [SerializeField]
    private int staminaInitialValue;

	private float stamin;

	public float Stamina
	{
		get { return stamin; }
		set {
			stamin = Mathf.Clamp(value, 0, staminaInitialValue);
			uiPlayerStats.SetStamina(stamin / staminaInitialValue);
		}
	}

	private float health;

	public float Health
	{
		get { return health; }
		set { 
			health = Mathf.Clamp(value, 0, healthInitialValue);
			uiPlayerStats.SetHealth(health / healthInitialValue);
			if (health == 0)
			{
				Debug.Log("Dead");
				OnDeath.Invoke();
			}
		}
	}

	public UiPlayerStats uiPlayerStats;

	private void Awake()
	{
		Health = healthInitialValue;
		Stamina = staminaInitialValue;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Health -= 40;
			Stamina -= 10;
		}
	}

}
