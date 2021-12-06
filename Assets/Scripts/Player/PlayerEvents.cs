using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerEvents 
{
    // Start is called before the first frame update
    public static event Action onDead;
    public static void IsDead()
    {
        onDead?.Invoke();
    }


}
