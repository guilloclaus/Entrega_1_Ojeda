using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedBack = 0.5f;
    [SerializeField] private float fuerzaSalto = 1f;
    [SerializeField] private float Giro = 1f;


    [SerializeField] private Animator animaPlayer;
    [SerializeField] private AudioClip walkSound;

    private bool isGrounded = true;
    private bool isRotate = false;
    private bool isWalk = false;
    private bool isHit = false;
    private bool isPunch = false;
    private bool isDead = false;

    [SerializeField] private int lifePlayer = 100;
    [SerializeField] private int shieldPlayer = 100;
    [SerializeField] private int attackPlayer = 100;

    private AudioSource audioPlayer;
    public event Action PlayerDead;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();

        animaPlayer.SetBool("IsIdle", true);
        animaPlayer.SetBool("IsRun", false);
        animaPlayer.SetBool("IsWalkBack", false);
        animaPlayer.SetBool("IsJump", false);
        animaPlayer.SetBool("IsPunch", false);

    }

    // Update is called once per frame
    void Update()
    {
        IamAlive();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Mover();
            ControlAnimacion();
        }
    }

    private void Mover()
    {

        groundedPlayer = controller.isGrounded;
        float ejeVertical = Input.GetAxis("Vertical");
        float ejeHorizontal = Input.GetAxis("Horizontal");

        if (groundedPlayer)
        {
            moveDirection = transform.forward * ejeVertical * (ejeVertical > 0 ? speed : speedBack);
            controller.Move(moveDirection * Time.deltaTime);

            transform.Rotate(0, ejeHorizontal * Giro * Time.deltaTime, 0);
            isRotate = (ejeHorizontal != 0 ? true : false);

            if (Input.GetButtonDown("Jump") && groundedPlayer && !isRotate)
            {
                moveDirection.y += Mathf.Sqrt(fuerzaSalto * -3.0f * gravityValue);
            }

            if (Input.GetMouseButton(0))
            {
                isPunch = true;
            }
            else isPunch = false;

        }

        moveDirection.y += gravityValue * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (!audioPlayer.isPlaying && ejeVertical != 0 && groundedPlayer)
        {
            audioPlayer.PlayOneShot(walkSound, 0.5f);
        }

    }
    private void ControlAnimacion()
    {
        bool ejeVUp = Input.GetKey(KeyCode.W);
        bool ejeVDown = Input.GetKey(KeyCode.S);


        animaPlayer.SetBool("IsRun", ejeVUp || Input.GetAxis("Vertical") > 0);
        animaPlayer.SetBool("IsWalkBack", isRotate || ejeVDown);
        animaPlayer.SetBool("IsJump", !groundedPlayer);
        animaPlayer.SetBool("IsIdle", !ejeVDown && !ejeVUp && !isRotate && !animaPlayer.GetBool("IsJump"));
        animaPlayer.SetBool("IsHit", false);
        animaPlayer.SetBool("IsPunch", isPunch);
        animaPlayer.SetBool("IsDead", isDead);
        //Debug.Log($"IsIdle {animaPlayer.GetBool("IsIdle")} ; IsJump {animaPlayer.GetBool("IsJump")}; IsRun {animaPlayer.GetBool("IsRun")} ; IsWalkBack {animaPlayer.GetBool("IsWalkBack")}; giroPlayer {giroPlayer}; isGround {isGrounded}");

    }
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
        {
            
            animaPlayer.SetBool("IsHit", true);
            //gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speedBack * -0.25f, ForceMode.Impulse);
            Debug.Log($"Golpe al Player: {lifePlayer} ");
        }


        lifePlayer += _life;
    }

    public int Shield { get { return shieldPlayer; } set { shieldPlayer = value; } }
    public int Attack { get { return attackPlayer; } set { attackPlayer = value; } }
    public int Life { get { return lifePlayer; } set { lifePlayer = value; } }

    private void IamAlive()
    {
        if (Life <= 0)
        {
            Debug.Log("Player esta Muerto");
            animaPlayer.SetBool("IsDead", true);
            isDead = true;
            PlayerEvents.IsDead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isPunch)
        {
            Debug.Log("Golpe al enemigo");
            GameObject objEnemy = other.gameObject;
            objEnemy.GetComponent<EnemyController>().AddLife(-Attack);
        }
    }


}
