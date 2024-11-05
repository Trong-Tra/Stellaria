using System.Collections;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public int id;
    private GameManager gameManager;
    private bool hasCollided = false;
    public Handler handler;
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
        handler.currentPlanet = null;
        handler.SpawnNewPlanet();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        isTouchingTrigger = true;
        StartCoroutine(WaitForEndGame());
    }

    void OnTriggerExit2D(Collider2D collision)
    {
       isTouchingTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Planet otherPlanet = collision.gameObject.GetComponent<Planet>();
        if (otherPlanet != null && 
            otherPlanet.id == this.id && 
            this.gameObject.GetInstanceID() < collision.gameObject.GetInstanceID()) 
        {
            gameManager.CreatePlanet(this.transform.position, this.id + 1);
            gameManager.addScore(this.id);
            StartCoroutine(DestroyPlanets(this.gameObject, collision.gameObject));
            
        }
    }
}
