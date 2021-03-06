﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] float maxHealth = 100f;
    [SerializeField] Slider healthSlider;

    private float currentHealth;
    public float CurrentHealth { get { return currentHealth; } }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthSlider)
        {
            healthSlider.value = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
        if (currentHealth <= 0)
        {
            CrushCatapult();
        }
    }

    private void CrushCatapult()
    {
        Destroy(gameObject.GetComponentInChildren<HingeJoint>());
        var xVelosity = Random.Range(-15, -5) * Mathf.Sign(transform.rotation.y); //чтобы назад башню снесло
        var yVelosity = Random.Range(3, 15);
        var zVelosity = 0;//Random.Range(-3, 30);
        var xTorque = Random.Range(-300, 300);
        var yTorque = Random.Range(-300, 300);
        var zTorque = Random.Range(-300, 300);
        foreach (var rig in GetComponentsInChildren<Rigidbody>())
        {
            rig.velocity = new Vector3(xVelosity, yVelosity, zVelosity);
            rig.AddTorque(new Vector3(xTorque, yTorque, zTorque));
        }
    }
}
