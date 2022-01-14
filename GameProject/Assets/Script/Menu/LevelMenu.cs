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
    [SerializeField] GameObject mainBackground;
    // Update is called once per frame
    private int index = -1;

    void Update()
    {
        if(gameObject.activeSelf) {
            mainBackground.SetActive(false);
        }
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
            StartCoroutine(Back(0.35f));
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
        SceneManager.LoadScene("GameStage1", LoadSceneMode.Single);
    }
    IEnumerator Stage2(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("GameStage2", LoadSceneMode.Single);
    }
    IEnumerator Stage3(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("GameStage3", LoadSceneMode.Single);
    }
    IEnumerator Back(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DestinationMenu.SetActive(true);
        MainMenu.SetActive(false);
        mainBackground.SetActive(true);
    }
}
