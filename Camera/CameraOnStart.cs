using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnStart : MonoBehaviour {

    [SerializeField] GameObject player1, player2;
    [SerializeField] float speed;
    [SerializeField] float delaySeconds = 1f;

    public bool HasAllMoving { get; private set; }

    private bool hasMovingToPlayer2, hasMovingToPlayer1;
    private Vector3 player2CameraPosition, player1CameraPosition, startPosition;

    //public delegate void CameraHasShownPlayers();
    public static event EventHandler OnCameraHasShownPlayers;

    // Use this for initialization
    void Start () {
        player1CameraPosition = new Vector3(player1.transform.position.x, transform.position.y, transform.position.z);
        player2CameraPosition = new Vector3(player2.transform.position.x, transform.position.y, transform.position.z);
        startPosition = transform.position;
    }
	
    //TODO сделать, чтобы камера сначала показывала игрока 1, а потом игрока 2, затем возвращалась к игроку 1, но чтобы он был с краю
	// Update is called once per frame
	void Update () {
        if (!HasAllMoving)
        {
            if (!hasMovingToPlayer1)
            {
                MoveToPlayer1();
            }
            else if (!hasMovingToPlayer2)
            {
                MoveToPlayer2();
            }
            else
            {
                //MoveToStartPosition();
                MoveToPlayer1();
            }
        }
	}

    private void MoveToPlayer2()
    {
        if (transform.position != player2CameraPosition)
        {
            MoveToPoint(player2CameraPosition);
        }
        else
        {
            StartCoroutine(new Delay().DelayAndProcessAction(
                () => hasMovingToPlayer2 = true, 
                delaySeconds
            ));
        }
    }

    private void MoveToPlayer1()
    {
        if (transform.position != player1CameraPosition)
        {
            MoveToPoint(player1CameraPosition);
        }
        else if (!hasMovingToPlayer1)
        {
            StartCoroutine(new Delay().DelayAndProcessAction(
                () => hasMovingToPlayer1 = true,
                delaySeconds
            ));
        }
        else
        {
            HasAllMoving = true;
            OnCameraHasShownPlayers(this, null);
        }
    }

    //private void MoveToStartPosition()
    //{
    //    if (transform.position != startPosition)
    //    {
    //        MoveToPoint(startPosition);
    //    }
    //    else
    //    {
    //        //StartCoroutine(Delay(() => {
    //            hasAllMoving = true;
    //            OnCameraHasShownPlayers(this, null);
    //        //}));
    //    }
    //}

    //Todo высчитывать позицию камеры, чтобы эта точка была посередине экрана moving camera to center
    private void MoveToPoint(Vector3 point)
    {
        var journeyLength = Vector3.Distance(transform.position, point);
        float distCovered = Time.deltaTime * speed;
        float fracJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(transform.position, point, fracJourney);
    }
}
