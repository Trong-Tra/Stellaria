using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject currentPlanet;
    private bool hasPlanet = false;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        currentPlanet = gameManager.CreatePlanet(this.transform.position, 0);
        currentPlanet.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
}
