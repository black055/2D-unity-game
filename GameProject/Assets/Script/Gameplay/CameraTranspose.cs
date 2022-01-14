using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTranspose : MonoBehaviour
{
  [SerializeField]
  CinemachineVirtualCamera vcam;
  [SerializeField]
  float prevX, prevY, postX, postY;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (vcam != null && other.tag == "Knight")
    {
      vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = postX;
      vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = postY;
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (vcam != null && other.tag == "Knight")
    {
      vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = prevX;
      vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = prevY;
    }
  }
}
