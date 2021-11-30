using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MutantController : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speedForce;
    float Speed
    {
        get { return speedForce * Time.deltaTime * 1000; }
    }

    [SerializeField] float rotationSpeed;
    [SerializeField] float minimumDistance;
    [SerializeField] float rangeOfView;
    [SerializeField] private AudioClip AlertSound;
    [SerializeField] private AudioClip DangerSound;
    [SerializeField] private AudioClip AttackSound;
    [SerializeField] private Animator animaMutant;
    [SerializeField] private int life;
    [SerializeField] private int shield;
    [SerializeField] private int attack;
    [SerializeField] bool CanPatrol = false;



    bool isGrounded = true;
    bool isRotate = false;
    bool isWalk = false;
    bool IsRoaring = false;
    bool isAttack = false;
    bool isPunch = false;

    bool isDead = false;
    bool isDisaper = false;


    int currentIndex = 0;
    bool goBack = false;
    float i = 500f;
    bool iSeeTheCharacter = false;

    GameObject playerObject;



    AudioSource audioEnemy;
    Rigidbody rbEnemy;


    private void Start()
    {
        playerObject = GameManager.playerObject;

        rbEnemy = GetComponent<Rigidbody>();
        audioEnemy = GetComponent<AudioSource>();

        isDead = false;
    }

    void Update()
    {
        if (life <= 0 && !isDead)
        {
            isDead = true;

            animaMutant.SetBool("IsHit", true);
            animaMutant.SetBool("IsDead", true);

        }


        if (!isDead)
        {
            if (Vector3.Distance(transform.position, playerObject.transform.position) <= rangeOfView)
            {
                iSeeTheCharacter = true;
            }
            else
            {
                iSeeTheCharacter = false;
                IsRoaring = false;
                isAttack = false;
            }
            if (iSeeTheCharacter)
            {
                animaMutant.SetBool("IsWalk", false);
                animaMutant.SetBool("IsRoaring", true);
                animaMutant.SetBool("IsIdle", false);

                if (!isAttack && !IsRoaring)
                {
                    if (animaMutant.GetCurrentAnimatorStateInfo(0).IsName("Mutant Roaring") && !animaMutant.IsInTransition(0))
                    {
                        IsRoaring = true;
                        doTask();
                    }
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
        //ControlAnimacion();

        if (animaMutant.GetCurrentAnimatorStateInfo(0).IsName("Mutant Dying") && !animaMutant.IsInTransition(0))
        {
            rbEnemy.velocity = Vector3.zero;
            animaMutant.SetBool("IsHit", false);
        }

        if (isDead)
        {
            i -= 1;
            if (i <= 0)
                Destroy(gameObject);
        }


    }

    async void doTask()
    {
        await Task.Delay(TimeSpan.FromSeconds(5f));
    }

    private void Patrol()
    {


        Vector3 deltaVector = waypoints[currentIndex].position - transform.position;
        Vector3 direction = deltaVector.normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)), rotationSpeed * Time.deltaTime);
        rbEnemy.AddRelativeForce(Vector3.forward * Speed, ForceMode.Force);
        float distance = deltaVector.magnitude;

        if (distance < 1.5f)
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
    }

    private void ChaseCharacter()
    {

        Vector3 direction = (playerObject.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)), rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, playerObject.transform.position) <= minimumDistance)
        {
            animaMutant.SetBool("IsAttack", true);
            animaMutant.SetBool("IsWalk", false);
            animaMutant.SetBool("IsRoaring", false);
            animaMutant.SetBool("IsRun", false);
            rbEnemy.velocity = Vector3.zero;
            isAttack = true;
        }
        else
        {
            rbEnemy.AddRelativeForce(Vector3.forward * Speed * 1.25f, ForceMode.Force);
            animaMutant.SetBool("IsAttack", false);
            animaMutant.SetBool("IsRoaring", false);
            animaMutant.SetBool("IsWalk", false);
            animaMutant.SetBool("IsRun", true);
            isAttack = false;
        }

    }

    private void ControlAnimacion()
    {

        animaMutant.SetBool("IsWalk", isWalk);
        animaMutant.SetBool("IsIdle", !isWalk && !isAttack);
        animaMutant.SetBool("IsRoaring", isAttack);

        Debug.Log($"Walk {animaMutant.GetBool("IsWalk")} Idle {animaMutant.GetBool("IsIdle")} ");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * Speed * -0.25f, ForceMode.Impulse);

            GameManager.instance.AddPlayerLife(-attack);

        }
    }

    public void AddLife(int _life)
    {
        if (_life <= 0)
            animaMutant.SetBool("IsHit", true);

        life += _life;
    }


    private void OnDrawGizmos()
    {

        if (iSeeTheCharacter)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }


        Gizmos.DrawWireSphere(transform.position, minimumDistance);

        Gizmos.DrawWireSphere(transform.position, rangeOfView);
    }
}
