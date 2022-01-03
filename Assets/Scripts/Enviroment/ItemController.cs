using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    public enum ItemType { NoType, Arma, Escudo, Item, Cura, Daño };


    [SerializeField] private ItemProperty ObjData;
    [SerializeField] private bool rotate;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private ItemType itemType;


    private int premio;
    private int valor;
    private string Nombre;

    // Start is called before the first frame update
    void Start()
    {
        premio = ObjData.Premio;
        valor = ObjData.Valor;
        Nombre = ObjData.Nombre;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Collect();
        }
    }

    public void Collect()
    {


        if (collectSound) AudioSource.PlayClipAtPoint(collectSound, transform.position);


        switch (itemType)
        {
            case ItemType.NoType:
                break;
            case ItemType.Arma:
                GameManager.instance.AddPlayerAttack(valor);
                break;
            case ItemType.Cura:
                GameManager.instance.AddPlayerLife(valor);
                break;
            case ItemType.Daño:
                GameManager.instance.AddPlayerLife(valor);
                break;
            case ItemType.Escudo:
                GameManager.instance.AddPlayerShield(valor);
                break;
            case ItemType.Item:
                GameManager.instance.AddScore(premio * 2);
                break;
        }

        GameManager.instance.AddScore(premio);
        GameManager.Destroy(gameObject);

    }
}
