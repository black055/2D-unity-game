using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditMenu : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            MainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
