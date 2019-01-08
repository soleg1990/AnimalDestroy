using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] Button ButtonSheep;

    private PlayerAttack player1Attack;
    private PlayerAttack player2Attack;

    private void Awake()
    {
        player1Attack = player1.GetComponent<PlayerAttack>();
        player2Attack = player2.GetComponent<PlayerAttack>();
        //player1Attack.enabled = false;
        //player2Attack.enabled = false;
        SetPlayer2RandomPosition();
    }

    private void SetPlayer2RandomPosition()
    {
        var temp = player2.transform.position;
        temp.x = Random.Range(5f, 22f);
        player2.transform.position = temp;
    }

    void Start () {
        SetButtonsDisable();

        CameraOnStart.OnCameraHasShownPlayers += OnCameraHasShownPlayers;
	}

    private void OnCameraHasShownPlayers(object sender, System.EventArgs e)
    {
        SetButtonsEnable();
        //gameHasStarted = true;
        player1Attack.IsMyTurn = true;
    }

    private void SetButtonsDisable()
    {
        ButtonSheep.interactable = false;
    }

    private void SetButtonsEnable()
    {
        ButtonSheep.interactable = true;
    }

    // Update is called once per frame
    void Update () {
        //if (gameHasStarted)
        //TODO disable кнопок после сбрасывания зверя
        if (player1Attack.IsMyTurn)
        {
            if (player1Attack.HasShot)
            {
                player2Attack.IsMyTurn = true;
                player1Attack.IsMyTurn = false;
            }
        }
        else if (player2Attack.IsMyTurn)
        {
            if (player2Attack.HasShot)
            {
                player1Attack.IsMyTurn = true;
                player2Attack.IsMyTurn = false;
            }
        }
	}
}
