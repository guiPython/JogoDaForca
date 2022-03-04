using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    void Update()
    {
        
    }

    public void StartJogoDaForca()
    {
        SceneManager.LoadScene("Jogo");
    }
}
