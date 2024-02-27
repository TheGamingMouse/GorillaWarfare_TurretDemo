using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    float moveSpeed = 3;

    [Header("Components")]
    Rigidbody2D rb;
    GameObject bazooka;
    Vector2 move;

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        bazooka = GameObject.FindWithTag("Bazooka");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y =Input.GetAxisRaw("Vertical");

        LookAtMouse();
    }

    #endregion

    #region Functions

    void LookAtMouse()
    {
        Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bazooka.transform.up = mPos - new Vector2(transform.position.x, transform.position.y);
        bazooka.transform.rotation *= Quaternion.Euler(0,0,90);
    }

    #endregion
}
