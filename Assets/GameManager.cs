using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject planetPrefab;
    public GameObject[] planetPrefabs;
    public int score = 0;
    public TMP_Text scoreText;

    public void addScore(int id)
    {
        switch(id)
        {
            case 0:
                score += 1;
                break;
            case 1:
                score += 2;
                break;
            case 2:
                score += 4;
                break;
            case 3:
                score += 8;
                break;
            case 4:
                score += 16;
                break;
            case 5:
                score += 32;
                break;
            case 6:
                score += 64;
                break;
            case 7:
                score += 128;
                break;
            case 8:
                score += 256;
                break;
            case 9:
                score += 512;
                break;
            case 10:
                score += 1024;
                break;
        }
        scoreText.text = "Score: " + score.ToString();
    }
}
