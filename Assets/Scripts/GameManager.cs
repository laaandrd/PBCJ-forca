using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject centroDaTela;
    public GameObject letra;
    public GameObject indicadorDeTentativas;

    private string palavraSorteada;
    private int tamanhoPalavra;
    private char[] letrasPalavraSorteada;
    private bool[] gabaritoPalavraSorteada;

    private char[] letrasUsadas;

    private int numChances;

    // Start is called before the first frame update
    void Start()
    {
        SortearPalavra();
        CarregarLetras();
        this.numChances = 5;
        this.letrasUsadas = new char[26];

        //verifica se a música tema já está tocando, e em caso negativo começa a tocá-la
        if (!FindObjectOfType<AudioManager>().sounds[0].source.isPlaying)
        {
            FindObjectOfType<AudioManager>().Play("Theme");
        }

        GameObject.Find("indicadorDeTentativas").GetComponent<Text>().text = ("Tentativas restantes: " + numChances);
    }

    // Update is called once per frame
    void Update()
    {
        VerificarInput();
    }

    
    /* Esse método é responsável por sortear uma palavra de um arquivo .txt, carregar as informações
     * referentes ao tamanho da palvra sorteada, criar um vetor de char com as letras da palavra sorteada
     * e também criar um vetor de booleanos com o tamanho adequado para ser usado posteriormente como gabarito */
    void SortearPalavra()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int index = Random.Range(0, palavras.Length);
        this.palavraSorteada = palavras[index].ToUpper();
        this.tamanhoPalavra = palavras[index].Length;
        this.letrasPalavraSorteada = this.palavraSorteada.ToCharArray();
        this.gabaritoPalavraSorteada = new bool[palavras[index].Length];
    }
    
    /*Esse método é responsável por carregar os GameObjects referentes às letras da palavra sorteada
     e posicioná-los na tela, tomando como referência um GameObject com as coordenadas do centro da tela*/
    void CarregarLetras()
    {
        this.centroDaTela = GameObject.Find("centroDaTela");
        for(int i = 0; i < this.tamanhoPalavra; i++)
        {
            Vector3 pos;
            pos = new Vector3(centroDaTela.transform.position.x + (((float)i - tamanhoPalavra / 2.0f) * 15), centroDaTela.transform.position.y, centroDaTela.transform.position.z);

            GameObject l = (GameObject)Instantiate(letra, pos, Quaternion.identity);
            l.name = "letra" + (i + 1);
            l.transform.SetParent(GameObject.Find("Canvas").transform);
        }
        
    }
    
    /*Esse método verifica se a letra digitada já foi digitada anteriormente, devolvendo true se
     *sim e false caso contrário*/
    bool LetraJaUsada(char letraTeclada)
    {
        int i = 0;

        while (letrasUsadas[i] != 0 && i < 26)
        {
            if (letraTeclada == letrasUsadas[i])
            {
                return true;
            }

            i++;
        }
        return false;
    }

    /*Esse método atualiza a lista de letras já usadas, acrescentando somente as que não foram usadas anteriormente*/
    void AtualizarLetrasUsadas(char letraTeclada)
    {
        if (!LetraJaUsada(letraTeclada))
        {
            int i = 0;
            while(letrasUsadas[i] != 0 && i<26)
            {
                i++;
            }
            letrasUsadas[i] = letraTeclada;
        }

        //é atualizado na tela um textbox indicando todas as letras já utilizadas
        string textBoxLetrasUsadas = "Letras usadas: ";
        for(int i = 0; letrasUsadas[i] != 0 && i < 26; i++)
        {
            textBoxLetrasUsadas = textBoxLetrasUsadas + letrasUsadas[i].ToString() + " ";
        }
        GameObject.Find("letrasUsadas").GetComponent<Text>().text = textBoxLetrasUsadas;

    }

    /*Esse método é responsável por verificar o vetor utilizado como gabarito, comparando a quantidade de
     elementos corretos (true no vetor de bool) com o tamanho da palava sorteada. Retorna true se todas letras
    foram adivinhada e false em caso contrário*/
    bool VerificarGabarito()
    {
        int i = 0;
        int acertos = 0;
        while (i<tamanhoPalavra && gabaritoPalavraSorteada[i])
        {
            i++;
            acertos++;
        }
        return (acertos == tamanhoPalavra);
    }
    
    /*Esse método inica a musica e a tela de finalização conforme um booleano. Caso o valor seja true irá iniciar a musica e tela de sucesso. Caso contrario será a de derrota.*/
    void StartTelaResultado(bool resultado)
    {
        FindObjectOfType<AudioManager>().Stop("Theme");
        if (resultado)
        {
            
            FindObjectOfType<AudioManager>().Play("Victory");
            SceneManager.LoadScene("TelaDeSucesso");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Fail");
            SceneManager.LoadScene("TelaDeDerrota");
        }
    }

    /*Esse é o método em que toda lógica do jogo está concentrada. Ele contabiliza o número de chances restantes
     para acertar a palavra sorteada, e em caso desse valor ser maior que 0, ele filtra as entradas permitidas e
     usa essas entradas para conferir se houve acertos ou não. Em caso de acerto, a letra é revelada. Se nenhuma
     letra for acertada, o número de chances é subtraído, e caso chegue a 0 a tela de derrota é carregada. Caso
     todas as letras sejam reveladas antes do número de chances restantes chegar a 0, a tela de sucesso é carregada.
     Também há uma programação defensiva para impedir a contabilização de letras já inseridas anteriormente.*/
    void VerificarInput()
    {
        if (numChances > 0)
        {
            if (Input.anyKeyDown)
            {
                char letraTeclada = Input.inputString.ToCharArray()[0];
                int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);
                // filtra as entradas permitidas
                if(letraTecladaComoInt>=94 && letraTecladaComoInt <= 122)
                {
                    int acertos = 0;
                    letraTeclada = System.Char.ToUpper(letraTeclada);
                    
                    //impede a contabilização de erros no caso de entrada de letras já usadas
                    if (LetraJaUsada(letraTeclada))
                    {
                        acertos++;
                    }
                    else
                    {
                        AtualizarLetrasUsadas(letraTeclada);
                        for (int i = 0; i < tamanhoPalavra; i++)
                        {
                            if (!gabaritoPalavraSorteada[i] && letraTeclada == letrasPalavraSorteada[i])
                            {
                                gabaritoPalavraSorteada[i] = true;
                                GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();
                                acertos++;
                            }
                        }
                    }

                    //indica erro e a perca de uma chance
                    if(acertos == 0)
                    {
                        numChances--;
                        GameObject.Find("indicadorDeTentativas").GetComponent<Text>().text = ("Tentativas restantes: " + numChances);
                        FindObjectOfType<AudioManager>().Play("Error");
                    }

                    //indica um acerto e não há redução no número de chances
                    else
                    {
                        FindObjectOfType<AudioManager>().Play("Beep");
                    }

                    if (VerificarGabarito())
                    {
                        //cena de vitoria
                        StartTelaResultado(true);
                    }
                    
                }
            }
        }
        else
        {
            //cena de derrota
            StartTelaResultado(false);
        }
    }
}
