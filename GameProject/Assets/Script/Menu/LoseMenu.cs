using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoseMenu : MonoBehaviour
{
  [SerializeField] LoseMenuController loseMenuController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;

    // Update is called once per frame
    void Update()
    {
		if(loseMenuController.index == thisIndex)
		{
			animator.SetBool ("selected", true);
			if(Input.GetAxis ("Submit") == 1){
				animator.SetBool ("pressed", true);

				if(thisIndex == 0) {
					Debug.Log("Play again");
          StartCoroutine(PlayAgain(0.35f));
				}
        else if(thisIndex == 1) {
					StartCoroutine(BackToMenu(0.35f));
				}
			}else if (animator.GetBool ("pressed")){
				animator.SetBool ("pressed", false);
				animatorFunctions.disableOnce = true;
			}
		}else{
			animator.SetBool ("selected", false);
		}
    }

		IEnumerator PlayAgain(float seconds)
    {
        yield return new WaitForSeconds(seconds);
				AudioSource source = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
				if(source != null) {
					source.mute = true;
				}
        Scene scene = SceneManager.GetActiveScene();
				SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
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
}
