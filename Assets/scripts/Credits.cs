using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Essa classe gerencia a cena de créditos que só tem a função de fechar o game.
/// </para>
/// </summary>
public class Credits : MonoBehaviour
{

    /// <summary>
    /// Método para fechar o Jogo
    /// </summary>
    public void ExitGame()
    {
      Application.Quit();
    }
}
