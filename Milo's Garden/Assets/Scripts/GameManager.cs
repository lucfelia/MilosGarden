using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	//  Hacemos el GameManager static para que sea accesible en cualquier script!
	public static GameManager Manager { get; private set; }
	//	Hacemos un enum con el estado del juego (esta plantando? regando? recolectando?)
	public enum GameState { Plant, Water, Harvest }

	[Header("Referencias")]
	public TextMeshProUGUI info;
	public GameObject panel;
	public Sprite[] sprites;
	public SpriteRenderer playerSprite;
	public Slider slider;
	public GameObject seed;
	public float waterTime = 5.0f;
	public GameObject fruitHolder;
	public GameObject waterHolder;

	[HideInInspector] public GameState currentState = GameState.Plant;
	[HideInInspector] public bool isInteractionSucceed = false;
	private float timer = 0.0f;
	private bool gameFinished = false;

	private void Awake()
	{
		Manager = this;
	}

	//	Al iniciar "Start()", cambiamos el sprite del jugador segun el estado del juego
	private void Start()
	{
		playerSprite.sprite = sprites[(int)currentState];
		slider.gameObject.SetActive(false);
		fruitHolder.SetActive(false);
		waterHolder.SetActive(false);
		panel.SetActive(false);
		seed.SetActive(false);
		InfoUpdate(currentState);
	}

	//	Cada frame validamos en que estado esta el juego y ejecutamos la logica de este
	void Update()
	{
		if (gameFinished)
		{
			SceneManager.LoadScene("GameOver");
            return;
			
        }
        //	Plantamos la semilla si se ha colocado esta correctamente "isInteractionSucceed = true" y cambiamos estado "ChangeState()"
        if (currentState == GameState.Plant)
		{
			if (isInteractionSucceed)
			{
				isInteractionSucceed = false;
				Debug.Log("Seed planted!");
				seed.SetActive(true);
				ChangeState();
			}
		}
		//	Si detecta agua en el terreno "isInteractionSucceed = true" entonces el slider va avanzando "timer / waterTime"
		else if (currentState == GameState.Water)
		{
			slider.gameObject.SetActive(true);
			waterHolder.SetActive(true);
			if (isInteractionSucceed)
			{
				timer += Time.deltaTime;
				slider.value = timer / waterTime;
	
				//	Si se alcanza el maximo valor cambiamos de estado "ChangeState()"
				if (timer >= waterTime)
				{
					timer = 0f;
					slider.value = 0;
					slider.gameObject.SetActive(false);
					isInteractionSucceed = false;
					Debug.Log("Plant Watered!");
					ChangeState();
				}
			}
		}
		//	Si detecta que ya no quedan frutas "fruitHolder.transform.childCount <= 0", entonces se termina el juego!
		else if (currentState == GameState.Harvest)
		{
			slider.gameObject.SetActive(false);
			waterHolder.SetActive(false);
			fruitHolder.SetActive(true);
			if (fruitHolder.transform.childCount <= 0)
			{
				StartCoroutine(Complete(currentState));
				gameFinished = true;
				Debug.Log("Strawberrys Harvested!");
			}
		}
	}

	//	Metodo para cambiar de estado "currentState++" y el sprite del jugador "playerSprite"
	public void ChangeState()
	{
		currentState++;
		if ((int)currentState >= sprites.Length)
			currentState = GameState.Plant;
		playerSprite.gameObject.GetComponent<DragAndDrop>().ResetPosition();
		playerSprite.sprite = sprites[(int)currentState];
		StartCoroutine(Complete(currentState));
	}

	//	Info para saber que hay que hacer en un texto
	public void InfoUpdate(GameState state)
	{
		switch (state)
		{
			case GameState.Plant:
				info.text = "Plantar semilla";
				break;
			case GameState.Water:
				info.text = "Regar semilla";
				break;
			case GameState.Harvest:
				info.text = "Recollectar fresas";
				break;
			default:
				break;
		}
	}

	//	Pequeno feedback de que el jugador ha completado la fase
	IEnumerator Complete(GameState state)
	{
		panel.SetActive(true);
		Time.timeScale = 0.25f;
		yield return new WaitForSeconds(0.25f);
		panel.SetActive(false);
		InfoUpdate(state);
		Time.timeScale = 1.0f;
	}
}
