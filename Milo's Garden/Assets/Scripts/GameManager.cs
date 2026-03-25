using System.Collections;
using TMPro;
using UnityEngine;
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
	public float waterTime = 5.0f;// que sea igual que la animacion de crecimiento de la planta
    public GameObject fruitHolder;
	public GameObject waterHolder;
	private Animator plantAnim;
	public GameObject gameOver;

    public AudioSource audioSource;
    public AudioClip plantSound;
    public AudioClip growSound;
    public AudioClip harvestSound;
    public AudioClip gameoverSound;

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
		plantAnim = seed.gameObject.GetComponent<Animator>();
        playerSprite.sprite = sprites[(int)currentState];
		slider.gameObject.SetActive(false);
		fruitHolder.SetActive(false);
		waterHolder.SetActive(false);
		panel.SetActive(false);
		seed.SetActive(false);
		InfoUpdate(currentState);
        gameOver.SetActive(false);
    }

	//	Cada frame validamos en que estado esta el juego y ejecutamos la logica de este
	void Update()
	{
		if (gameFinished)
		{
            StartCoroutine(Complete(currentState));
            return;
			
        }
        //	Plantamos la semilla si se ha colocado esta correctamente "isInteractionSucceed = true" y cambiamos estado "ChangeState()"
        if (currentState == GameState.Plant)
		{
			if (isInteractionSucceed)
			{
				isInteractionSucceed = false;
				Debug.Log("Seed planted!");
                audioSource.PlayOneShot(plantSound);
                seed.SetActive(true);
				ChangeState();
			}
		}
		//	Si detecta agua en el terreno "isInteractionSucceed = true" entonces el slider va avanzando "timer / waterTime"
		else if (currentState == GameState.Water)
		{
			if (isInteractionSucceed)
			{
				timer += Time.deltaTime;
				slider.value = timer / waterTime;
                if (plantAnim != null)
                {
                    AnimationClip clip = plantAnim.runtimeAnimatorController.animationClips[0];
                    float animLength = clip.length;
                    plantAnim.speed = animLength / waterTime;
                }
                //	Si se alcanza el maximo valor cambiamos de estado "ChangeState()"
                if (timer >= waterTime)
				{
					timer = 0f;
					slider.value = 0;
					slider.gameObject.SetActive(false);
					isInteractionSucceed = false;
					Debug.Log("Plant Watered!");
					ChangeState();
					if (plantAnim != null)
					{
						plantAnim.speed = 0f;
					}
                    audioSource.PlayOneShot(growSound);
                }
			}
			else
			{
                if (plantAnim.GetCurrentAnimatorClipInfo(0).Length > 0 && plantAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Growing")
                    plantAnim.speed = 0f;
            }
        }
		//	Si detecta que ya no quedan frutas "fruitHolder.transform.childCount <= 0", entonces se termina el juego!
		else if (currentState == GameState.Harvest)
		{

			if (fruitHolder.transform.childCount <= 0)
			{
                audioSource.PlayOneShot(harvestSound);
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
        GameState state = currentState;
        switch (state)
        {
            case GameState.Plant:
                break;
            case GameState.Water:
				if (plantAnim != null)
				{
					plantAnim.gameObject.SetActive(true);
                    plantAnim.SetTrigger("StartGrowing");
				}
                slider.gameObject.SetActive(true);
                waterHolder.SetActive(true);
                break;
            case GameState.Harvest:
                slider.gameObject.SetActive(false);
                waterHolder.SetActive(false);
                fruitHolder.SetActive(true);
				if(plantAnim != null)
					plantAnim.SetTrigger("IdleRecolect");
                break;
            default:
                break;
        }
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
				info.text = "Planta la semilla";

				break;
			case GameState.Water:
				info.text = "Riega la planta";

				break;
			case GameState.Harvest:
				info.text = "Recoge las fresas";
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
		if (gameFinished)
		{
            audioSource.PlayOneShot(gameoverSound);
            gameOver.SetActive(true);
			info.transform.parent.transform.gameObject.SetActive(false);
			Destroy(this);
		}
    }
}
