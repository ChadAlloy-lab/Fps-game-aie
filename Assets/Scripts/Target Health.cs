using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TargetHealth : MonoBehaviour
{

    public int maxHealth = 5;
    private int currentHealth;
    

    private GameManager gameManager;

    public GameManager GameManager { get { return gameManager;  } set { gameManager = value; } }

    // Start is called before the first frame update

    void OnEnable()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        currentHealth = maxHealth;
    }

    private void DisableTarget()
    {
        Debug.Log("Pew");
        if (GameManager != null)
        {
            gameManager.AddScore(2);
        }

        gameObject.SetActive(false);
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            DisableTarget();
        }
        
            
    }
}
