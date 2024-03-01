using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Variables

    [Header("Cameras")]
    Camera mainCam;

    [Header("Floats")]
    readonly float moveSpeed = 1f;

    [Header("Bools")]
    bool mainMenuCamBool;
    bool playerCamBool1;
    public bool playerCamBool2;
    // bool printed;

    [Header("GameObjects")]
    GameObject playerCamPos;
    GameObject mainMenuCamPos;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        
        playerCamPos = GameObject.FindGameObjectWithTag("PlayerCamPos");
        mainMenuCamPos = GameObject.FindGameObjectWithTag("MainMenuCamPos");

        playerCamPos.transform.position = new Vector3(playerCamPos.transform.position.x, playerCamPos.transform.position.y, mainCam.transform.position.z);

        mainMenuCamBool = true;
        playerCamBool1 = false;
        playerCamBool2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCamBool1)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, playerCamPos.transform.position, moveSpeed * Time.deltaTime);
        }

        if (playerCamBool2)
        {
            mainCam.transform.position = playerCamPos.transform.position;
        }

        if (mainMenuCamBool)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, mainMenuCamPos.transform.position, moveSpeed * Time.deltaTime);
        }
        if (mainCam.transform.position == playerCamPos.transform.position)
        {
            // if (!printed)
            // {
            //     print("Time for transform.position " + Time.realtimeSinceStartup);
            //     printed = true;
            // }
            ActivateFastPlayerCam();
        }
    }

    public void ActivatePlayerCam()
    {
        playerCamBool1 = true;
        mainMenuCamBool = false;
    }

    void ActivateFastPlayerCam()
    {
        playerCamBool1 = false;
        playerCamBool2 = true;
    }
}
