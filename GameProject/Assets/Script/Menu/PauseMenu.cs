using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
  [SerializeField] PauseMenuController pauseMenuController;
	[SerializeField] Animator animator;
	[SerializeField] int thisIndex;
	[SerializeField] GameObject DestinationMenu;
	[SerializeField] GameObject MainMenu;
	int index = -1;
    // Update is called once per frame
    void Update()
    {
			if(pauseMenuController.index == thisIndex)
			{
				animator.SetBool ("selected", true);
				if(Input.GetAxis ("Submit") == 1){
					animator.SetBool ("pressed", true);
				}
				else if (animator.GetBool ("pressed")){
					animator.SetBool ("pressed", false);
					index = thisIndex;
				}
			}
			else {
				animator.SetBool ("selected", false);
			}

			if(index != -1) {
				Run(index);
				index = -1;
			}
    }

		void Run(int index) {
			if(index == 0) {
					StartCoroutine(Resume(0.35f));
			}
			else if(index == 1) {
					StartCoroutine(Settings(0.35f));
			}
			else if(index == 2) {
					StartCoroutine(BackMainMenu(0.35f));
			}
		}
		IEnumerator BackMainMenu(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

		IEnumerator Resume(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DestinationMenu.SetActive(false);
    }

		IEnumerator Settings(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DestinationMenu.SetActive(true);
				DestinationMenu.GetComponent<OptionMenuController>().index = 0;
				MainMenu.SetActive(false);
    }

}
