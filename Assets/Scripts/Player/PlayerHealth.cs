using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region  Variables

    [Header("Floats")]
    readonly float maxHealth = 3f;
    float health;
    readonly float damageCooldown = 1f;

    [Header("Bools")]
    public bool cooldown;

    #endregion

    #region Event Subscribtions
    
    void OnEnable()
    {
        BulletScript.OnDamage += HandleDamage;
    }

    void OnDisable()
    {
        BulletScript.OnDamage -= HandleDamage;
    }

    #endregion

    #region StartUpdate
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        print("Player has " + health + " health");

        cooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    #endregion

    #region Methods
    
    void CheckHealth()
    {
        if (health <= 0)
        {
            print("Player is dead");
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        print("Player has " + health + " health");
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(damageCooldown);
        cooldown = false;
    }

    #endregion

    #region Subribtion Handlers

    void HandleDamage()
    {
        StopCoroutine(nameof(CooldownRoutine));
        cooldown = true;
        StartCoroutine(CooldownRoutine());
    }

    #endregion
}
