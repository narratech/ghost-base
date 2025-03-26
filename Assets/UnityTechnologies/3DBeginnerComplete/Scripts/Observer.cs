using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    Transform player;
    GameManager gameManager;
    AudioSource bubbleAudio;

    bool m_IsPlayerInRange;

    void Start()
    { 
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>(); 
            if (gameManager == null)
            {
                Debug.LogWarning("No se encontró un componente 'GameManager' en el objeto 'GameManager' correspondiente.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'GameManager'.");
        }

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;  
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'Player'.");
        }

        GameObject bubbleObject = GameObject.FindWithTag("Bubble");
        if (bubbleObject != null)
        {
            bubbleAudio = bubbleObject.GetComponent<AudioSource>();
            if (bubbleAudio == null)
            {
                Debug.LogWarning("No se encontró un componente 'AudioSource' en el objeto 'Bubble' correspondiente.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'Bubble'.");
        }

    }

    void OnTriggerEnter (Collider other)
    { 
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
        else // Si entra algo del mismo color que el padre de este observador, este padre desaparecerá con un sonidito 
            if (!other.gameObject.CompareTag("Untagged") && other.gameObject.CompareTag(transform.parent?.gameObject.tag))
            {
                bubbleAudio.Play(); // Reproduce el sonido
                Destroy(transform.parent?.gameObject); // Destruye el objeto
            }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            
            if (Physics.Raycast (ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    gameManager.CaughtPlayer();
                    m_IsPlayerInRange = false; // Añado esto para que sólo se llame a CaughtPlayer el primer fotograma
                }
            }
        }
    }
}
