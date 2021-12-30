using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [SerializeField] protected EnemyProperty enemyData;
    [SerializeField] private AudioClip AlertSound;
    [SerializeField] private AudioClip DangerSound;
    [SerializeField] private AudioClip AttackSound;

    [SerializeField] protected Transform[] waypoints;    
    [SerializeField] protected NavMeshAgent enemyAgent;
    [SerializeField] protected bool CanPatrol = false;



    protected GameObject playerObject;
    protected AudioSource audioEnemy;
    protected Rigidbody rbEnemy;
    protected int Energia;

    private float Speed
    {
        get { return enemyData.Velocidad * Time.deltaTime * 1000; }
    }

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
        playerObject = GameManager.playerObject;
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
            isDead = true;
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
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
    }

    public void AddLife(int _life)
    {
        //if (_life <= 0)
        //animaMutant.SetBool("IsHit", true);
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
            //playerObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * Speed * -0.25f, ForceMode.Impulse);
            GameManager.instance.AddPlayerLife(-enemyData.Ataque);
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
}
