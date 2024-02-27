using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretTrackAndShoot : MonoBehaviour
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
    [SerializeField] Transform target;
    Transform player;
    Transform playerHead;
    Transform playerFeet;
    Vector3 FoVPointA;
    Vector3 FoVPointB;

    [Header("FoV")]
    static float viewAngle = 135f;
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
        playerHead = player.Find("Head").transform;
        playerFeet = player.Find("Feet").transform;

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
        if ((Vector2.Distance(player.position, transform.position) <= range) || 
            (Vector2.Distance(playerHead.position, transform.position) <= range) || 
            (Vector2.Distance(playerFeet.position, transform.position) <= range))
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
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + topAngle * range);
        Gizmos.DrawLine(transform.position, transform.position + bottomAngle * range);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);

        if (player)
        {
            Gizmos.DrawLine(transform.position, player.position);
            Gizmos.DrawLine(transform.position, playerHead.position);
            Gizmos.DrawLine(transform.position, playerFeet.position);
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
