using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{

    [SerializeField] protected EnemyProperty ObjData;
    [SerializeField] private AudioClip AlertSound;
    [SerializeField] private AudioClip DangerSound;
    [SerializeField] private AudioClip AttackSound;

    [SerializeField] protected Transform[] waypoints;
    [SerializeField] protected NavMeshAgent NavAgent;
    [SerializeField] protected bool CanPatrol = false;

    [SerializeField] private AnimationsController _animationsControllers;

    protected GameObject playerObject;
    protected AudioSource audioEnemy;
    protected Rigidbody rbEnemy;
    protected int Energia;
    protected float Vision;

    protected bool isGrounded = true;
    protected bool isWalk = false;
    protected bool IsRoaring = false;
    protected bool isAttack = false;

    protected bool isHit = false;


    protected bool isDead = false;
    protected bool PlayerAlert = true;
    protected bool iSeeTheCharacter = false;

    private Vector3 _initPosition;

    int currentIndex = 0;
    bool goBack = false;

    private void Awake()
    {

        _initPosition = transform.position;
        PlayerEvents.onDead += PlayerDead;
    }

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");//GameManager.playerObject;
        rbEnemy = GetComponent<Rigidbody>();
        audioEnemy = GetComponent<AudioSource>();
        NavAgent = GetComponent<NavMeshAgent>();
        isDead = false;
        Energia = ObjData.Energia;
        Vision = ObjData.RangoVision;
    }

    void Update()
    {

        if (Energia <= 0 && !isDead)
        {
            isDead = true;
            ClearAll();
            StartCoroutine(DoDeath());
        }

        if (!isDead )
        {
            
            if (playerObject != null && Vector3.Distance(transform.position, playerObject.transform.position) <= Vision && PlayerAlert)
            {
                iSeeTheCharacter = true;
                Vision = ObjData.RangoVision * 1.5f;
            }
            else
            {
                iSeeTheCharacter = false;
                IsRoaring = false;
                isAttack = false;
                Vision = ObjData.RangoVision;
                ReturnToPosition();
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

        NavAgent.destination = waypoints[currentIndex].position;


        if (distance < ObjData.RangoAtaque)
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
        if (!isDead)
        {
            NavAgent.destination = playerObject.transform.position;
            if (Vector3.Distance(transform.position, playerObject.transform.position) <= ObjData.RangoAtaque)
            {
                NavAgent.velocity = Vector3.zero;
                StartCoroutine(DoAttack());
                isAttack = true;
            }
            else
            {
                isAttack = false;
            }
        }
    }
    public void AddLife(int _life)
    {
        if (_life <= 0 && !isDead)
        {
            isHit = true;
            Debug.Log($"Golpe al Enemigo: {Energia} ");
            StartCoroutine(DoHit());
        }

        Energia += _life;
    }
    private void PlayerDead()
    {
        PlayerAlert = false;
        ClearAll();
        ReturnToPosition();
    }

    private void ReturnToPosition()
    {

        if (Vector3.Distance(transform.position, _initPosition) <= ObjData.RangoAtaque)
            NavAgent.velocity = Vector3.zero;
        else
            NavAgent.destination = _initPosition;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isAttack)
        {
            playerObject.GetComponent<PlayerController>().AddLife(-ObjData.Ataque);
        }
    }
    private void OnDrawGizmos()
    {

        if (iSeeTheCharacter)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, ObjData.RangoAtaque);
        Gizmos.DrawWireSphere(transform.position, Vision);
    }

    //Ejecucion de animaciones

    IEnumerator DoAttack()
    {
        _animationsControllers.Attack();
        yield return new WaitForSeconds(Random.value * 0.1f);

        StopAllCoroutines();
    }
    IEnumerator DoHit()
    {
        _animationsControllers.Hit();
        yield return new WaitForSeconds(Random.value * 0.1f);

        isHit = false;
    }
    IEnumerator DoMove()
    {
        _animationsControllers.SetMovingState(true);
        yield return new WaitForSeconds(4.2f);
        _animationsControllers.SetMovingState(false);
    }
    IEnumerator DoDeath()
    {
        _animationsControllers.SetDead();
        yield return new WaitForSeconds(Random.value * 0.1f);
    }
    void ClearAll()
    {
        StopAllCoroutines();
        _animationsControllers.ClearAll();
    }


}
