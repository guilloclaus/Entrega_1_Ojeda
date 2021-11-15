using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float velocidadPlayer = 1000f;
    [SerializeField] private float fuerzaSalto = 500f;
    [SerializeField] private float velocidadGiro = 10f;    
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private Animator animaPlayer;

    private bool isGrounded = true;
    private float giroPlayer = 0f;


    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";


    //[SerializeField] private Animator animaPlayer = new Animator();


    private Rigidbody rbPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();

    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isGrounded)
            Mover();
    }

    private void Mover()
    {

        float ejeHorizontal = Input.GetAxis("Horizontal");
        float ejeVertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A))
        {
            giroPlayer -= Time.deltaTime * velocidadGiro * 10;
            transform.rotation = Quaternion.Euler(0, giroPlayer, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            giroPlayer += Time.deltaTime * velocidadGiro * 10;
            transform.rotation = Quaternion.Euler(0, giroPlayer, 0);
        }


        if (ejeHorizontal != 0 || ejeVertical != 0)
        {
            rbPlayer.AddRelativeForce(Vector3.forward * velocidadPlayer * ejeVertical, ForceMode.Force);
         //   rbPlayer.AddRelativeForce(Vector3.right * velocidadPlayer * ejeHorizontal, ForceMode.Force);
        }

        if (Input.GetKey(KeyCode.Space))
            rbPlayer.AddRelativeForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {

            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {

            isGrounded = false;
        }
    }

    //private bool IsGrounded()
    //{
    //    if (Physics.Raycast(transform.position, Vector3.down, 0.01f, groundLayer))
    //    {
    //        Debug.Log("SI el Piso");
    //        return true;

    //    }
    //    else
    //    {
    //        Debug.Log("NO el Piso");
    //        return false;
    //    }
    //}


}
