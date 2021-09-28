using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //carrega a cena de créditos
    public void StartTelaDeCreditos()
    {
        SceneManager.LoadScene("TelaDeCreditos");
    }
}
