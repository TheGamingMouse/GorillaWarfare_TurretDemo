using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables

    [SerializeField] AmmoState aState;
    
    [Header("Ints")]
    int bananaAmount;
    int health;
    int specialAmmo;

    [Header("Bools")]
    bool elementsFound;
    
    [Header("TMPs")]
    TMP_Text bananaText;
    TMP_Text specialAmmoText;

    [Header("GameObjects")]
    GameObject canvas;
    GameObject health1Obj;
    GameObject health2Obj;
    GameObject health3Obj;
    GameObject ammoObj;
    GameObject specialAmmoObj;
    
    [Header("Transforms")]
    Transform ammoFront;
    Transform ammoBack;

    [Header("Images")]
    Image healthImg1;
    Image healthImg2;
    Image healthImg3;

    [Header("Colors")]
    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    #endregion

    #region Subscriptions

    void OnEnable()
    {
        CameraManager.OnPlayerFastCamActive += HandleStartGame;
    }

    void OnDisable()
    {
        CameraManager.OnPlayerFastCamActive -= HandleStartGame;
    }

    #endregion
    
    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("MainUI");
        canvas.SetActive(false);
        elementsFound = false;
    }

    // Update is called once per frame
    void Update()
    {
        bananaAmount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCounts>().bananaAmount;
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().health;
        specialAmmo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCounts>().specialAmmo;
        aState = (AmmoState)GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCounts>().aState;

        if (elementsFound)
        {
            UpdateBananaText();
            UpdateHealthBar();
            UpdateSpecialAmmo();
            UpdateAmmoType();
        }
    }

    #endregion

    #region Methods

    void FindElements()
    {
        bananaText = canvas.transform.Find("BananasText (TMP)").GetComponent<TextMeshProUGUI>();
        specialAmmoText = canvas.transform.Find("Ammo/SpecialAmmo/SpecialAmmo (TMP)").GetComponent<TextMeshProUGUI>();

        health1Obj = canvas.transform.Find("Health/Health 1").gameObject;
        health2Obj = canvas.transform.Find("Health/Health 2").gameObject;
        health3Obj = canvas.transform.Find("Health/Health 3").gameObject;
        ammoObj = canvas.transform.Find("Ammo/Ammo").gameObject;
        specialAmmoObj = canvas.transform.Find("Ammo/SpecialAmmo").gameObject;
        ammoFront = canvas.transform.Find("Ammo/AmmoFrontPos").transform;
        ammoBack = canvas.transform.Find("Ammo/AmmoBackPos").transform;

        healthImg1 = health1Obj.GetComponent<Image>();
        healthImg2 = health2Obj.GetComponent<Image>();
        healthImg3 = health3Obj.GetComponent<Image>();

        elementsFound = true;
    }

    #endregion

    #region UpdateMethods

    void UpdateBananaText()
    {
        bananaText.text = $"Bananas: {bananaAmount}";
    }

    void UpdateHealthBar()
    {
        switch (health)
        {
            case 3:
                healthImg1.fillAmount = 1;
                healthImg2.fillAmount = 1;
                healthImg3.fillAmount = 1;
                break;
            
            case 2:
                healthImg1.fillAmount = 1;
                healthImg2.fillAmount = 1;
                healthImg3.fillAmount = 0;
                break;
            
            case 1:
                healthImg1.fillAmount = 1;
                healthImg2.fillAmount = 0;
                healthImg3.fillAmount = 0;
                break;

            case 0:
                healthImg1.fillAmount = 0;
                healthImg2.fillAmount = 0;
                healthImg3.fillAmount = 0;
                break;
        }
    }

    void UpdateSpecialAmmo()
    {
        specialAmmoText.text = $"x{specialAmmo}";
    }

    void UpdateAmmoType()
    {
        switch (aState)
        {
            case AmmoState.normal:
                ammoObj.transform.position = ammoFront.position;
                ammoObj.transform.SetAsLastSibling();
                ammoObj.GetComponent<Image>().color = activeColor;

                specialAmmoObj.transform.position = ammoBack.position;
                specialAmmoObj.transform.SetAsFirstSibling();
                specialAmmoObj.GetComponent<Image>().color = inactiveColor;
                break;

            case AmmoState.special:
                ammoObj.transform.position = ammoBack.position;
                ammoObj.transform.SetAsFirstSibling();
                ammoObj.GetComponent<Image>().color = inactiveColor;

                specialAmmoObj.transform.position = ammoFront.position;
                specialAmmoObj.transform.SetAsLastSibling();
                specialAmmoObj.GetComponent<Image>().color = activeColor;
                break;
        }
    }

    #endregion

    #region HandleMethods

    void HandleStartGame()
    {
        canvas.SetActive(true);
        FindElements();
    }

    #endregion

    #region Enums
    
    enum AmmoState
    {
        normal,
        special
    }

    #endregion
}
