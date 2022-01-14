using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FinishMenu : MonoBehaviour
{
  [SerializeField] FinishMenuController finishMenuController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] TextMeshProUGUI textButton;
	// private GameObject Knight;
	private Scene stage;
	[SerializeField] int thisIndex;

		private void Start() {
			stage = SceneManager.GetActiveScene();
			if(stage.name == "GameStage3") {
				textButton.text = "Play Again";
			}

			//fixed knight !!!
			// Knight = GameObject.FindGameObjectWithTag("Knight");
			// Knight.GetComponent<KnightController>().enabled = false;
		}
    // Update is called once per frame
    void Update()
    {
			if(finishMenuController.index == thisIndex)
			{
				animator.SetBool ("selected", true);
				if(Input.GetAxis ("Submit") == 1){
					animator.SetBool ("pressed", true);

				}else if (animator.GetBool ("pressed")){
					animator.SetBool ("pressed", false);
					animatorFunctions.disableOnce = true;
					if(thisIndex == 0) {
						StartCoroutine(NextLevel(0.35f));
					}
					else if(thisIndex == 1) {
						StartCoroutine(BackToMenu(0.35f));
					}
				}
			}else{
				animator.SetBool ("selected", false);
			}
    }

		IEnumerator BackToMenu(float seconds)
    {
        yield return new WaitForSeconds(seconds);
				AudioSource source = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
				if(source != null) {
					source.mute = true;
				}
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    IEnumerator NextLevel(float seconds)
    {
        yield return new WaitForSeconds(seconds);
				AudioSource source = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
				if(source != null) {
					source.mute = true;
				}
        if(stage.name == "GameStage1") {
					SceneManager.LoadScene("GameStage2", LoadSceneMode.Single);
				}
				else if(stage.name == "GameStage2") {
					SceneManager.LoadScene("GameStage3", LoadSceneMode.Single);
				}
				else if(stage.name == "GameStage3") {
					SceneManager.LoadScene(stage.name, LoadSceneMode.Single);
				} else {
					Debug.Log("No options");
				}
    }
}
