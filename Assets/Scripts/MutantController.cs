using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantController : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed;
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



    int currentIndex = 0;
    bool goBack = false;

    bool iSeeTheCharacter = false;

    GameObject playerObject;



    AudioSource audioEnemy;
    Rigidbody rbEnemy;


    private void Start()
    {
        playerObject = GameManager.playerObject;

        rbEnemy = GetComponent<Rigidbody>();
        audioEnemy = GetComponent<AudioSource>();
    }

    void Update()
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
            animaMutant.SetBool("IsIdle", true);
        }

    }

    private void FixedUpdate()
    {
        //ControlAnimacion();
    }

    private void Patrol()
    {


        Vector3 deltaVector = waypoints[currentIndex].position - transform.position;
        Vector3 direction = deltaVector.normalized;
        //transform.rotation = Quaternion.LookRotation(deltaVector, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)), rotationSpeed * Time.deltaTime);
        rbEnemy.AddRelativeForce(Vector3.forward * speed * 1, ForceMode.Force);
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
    }

    private void ChaseCharacter()
    {

        Vector3 direction = (playerObject.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)), rotationSpeed * Time.deltaTime);
        rbEnemy.AddRelativeForce(Vector3.forward * speed * 0.7f, ForceMode.Force);

        if (Vector3.Distance(transform.position, playerObject.transform.position) <= minimumDistance)
        {
            animaMutant.SetBool("IsAttack", true);
            animaMutant.SetBool("IsWalk", false);
            animaMutant.SetBool("IsRoaring", false);
            animaMutant.SetBool("IsIdle", false);
            isAttack = true;

        }
        else
        {
            animaMutant.SetBool("IsAttack", false);
            animaMutant.SetBool("IsWalk", true);
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

            playerObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed * -0.25f, ForceMode.Impulse);
            GameManager.instance.AddPlayerLife(-attack);
        }
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
