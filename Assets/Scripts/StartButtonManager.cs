using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //carrega a cena em que se passa toda dinâmica do jogo
    public void StartTelaDeJogo()
    {
        SceneManager.LoadScene("TelaDeJogo");
    }
}
