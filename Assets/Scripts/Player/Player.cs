using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Floats")]
    readonly float maxHealth = 3f;
    float health;
    readonly float damageCooldown = 1f;

    [Header("Bools")]
    public bool cooldown = false;

    void OnEnable()
    {
        BulletScript.OnDamage += HandleDamage;
    }

    void OnDisable()
    {
        BulletScript.OnDamage -= HandleDamage;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        print("Player has " + health + " health");
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

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

    void HandleDamage()
    {
        StopCoroutine(nameof(CooldownRoutine));
        cooldown = true;
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(damageCooldown);
        cooldown = false;
    }
}
