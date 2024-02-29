using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public static event Action OnDamage;

    [Header("Floats")]
    readonly float damage = 1;

    [Header("Bools")]
    bool cooldown;

    [Header("Components")]
    Rigidbody2D rb;

    void Awake()
    {
        StartCoroutine(DespawnTimer());
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = GameObject.FindWithTag("PlayerObject").GetComponent<Player>().cooldown;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.TryGetComponent<Player>(out Player playerComp) && !cooldown)
        {
            playerComp.TakeDamage(damage);
            OnDamage?.Invoke();
        }
        Destroy(gameObject);
    }

    IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
