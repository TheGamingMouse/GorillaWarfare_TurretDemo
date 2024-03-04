using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounts : MonoBehaviour
{
    #region Variables

    public AmmoState aState;
    
    [Header("Ints")]
    public int bananaAmount;
    public int specialAmmo;

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            switch (aState)
            {
                case AmmoState.normal:
                    aState = AmmoState.special;
                    break;
                
                case AmmoState.special:
                    aState = AmmoState.normal;
                    break;
            }
        }
    }

    #endregion

    #region Enums
    
    public enum AmmoState
    {
        normal,
        special
    }

    #endregion
}
