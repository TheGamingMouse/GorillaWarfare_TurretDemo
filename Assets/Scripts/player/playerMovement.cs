using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float moveSpeed = 3f;
    float startPos;
    readonly float startPosOffset = 0.276f;

    [Header("Bools")]
    bool moveBool;
    bool beginBool;
    bool beginRoutineBool;

    [Header("GameObjects")]
    GameObject playerObj;

    [Header("Components")]
    Rigidbody2D rb;
    Vector2 move;

    #endregion

    #region Event Subscribtions
    
    void OnEnable()
    {
        CameraManager.OnPlayerFastCamActive += HandleEnableMovement;
        CameraManager.OnBeginGame += HandleBeginGame;
    }

    void OnDisable()
    {
        CameraManager.OnPlayerFastCamActive -= HandleEnableMovement;
        CameraManager.OnBeginGame -= HandleBeginGame;
    }

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        playerObj = GameObject.FindWithTag("PlayerObject");
        startPos = transform.localPosition.x;

        DisableMovement();

        beginBool = false;
        beginRoutineBool = true;
    }

    void FixedUpdate()
    {
        if (moveBool)
        {
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * move);
        }

        if (beginBool)
        {
            if (beginRoutineBool)
            {
                gameObject.AddComponent<Rigidbody2D>();
                rb = GetComponent<Rigidbody2D>();
                rb.freezeRotation = true;

                StartCoroutine(BeginGameMove());
                transform.SetParent(null, true);
                beginRoutineBool = false;
            }
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * new Vector2(1f,0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y =Input.GetAxisRaw("Vertical");
    }

    #endregion

    #region Methods

    IEnumerator BeginGameMove()
    {
        yield return new WaitForSeconds((Mathf.Abs(startPos) + startPosOffset) / moveSpeed);
        Destroy(rb.GetComponent<Rigidbody2D>());

        yield return new WaitForSeconds(0.01f);
        transform.SetParent(playerObj.transform, true);
        rb = GetComponentInParent<Rigidbody2D>();
        beginBool = false;
    }
    
    void EnableMovement()
    {
        moveBool = true;
    }

    void DisableMovement()
    {
        moveBool = false;
    }

    #endregion

    #region Subribtion Handlers

    void HandleEnableMovement()
    {
        EnableMovement();
    }

    void HandleBeginGame()
    {
        beginBool = true;
    }

    #endregion
}
