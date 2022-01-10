using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lose : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider heald;
    [SerializeField] GameObject LoseMenu;

    // Update is called once per frame
    void Update()
    { 
        if(heald.value == 0) {
            LoseMenu.SetActive(true);
        }
    }
}
