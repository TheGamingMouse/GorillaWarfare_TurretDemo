using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrackTarget : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool targetIsInArea;

    #endregion

    #region Methods

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            targetIsInArea = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            targetIsInArea = false;
        }
    }

    #endregion
}
