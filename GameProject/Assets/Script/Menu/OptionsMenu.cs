using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Threading;
public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] OptionMenuController optionMenuController;
	[SerializeField] Animator animator;
	[SerializeField] int thisIndex;
	[SerializeField] GameObject DestinationMenu;
	[SerializeField] GameObject OptionMenu;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] Slider musicVolume, sfxVolume;
    [SerializeField] Toggle isFullScreen;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Dropdown resolutionDropdown;

    SoundManager soundManager;
    Resolution[] resolutions;
    private void Start() {
        setFullScreen(Screen.fullScreen);

        soundManager = SoundManager.instance;
        sfxVolume.value = soundManager.GetCurrentVolume() * sfxVolume.maxValue;

        float musicCurrentVolume;
        audioMixer.GetFloat("volume", out musicCurrentVolume);
        if (musicCurrentVolume == -80f) musicVolume.value = 0f;
        else musicVolume.value = Mathf.Round(Mathf.Pow(10, musicCurrentVolume/20) * 100);

        //resolution
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i< resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height){
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        ChangeResolution(currentResolutionIndex);
        setResolution(currentResolutionIndex);
    }
    // Update is called once per frame
    void Update()
    {
        if(optionMenuController.index == thisIndex)
		{
			animator.SetBool ("selected", true);
			if(Input.GetAxis ("Submit") == 1){
				animator.SetBool ("pressed", true);

			}else if (animator.GetBool ("pressed")){
				animator.SetBool ("pressed", false);
				animatorFunctions.disableOnce = true;
			}
		}else{
			animator.SetBool ("selected", false);
		}

        if(Input.GetKeyDown(KeyCode.Escape)) {
            DestinationMenu.SetActive(true);
            OptionMenu.SetActive(false);
        }
        
        if(optionMenuController.index == 0) {
            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                int index = resolutionDropdown.value - 1;
                if(index<0) {
                index = resolutions.Length - 1;
                }
                ChangeResolution(index);
                setResolution(index);
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                int index = resolutionDropdown.value + 1;
                if(index >= resolutions.Length) {
                    index = 0;
                }
                ChangeResolution(index);
                setResolution(index);
            }
		}
        else if(optionMenuController.index == 1) {
            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                setFullScreen(false);
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                setFullScreen(true);
            }
            if(Input.GetKeyDown(KeyCode.Return)) {
                setFullScreen(!isFullScreen.isOn);
            }
		}
        else if(optionMenuController.index == 2) {
            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                musicVolume.value -= 5;
                SetVolume(musicVolume.value/musicVolume.maxValue);
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                musicVolume.value += 5;
                SetVolume(musicVolume.value/musicVolume.maxValue);
            }
		}
        else if(optionMenuController.index == 3) {
            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                sfxVolume.value -= 5;
                if(soundManager != null) {
                    soundManager.VolumeChange(sfxVolume.value/sfxVolume.maxValue);
                }
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                sfxVolume.value += 5;
                if(soundManager != null) {
                    soundManager.VolumeChange(sfxVolume.value/sfxVolume.maxValue);
                }
            }
		}
    }

    public void SetVolume(float volume) {
        if (volume == 0) audioMixer.SetFloat("volume", -80f);
        else audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    public void ChangeResolution(int index) {
        resolutionDropdown.value = index;
        resolutionDropdown.RefreshShownValue();
    }

    public void setResolution(int index) {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void setFullScreen(bool isfullScreen) {
        isFullScreen.isOn = isfullScreen;
        Screen.fullScreen = isfullScreen;
    }
}
