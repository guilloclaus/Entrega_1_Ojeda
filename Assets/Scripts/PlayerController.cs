using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{



    private enum Movimiento { UP, DOWN, LEFT, RIGHT, JUMP, WALK, IDLE }


    // Start is called before the first frame update
    [SerializeField] private float speedForce = 50f;
    [SerializeField] private float speedForceBack = 8f;
    [SerializeField] private float fuerzaSalto = 500f;
    [SerializeField] private float velocidadGiro = 10f;
    [SerializeField] private Animator animaPlayer;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] LayerMask groundLayer;


    float SpeedForce
    {
        get { return speedForce * Time.deltaTime * 1000; }
    }

    float SpeedForceBack
    {
        get { return speedForceBack * Time.deltaTime * 1000; }
    }

    float JumpForce
    {
        get { return fuerzaSalto * Time.deltaTime * 1000; }
    }



    private bool isGrounded = true;
    private bool isRotate = false;
    private bool isWalk = false;
    private bool isHit = false;
    private bool isPunch = false;


    [SerializeField] private int lifePlayer;
    [SerializeField] private int shieldPlayer;
    [SerializeField] private int attackPlayer;


    private Movimiento movimiento;



    private float giroPlayer = 0f;
    private AudioSource audioPlayer;



    //[SerializeField] private Animator animaPlayer = new Animator();


    private Rigidbody rbPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        audioPlayer = GetComponent<AudioSource>();

        animaPlayer.SetBool("IsIdle", true);
        animaPlayer.SetBool("IsRun", false);
        animaPlayer.SetBool("IsWalkBack", false);
        animaPlayer.SetBool("IsJump", false);
        animaPlayer.SetBool("IsPunch", false);

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
            isRotate = true;
            movimiento = Movimiento.WALK;

        }
        else if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            giroPlayer += Time.deltaTime * velocidadGiro * 10;
            transform.rotation = Quaternion.Euler(0, giroPlayer, 0);
            isRotate = true;
            movimiento = Movimiento.WALK;
        }
        else
        {
            isRotate = false;
        }

        if (ejeVertical != 0 && isGrounded && !isWalk)
        {
            if (ejeVertical > 0)
            {
                rbPlayer.AddRelativeForce(Vector3.forward * speedForce * ejeVertical, ForceMode.Force);
                movimiento = Movimiento.UP;
            }
            else if (ejeVertical < 0)
            {
                rbPlayer.AddRelativeForce(Vector3.forward * speedForceBack * ejeVertical, ForceMode.Force);
                movimiento = Movimiento.DOWN;
            }
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded && ejeVertical >= 0)
        {
            rbPlayer.AddRelativeForce(Vector3.up * JumpForce, ForceMode.Impulse);
            movimiento = Movimiento.JUMP;
        }

        if (Input.GetMouseButton(0))  
        {
            isPunch = true;
        }
        else isPunch = false;

        if (!audioPlayer.isPlaying && ejeVertical != 0 && isGrounded)
        {
            audioPlayer.PlayOneShot(walkSound, 0.5f);
        }

        movimiento = Movimiento.IDLE;
    }

    private void ControlAnimacion()
    {
        bool ejeVUp = Input.GetKey(KeyCode.W);
        bool ejeVDown = Input.GetKey(KeyCode.S);


        animaPlayer.SetBool("IsRun", ejeVUp || Input.GetAxis("Vertical") > 0);
        animaPlayer.SetBool("IsWalkBack", isRotate || ejeVDown);
        animaPlayer.SetBool("IsJump", !isGrounded);
        animaPlayer.SetBool("IsIdle", !ejeVDown && !ejeVUp && !isRotate && !animaPlayer.GetBool("IsJump"));
        animaPlayer.SetBool("IsHit", false);
        animaPlayer.SetBool("IsPunch", isPunch);

        //Debug.Log($"IsIdle {animaPlayer.GetBool("IsIdle")} ; IsJump {animaPlayer.GetBool("IsJump")}; IsRun {animaPlayer.GetBool("IsRun")} ; IsWalkBack {animaPlayer.GetBool("IsWalkBack")}; giroPlayer {giroPlayer}; isGround {isGrounded}");

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isPunch)
        {
            Debug.Log("Golpe al enemigo");

            GameObject objEnemy = other.gameObject;
            objEnemy.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speedForce * -0.25f, ForceMode.Impulse);
            objEnemy.GetComponent<MutantController>().AddLife(-10);
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



    public void AddShield(int _shield)
    {
        shieldPlayer += _shield;
    }
    public void AddAttack(int _attack)
    {
        attackPlayer += _attack;
    }
    public void AddLife(int _life)
    {
        if (_life <= 0)
            animaPlayer.SetBool("IsHit", true);


        lifePlayer += _life;
    }



    public int Shield { get { return shieldPlayer; } set { shieldPlayer = value; } }
    public int Attack { get { return attackPlayer; } set { attackPlayer = value; } }
    public int Life { get { return lifePlayer; } set { lifePlayer = value; } }




}
