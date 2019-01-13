using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField] float delayBeforeChangePlayer = 1f;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] Projectile projectile;
    [SerializeField] Button ButtonSheep;
    [SerializeField] Button ButtonPig;
    [SerializeField] Button ButtonCow;
    [SerializeField] ParticleSystem bangPrefab;

    private PlayerAttack player1Attack;
    private PlayerAttack player2Attack;
    private PlayerHealth player1Health;
    private PlayerHealth player2Health;

    private bool canChangeActivePlayer;

    private void Awake()
    {
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

        CameraOnStart.OnCameraHasShownPlayers += OnCameraHasShownPlayers;
    }

    private void OnCameraHasShownPlayers(object sender, System.EventArgs e)
    {
        //SetButtonsEnable();
        player1Attack.TakeTurn(projectile);
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
    void Update() {
        //TODO disable кнопок после сбрасывания зверя
        if (projectile.HasFinished)
        {
            BlowUp();
        }
        if (canChangeActivePlayer)
        {
            ChangePlayer();
        }
        if ((player1Attack.IsMyTurn && !player1Attack.AnimalSpent) || (player2Attack.IsMyTurn && !player2Attack.AnimalSpent))
        {
            SetButtonsEnable();
        }
        else
        {
            SetButtonsDisable();
        }
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

    private void BlowUp()
    {
        var bang = Instantiate(bangPrefab);
        bang.transform.position = projectile.transform.position;
        projectile.gameObject.SetActive(false);
        projectile.HasFinished = false;
        bang.Play();
        StartCoroutine(new Delay().DelayAndProcessAction(
            () => {
                Destroy(bang.gameObject);
                canChangeActivePlayer = true;
            },
            delayBeforeChangePlayer //bang.main.duration
        ));
    }
}
