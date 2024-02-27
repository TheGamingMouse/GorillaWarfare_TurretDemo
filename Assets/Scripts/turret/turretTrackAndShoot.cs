using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class turretTrackAndShoot : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    float rotSpeed = 150f;
    float range = 5f;
    float firerate = 0.7f;
    float cooldown = 2f;

    [Header("Bools")]
    bool shooting;
    bool tracking;

    [Header("LayerMasks")]
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstructionMask;

    [Header("Components")]
    Transform target;
    Transform player;
    Vector3 FoVPointA;
    Vector3 FoVPointB;

    [Header("FoV")]
    float viewRadius;
    float viewAngle = 135f;
    Vector3 topAngle;
    Vector3 bottomAngle;
    Vector3 DirFromAngle(float angleIndDegrees)
    {
        return new Vector3(Mathf.Sin((angleIndDegrees - 90f) * Mathf.Deg2Rad), Mathf.Cos((angleIndDegrees - 90f) * Mathf.Deg2Rad), 0);
    }

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        // FoV
        topAngle = DirFromAngle(-viewAngle / 2);
        bottomAngle = DirFromAngle(viewAngle / 2);

        // Fov Triangle
        FoVPointA = transform.position + topAngle * (range + 8);
        FoVPointB = transform.position + bottomAngle * (range + 8);
    }

    // Update is called once per frame
    void Update()
    {
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
            target = hits[0].transform;
            tracking = true;
        }
    }

    bool IsObstructed()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, obstructionMask);

        if (hit == true)
        {
            // print("Target obstructed");
            return true;
        }
        // print("Target not obstructed");
        return false;
    }

    bool TargetInRange()
    {
        return Vector2.Distance(player.position, transform.position) <= range;
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

        if (!IsObstructed())
        {
            shooting = true;
            StartCoroutine(ShootBurst());
            // print("Burst started");
        }
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
        if (player)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, range);
            Gizmos.DrawLine(transform.position, player.position);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + topAngle * range);
            Gizmos.DrawLine(transform.position, transform.position + bottomAngle * range);
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
