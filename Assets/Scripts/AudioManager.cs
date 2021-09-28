using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; // Lista de audios

    public static AudioManager instance; //Gerenciador de Audios

    /*Esse m�todo � respons�vel por configurar os soms que s�o usados durante o jogo*/
    void Awake() {

        /*Instancia o Gerenciador de audios e o destroi caso j� exista*/
        if (instance == null) instance = this;
        else 
        {
            Destroy(gameObject);
            return;
        }

        /*Evita que o Gerenciador de Audios seja destruido ao abrir outra cena*/
        DontDestroyOnLoad(gameObject);

        /*Configura os audios conforme as informa��es passadas na lista de audios*/
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }   
    }

    /*Esse m�todo executa a m�sica tema quando o jogo � iniciado*/
    void Start() {
        Play("Theme");
    }

    /*Esse m�todo toca um audio de acordo com um nome passado. Para isso ele busca na lista de sons o nome que foi passado.*/
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    /*Esse m�todo para um audio de ser tocado de acordo com um nome passado. Para isso ele busca na lista de sons o nome que foi passado.*/
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }
}
