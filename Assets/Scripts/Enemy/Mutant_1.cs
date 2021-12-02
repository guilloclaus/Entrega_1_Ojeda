using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutant_1 : EnemyController
{

    [SerializeField] protected GameObject humoEnemigo;

    private void FixedUpdate()
    {
        humoEnemigo.SetActive(base.iSeeTheCharacter && !base.isDead);
    }


}
