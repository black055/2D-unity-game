using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;
public class LevelMenu : MonoBehaviour
{
   [SerializeField] LevelMenuController levelMenuController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
	[SerializeField] GameObject DestinationMenu;
	[SerializeField] GameObject MainMenu;
    [SerializeField] Text levelText;
    [SerializeField] Image background;
    [SerializeField] Sprite levelBackground;
    // Update is called once per frame
    private int index = -1;
    void Update()
    {
		if(levelMenuController.index == thisIndex)
		{
			animator.SetBool ("selected", true);
            levelText.text = "Level " + (levelMenuController.index+1);
            background.sprite = levelBackground;
			if(Input.GetAxis ("Submit") == 1) {
				animator.SetBool ("pressed", true);
			} else if (animator.GetBool ("pressed")){
				animator.SetBool ("pressed", false);
				animatorFunctions.disableOnce = true;
                index = thisIndex;
			}
		}else{
			animator.SetBool ("selected", false);
		}
        if(Input.GetKeyDown(KeyCode.Escape)) {
            DestinationMenu.SetActive(true);
            MainMenu.SetActive(false);
            Thread.Sleep(100);
        }
        if(index != -1) {
            Run(index);
            index = -1;
        }
    }

    public void Run(int index) {
        
        switch (index)
        {
            case 0: {
                StartCoroutine(Stage1(0.5f));
                break;
            }
            case 1: {
                StartCoroutine(Stage2(0.35f));
                break;
            }
            case 2: {
                StartCoroutine(Stage3(0.5f));
                break;
            }
            default: break;
        }
    }

    IEnumerator Stage1(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("GameStage1");
    }
    IEnumerator Stage2(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("GameStage2");
    }
    IEnumerator Stage3(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("GameStage3");
    }
}
