using UnityEngine;

public class ParallaxMouse : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Range(0f, 1f)]
    public float parallaxEffect = 0.05f;

    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        transform.position = initialPos + new Vector3(mouseX, mouseY, 0) * parallaxEffect;
    }
}
