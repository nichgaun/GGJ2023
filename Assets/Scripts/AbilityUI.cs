using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AbilityUI : MonoBehaviour {
    public GUISkin guiSkin;
    public Texture2D background, LOGO;
    public bool DragWindow = false;
    public bool showUI = true;
    [SerializeField] Font font;

    private string clicked = "";
    private Rect WindowRect = new Rect((Screen.width / 2) - 400, Screen.height -120, 800, 100);
    GameManager gameManager;
    GUIStyle ButtonStyle = new GUIStyle();

    List<Ability> abilities = new List<Ability>();

    // Start is called before the first frame update
    void Start () {
        gameManager = Camera.main.GetComponent<GameManager>();
        gameManager.SetAbilityUI(this);

        foreach (AbilityType type in Enum.GetValues(typeof(AbilityType))) {
            Ability ability = new Ability();
            ability.Initialize(type);
            abilities.Add(ability);
        }
    }

    public void Turn () {
        foreach (var ability in abilities) {
            ability.Turn();
        }
    }

    private void Update () {
        //TODO: Switch to turn based rather than button toggle
        if (Input.GetKeyDown(KeyCode.Tab)) {
            showUI = !showUI;
        }

        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     Turn();
        // }
    }
    
    private void OnGUI() {
        GUI.skin.font = font;
        ButtonStyle = GUI.skin.button;
        
        
        WindowRect = GUI.Window(1, WindowRect, AbilityMenu, "Abilties");
    }

    private void AbilityMenu(int id) {
        GUILayout.BeginHorizontal();
        int i = 0;
        foreach (var ability in abilities) {
            GUI.enabled = !ability.used;
            if (GUI.Button(new Rect(15 + 105*i,15,80,80), ability.description(ability))) {
                ability.OnClick();
            }
            i++;
        }
        
        
        GUI.enabled = !Enemy.moving;
        if (GUI.Button(new Rect(15 + 105*i,15,80,80), "Next Turn")) {
            gameManager.Turn();
        }

        GUILayout.EndHorizontal();
    }

}
