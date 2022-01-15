using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
	[SerializeField] GameObject DestinationMenu;
	[SerializeField] GameObject MainMenu;
	int checkPressed = -1;

    // Update is called once per frame
		private void Start() {
			//resolution
        Resolution[] resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for(int i = 0; i< resolutions.Length; i++) {
            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height){
                currentResolutionIndex = i;
            }
        }
				Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, Screen.fullScreen);
		}
    void Update()
    {
			if(menuButtonController.index == thisIndex)
			{
				animator.SetBool ("selected", true);
				if(Input.GetAxis ("Submit") == 1) {
					animator.SetBool ("pressed", true);
				}
				else if (animator.GetBool ("pressed")){
					animator.SetBool ("pressed", false);
					animatorFunctions.disableOnce = true;
					checkPressed = thisIndex;
				}
			}
			else {
				animator.SetBool ("selected", false);
			}
			if(checkPressed != -1) {
				Run(checkPressed);
				checkPressed = -1;
			}
    }
		public void Run(int checekPressed) {
			if(checkPressed == 3) {
				StartCoroutine(Exit(0.35f));
			}
			else if(checkPressed == 0 || checkPressed == 1 || checkPressed == 2) {
				
				StartCoroutine(LateCall(0.35f));
				if(thisIndex == 0) {
					DestinationMenu.GetComponent<LevelMenuController>().index =  0;
				}
				else if(thisIndex == 1) {
					DestinationMenu.GetComponent<OptionMenuController>().index =  0;
				}
			}
		}

		IEnumerator LateCall(float seconds)
     {
        yield return new WaitForSeconds(seconds);
        MainMenu.SetActive(false);
				DestinationMenu.SetActive(true);
     }

		 IEnumerator Exit(float seconds)
     {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
				#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
				#endif
     }

}
