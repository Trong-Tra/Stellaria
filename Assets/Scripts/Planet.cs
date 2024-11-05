using System.Collections;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public int id;
    private GameManager gameManager;
    public Handler handler;
    private bool hasCollided = false;
    public bool isTouchingTrigger = false;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        handler = GameObject.Find("Handler").GetComponent<Handler>();
    }

    IEnumerator DestroyPlanets(GameObject planet1, GameObject planet2)
    {
        yield return new WaitForSeconds(0.01f);
        Destroy(planet1);
        Destroy(planet2);
    }

    IEnumerator WaitForEndGame()
    {
        yield return new WaitForSeconds(5f);
        if(isTouchingTrigger == true)
        {
            gameManager.endGame();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
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
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!isTouchingTrigger)
        {
            isTouchingTrigger = true;
            StartCoroutine(WaitForEndGame());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
       isTouchingTrigger = false;
       StopAllCoroutines();
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
