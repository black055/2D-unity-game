using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInGame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject Enemies;
    GameObject currentKnight;
    // Update is called once per frame
    void Update()
    {
        GameObject Knight = GameObject.FindGameObjectWithTag("Knight");
        if(Knight != null) {
            currentKnight = Knight;
        }
        if(currentKnight != null) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                Pause();
            }
            if(!PauseMenu.activeSelf) {
                currentKnight.GetComponent<KnightController>().enabled = true;
            }
        }
    }
    public void Pause() {
        PauseMenu.SetActive(true);
        currentKnight.GetComponent<KnightController>().enabled = false;
    }
}
