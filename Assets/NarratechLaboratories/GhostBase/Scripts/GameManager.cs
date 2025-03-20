/*    
   Copyright (C) 2025 Narratech Laboratories
   https://www.narratech.com
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    private Label timeLabel; // Referencia a la etiqueta de UI Toolkit
    private float elapsedTime = 0f; // Tiempo transcurrido

    void Start()
    {
        // Obtener la etiqueta desde el UI Toolkit
        var root = FindObjectOfType<UIDocument>().rootVisualElement;
        timeLabel = root.Q<Label>("TimeValue");

        // Inicializar el texto del cronómetro
        UpdateTimerUI();
    }

    void Update()
    {
        // Incrementar el tiempo
        elapsedTime += Time.deltaTime;

        // Actualizar la UI
        UpdateTimerUI();
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

}
