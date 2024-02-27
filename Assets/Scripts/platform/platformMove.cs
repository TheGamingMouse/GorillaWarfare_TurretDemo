using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    float moveSpeed = 0.5f;

    [Header("Bools")]
    private bool isTop = true;

    #endregion

    #region StartUpdate

    // Update is called once per frame
    void Update()
    {
        Vector3 directionTranslation = isTop ? transform.up : -transform.up;
        directionTranslation *= Time.deltaTime * moveSpeed;

        transform.Translate(directionTranslation);
    }

    #endregion

    #region Functions

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("MovePoint"))
        {
            isTop = !isTop;
        }
    }

    #endregion
}
