using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{

    [SerializeField] protected EnemyProperty enemyData;
    [SerializeField] private AudioClip AlertSound;
    [SerializeField] private AudioClip DangerSound;
    [SerializeField] private AudioClip AttackSound;

    [SerializeField] protected Transform[] waypoints;
    [SerializeField] protected NavMeshAgent enemyAgent;
    [SerializeField] protected bool CanPatrol = false;

    [SerializeField] private AnimationsController _animationsControllers;

    protected GameObject playerObject;
    protected AudioSource audioEnemy;
    protected Rigidbody rbEnemy;
    protected int Energia;

    protected bool isGrounded = true;
    protected bool isWalk = false;
    protected bool IsRoaring = false;
    protected bool isAttack = false;


    protected bool isDead = false;
    protected bool PlayerAlert = true;
    protected bool iSeeTheCharacter = false;

    int currentIndex = 0;
    bool goBack = false;

    private void Awake()
    {
        PlayerEvents.onDead += PlayerDead;
    }

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");//GameManager.playerObject;
        rbEnemy = GetComponent<Rigidbody>();
        audioEnemy = GetComponent<AudioSource>();
        enemyAgent = GetComponent<NavMeshAgent>();
        isDead = false;
        Energia = enemyData.Energia;
    }

    void Update()
    {

        AnimaControl();

        if (Energia <= 0 && !isDead)
        {
            StartCoroutine(DoDeath()); 
        }

        if (!isDead)
        {
            if (Vector3.Distance(transform.position, playerObject.transform.position) <= enemyData.RangoVision && PlayerAlert)
            {
                iSeeTheCharacter = true;
            }
            else
            {
                iSeeTheCharacter = false;
                IsRoaring = false;
                isAttack = false;
            }

            if (iSeeTheCharacter && PlayerAlert)
            {
                if (!isAttack && !IsRoaring)
                {
                    IsRoaring = true;
                }

                if (IsRoaring)
                {
                    ChaseCharacter();
                }
            }
            else if (CanPatrol)
            {
                Patrol();
            }
        }
    }
    private void FixedUpdate()
    {
        if (isDead)
        {
            StartCoroutine(WaitingForDead());
        }
    }
    IEnumerator WaitingForDead()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    private void Patrol()
    {
        Vector3 deltaVector = waypoints[currentIndex].position - transform.position;
        float distance = deltaVector.magnitude;

        enemyAgent.destination = waypoints[currentIndex].position;


        if (distance < enemyData.RangoAtaque)
        {
            if (currentIndex >= waypoints.Length - 1)
            {
                goBack = true;
            }
            else if (currentIndex <= 0)
            {
                goBack = false;
            }

            if (!goBack)
            {
                currentIndex++;
            }
            else currentIndex--;
        }
    }
    public virtual void ChaseCharacter()
    {
        enemyAgent.destination = playerObject.transform.position;
        if (Vector3.Distance(transform.position, playerObject.transform.position) <= enemyData.RangoAtaque)
        {
            enemyAgent.velocity = Vector3.zero;
            StartCoroutine(DoAttack());
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
    }
    public void AddLife(int _life)
    {
        if (_life <= 0)
        {
            Debug.Log($"Golpe al Enemigo: {Energia} ");
            StartCoroutine(DoHit()); 
        }

        Energia += _life;
    }
    private void PlayerDead()
    {
        PlayerAlert = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isAttack)
        {            
            playerObject.GetComponent<PlayerController>().AddLife(-enemyData.Ataque);
        }
    }


    private void OnDrawGizmos()
    {

        if (iSeeTheCharacter)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;


        Gizmos.DrawWireSphere(transform.position, enemyData.RangoAtaque);
        Gizmos.DrawWireSphere(transform.position, enemyData.RangoVision);
    }
    public virtual void AnimaControl() { }


    //Ejecucion de animaciones

    IEnumerator DoAttack()
    {
        _animationsControllers.Attack();
        yield return new WaitForSeconds(Random.value * 0.1f);
    }

    IEnumerator DoHit()
    {
        _animationsControllers.Hit();
        yield return new WaitForSeconds(Random.value * 0.1f);
    }

    IEnumerator DoMove()
    {
        _animationsControllers.SetMovingState(true);
        yield return new WaitForSeconds(4.2f);
        _animationsControllers.SetMovingState(false);
    }

    IEnumerator DoDeath()
    {
        _animationsControllers.ClearDead();
        _animationsControllers.SetDead();
        yield return new WaitForSeconds(Random.value * 0.1f);

        //yield return new WaitForSeconds(1.2f);
        //_animationsControllers.ClearDead();
        isDead = true;

    }

    void ClearAll()
    {
        StopAllCoroutines();

        _animationsControllers.ClearDead();
        _animationsControllers.SetMovingState(false);

    }

    private void acciones()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ClearAll();
            StartCoroutine(DoMove());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ClearAll();
            StartCoroutine(DoHit());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ClearAll();
            StartCoroutine(DoDeath());
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ClearAll();
            StartCoroutine(DoAttack());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearAll();
        }
    }





}
