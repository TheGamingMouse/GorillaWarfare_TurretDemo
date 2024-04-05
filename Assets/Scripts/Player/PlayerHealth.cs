using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    #region Events

    public static event Action OnPlayerDeath;

    #endregion
    
    #region  Variables

    [Header("Ints")]
    readonly int maxHealth = 3;
    public int health;
    
    [Header("Floats")]
    readonly float damageCooldown = 1f;

    [Header("Bools")]
    public bool cooldown;
    [SerializeField] bool invinsible = false;

    #endregion

    #region Event Subscribtions
    
    void OnEnable()
    {
        BulletScript.OnDamage += HandleDamage;
        EnemyDamage.OnDamage += HandleDamage;
        
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
        // print("Player has " + health + " health");

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
            // print("Player is dead");
            
            Time.timeScale = 0;
            OnPlayerDeath?.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!invinsible)
        {
            health -= damage;
        }
        // print("Player has " + health + " health");
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
