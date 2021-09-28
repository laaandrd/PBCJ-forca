using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenuButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //carrega a cena da tela inicial do jogo
    public void StartTelaDeInicio()
    {
        //verifica se a música já está tocando, e em caso contrário começa a tocá-la
        if (!FindObjectOfType<AudioManager>().sounds[0].source.isPlaying)
        {
            FindObjectOfType<AudioManager>().Play("Theme");
        }
        //carrega a tela inicial do jogo
        SceneManager.LoadScene("TelaDeInicio");
    }
}
