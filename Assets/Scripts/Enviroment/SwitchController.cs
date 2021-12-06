using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchController : MonoBehaviour
{

    [SerializeReference] private Animator animaSwitch;
    [SerializeField] private UnityEvent onSwitchPress;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Se ejecuta Invoke");
            animaSwitch.SetBool("IsPress", true);
            onSwitchPress?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Se presiona Switch");
            animaSwitch.SetBool("IsPress", false);
        }
    }

}
