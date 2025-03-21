/*    
   Copyright (C) 2025 Narratech Laboratories
   https://www.narratech.com
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com

   Modificación del GameEnding original para convertirlo en algo más parecido a un GameManager
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer = 0.0f;
    bool m_HasAudioPlayed;
    GameObject start; // Referencia al waypoint de inicio

    public float elapsedTime = 0f; // Tiempo transcurrido
    public int gotchas = 0; // Pilladas
    public int wins = 0; // Ganadas

    private Label timeLabel; // Referencia a la etiqueta de UI Toolkit
    private Label gotchasLabel; // Referencia a la etiqueta de UI Toolkit
    private Label winsLabel; // Referencia a la etiqueta de UI Toolkit

    void Start()
    {
        // Obtener la etiqueta desde el UI Toolkit
        var root = FindObjectOfType<UIDocument>().rootVisualElement;
        timeLabel = root.Q<Label>("TimeValue");
        gotchasLabel = root.Q<Label>("GotchasValue");
        winsLabel = root.Q<Label>("WinsValue");

        // Obtener el waypoint de inicio
        start = GameObject.FindWithTag("Start"); // Debería chequear que exista

        // Inicializar el texto del cronómetro y los otros textos a cero
        UpdateTimerUI();
        UpdateGotchasUI();
        UpdateWinsUI();
    }

    private void UpdateTimerUI()
    {
        // Convertir tiempo a minutos, segundos y décimas
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int hundredths = Mathf.FloorToInt((elapsedTime * 100) % 100); // Centésimas de segundo

        // Actualizar el texto del Label con el formato MM:SS:FF
        timeLabel.text = $"{minutes:00}:{seconds:00}:{hundredths:00}";
    }

    private void UpdateGotchasUI()
    {
        gotchasLabel.text = gotchas.ToString();

    }

    private void UpdateWinsUI()
    {
        winsLabel.text = wins.ToString();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Update()
    {
        // Incrementar el tiempo
        elapsedTime += Time.deltaTime;

        // Actualizar la UI
        UpdateTimerUI();

        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                // Reiniciar significa simplemente sumar 1 a las 'pilladas' y volver a llevar a nuestro avatar al punto de inicio
                m_IsPlayerCaught = false; // Para que no se repita
                gotchas++;
                UpdateGotchasUI();
                player.transform.position = start.transform.position;
                // Hago todo lo necesario para que todo vuelta a la normalidad
                imageCanvasGroup.alpha = 0.0f;
                m_Timer = 0.0f;
                m_HasAudioPlayed = false; 
            }
            else
            {
                // No reiniciar significa sumar 1 a las 'ganadas' y volver a cargar la escena, con lo que esta podría ser diferente
                m_IsPlayerAtExit = false; // Para que no se repita
                wins++;
                UpdateWinsUI();
                SceneManager.LoadScene(0); // No me hará falta esto imageCanvasGroup.alpha = 0.0f; pero ojo, mantener este como un EJEMPLAR ÚNICO PARA QUE NO ME CAMBIE LA UI
                // En ningún caso haremos Application.Quit ();
                // ...si acaso un botón para hacer auténtico Reset, de tiempo y todo
            }
        }
    }
}

