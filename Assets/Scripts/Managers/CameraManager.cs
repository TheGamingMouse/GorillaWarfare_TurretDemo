using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Events

    public static event Action OnPlayerFastCamActive;
    public static event Action OnBeginGame;

    #endregion
    
    #region Variables

    [Header("Cameras")]
    Camera mainCam;

    [Header("Floats")]
    readonly float moveSpeed = 4f;

    [Header("Bools")]
    bool mainMenuCamBool;
    bool playerCamBool;
    bool playerFastCamBool;
    // bool printed;

    [Header("GameObjects")]
    GameObject playerCamPos;
    GameObject mainMenuCamPos;

    // [Header("Debugging")]
    // System.Diagnostics.Stopwatch watch;
    // bool printed = false;

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        
        playerCamPos = GameObject.FindGameObjectWithTag("PlayerCamPos");
        mainMenuCamPos = GameObject.FindGameObjectWithTag("MainMenuCamPos");

        playerCamPos.transform.position = new Vector3(playerCamPos.transform.position.x, playerCamPos.transform.position.y, mainCam.transform.position.z);

        mainMenuCamBool = true;
        playerCamBool = false;
        playerFastCamBool = false;
    }

    void FixedUpdate()
    {
        if (mainMenuCamBool)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, mainMenuCamPos.transform.position, moveSpeed * Time.deltaTime);
        }
        if (playerCamBool)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, playerCamPos.transform.position, moveSpeed * Time.deltaTime);
        }
        if (playerFastCamBool)
        {
            mainCam.transform.position = playerCamPos.transform.position;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if ((mainCam.transform.position == Vector3.Lerp(mainCam.transform.position, playerCamPos.transform.position, moveSpeed * Time.deltaTime)) && playerCamBool)
        {
            ActivatePlayerFastCam();
        }
    }

    #endregion

    #region Methods

    public void ActivatePlayerCam()
    {
        playerCamBool = true;
        mainMenuCamBool = false;
        OnBeginGame?.Invoke();

        // watch = System.Diagnostics.Stopwatch.StartNew();
        // print("Timer started");
    }

    void ActivatePlayerFastCam()
    {
        playerCamBool = false;
        playerFastCamBool = true;
        OnPlayerFastCamActive?.Invoke();

        // if (!printed)
        // {
        //     watch.Stop();
        //     print("Time for transform.position " + watch.ElapsedMilliseconds);

        //     printed = true;
        // }
    }

    #endregion
}
