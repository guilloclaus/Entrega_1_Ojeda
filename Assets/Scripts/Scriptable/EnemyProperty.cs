using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]

public class EnemyProperty : ScriptableObject
{

    [SerializeField]
    private int energia;
    public int Energia { get { return energia; } }
    [SerializeField]
    private int escudo;
    public int Escudo { get { return escudo; } }

    [SerializeField]
    private int ataque;
    public int Ataque { get { return ataque; } }

    [SerializeField]
    private int velocidad;
    public int Velocidad { get { return velocidad; } }

    [SerializeField]
    private int velocidadGiro;
    public int VelocidadGiro { get { return velocidadGiro; } }

    [SerializeField]
    private float rangoVision;
    public float RangoVision { get { return rangoVision; } }

    [SerializeField]
    private float rangoAtaque;
    public float RangoAtaque { get { return rangoAtaque; } }

    [SerializeField]
    private int puntuacion;
    public int Puntuacion { get { return puntuacion; } }

}
