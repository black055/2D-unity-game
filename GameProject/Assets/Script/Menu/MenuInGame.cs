using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInGame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject LoseMenu;
    [SerializeField] GameObject WinMenu;
    [SerializeField] GameObject Enemies;
    [SerializeField] GameObject Knight;
    public static bool isPaused = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(isPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
        if(!PauseMenu.activeSelf) {
            Enemies.SetActive(true);
            Knight.SetActive(true);
        }
    }

    public void Resume() {
        isPaused = false;
    }

    public void Pause() {
        isPaused = true;
        PauseMenu.SetActive(true);
        Enemies.SetActive(false);
        Knight.SetActive(false);
    }
}
