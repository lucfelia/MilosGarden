using UnityEngine;

//	Utilice OnMouseX() porque IDragHandle es solo para UI! https://discussions.unity.com/t/drag-gameobject-with-mouse/1798/8
[RequireComponent(typeof(BoxCollider2D))]
public class DragAndDrop : MonoBehaviour
{
	private Vector3 offset;
	[HideInInspector]public bool isDragging = false;
	[Header("Posicion a la que volver al soltarlo")]
	public Transform startPoint;
	public float speed = 2.5f;

	//	Al cogerlo "isDragging = true"
	void OnMouseDown()
	{
		Vector3 mouseWorld = GetMouseWorldPosition();
		offset = transform.position - mouseWorld;
		isDragging = true;
	}

	//	Al arrastrar "isDragging?"
	void OnMouseDrag()
	{
		if (isDragging)
		{
			Vector3 mouseWorld = GetMouseWorldPosition();
			transform.position = mouseWorld + offset;
		}
	}

	//	Al soltar "isDragging = false"
	void OnMouseUp()
	{
		isDragging = false;
	}

	//	Si se ha soltado "isDragging?", regresa a su pos inicial "startPoint"!
	void Update()
	{
		if (!isDragging)
			transform.position = Vector3.Lerp(transform.position, startPoint.position, Time.deltaTime * speed);
	}

	//	Calculo entre la pos del mouse con la pos del mundo
	Vector3 GetMouseWorldPosition()
	{
		Vector3 mouseScreen = Input.mousePosition;
		mouseScreen.z = Mathf.Abs(Camera.main.transform.position.z);
		return Camera.main.ScreenToWorldPoint(mouseScreen);
	}

	//	Directamente hace teletransportacion al punto de inicio
	public void ResetPosition()
	{
		isDragging = false;
		transform.position = startPoint.position;
	}
}
