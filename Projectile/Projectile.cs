using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float damage = 35f;

    private bool hasFinished;
    public bool HasFinished
    {
        get { return hasFinished; }
        set
        {
            hasFinished = value;
            //if (hasFinished)
            //{
            //    gameObject.SetActive(false);
            //}
            //else
            if (!hasFinished)
            {
                hasFly = false;
            }
        }
    }

    private bool hasFly;
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < 0)
        {
            HasFinished = true;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasFly || hasFinished) return;

        var collisionObject = collision.gameObject;

        if (collisionObject.tag == "Ground" || collisionObject.tag == "Player")
        {
            if (collisionObject.tag == "Player")
            {
                collisionObject.GetComponentInParent<PlayerHealth>().TakeDamage(damage);
            }

            HasFinished = true;
        }
    }

    /// <summary>
    /// Проверяем, улетел ли наш снаряд с родного слота (на слоте box collider висит триггерный)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!hasFly && other.gameObject.tag == "Player")
        {
            hasFly = true;
        }
    }
}
