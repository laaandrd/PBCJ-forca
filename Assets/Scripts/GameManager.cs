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

        //verifica se a m�sica tema j� est� tocando, e em caso negativo come�a a toc�-la
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

    
    /* Esse m�todo � respons�vel por sortear uma palavra de um arquivo .txt, carregar as informa��es
     * referentes ao tamanho da palvra sorteada, criar um vetor de char com as letras da palavra sorteada
     * e tamb�m criar um vetor de booleanos com o tamanho adequado para ser usado posteriormente como gabarito */
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
    
    /*Esse m�todo � respons�vel por carregar os GameObjects referentes �s letras da palavra sorteada
     e posicion�-los na tela, tomando como refer�ncia um GameObject com as coordenadas do centro da tela*/
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
    
    /*Esse m�todo verifica se a letra digitada j� foi digitada anteriormente, devolvendo true se
     *sim e false caso contr�rio*/
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

    /*Esse m�todo atualiza a lista de letras j� usadas, acrescentando somente as que n�o foram usadas anteriormente*/
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

        //� atualizado na tela um textbox indicando todas as letras j� utilizadas
        string textBoxLetrasUsadas = "Letras usadas: ";
        for(int i = 0; letrasUsadas[i] != 0 && i < 26; i++)
        {
            textBoxLetrasUsadas = textBoxLetrasUsadas + letrasUsadas[i].ToString() + " ";
        }
        GameObject.Find("letrasUsadas").GetComponent<Text>().text = textBoxLetrasUsadas;

    }

    /*Esse m�todo � respons�vel por verificar o vetor utilizado como gabarito, comparando a quantidade de
     elementos corretos (true no vetor de bool) com o tamanho da palava sorteada. Retorna true se todas letras
    foram adivinhada e false em caso contr�rio*/
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
    
    /*Esse m�todo inica a musica e a tela de finaliza��o conforme um booleano. Caso o valor seja true ir� iniciar a musica e tela de sucesso. Caso contrario ser� a de derrota.*/
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

    /*Esse � o m�todo em que toda l�gica do jogo est� concentrada. Ele contabiliza o n�mero de chances restantes
     para acertar a palavra sorteada, e em caso desse valor ser maior que 0, ele filtra as entradas permitidas e
     usa essas entradas para conferir se houve acertos ou n�o. Em caso de acerto, a letra � revelada. Se nenhuma
     letra for acertada, o n�mero de chances � subtra�do, e caso chegue a 0 a tela de derrota � carregada. Caso
     todas as letras sejam reveladas antes do n�mero de chances restantes chegar a 0, a tela de sucesso � carregada.
     Tamb�m h� uma programa��o defensiva para impedir a contabiliza��o de letras j� inseridas anteriormente.*/
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
                    
                    //impede a contabiliza��o de erros no caso de entrada de letras j� usadas
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

                    //indica um acerto e n�o h� redu��o no n�mero de chances
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
