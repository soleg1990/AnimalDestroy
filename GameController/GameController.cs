using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField] float delayBeforeChangePlayer = 1f;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Button ButtonSheep;
    [SerializeField] Button ButtonPig;
    [SerializeField] Button ButtonCow;
    [SerializeField] Text playerWinText;
    [SerializeField] ParticleSystem bangPrefab;
    [SerializeField] AudioClip explosion;

    private PlayerAttack player1Attack;
    private PlayerAttack player2Attack;
    private PlayerHealth player1Health;
    private PlayerHealth player2Health;
    private Projectile projectile;
    private CameraOnStart cameraOnStart;
    private AudioSource audioSource;

    private bool canChangeActivePlayer;
    private bool gameOver;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = explosion;
        cameraOnStart = FindObjectOfType<CameraOnStart>();
        projectile = Instantiate(projectilePrefab);
        projectile.gameObject.SetActive(false);
        player1Attack = player1.GetComponent<PlayerAttack>();
        player2Attack = player2.GetComponent<PlayerAttack>();
        player1Health = player1.GetComponent<PlayerHealth>();
        player2Health = player2.GetComponent<PlayerHealth>();
        SetPlayer2RandomPosition();
    }

    private void SetPlayer2RandomPosition()
    {
        var temp = player2.transform.position;
        temp.x = Random.Range(5f, 22f);
        player2.transform.position = temp;
    }

    void Start() {
        SetButtonsDisable();

        cameraOnStart.OnCameraHasShownPlayers += OnCameraHasShownPlayers;
    }

    private void OnCameraHasShownPlayers(object sender, System.EventArgs e)
    {
        //SetButtonsEnable();
        player1Attack.TakeTurn(projectile);
        //player1Health.TakeDamage(1000);
        //player2Health.TakeDamage(1000);
    }

    private void SetButtonsDisable()
    {
        ButtonSheep.interactable = false;
        ButtonPig.interactable = false;
        ButtonCow.interactable = false;
    }

    private void SetButtonsEnable()
    {
        ButtonSheep.interactable = true;
        ButtonPig.interactable = true;
        ButtonCow.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        if (projectile.HasFinished)
        {
            BlowUpProjectile();
        }
        if (canChangeActivePlayer)
        {
            ChangePlayer();
        }
        CheckAnimalButtonsEnable();
        CheckWin();
    }

    private void CheckAnimalButtonsEnable()
    {
        //TODO вынести всю эту хурму в отдельный компонент
        if ((player1Attack.IsMyTurn && !player1Attack.AnimalSpent) || (player2Attack.IsMyTurn && !player2Attack.AnimalSpent))
        {
            SetButtonsEnable();
        }
        else
        {
            SetButtonsDisable();
        }
    }

    private void CheckWin()
    {
        if (player1Health.CurrentHealth <= 0)
        {
            OnPlayerWin("Player2");
        }
        else if (player2Health.CurrentHealth <= 0)
        {
            OnPlayerWin("Player1");
        }
    }

    private void OnPlayerWin(string playerName)
    {
        gameOver = true;
        StartCoroutine(new Delay().DelayAndProcessAction(
            () => {
                playerWinText.gameObject.SetActive(true);
                playerWinText.text = playerName + " Win!";
            },
            2f
        ));
        StartCoroutine(new Delay().DelayAndProcessAction(
            () => SceneManager.LoadScene(0),
            5f
        ));
    }

    private void ChangePlayer()
    {
        canChangeActivePlayer = false;

        if (player1Attack.IsMyTurn)
        {
            player1Attack.GiveTurn();
            if (player2Health.CurrentHealth > 0) player2Attack.TakeTurn(projectile);
        }
        else
        {
            player2Attack.GiveTurn();
            if (player1Health.CurrentHealth > 0) player1Attack.TakeTurn(projectile);
        }
    }
 
    private void BlowUpProjectile()
    {
        var bang = Instantiate(bangPrefab);
        bang.transform.position = projectile.transform.position;
        bang.Play();
        audioSource.Play(); 
        projectile.gameObject.SetActive(false);
        projectile.HasFinished = false;
        StartCoroutine(new Delay().DelayAndProcessAction(
            () => {
                Destroy(bang.gameObject);
                canChangeActivePlayer = true;
            },
            delayBeforeChangePlayer //bang.main.duration
        ));
    }
}
