using UnityEngine;
using UnityEngine.UIElements;


public class HUDController : MonoBehaviour
{

    [SerializeField] private string life;
    [SerializeField] private string shield;
    [SerializeField] private string attack;

    [SerializeField] private GameObject objPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player();
    }


    private void Player()
    {
        life = objPlayer.GetComponent<PlayerController>().Life.ToString();
        shield = objPlayer.GetComponent<PlayerController>().Shield.ToString();
        attack = objPlayer.GetComponent<PlayerController>().Attack.ToString();

    }



}
