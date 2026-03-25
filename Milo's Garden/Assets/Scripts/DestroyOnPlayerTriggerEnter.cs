using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DestroyOnPlayerTriggerEnter : MonoBehaviour
{
    public AudioClip sound;
    public AudioSource audioSource;
    //	Si jugador "CompareTag("Player")" ha entrado en contacto con el objeto "OnTriggerEnter2D" se oculta "SetActive(false)"
    private void OnTriggerEnter2D(Collider2D other)
	{
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sound);
            Destroy(gameObject);
        }
	}
}
