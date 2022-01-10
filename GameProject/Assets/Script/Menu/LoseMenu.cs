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
        Scene scene = SceneManager.GetActiveScene();
				SceneManager.LoadScene(scene.name);
    }
		IEnumerator BackToMenu(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Menu");
    }
}
