using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    [SerializeField] float speed = 10f;

    private CameraOnStart cameraOnStart;

	// Use this for initialization
	void Start () {
        cameraOnStart = GetComponent<CameraOnStart>();
	}
	
	// Update is called once per frame
	void Update () {
		if (cameraOnStart.HasAllMoving)
        {
            var currentProjectile = GameObject.FindGameObjectWithTag("Projectile");
            if (!currentProjectile) return;

            var targetPosition = currentProjectile.transform.position;
            targetPosition.y = transform.position.y;
            targetPosition.z = transform.position.z;
            Vector3 currentPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
            transform.position = currentPosition;
        }
	}
}
