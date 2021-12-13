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
    [SerializeField] private Animator animaMutant;
    [SerializeField] Transform[] waypoints;
    [SerializeField] bool CanPatrol = false;
    [SerializeField] NavMeshAgent enemyAgent;



    private GameObject playerObject;
    private AudioSource audioEnemy;
    private Rigidbody rbEnemy;
    private int Energia;

    private float Speed
    {
        get { return enemyData.Velocidad * Time.deltaTime * 1000; }
    }

    bool isGrounded = true;
    bool isWalk = false;
    bool IsRoaring = false;
    bool isAttack = false;


    protected bool isDead = false;
    private bool PlayerAlert = true;
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
        if (Energia <= 0 && !isDead)
        {
            isDead = true;

            animaMutant.SetBool("IsHit", true);
            animaMutant.SetBool("IsDead", true);

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
                animaMutant.SetBool("IsWalk", false);
                animaMutant.SetBool("IsRoaring", true);
                animaMutant.SetBool("IsIdle", false);
                animaMutant.SetBool("IsRun", false);

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
            else
            {
                animaMutant.SetBool("IsWalk", false);
                animaMutant.SetBool("IsRoaring", false);
                animaMutant.SetBool("IsRun", false);
                animaMutant.SetBool("IsIdle", true);
                animaMutant.SetBool("IsDead", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (animaMutant.GetCurrentAnimatorStateInfo(0).IsName("Mutant Dying") && !animaMutant.IsInTransition(0))
        {
            rbEnemy.velocity = Vector3.zero;
            animaMutant.SetBool("IsHit", false);
        }
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
        //Vector3 direction = deltaVector.normalized;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)), enemyData.VelocidadGiro * Time.deltaTime);
        //rbEnemy.AddRelativeForce(Vector3.forward * Speed, ForceMode.Force);
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

        animaMutant.SetBool("IsWalk", true);
        animaMutant.SetBool("IsRoaring", false);
        animaMutant.SetBool("IsIdle", false);
        animaMutant.SetBool("IsRun", false);
        animaMutant.SetBool("IsAttack", false);
    }
    private void ChaseCharacter()
    {

        //Vector3 direction = (playerObject.transform.position - transform.position).normalized;
        enemyAgent.destination = playerObject.transform.position;

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)), enemyData.VelocidadGiro * Time.deltaTime);

        if (Vector3.Distance(transform.position, playerObject.transform.position) <= enemyData.RangoAtaque)
        {
            animaMutant.SetBool("IsAttack", true);
            animaMutant.SetBool("IsWalk", false);
            animaMutant.SetBool("IsRoaring", false);
            animaMutant.SetBool("IsRun", false);
            //rbEnemy.velocity = Vector3.zero;
            enemyAgent.velocity = Vector3.zero;
            isAttack = true;
        }
        else
        {
            //arbEnemy.AddRelativeForce(Vector3.forward * Speed * 1.25f, ForceMode.Force);
            animaMutant.SetBool("IsAttack", false);
            animaMutant.SetBool("IsRoaring", false);
            animaMutant.SetBool("IsWalk", false);
            animaMutant.SetBool("IsRun", true);
            isAttack = false;
        }

    }
    public void AddLife(int _life)
    {
        if (_life <= 0)
            animaMutant.SetBool("IsHit", true);

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
            playerObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * Speed * -0.25f, ForceMode.Impulse);
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
}
