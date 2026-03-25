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
	public float rotspeed = 0.1f;
	public ParticleSystem water;
	public GameObject holder;
	public GameObject arrow1;
	public GameObject arrow2;
	private bool isWatering = false;
    Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
    Quaternion currentRotation;

    private void Start()
    {
		currentRotation = transform.rotation;
        arrow1.SetActive(true);
        arrow2.SetActive(false);
        holder.SetActive(true);
    }

    //	Al cogerlo "isDragging = true"
    void OnMouseDown()
	{
		Vector3 mouseWorld = GetMouseWorldPosition();
		offset = transform.position - mouseWorld;
		isDragging = true;
        arrow1.SetActive(false);
        holder.SetActive(false);
        if (GameManager.Manager.currentState == GameManager.GameState.Plant)
			arrow2.SetActive(true);
		if (GameManager.Manager.currentState==GameManager.GameState.Water)
		{
			isWatering = true;
            water.Play();
			return;
        }
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
        if (GameManager.Manager.currentState == GameManager.GameState.Water)
        {
            isWatering = false;
            water.Stop();
        }
        arrow1.SetActive(true);
        holder.SetActive(true);
        arrow2.SetActive(false);
    }

	//	Si se ha soltado "isDragging?", regresa a su pos inicial "startPoint"!
	void Update()
	{
		if (!isDragging)
			transform.position = Vector3.Lerp(transform.position, startPoint.position, Time.deltaTime * speed);

		if (isWatering)
            transform.rotation = Quaternion.Lerp(targetRotation, currentRotation, Time.deltaTime * rotspeed);
		else
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * rotspeed);
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
        isWatering = false;
        transform.position = startPoint.position;
	}
}
