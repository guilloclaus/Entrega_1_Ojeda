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
        animaDoor.SetBool("IsOpen", false);
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && IsAutomatic)
        {
            animaDoor.SetBool("IsOpen", true);
        }
        else Debug.LogWarning("LA PUERTA NO ESTA HABILITADA");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && IsAutomatic)
        {
            animaDoor.SetBool("IsOpen", false);
        }
        else Debug.LogWarning("LA PUERTA NO ESTA HABILITADA");
    }

    public void OpenDoor(bool _open)
    {
        animaDoor.SetBool("IsOpen", _open);
        if (_open) Debug.LogWarning("La puerta fue Abierta");
    }

    public void CloseDoor(bool _Close)
    {
        animaDoor.SetBool("IsOpen", !_Close);
        if (!_Close) Debug.LogWarning("La puerta fue Cerrada");
    }

    public void AutomaticDoor(bool _automatic)
    {
        IsAutomatic = _automatic;
        if (_automatic) Debug.LogWarning("La puerta fue Habilitada");
    }

}
