using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{


    [SerializeField] private GameObject[] Enemies;
    [SerializeField] private GameObject[] Respawns;

    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject PlayerIni;





    // Start is called before the first frame update
    void Awake()
    {    
     
        
        foreach (GameObject objEnemy in Enemies)
        {

            foreach (GameObject objRespawn in Respawns)
            {
                GameObject enemy = Instantiate(objEnemy, objRespawn.transform.position, objEnemy.transform.rotation);                
                enemy.GetComponent<Rigidbody>().AddForce(objRespawn.transform.TransformDirection(Vector3.forward) * 10f, ForceMode.Impulse);
            }

        }


    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
