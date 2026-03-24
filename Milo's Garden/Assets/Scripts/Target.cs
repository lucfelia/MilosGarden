using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Target : MonoBehaviour
{
	//public ParticleSystem water;
	//private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

	//	Si jugador "CompareTag("Player")" ha entrado en contacto con el objeto "OnTriggerEnter2D" se oculta "SetActive(false)"
	private void OnTriggerStay2D(Collider2D other)
	{
		if(GameManager.Manager.currentState == GameManager.GameState.Water)
		{
			//if (other.CompareTag("Water"))
			if (other.CompareTag("Player") || other.CompareTag("Water"))
				GameManager.Manager.isInteractionSucceed = true;
		}
		else if(GameManager.Manager.currentState == GameManager.GameState.Plant || GameManager.Manager.currentState == GameManager.GameState.Harvest)
		{
			if (other.CompareTag("Player"))
			{
				DragAndDrop dragScript = other.GetComponent<DragAndDrop>();
				if (!dragScript.isDragging)
					GameManager.Manager.isInteractionSucceed = true;
			}
		}
	}

	//private void OnParticleTrigger()
	//{
	//    Debug.Log("c");
	//    int numberOfParticlesEntered = water.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
	//    Debug.Log(numberOfParticlesEntered);
	//    if (numberOfParticlesEntered > 0)
	//    {
	//        GameManager.Manager.isInteractionSucceed = true;
	//    }
	//    else
	//    {
	//        GameManager.Manager.isInteractionSucceed = false;
	//    }
	//}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Water"))
			GameManager.Manager.isInteractionSucceed = false;
	}
}
