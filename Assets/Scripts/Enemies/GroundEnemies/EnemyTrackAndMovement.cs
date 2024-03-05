using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTrackAndMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float range = 10f;

    [Header("Bools")]
    [SerializeField] bool isGoingRight = true;
    public bool targetIsInArea;
    public bool targetInRange;

    [Header("GameObjects")]
    GameObject player;
    [SerializeField] GameObject targetTracker;
    
    [Header("Transforms")]
    Transform target;

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
    }

    void FixedUpdate()
    {
        if (target)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            direction.y = 0f;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            Vector3 directionTranslation = isGoingRight ? transform.right : -transform.right;
            directionTranslation *= Time.deltaTime * moveSpeed;

            transform.Translate(directionTranslation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetIsInArea = targetTracker.GetComponent<TrackGroundTarget>().targetIsInArea;
        targetInRange = TargetInRange();

        if (!target)
        {
            FindTarget();
            return;
        }

        if (!TargetInRange() || !targetIsInArea)
        {
            target = null;
        }
    }

    #endregion

    #region  Methods

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            isGoingRight = !isGoingRight;
        }
    }

    #endregion

    #region FindTargetMethods

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
