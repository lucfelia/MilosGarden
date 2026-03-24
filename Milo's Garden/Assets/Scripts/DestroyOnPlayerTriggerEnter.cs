using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DestroyOnPlayerTriggerEnter : MonoBehaviour
{
	//	Si jugador "CompareTag("Player")" ha entrado en contacto con el objeto "OnTriggerEnter2D" se oculta "SetActive(false)"
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
			Destroy(gameObject);
	}
}
