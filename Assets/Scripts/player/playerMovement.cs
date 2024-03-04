using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float moveSpeed = 3f;
    float startPos;
    
    readonly float startPosOffset = 1f;

    [Header("Bools")]
    bool moveBool;
    bool beginBool;
    bool beginRoutineBool;

    [Header("GameObjects")]
    GameObject playerCamPos;

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
        rb = GetComponent<Rigidbody2D>();
        playerCamPos = GameObject.FindWithTag("PlayerCamPos");
        startPos = transform.position.x;

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
                StartCoroutine(BeginGameMove());
                playerCamPos.transform.SetParent(null, true);
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
        yield return new WaitForSeconds(Mathf.Abs(startPos - startPosOffset - playerCamPos.transform.position.x) / moveSpeed);

        playerCamPos.transform.SetParent(transform, true);
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
