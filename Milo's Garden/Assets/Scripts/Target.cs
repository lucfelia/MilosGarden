using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Target : MonoBehaviour
{
	//	Si jugador "CompareTag("Player")" ha entrado en contacto con el objeto "OnTriggerEnter2D" se oculta "SetActive(false)"
	private void OnTriggerStay2D(Collider2D other)
	{
		if(GameManager.Manager.currentState == GameManager.GameState.Water)
			return;

		if (other.CompareTag("Player"))
		{
			DragAndDrop dragScript = other.GetComponent<DragAndDrop>();
			if (!dragScript.isDragging)
				GameManager.Manager.isInteractionSucceed = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
			GameManager.Manager.isInteractionSucceed = false;
	}
}
