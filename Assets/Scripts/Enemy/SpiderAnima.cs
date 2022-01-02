using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnima : AnimationsController
{


    public override void Inicializar() 
    {
        MovingHash = Animator.StringToHash("IsMoving");
        AttackHash = Animator.StringToHash("Attack");
        HitHash = Animator.StringToHash("Hit");
        IsDeadHash = Animator.StringToHash("IsDead");
    }

}
