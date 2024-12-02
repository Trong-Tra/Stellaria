using UnityEngine;
using System.Collections;

public class Handler : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject currentPlanet;
    public bool canSpawn = true;
    private bool isSpawning = false;
    public float spawnY = -4f;
    public float spawnDelay = 0.5f;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("Handler initialized");
        SpawnNewPlanet();
    }

    public void ReadyForNewPlanet()
    {
        Debug.Log("ReadyForNewPlanet called");
        if (!isSpawning)
        {
            StartCoroutine(SpawnWithDelay());
        }
    }

    private IEnumerator SpawnWithDelay()
    {
        currentPlanet = null;
        canSpawn = false; // Prevent spawning during delay

        yield return new WaitForSeconds(spawnDelay);

        canSpawn = true;
        SpawnNewPlanet();
    }

    public void SpawnNewPlanet()
    {
        if (gameManager.gameOver || !canSpawn || currentPlanet != null || isSpawning)
        {
            Debug.Log("Can not spawn new planet");
            return;
        }

        isSpawning = true;
        Debug.Log("Spawning new planet");

        // Set spawn position
        Vector3 spawnPosition = new Vector3(transform.position.x, spawnY, 0);

        int randomPlanet;
        if (gameManager.isCheatMode) {
            randomPlanet = 9;
        } else {
            randomPlanet = Random.Range(0, 5);
        }
        currentPlanet = gameManager.CreatePlanet(spawnPosition, randomPlanet);

        if (currentPlanet != null)
        {
            Rigidbody2D rb = currentPlanet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                canSpawn = false;
                Debug.Log("New planet spawned successfully");
            }
            else
            {
                Debug.LogError("Rigidbody2D not found on spawned planet");
                Destroy(currentPlanet);
                currentPlanet = null;
            }
        }
        else
        {
            Debug.LogError("Failed to spawn planet");
        }

        isSpawning = false;
    }

    private void Update()
    {
        if (gameManager.gameOver) return;
        HandleMouseMovement();
        HandlePlanetPosition();
        HandlePlanetRelease();
    }

    private void HandleMouseMovement()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < 2f && mousePosition.x > -2f)
        {
            mousePosition.z = 0;
            mousePosition.y = transform.position.y;
            transform.position = mousePosition;
        }
    }

    private void HandlePlanetPosition()
    {
        if (currentPlanet != null)
        {
            Rigidbody2D rb = currentPlanet.GetComponent<Rigidbody2D>();
            if (rb != null && rb.gravityScale == 0)
            {
                Vector3 newPosition = transform.position;
                newPosition.y = spawnY;
                currentPlanet.transform.position = newPosition;
            }
        }
    }

    private void HandlePlanetRelease()
    {
        if (Input.GetMouseButtonDown(0) && currentPlanet != null)
        {
            Debug.Log("Releasing planet");
            Rigidbody2D rb = currentPlanet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = -1;
                rb.velocity = Vector2.zero;
            }
        }
    }
}