using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Variables

    [Header("TMPs")]
    TMP_Text bananaText;
    TMP_Text ammoText;
    TMP_Text specialAmmoText;

    [Header("GameObjects")]
    GameObject canvas;
    GameObject health1Obj;
    GameObject health2Obj;
    GameObject health3Obj;
    GameObject ammoObj;
    GameObject specialAmmoObj;

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("MainUI");

        bananaText = canvas.transform.Find("BananasText (TMP)").GetComponent<TextMeshProUGUI>();
        ammoText = canvas.transform.Find("Ammo/Ammo/AmmoText (TMP)").GetComponent<TextMeshProUGUI>();
        specialAmmoText = canvas.transform.Find("Ammo/SpecialAmmo/SpecialAmmo (TMP)").GetComponent<TextMeshProUGUI>();

        health1Obj = canvas.transform.Find("Health/Health 1").gameObject;
        health2Obj = canvas.transform.Find("Health/Health 1").gameObject;
        health3Obj = canvas.transform.Find("Health/Health 1").gameObject;
        ammoObj = canvas.transform.Find("Ammo/Ammo").gameObject;
        specialAmmoObj = canvas.transform.Find("Ammo/SpeicalAmmo").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
