using UnityEngine;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject player;
    public Vector3 initialPlayerPosition;
    public Sprite[] sprites;
    public SpriteRenderer currentSprite;
    public enum GameState { Plant, Water, Harvest }
    public GameState currentState = GameState.Plant;
    public bool isReleased;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentSprite.sprite = sprites[(int)currentState];
    }

    public void ChangeState()
    {
        currentState++;

        if ((int)currentState >= sprites.Length) currentState = GameState.Plant;
        currentSprite.sprite = sprites[(int)currentState];
    }
    public void ResetPlayer()
    {
        TestingDrag drag = player.GetComponent<TestingDrag>();
        drag.draggedObject.transform.position = initialPlayerPosition;
        drag.draggedObject = null;
        isReleased = true;
        player.SetActive(false);
        player.transform.position = initialPlayerPosition;
        player.SetActive(true);
    }
}