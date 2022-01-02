using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mutant_Araña : AnimationsController
{

    public override void Inicializar()
    {
        _animator.SetInteger("battle", 1);
    }

    public override void SetAlert(bool val)
    {
        _animator.SetInteger("battle", (val ? 1 : 0));
    }

    public override void SetMovingState(bool val)
    {

        //_animator.SetInteger("battle", (val ? 1 : 0));
        _animator.SetInteger("moving", (val ? 1 : 0));
    }

    public override void SetDead()
    {
        _animator.SetInteger("IsDead", 1);
    }
    public override void ClearDead()
    {
        _animator.SetInteger("IsDead", 3);
    }
    public override void Attack()
    {
        SetAlert(true);
        int rndAtack = Random.Range(1, 4);
        _animator.SetInteger("Attack", rndAtack);
        _animator.SetInteger("Hit", 0);
    }

    public override void Hit()
    {
        SetAlert(true);
        int rndHit = Random.Range(1, 3);
        _animator.SetInteger("Hit", rndHit);
       
    }

    public override bool IsMoving
    {
        get { return (_animator.GetInteger("moving") == 1 ? true : false); }
    }

    public override void ClearAll()
    {
        _animator.SetInteger("Attack", 0);
        _animator.SetInteger("Hit", 0);
    }


}
