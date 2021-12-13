using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeReference] private GameObject cameraMain;
    [SerializeReference] private GameObject cameraControl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            cameraMain.SetActive(false);
            cameraControl.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            cameraMain.SetActive(true);
            cameraControl.SetActive(false);
        }
    }

}
