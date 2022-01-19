using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishMenuController : MonoBehaviour
{
    // Use this for initialization
	public int index;
	[SerializeField] bool keyDown;
	[SerializeField] int maxIndex;
	[SerializeField] GameObject mainCamera;
	[SerializeField] Text timeCount;
	[SerializeField] Text lifeCount;
	int second = 0,minute = 0,hour = 0, life;
	public AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource>();
		GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
		life = gameManager.GetComponent<GameManager>().getLifeLeft();
		second = mainCamera.GetComponent<TimeManager>().countTime - mainCamera.GetComponent<TimeManager>().prevTime;

		if(second >= 60) {
			minute = (int)second/60;
			second -= 60 * minute;
			if(minute >= 60) {
				hour = (int)minute/60;
				minute -= 60 * hour;
			}
		}

		timeCount.text = hour + ":" + minute + ":" + second;
		lifeCount.text = "X " + life;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis ("Vertical") != 0){
			if(!keyDown){
				if (Input.GetAxis ("Vertical") < 0) {
					if(index < maxIndex){
						index++;
					}else{
						index = 0;
					}
				} else if(Input.GetAxis ("Vertical") > 0){
					if(index > 0){
						index --; 
					}else{
						index = maxIndex;
					}
				}
				keyDown = true;
			}
		}else{
			keyDown = false;
		}
	}
}
