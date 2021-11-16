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

        animaPlayer.SetBool("IsIdle", true);
        animaPlayer.SetBool("IsRun", false);
        animaPlayer.SetBool("IsWalkBack", false);
        animaPlayer.SetBool("IsJump", false);

    }


    // Update is called once per frame
    void Update()
    {
        //isGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {

        
        Mover();
        ControlAnimacion();
    }   

    private void Mover()
    {
        float ejeVertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A) && isGrounded)
        {
            giroPlayer -= Time.deltaTime * velocidadGiro * 10;
            transform.rotation = Quaternion.Euler(0, giroPlayer, 0);
        }
        else if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            giroPlayer += Time.deltaTime * velocidadGiro * 10;
            transform.rotation = Quaternion.Euler(0, giroPlayer, 0);
        }


        if (ejeVertical != 0 && isGrounded)
        {
            rbPlayer.AddRelativeForce(Vector3.forward * velocidadPlayer * ejeVertical, ForceMode.Force);
            //transform.Translate(velocidadPlayer * Time.deltaTime * new Vector3(0, 0, ejeVertical));
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rbPlayer.AddRelativeForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }



    }

    private void ControlAnimacion()
    {

        float ejeVertical = Input.GetAxis("Vertical");
        bool salto = Input.GetKey(KeyCode.Space);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        { 
            animaPlayer.SetBool("IsWalkBack", true);
        }
        else
        {
            animaPlayer.SetBool("IsWalkBack", false);
        }
        
        if (ejeVertical != 0)
        {
            animaPlayer.SetBool("IsRun", ejeVertical > 0);
            animaPlayer.SetBool("IsWalkBack", ejeVertical < 0);
        }
        else
        {
            animaPlayer.SetBool("IsRun", false);
        }

        if (salto)
            animaPlayer.SetBool("IsJump", true);
        else
            animaPlayer.SetBool("IsJump", false);


        animaPlayer.SetBool("IsIdle", ejeVertical == 0 && !salto && !Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D));


        Debug.Log($"IsIdle {animaPlayer.GetBool("IsIdle")} ; IsJump {animaPlayer.GetBool("IsJump")}; IsRun {animaPlayer.GetBool("IsRun")} ; IsBack {animaPlayer.GetBool("IsWalkBack")}; giroPlayer {giroPlayer}; isGround {isGrounded}");



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
    //    if (Physics.Raycast(transform.position, Vector3.down, 0.05f, groundLayer))
    //    {         
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}


}
