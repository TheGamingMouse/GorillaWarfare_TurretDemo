using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrackAndMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    [SerializeField] float baseSpeed = 2f;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float jumpTime = 2f;
    [SerializeField] float range = 10f;

    [Header("Bools")]
    [SerializeField] bool isGoingRight = true;
    public bool targetIsInArea;
    public bool targetInRange;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isJumping;
    [SerializeField] bool playerIsRight;

    [Header("GameObjects")]
    GameObject player;
    [SerializeField] GameObject targetTracker;
    
    [Header("Transforms")]
    [SerializeField] Transform target;

    [Header("LayerMasks")]
    [SerializeField] LayerMask playerMask;

    [Header("Components")]
    Rigidbody2D rb;

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        targetIsInArea = false;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        targetIsInArea = targetTracker.GetComponent<SlimeTrackTarget>().targetIsInArea;
        targetInRange = TargetInRange();

        moveSpeed = isGoingRight ? baseSpeed : -baseSpeed;
        
        if (!isJumping && isGrounded)
        {
            StartCoroutine(StartJump());
            isJumping = true;
        }

        if (!target)
        {
            FindTarget();
            return;
        }

        if (target)
        {
            CheckDirection();

            moveSpeed = playerIsRight ? baseSpeed : -baseSpeed;
        }

        if (!TargetInRange() || !targetIsInArea)
        {
            target = null;
        }
    }

    #endregion

    #region  Methods

    void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2) transform.position, 0f, playerMask);

        if ((hits.Length > 0) && TargetInRange() && targetIsInArea)
        {
            if (hits[0].transform.CompareTag("Player"))
            {
                target = player.transform;
            }
        }
        else
        {
            target = null;
        }
    }

    bool TargetInRange()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= range)
        {
            return true;
        }
        return false;
    }

    IEnumerator StartJump()
    {
        yield return new WaitForSeconds(jumpTime);

        Jump();
    }

    void Jump()
    {
        isJumping = false;

        rb.AddForce(new Vector2(moveSpeed, jumpHeight), ForceMode2D.Impulse);
    }

    void CheckDirection()
    {
        if ((player.transform.position.x - transform.position.x) < 0)
        {
            playerIsRight = false;
            baseSpeed = 2f;
        }
        else if ((player.transform.position.x - transform.position.x) > 0)
        {
            playerIsRight = true;
            baseSpeed = 2f;
        }
        else
        {
            baseSpeed = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            isGoingRight = !isGoingRight;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    #endregion

    #region Gizmos:

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);

        if (player)
        {
            Gizmos.DrawLine(transform.position, player.transform.position);
            Gizmos.color = Color.red;
        }

        if (target)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

    #endregion
}
