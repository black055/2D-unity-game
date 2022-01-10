using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] OptionMenuController optionMenuController;
	[SerializeField] LevelMenuController levelMenuController;
	[SerializeField] PauseMenuController pauseMenuController;
	[SerializeField] FinishMenuController finishMenuController;
	[SerializeField] LoseMenuController loseMenuController;
	public bool disableOnce;

	void PlaySound(AudioClip whichSound){
		if(menuButtonController != null) {
			if(!disableOnce){
				menuButtonController.audioSource.PlayOneShot(whichSound);
			}
			else {
				disableOnce = false;
			}
		}
		else if(optionMenuController != null) {
			if(!disableOnce){
				optionMenuController.audioSource.PlayOneShot(whichSound);
			}
			else {
				disableOnce = false;
			}
		}
		else if(levelMenuController != null) {
			if(!disableOnce){
				levelMenuController.audioSource.PlayOneShot(whichSound);
			}
			else {
				disableOnce = false;
			}
		}
		else if(pauseMenuController != null) {
			if(!disableOnce){
				pauseMenuController.audioSource.PlayOneShot(whichSound);
			}
			else {
				disableOnce = false;
			}
		}
		else if(finishMenuController != null) {
			if(!disableOnce){
				finishMenuController.audioSource.PlayOneShot(whichSound);
			}
			else {
				disableOnce = false;
			}
		}
		else if(loseMenuController != null) {
			if(!disableOnce){
				loseMenuController.audioSource.PlayOneShot(whichSound);
			}
			else {
				disableOnce = false;
			}
		}
	}
}	
