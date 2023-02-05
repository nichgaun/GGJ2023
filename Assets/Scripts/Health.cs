using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    [SerializeField] Slider healthBar; //set in editor
    [SerializeField] GameObject gameOverUIPrefab; //set in editor
    [SerializeField] float timeToWaitAfterDeath; //set in editor


    public void Damage (int damage) {
        currentHealth -= damage;

        if (healthBar == null) {
            healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        }

        if (healthBar != null) {
            healthBar.value = (float)currentHealth / maxHealth;
        } 

        if (currentHealth <= 0) {
            GameObject g = Instantiate(gameOverUIPrefab);
            g.transform.SetParent(GameObject.Find("Canvas").transform, false);
            StartCoroutine(SendToMainMenuAfterTime());
        }
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     Damage(10);
        // }
    }

    IEnumerator SendToMainMenuAfterTime()
    {
        yield return new WaitForSeconds(timeToWaitAfterDeath);

        SceneManager.LoadScene(0);
    }
}
