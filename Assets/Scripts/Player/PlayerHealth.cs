using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region  Variables

    [Header("Ints")]
    readonly int maxHealth = 3;
    public int health;
    
    [Header("Floats")]
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

    public void TakeDamage(int damage)
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
