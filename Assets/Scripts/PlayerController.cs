using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float velocidadPlayer = 1000f;
    [SerializeField] private float fuerzaSalto = 500f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] LayerMask groundLayer;
    private bool isGrounded = true;


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
        if (IsGrounded())
            Mover();
    }

    private void Mover()
    {

        float ejeHorizontal = Input.GetAxis("Horizontal");
        float ejeVertical = Input.GetAxis("Vertical");

        if (ejeHorizontal != 0 || ejeVertical != 0)
        {
            rbPlayer.AddRelativeForce(Vector3.forward * velocidadPlayer * ejeVertical, ForceMode.Force);
            rbPlayer.AddRelativeForce(Vector3.right * velocidadPlayer * ejeHorizontal, ForceMode.Force);
        }

        if (Input.GetKey(KeyCode.Space))
            rbPlayer.AddRelativeForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {

    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {

    //        isGrounded = false;
    //    }
    //}

    private bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f, groundLayer))
        {
            Debug.Log("SI el Piso");
            return true;

        }
        else
        {
            Debug.Log("NO el Piso");
            return false;
        }
    }


}
