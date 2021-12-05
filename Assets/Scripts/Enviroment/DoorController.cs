using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeReference] private Animator animaDoor;
    [SerializeReference] private bool IsAutomatic;


    // Start is called before the first frame update
    void Start()
    {
        animaDoor.SetBool("IsIdle", true);
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && IsAutomatic)
        {
            animaDoor.SetBool("IsClose", false);
            animaDoor.SetBool("IsOpen", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && IsAutomatic)
        {
            animaDoor.SetBool("IsClose", true);
            animaDoor.SetBool("IsOpen", false);
        }
    }

    public void OpenDoor(bool _open)
    {
        animaDoor.SetBool("IsClose", !_open);
        animaDoor.SetBool("IsOpen", _open);
    }


}
