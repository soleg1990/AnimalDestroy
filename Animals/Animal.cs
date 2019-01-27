using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

    public bool IsGroundCollision; //грубо говоря, если скинули мимо катапульты

    private void OnEnable()
    {
        isCollisionEnterOnce = false;
        IsGroundCollision = false;
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private bool isCollisionEnterOnce;
    private void OnCollisionEnter(Collision collision)
    {
        if (isCollisionEnterOnce) return;

        isCollisionEnterOnce = true;

        StartCoroutine(new Delay().DelayAndProcessAction(
            () => 
            {
                if (collision.gameObject.tag == "Ground")
                {
                    IsGroundCollision = true;
                }
                gameObject.SetActive(false);
            },
            3f
        ));
    }
}
