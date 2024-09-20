using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public int id;
    private GameManager game;
    private bool hasCollided = false;
    public Handler handler;
    public bool isTouchingTrigger = false;
    void Start()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
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
            game.endGame();
        }
    }
}
