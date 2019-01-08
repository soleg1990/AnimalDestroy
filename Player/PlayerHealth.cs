using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] float maxHealth = 100f;

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

        if (currentHealth <= 0)
        {
            CrushCatapult();
        }
    }

    private void CrushCatapult()
    {
        Destroy(gameObject.GetComponentInChildren<HingeJoint>());
        var xVelosity = Random.Range(-30, 30);
        var yVelosity = Random.Range(1, 30);
        var zVelosity = Random.Range(-3, 30);
        foreach (var rig in GetComponentsInChildren<Rigidbody>())
        {
            rig.velocity = new Vector3(xVelosity, yVelosity, zVelosity);
        }
    }
}
