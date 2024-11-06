using System.Collections;
using UnityEngine;

public class MockPlanet : MonoBehaviour
{
	public int id;
	private GameManager gameManager;
    public GameManager gameOverLogic;
    public MockHandler handler;
	private bool hasCollided = false;
	private static int triggerCounter = 0;
	private bool isCheckingGameOver = false;

	void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		handler = GameObject.Find("MockHandler").GetComponent<MockHandler>();
        gameOverLogic = GameObject.FindGameObjectWithTag("GameOverLogic").GetComponent<GameManager>();
    }

	IEnumerator DestroyPlanets(GameObject planet1, GameObject planet2)
	{
		yield return new WaitForSeconds(0.01f);
		Destroy(planet1);
		Destroy(planet2);
	}

	IEnumerator CheckGameOver()
	{
		isCheckingGameOver = true;
		yield return new WaitForSeconds(0.7f);

		if (triggerCounter > 0)
		{
			gameManager.endGame();
			Debug.LogError("Game Over");
		}

		isCheckingGameOver = false;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		// Handle planet spawning logic
		if (GetComponent<Rigidbody2D>().gravityScale != 0 && !hasCollided)
		{
			hasCollided = true;
			if (handler != null)
			{
				handler.ReadyForNewPlanet();
			}
			else
			{
				Debug.LogError("Handler reference is null!");
			}
		}

		// Increment counter and start game over check
		triggerCounter++;
		Debug.Log($"Trigger Enter - Counter: {triggerCounter}");

		if (!isCheckingGameOver)
		{
			StartCoroutine(CheckGameOver());
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		// Decrease counter but ensure it never goes below 0
		triggerCounter = Mathf.Max(0, triggerCounter - 1);
		Debug.Log($"Trigger Exit - Counter: {triggerCounter}");
	}

	private void OnDestroy()
	{
		// Ensure counter is decreased when planet is destroyed while in trigger
		if (isCheckingGameOver)
		{
			triggerCounter = Mathf.Max(0, triggerCounter - 1);
			Debug.Log($"Planet Destroyed - Counter: {triggerCounter}");
		}
	}

	/**
     * @dev new planet after merge will transform in the middle of 2 planets that collided
     * @note score increase after a successful collided (2 planets have the same id handle in if condition)
     */
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Planet otherPlanet = collision.gameObject.GetComponent<Planet>();
		if (otherPlanet != null &&
			otherPlanet.id == this.id &&
			otherPlanet.id != 9 &&
			!gameManager.gameOver &&
			this.gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
		{
			Vector3 midPoint = (transform.position + collision.transform.position) / 2f;
			GameObject newPlanet = gameManager.CreatePlanet(midPoint, this.id + 1);
			newPlanet.GetComponent<Rigidbody2D>().gravityScale = -1; // reverse gravity for new planet

			gameManager.addScore(this.id);
			StartCoroutine(DestroyPlanets(this.gameObject, collision.gameObject)); // destroy 2 collided planets
		}
	}
}