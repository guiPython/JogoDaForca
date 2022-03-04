using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var nameOfScene = SceneManager.GetActiveScene().name;
        var statusObject = GameObject.Find("status");

        var lastWordObject = GameObject.Find("lastWord");
        lastWordObject.GetComponent<Text>().text = PlayerPrefs.GetString("lastWord");
        lastWordObject.GetComponent<Text>().color = Color.white;
        if (nameOfScene == "EndGameForLoser")
        {
            statusObject.GetComponent<Text>().color = Color.red;
            statusObject.GetComponent<Text>().text = "ENFORCADO";
        }else if(nameOfScene == "EndGameForWinner")
        {
            statusObject.GetComponent<Text>().color = Color.green;
            statusObject.GetComponent<Text>().text = "VENCEDOR";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("Jogo");
    }

    /// <summary>
    /// 
    /// </summary>
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// 
    /// </summary>
    public void ExitGame()
    {
        SceneManager.LoadScene("Credits");
    }
}
