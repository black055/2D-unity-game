using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float deadTime;
    [SerializeField]
    private Transform respawnPosition;
    [SerializeField]
    private GameObject knight;
    [SerializeField]
    private int lifeLeft = 3;
    [SerializeField]
    private Text lifeCounter, villagerCounter;
    private CinemachineVirtualCamera playerCamera;
    private bool respawn;
    private float respawnTime = -1f; 
    private SoundManager soundManager;
    private int villagerRescued;
    [SerializeField] GameObject loseMenu;

    public void Respawn() {
        if (lifeLeft > 1) {
            lifeLeft--;
            lifeCounter.text = "x " + lifeLeft.ToString();
            respawnTime = Time.time + deadTime;
            respawn = true;
            soundManager.PlaySound("RespawnCountdown");
        } else {
            soundManager.PlaySound("Lose");
            loseMenu.SetActive(true);
        }
    }

    void Start()
    {
        soundManager = SoundManager.instance;
        playerCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        lifeCounter.text = "x " + lifeLeft.ToString();
        villagerRescued = 0;
        villagerCounter.text = villagerRescued.ToString() + "/" + GameObject.FindGameObjectsWithTag("Villager").Length.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (respawn && Time.time >= respawnTime) {
            respawn = false;
            var respawnKnight = Instantiate(knight, respawnPosition.position, Quaternion.identity);
            playerCamera.m_Follow = respawnKnight.transform;
        }
    }

    public void UpdateRespawnPosition(Vector3 newRespawnPosition) {
        respawnPosition.position = newRespawnPosition;
    }

    public void RescuseVillager() {
        villagerRescued++;
        villagerCounter.text = villagerRescued.ToString() + "/" + GameObject.FindGameObjectsWithTag("Villager").Length.ToString();
        if (villagerRescued == GameObject.FindGameObjectsWithTag("Villager").Length)
            villagerCounter.color = Color.yellow; 
    }

    public bool IsFinishedStage() {
        return villagerRescued == GameObject.FindGameObjectsWithTag("Villager").Length;
    }

    public int getLifeLeft() {
        return lifeLeft;
    }
}
