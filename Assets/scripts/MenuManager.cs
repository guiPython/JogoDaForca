using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// <para>
/// Essa classe gerencia a cena de menu que só pode carregar a cena do jogo;
/// </para>
/// </summary>
public class MenuManager : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    /// <summary>
    /// Carrega a cena do Jogo
    /// </summary>
    public void StartJogoDaForca()
    {
        SceneManager.LoadScene("Jogo");
    }
}
