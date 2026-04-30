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

    private void DisableCoin()
    {
        Debug.Log("Bling");
        if (GameManager != null)
        {
            gameManager.AddScore(1);
        }

        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        DisableCoin();
        
    }





}
