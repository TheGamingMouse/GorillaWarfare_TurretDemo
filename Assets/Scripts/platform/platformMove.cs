using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    #region Variables

    [SerializeField] DirectionState dState;

    [Header("Floats")]
    [SerializeField] float moveSpeed = 0.5f;

    [Header("Bools")]
    [SerializeField] bool isDefaultDirection = true;

    [Header("Vector3s")]
    Vector3 directionTranslation;

    #endregion

    #region StartUpdate

    // Update is called once per frame
    void Update()
    {
        switch (dState)
        {
            case DirectionState.vertical:
                directionTranslation = isDefaultDirection ? transform.up : -transform.up;
                break;

            case DirectionState.horizontal:
                directionTranslation = isDefaultDirection ? transform.right : -transform.right;
                break;
        }
        directionTranslation *= Time.deltaTime * moveSpeed;

        transform.Translate(directionTranslation);
    }

    #endregion

    #region Methods

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("MovePoint"))
        {
            isDefaultDirection = !isDefaultDirection;
        }
    }

    #endregion

    public enum DirectionState
    {
        vertical,
        horizontal
    }
}
