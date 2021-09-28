
using UnityEngine;
using UnityEngine.Audio;

/*Classe usada para configurar os audios dentro do jogo*/
[System.Serializable]
public class Sound {

  public string name; //Nome do audio dentro do jogo
  public AudioClip clip; //Audio

  [Range(0f, 1f)] //Determina a varia��o m�xima e minima volume
  public float volume; //Volume do audio no jogo

  [Range(.1f, 3f)] //Determina a varia��o m�xima e minima do pitch
  public float pitch; //Pitch do audio no jogo

  public bool loop; //Caso verdadeiro o som ser� repetido at� que algo o pare

  [HideInInspector] //Esconde o m�todo para que ele n�o apare�a nas op��es do Componete no Unity
  public AudioSource source; //Fonte do audio
}