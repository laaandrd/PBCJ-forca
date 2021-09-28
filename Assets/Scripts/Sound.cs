
using UnityEngine;
using UnityEngine.Audio;

/*Classe usada para configurar os audios dentro do jogo*/
[System.Serializable]
public class Sound {

  public string name; //Nome do audio dentro do jogo
  public AudioClip clip; //Audio

  [Range(0f, 1f)] //Determina a variação máxima e minima volume
  public float volume; //Volume do audio no jogo

  [Range(.1f, 3f)] //Determina a variação máxima e minima do pitch
  public float pitch; //Pitch do audio no jogo

  public bool loop; //Caso verdadeiro o som será repetido até que algo o pare

  [HideInInspector] //Esconde o método para que ele não apareça nas opções do Componete no Unity
  public AudioSource source; //Fonte do audio
}