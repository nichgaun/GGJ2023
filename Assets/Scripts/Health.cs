using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    public Slider HealthBar;

    public void Damage(int damage) {
        currentHealth -= damage;
        HealthBar.value = (float)currentHealth/maxHealth;

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
