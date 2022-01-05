using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float deadTime;
    [SerializeField]
    private Transform respawnPosition;
    [SerializeField]
    private GameObject knight;

    private CinemachineVirtualCamera playerCamera;
    private bool respawn;
    private float respawnTime = -1f; 

    public void Respawn() {
        respawnTime = Time.time + deadTime;
        respawn = true;
    }

    void Start()
    {
        playerCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
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
}
