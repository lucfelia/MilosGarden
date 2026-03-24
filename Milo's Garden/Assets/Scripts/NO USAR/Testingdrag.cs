using UnityEngine;

public class TestingDrag : MonoBehaviour
{

    public LayerMask draggableLayer;

    private Camera mainCamera;
    public GameObject draggedObject;
    private Vector3 offset;
    private Vector3 initialPosition;

    void Start()
    {
        mainCamera = Camera.main;
        draggedObject = gameObject;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, draggableLayer);

            if (hit.collider != null)
            {
                draggedObject = hit.collider.gameObject;
                initialPosition = draggedObject.transform.position;
                GameManager.Instance.initialPlayerPosition = initialPosition;
                offset = draggedObject.transform.position - mouseWorldPos;
            }
        }
        if (Input.GetMouseButton(0) && draggedObject != null)
        {
            if (GameManager.Instance.isReleased)
            {
                Debug.Log("Object is being dragged");
                GameManager.Instance.isReleased = false;
            }
            Rigidbody2D rb = draggedObject.GetComponent<Rigidbody2D>();
            rb.MovePosition(mouseWorldPos + offset);
        }

        if (Input.GetMouseButtonUp(0) && draggedObject != null)
        {
            draggedObject.transform.position = initialPosition;
            draggedObject = null;
            Debug.Log("Object released, returning to initial position");
            GameManager.Instance.isReleased = true;
        }
    }
}