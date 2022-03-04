using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// <para>
/// Essa classe gerencia toda a cena do Jogo.
/// </para>
/// <para>
/// Carrega o arquivo de palavras e escolhe uma de maneira aleatória.
/// </para>
/// <para>
/// Coorderna a chamada dos métodos de audio.
/// </para>
/// <para>
/// Atualiza a cena conforme acertamos e erramos as letras.
/// </para>
/// <para>
/// Carrega as cenas de vitoria ou derrota dependendo dos inputs do jogador.
/// </para>
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject Letter;
    public GameObject ScreenCenter;
    private int _quotaPositionX = 80;
    private string _secretWord;
    private string[] _secretWords;
    bool[] _findedLetters;
    private int _score = 0;
    private int _tentativeNumber;
    private int _maxTentativeNumber;
    [HideInInspector]
    private AudioSource _audioSourceError;
    private AudioSource _audioSourceAssert;

    // Start is called before the first frame update
    void Start()
    {
        ScreenCenter = GameObject.Find("screenCenter");
        _audioSourceError = GameObject.Find("audioError").GetComponent<AudioSource>();
        _audioSourceAssert = GameObject.Find("audioAssert").GetComponent<AudioSource>();
        InitGame();
    }

    // Update is called once per frame
    async void Update()
    {
        CheckVictory();
        await CheckKeyboard();
    }


    ///<summary>
    /// <para>Inicializa os GameObjects que representam as letras.</para>
    /// <para>Define a posição das letras na tela.</para>
    /// <para>Nomeia as letras como letter{indexOfLetter + 1}</para>
    /// <code>
    /// gameObj.name = "letter{indexOfLetter + 1}"
    /// </code>
    /// <para>Insere as letras na tela.</para>
    ///</summary>
    private void InitLetters()
    {
        Vector3 position;
        foreach (var indexOfLetter in Enumerable.Range(0, _secretWord.Length))
        {
            position = new Vector3(
                ScreenCenter.transform.position.x + (indexOfLetter - _secretWord.Length / 2.0f) * _quotaPositionX,
                ScreenCenter.transform.position.y,
                ScreenCenter.transform.position.z
            );
            GameObject gameObj = (GameObject)Instantiate(Letter, position, Quaternion.identity);
            gameObj.name = $"letter{indexOfLetter + 1}";
            gameObj.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }

    /// <summary>
    /// <para>
    /// Inicializa variavel _score o valor da propriedade score de PlayerPrefs.
    /// </para>
    /// <para>
    /// Injeta no GameObject score o valor da variavel _score.
    /// </para>
    /// </summary>
    private void InitScore()
    {
        _score = PlayerPrefs.GetInt("score");
        GameObject.Find("score").GetComponent<Text>().text = $"Score {_score}";
    }

    /// <summary>
    /// <para>
    /// Atualiza a variavel _score.
    /// </para>
    /// <para>
    /// Injeta no GameObject score o valor atualizado da variavel _score.
    /// </para>
    /// </summary>
    private void UpdateScore()
    {
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
        _score = PlayerPrefs.GetInt("score");
        GameObject.Find("score").GetComponent<Text>().text = $"Score {_score}";
    }

    /// <summary>
    /// <para>
    /// Inicia a variavel numero de tentativas (_tentativeNumber).
    /// </para>
    /// <para>
    /// Injeta no GameObject tentatives o valor da variavel 
    /// numero de tentativas (_tentativeNumber).
    /// </para>
    /// </summary>
    private void InitTentative()
    {
        _tentativeNumber = 0;
        GameObject.Find("tentatives").GetComponent<Text>().text = $"{_tentativeNumber} | {_maxTentativeNumber}";
    }

    /// <summary>
    /// <para>
    /// Atualiza a variavel numero de tentativas (_tentativeNumber).
    /// </para>
    /// <para>
    /// Injeta no GameObject tentatives o valor atualizado da variavel
    /// numero de tentativas (_tentativeNumber).
    /// </para>
    /// </summary>
    private void UpdateTentative()
    {
        _tentativeNumber++;
        GameObject.Find("tentatives").GetComponent<Text>().text = $"{_tentativeNumber} | {_maxTentativeNumber}";
    }

    /// <summary>
    /// <para>
    /// Carrega o arquivo com as palavras secretas em memória.
    /// </para>
    /// <para>
    /// Inicializa o vetor _secretWords com as palavras do arquivo.
    /// </para>
    /// </summary>
    private void LoadFileOfSecretWords()
    {
        TextAsset secretWordsFile = (TextAsset)Resources.Load("secretWordsFile", typeof(TextAsset));
        _secretWords = secretWordsFile.text.Split(' ');
    }


    ///<summary>
    /// <para>
    /// Define a palavra a ser descoberta de maneira aleatória dentro do
    /// conjunto de palavras possiveis.
    /// </para>
    /// <para>
    /// Define o array de letras descobertas.
    /// </para>
    ///</summary>
    private void InitGame()
    {
        LoadFileOfSecretWords();
        _secretWord = _secretWords[Random.Range(0, _secretWords.Length)].ToUpper();
        _findedLetters = new bool[_secretWord.Length];
        _maxTentativeNumber = _secretWord.Length + 4;
        InitScore();
        InitTentative();
        InitLetters();
    }

    /// <summary>
    /// <para>
    /// Constrói a palavra secreta com base do vetor de letras encontradas (_findedLetters).
    /// </para>
    /// <example>
    ///     O código mostra um exemplo de saida para uma pessoa que perdeu
    ///     <code>
    ///         _findedLetters = [true, false, false];
    ///         _secreteWord = "boi";
    ///         return "b??";
    ///     </code>
    /// </example>
    /// <example>
    ///     O código mostra um exemplo de saida para uma pessoa que ganhou
    ///     <code>
    ///         _findedLetters = [true, true, true];
    ///         _secreteWord = "boi";
    ///         return "boi";
    ///     </code>
    /// </example>
    /// </summary>
    /// <returns>A palavra completa ou a palavra com ? nas posições das letras não encontradas.</returns>
    private string BuildStringOfSecretWord()
    {
        StringBuilder sb = new StringBuilder();
        for (int indexOfLetter = 0; indexOfLetter < _findedLetters.Length; indexOfLetter++)
        {
            if (_findedLetters[indexOfLetter])
            {
                sb.Append(_secretWord[indexOfLetter]);
            }
            else sb.Append('?');
        }
        return sb.ToString();
    }

    /// <summary>
    /// <para>Analisa se o jogador ganhou ou perdeu</para>
    /// <para>Injeta na PlayerPrefs lastWord a palavra com as letras encontradas.</para>
    /// <para>Carrega a cena EndGameForLoser se o jogador perdeu.</para>
    /// <para>Carrega a cena EndGameForWinner se o jogador ganhador.</para>
    /// </summary>
    private void CheckVictory()
    {
        if (_tentativeNumber > _maxTentativeNumber)
        {
            PlayerPrefs.SetString("lastWord", BuildStringOfSecretWord());
            SceneManager.LoadScene("EndGameForLoser");
        }
        else if (!_findedLetters.Contains(false))
        {
            PlayerPrefs.SetString("lastWord", BuildStringOfSecretWord());
            SceneManager.LoadScene("EndGameForWinner");
        }
    }

    ///<summary>
    ///<para>
    ///Toca o som de erro.
    ///</para>
    ///</summary>
    private async Task PlayErrorAudio()
    {
        
        _audioSourceError.mute = false;
        _audioSourceError.volume = 0.01f;
        _audioSourceError.Play();
        await Task.Delay(100);        
        _audioSourceAssert.mute = true;
    }

    ///<summary>
    ///<para>
    ///Toca o som de acerto.
    ///</para>
    ///</summary>
    private async Task PlayAssertAudio()
    {
        _audioSourceAssert.mute = false;
        _audioSourceAssert.volume = 0.01f;
        _audioSourceAssert.Play();
        await Task.Delay(300);  
        _audioSourceAssert.mute = true;
    }

    /// <summary>
    /// <para>
    /// Verifica se a tecla pressionada no teclado corresponde a alguma letra da
    /// palavra secreta.
    /// </para>
    /// <para>
    /// Atualiza a propriedade Text componente letter para o valor da letra 
    /// se existir correspondencia entre o input e alguma letra que ainda não foi encontrada.
    /// </para>
    /// <para>
    /// Chama os métodos de audio para tocar quando se erra ou acerta um letra.
    /// </para>
    /// </summary>
    private async Task CheckKeyboard()
    {
        bool assert = false;
        if (Input.anyKeyDown)
        {
            char pressedKey = Input.inputString.ToCharArray()[0];
            if ((pressedKey >= 'A' && pressedKey <= 'Z') || (pressedKey >= 'a' && pressedKey <= 'z'))
            {
                pressedKey = char.ToUpper(pressedKey);
                for (int indexOfLetter = 0; indexOfLetter < _secretWord.Length; indexOfLetter++)
                {
                    if (_findedLetters[indexOfLetter] is false && _secretWord[indexOfLetter] == pressedKey)
                    {
                        await PlayAssertAudio();
                        assert = true;
                        _findedLetters[indexOfLetter] = true;
                        GameObject.Find($"letter{indexOfLetter + 1}").GetComponent<Text>().text = pressedKey.ToString();
                        UpdateScore();
                    }
                }
                if (!assert) await PlayErrorAudio();
                UpdateTentative();
            }
        }
    }
}
