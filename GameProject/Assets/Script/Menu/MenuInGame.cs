using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInGame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject LoseMenu;
    [SerializeField] GameObject WinMenu;
    // [SerializeField] GameObject Enemies;
    GameObject Knight;
    GameObject[] Enemies;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
        if(!PauseMenu.activeSelf) {
            Knight = GameObject.FindGameObjectWithTag("Knight");
            //check Null
            pauseController(Knight, true);
            // if(Knight != null) {
            //     Knight.GetComponent<KnightController>().enabled = true;
            // }
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in Enemies)
            {
                pauseController(enemy,true);
            }
        }
        if(LoseMenu.activeSelf || WinMenu.activeSelf) {
            Knight = GameObject.FindGameObjectWithTag("Knight");
            pauseController(Knight, false);
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in Enemies)
            {
                pauseController(enemy,false);
            }
        }
    }
    public void Pause() {
        PauseMenu.SetActive(true);
        Knight = GameObject.FindGameObjectWithTag("Knight");
        pauseController(Knight, false);
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in Enemies)
        {
            pauseController(enemy,false);
        }
        // if(Knight != null) {
        //     Knight.GetComponent<KnightController>().enabled = false;
        // }
    }

    public void pauseController(GameObject objects, bool status) {
        if(objects != null) {
            MonoBehaviour[] scripts = objects.GetComponents<MonoBehaviour>();
            if(scripts != null) {
                foreach(MonoBehaviour script in scripts)
                {
                    script.enabled = status;
                }
            }
        }
    }
}
