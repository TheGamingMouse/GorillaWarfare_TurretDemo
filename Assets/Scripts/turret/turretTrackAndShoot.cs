using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretTrackAndShoot : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float rotSpeed = 150f;
    readonly float range = 7f;
    readonly float firerate = 0.7f;
    readonly float cooldown = 2f;

    [Header("Bools")]
    [SerializeField] bool shooting;
    [SerializeField] bool tracking;

    [Header("LayerMasks")]
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstructionMask;

    [Header("Components")]
    [SerializeField] Transform target;
    Transform player;
    Vector3 FoVPointA;
    Vector3 FoVPointB;

    [Header("FoV")]
    static readonly float viewAngle = 135f;
    static Vector3 DirFromAngle(float angleIndDegrees)
    {
        return new Vector3(Mathf.Sin((angleIndDegrees - 90f) * Mathf.Deg2Rad), Mathf.Cos((angleIndDegrees - 90f) * Mathf.Deg2Rad), 0);
    }
    Vector3 topAngle = DirFromAngle(-viewAngle / 2);
    Vector3 bottomAngle = DirFromAngle(viewAngle / 2);

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Fov Triangle
        FoVPointA = transform.position + topAngle * (range + 11.5f);
        FoVPointB = transform.position + bottomAngle * (range + 11.5f);

        if (!target)
        {
            FindTarget();
            return;
        }

        if (IsObstructed() || !TargetInRange() || !TargetWithinAngle())
        {
            target = null;
            StopCoroutine(nameof(Shooting));
        }
        
        if (!shooting)
        {
            RotateToTarget();
        }

        if (target && tracking)
        {
            StartCoroutine(nameof(Shooting));
        }
    }

    #endregion

    #region FindPlayer

    void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2) transform.position, 0f, playerMask);

        if ((hits.Length > 0) && TargetInRange() && TargetWithinAngle() && !IsObstructed())
        {
            tracking = true;
            if (hits[0].transform.Find("Player"))
            {
                target = player;
            }
        }
        else
        {
            target = null;
            tracking = false;
        }
    }

    bool IsObstructed()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, obstructionMask);

        if (hit)
        {
            // print("Target obstructed");
            return true;
        }
        // print("Target not obstructed");
        return false;
    }

    bool TargetInRange()
    {
        if (Vector2.Distance(player.position, transform.position) <= range)
        {
            return true;
        }
        return false;
    }

    bool TargetWithinAngle()
    {
        Vector3 d, e;
        Vector3 a = FoVPointA;
        Vector3 b = FoVPointB;
        Vector3 c = transform.position;
        Vector3 p = player.position;

        float w1, w2;

        d = b - a;
        e = c - a;

        w1 = (e.x * (a.y - p.y) + e.y * (p.x - a.x)) / (d.x * e.y - d.y * e.x);
        w2 = (p.y - a.y - w1 * d.y) / e.y;

        return (w1 >= 0.0) && (w2 >= 0.0) && ((w1 + w2) <= 1.0);
    }

    void RotateToTarget()
    {
        if (target)
        {
            float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + 180;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region Shooting

    IEnumerator Shooting()
    {
        tracking = false;
        // print("Preparing burst");
        
        yield return new WaitForSeconds(cooldown);

        shooting = true;
        StartCoroutine(ShootBurst());
        // print("Burst started");
    }

    IEnumerator ShootBurst()
    {
        WaitForSeconds wait = new(firerate);

        Shoot();
        yield return wait;

        Shoot();
        yield return wait;

        Shoot();
        yield return wait;

        Shoot();
        yield return wait;

        Shoot();
        yield return wait;

        shooting = false;
        target = null;
        // print("Burst ended");
    }

    void Shoot()
    {
        // print("Bang!");
    }

    #endregion

    #region Gizmos:

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + topAngle * range);
        Gizmos.DrawLine(transform.position, transform.position + bottomAngle * range);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);

        if (player)
        {
            Gizmos.DrawLine(transform.position, player.position);
        }

        if (target)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, FoVPointA);
            Gizmos.DrawLine(transform.position, FoVPointB);
            Gizmos.DrawLine(FoVPointA, FoVPointB);
        }
    }

    #endregion
}
