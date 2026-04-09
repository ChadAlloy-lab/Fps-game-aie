using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    public int maxHealth = 1;
    private int currentHealth;


    private GameManager gameManager;

    public GameManager GameManager { get { return gameManager; } set { gameManager = value; } }
    

    // Start is called before the first frame update

    void OnEnable()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        currentHealth = maxHealth;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Bling");
            GameManager.AddScore(1);
            Destroy(gameObject);
        }
            

            
        
    }





}
