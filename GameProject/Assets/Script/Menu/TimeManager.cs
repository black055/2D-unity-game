using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int prevTime;
    public int countTime;
    private void Start() {
        countTime = (int)Time.realtimeSinceStartup;
        prevTime = countTime;
        // Debug.Log(prevTime);

    }
    void Update()
    {
        countTime = (int)Time.realtimeSinceStartup;
        // Debug.Log(countTime);
    }
}
