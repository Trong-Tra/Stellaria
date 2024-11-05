using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Handler : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject currentPlanet;
    public bool canSpawn = true;
    private bool hasPlanet = false;
    private bool isSpawning = false;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        currentPlanet = gameManager.CreatePlanet(this.transform.position, 0);
        currentPlanet.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void SpawnNewPlanet()
    {
        if(gameManager.gameOver || !canSpawn || currentPlanet != null || isSpawning){
            return;
        }

        isSpawning = true;

        Vector3 spawnPosition = new Vector3(transform.position.x, spawnY, 0);

        int randomPlanet = Random.Range(0, 5);
        currentPlanet = gameManager.CreatePlanet(this.transform.position, randomPlanet);

        currentPlanet.GetComponent<Rigidbody2D>().gravityScale = 0;

        try
        {
            if (currentPlanet == null)
            {
                throw new System.NullReferenceException("Failed to spawn planet");
            }

            Rigidbody2D rb = currentPlanet.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                throw new System.MissingComponentException("Rigidbody2D not found on spawned planet");
            }
    
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            canSpawn = false;
            Debug.Log("New planet spawned successfully");
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError(e.Message);
            currentPlanet = null;
        }
        catch (System.MissingComponentException e)
        {
            Debug.LogError(e.Message);
            Destroy(currentPlanet);
            currentPlanet = null;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unexpected error while spawning planet: {e.Message}");
            if (currentPlanet != null)
            {
                Destroy(currentPlanet);
                currentPlanet = null;
            }
        }
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
