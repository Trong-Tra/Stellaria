using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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

    public void SpawnNewPlanet()
    {
        int randomPlanet = Random.Range(0, 5);
        currentPlanet = gameManager.CreatePlanet(this.transform.position, randomPlanet);

        currentPlanet.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void Update()
    {
        /**
         * @dev this is temporary solution and var
         * @todo design game and fix var
         * @todo developing new handler logic
         */
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!gameManager.gameOver && mousePosition.x < 2f && mousePosition.x > -2f)
        {
            mousePosition.z = 0;
            mousePosition.y = 0;
            transform.position = mousePosition;
        }

        if (currentPlanet != null && currentPlanet.GetComponent<Rigidbody2D>().gravityScale == 0)
        {
            currentPlanet.transform.position = this.transform.position;
        }

        if (!gameManager.gameOver && Input.GetMouseButton(0))
        {
            currentPlanet.GetComponent<Rigidbody2D>().gravityScale = -1;
        }
    }
}
