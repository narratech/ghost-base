using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    Transform player;
    GameManager gameManager;
    AudioSource bubbleAudio;
    //public GameObject explosionEffect; // Asigna un prefab de partículas

    bool m_IsPlayerInRange;

    void Start()
    { 
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>(); // Debería chequear que exista
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

        bubbleAudio = GetComponent<AudioSource>(); // Obtiene el AudioSource
    }

        void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
        else // Si entra algo del mismo color que el padre de este observador, este padre se irá desvaneciendo y se destruirá
            if (!other.gameObject.CompareTag("Untagged") && other.gameObject.CompareTag(transform.parent?.gameObject.tag))
            {
                Debug.Log(other.gameObject.tag);
                bubbleAudio.Play(); // Reproduce el sonido
                //Instantiate(explosionEffect, transform.position, Quaternion.identity); // Crea la explosión
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
                    gameManager.CaughtPlayer ();
                    m_IsPlayerInRange = false; // Añado esto para que sólo se llame a CaughtPlayer una vez
                }
            }
        }
    }
}
