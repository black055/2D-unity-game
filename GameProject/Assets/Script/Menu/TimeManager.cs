using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int countTime;

    void Update()
    {
        countTime = (int)Time.realtimeSinceStartup;
    }
}
