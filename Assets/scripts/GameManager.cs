using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

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

    // Start is called before the first frame update
    void Start()
    {
        ScreenCenter = GameObject.Find("screenCenter");
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        CheckVictory();
        CheckKeyboard();
    }


    ///<summary>
    /// <para>Inicializa os GameObjects que representam as letras.</para>
    /// <para>Define a posição das letras na tela.</para>
    /// <para>Nomeia as letras como letter{indexOfLetter + 1}.</para>
    /// <code>
    /// gameObj.name = "letter{indexOfLetter + 1}"
    /// </code>
    /// <para>Insere as letras na tela.</para>
    ///</summary>
    private void InitLetters() {
        Vector3 position;
        foreach (var indexOfLetter in Enumerable.Range(0, _secretWord.Length)) {
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
    /// 
    /// </summary>
    private void InitScore()
    {
        _score = PlayerPrefs.GetInt("score");
        GameObject.Find("score").GetComponent<Text>().text = $"Score {_score}";
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateScore()
    {
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
        _score = PlayerPrefs.GetInt("score");
        GameObject.Find("score").GetComponent<Text>().text = $"Score {_score}";
    }

    /// <summary>
    /// 
    /// </summary>
    private void InitTentative()
    {
        _tentativeNumber = 0;
        GameObject.Find("tentatives").GetComponent<Text>().text = $"{_tentativeNumber} | {_maxTentativeNumber}";
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateTentative()
    {
        _tentativeNumber++;
        GameObject.Find("tentatives").GetComponent<Text>().text = $"{_tentativeNumber} | {_maxTentativeNumber}";
    }

    /// <summary>
    /// 
    /// </summary>
    private void LoadFileOfSecretWords()
    {
        TextAsset secretWordsFile = (TextAsset) Resources.Load("secretWordsFile", typeof(TextAsset));
        _secretWords = secretWordsFile.text.Split(' ');
    }


    ///<summary>
    /// <para>Define a palavra a ser descoberta de maneira aleatória dentro do
    /// conjunto de palavras possiveis.</para>
    /// 
    /// <para>Define o array de letras descobertas.</para>
    ///</summary>
    private void InitGame() {
        LoadFileOfSecretWords();
        _secretWord = _secretWords[Random.Range(0, _secretWords.Length)].ToUpper();
        _findedLetters = new bool[_secretWord.Length];
        _maxTentativeNumber = _secretWord.Length + 4;
        InitScore();
        InitTentative();
        InitLetters();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
    /// 
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


        /// <summary>
        /// Verifica se a tecla pressionada no teclado corresponde a alguma letra da
        /// palavra secreta, caso tenha ele atualiza o array de letras descobertas e 
        /// atualiza a propriedade Text componente letter para o valor da letra
        /// </summary>
        private void CheckKeyboard()
        {
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
                            _findedLetters[indexOfLetter] = true;
                            GameObject.Find($"letter{indexOfLetter + 1}").GetComponent<Text>().text = pressedKey.ToString();
                            UpdateScore();
                        }
                    }
                    UpdateTentative();
                }
            }
        }
    }
