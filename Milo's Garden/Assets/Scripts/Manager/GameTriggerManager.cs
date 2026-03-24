using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameTriggerManager : MonoBehaviour
{
    private bool isTriggered = false;
    public GameObject[] fruits;
    private float timer = 0f;
    public Slider slider;
    private bool once;
    public float waterTime =10;
    void Update()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.currentState == GameManager.GameState.Plant)
        {
            if (isTriggered && GameManager.Instance.isReleased)
            {
                GameManager.Instance.ChangeState();
                isTriggered = false;
                Debug.Log("State changed to Water");
                once = true;
            }
        }
        else if (GameManager.Instance.currentState == GameManager.GameState.Water)
        {
            if (once)
            {
                once = false;
                slider.gameObject.SetActive(true);
            }
            if (isTriggered && !GameManager.Instance.isReleased)
            {
                timer += Time.deltaTime;
                slider.value = timer / waterTime;

                if (timer >= waterTime)
                {
                    GameManager.Instance.ChangeState();
                    timer = 0f;
                    Debug.Log("State changed to Harvest");
                    once = true;
                    slider.value = 0;
                    GameManager.Instance.ResetPlayer();
                }
            }
        }
        else if (GameManager.Instance.currentState == GameManager.GameState.Harvest)
        {
            if (once)
            {
                once = false;
                slider.gameObject.SetActive(false);
                for(int i = 0; i < fruits.Length; i++)
                {
                    fruits[i].SetActive(true);
                }
            }
            bool allFruitsCollected = true;

            for (int i = 0; i < fruits.Length; i++)
            {
                if (fruits[i].activeSelf)
                {
                    allFruitsCollected = false;
                }
            
            }
            if(allFruitsCollected)
            {
                GameManager.Instance.ResetPlayer(); 
                GameManager.Instance.ChangeState();
                Debug.Log("State reset to Plant");

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            Debug.Log("Entered GameZone");
            isTriggered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isTriggered)
        {
            Debug.Log("Exited GameZone");
            isTriggered = false;
        }
    }
}
