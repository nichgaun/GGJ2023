using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    [SerializeField] Slider healthBar; //set in editor

    public void Damage(int damage) {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }

        if (currentHealth <= 0) {
             //do  a kill
             Debug.Log("Player died");
        }
           
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            Damage(10);
        }
    }
}
