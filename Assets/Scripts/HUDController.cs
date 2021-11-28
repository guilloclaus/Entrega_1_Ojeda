using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{

    [SerializeField] private Text life;
    [SerializeField] private Text shield;
    [SerializeField] private Text attack;

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
        life.text = objPlayer.GetComponent<PlayerController>().Life.ToString();
        shield.text = objPlayer.GetComponent<PlayerController>().Shield.ToString();
        attack.text = objPlayer.GetComponent<PlayerController>().Attack.ToString();

    }



}
