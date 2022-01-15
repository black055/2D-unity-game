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

            pauseController(Knight, true);
            
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

            Animator animator = objects.GetComponent<Animator>();
            if (animator != null) animator.enabled = status;
        }
    }
}
