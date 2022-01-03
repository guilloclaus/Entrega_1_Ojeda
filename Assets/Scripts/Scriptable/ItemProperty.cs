using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data", menuName = "Item Data")]

public class ItemProperty : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
    private int premio;
    public int Premio { get { return premio; } }
    [SerializeField]
    private int valor;
    public int Valor { get { return valor; } }
    [SerializeField]
    private string nombre;
    public string Nombre { get { return nombre; } }
}
