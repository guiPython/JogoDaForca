using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// <para>
/// Essa classe gerencia toda a cena do final do jogo.
/// </para>
/// <para>
/// Atualiza os GameObject status e lastWord.
/// </para>
/// <para>
/// Carrega as cenas de Menu, Créditos ou Jogo dependendo do input do usuário.
/// </para>
/// </summary>
public class EndGame : MonoBehaviour
{
    /// <summary>
    /// <para>
    /// Encontra o GameObject status.
    /// </para>
    /// <para>
    /// Injeta no componente Text o texto "ENFORCADO" na cor vermelha.
    /// </para>
    /// </summary>
    private void SetStatusForLoser()
    {
        var statusObject = GameObject.Find("status");
        statusObject.GetComponent<Text>().color = Color.red;
        statusObject.GetComponent<Text>().text = "ENFORCADO";
    }

    /// <summary>
    /// <para>
    /// Encontra o GameObject status.
    /// </para>
    /// <para>
    /// Injeta no componente Text o texto "VENCEDOR" na cor verde.
    /// </para>
    /// </summary>
    private void SetStatusForWinner()
    {
        var statusObject = GameObject.Find("status");
        statusObject.GetComponent<Text>().color = Color.green;
        statusObject.GetComponent<Text>().text = "VENCEDOR";
    }

    /// <summary>
    /// <para>
    /// Encontra o GameObject lastWord e pega o valor de PlayerPrefs lastword.
    /// </para>
    /// <para>
    /// Injeta no componente Text o valor da PlayerPrefs lastWord na cor branca.
    /// </para>
    /// </summary>
    private void SetLastWordObject()
    {
        var lastWordObject = GameObject.Find("lastWord");
        lastWordObject.GetComponent<Text>().text = PlayerPrefs.GetString("lastWord");
        lastWordObject.GetComponent<Text>().color = Color.white;
    }

    // Start is called before the first frame update
    void Start()
    {
        var nameOfScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("score", 0);
        SetLastWordObject();
        if (nameOfScene == "EndGameForLoser") SetStatusForLoser();
        else if (nameOfScene == "EndGameForWinner") SetStatusForWinner();

    }


    /// <summary>
    /// Carrega a cena do Jogo novamente.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("Jogo");
    }

    /// <summary>
    /// Carrega a cena do Menu novamente.
    /// </summary>
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Carrega a cena de Créditos.
    /// </summary>
    public void ExitGame()
    {
        SceneManager.LoadScene("Credits");
    }
}
