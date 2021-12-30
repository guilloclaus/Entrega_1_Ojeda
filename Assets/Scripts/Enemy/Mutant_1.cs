using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutant_1 : EnemyController
{

    [SerializeField] private Animator animaMutant;
    [SerializeField] protected GameObject humoEnemigo;

    private void FixedUpdate()
    {
        humoEnemigo.SetActive(base.iSeeTheCharacter && !base.isDead);

        if (animaMutant.GetCurrentAnimatorStateInfo(0).IsName("Mutant Dying") && !animaMutant.IsInTransition(0))
        {
            rbEnemy.velocity = Vector3.zero;
            animaMutant.SetBool("IsHit", false);
        }
    }


    public override void ChaseCharacter()
    {
        enemyAgent.destination = playerObject.transform.position;

        if (Vector3.Distance(transform.position, playerObject.transform.position) <= enemyData.RangoAtaque)
        {
            animaMutant.SetBool("IsAttack", true);
            animaMutant.SetBool("IsWalk", false);
            animaMutant.SetBool("IsRoaring", false);
            animaMutant.SetBool("IsRun", false);
            enemyAgent.velocity = Vector3.zero;
            isAttack = true;
        }
        else
        {
            animaMutant.SetBool("IsAttack", false);
            animaMutant.SetBool("IsRoaring", false);
            animaMutant.SetBool("IsWalk", false);
            animaMutant.SetBool("IsRun", true);
            isAttack = false;
        }
    }

    public override void AnimaControl()
    {

        if (Energia <= 0 && !isDead)
        {
            animaMutant.SetBool("IsHit", true);
            animaMutant.SetBool("IsDead", true);
        }
        if (!isDead)
        {
            if (iSeeTheCharacter && PlayerAlert)
            {
                animaMutant.SetBool("IsWalk", false);
                animaMutant.SetBool("IsRoaring", true);
                animaMutant.SetBool("IsIdle", false);
                animaMutant.SetBool("IsRun", false);
            }
            else if (CanPatrol)
            {
                animaMutant.SetBool("IsWalk", true);
                animaMutant.SetBool("IsRoaring", false);
                animaMutant.SetBool("IsIdle", false);
                animaMutant.SetBool("IsRun", false);
                animaMutant.SetBool("IsAttack", false);
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

}
