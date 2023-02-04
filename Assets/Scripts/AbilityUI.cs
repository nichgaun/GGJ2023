using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUI : MonoBehaviour
{

    public GUISkin guiSkin;
    public Texture2D background, LOGO;
    public bool DragWindow = false;
    public bool showUI = false;
    [SerializeField] Font font;

    private string clicked = "";
    private Rect WindowRect = new Rect((Screen.width / 2) - 400, Screen.height -120, 800, 100);
    GameManager gameManager;
    GUIStyle ButtonStyle = new GUIStyle();
    // Start is called before the first frame update
    void Start()
    {
        gameManager = Camera.main.GetComponent<GameManager>();
        
        
    }

    private void Update() {
        //TODO: Switch to turn based rather than button toggle
        if (Input.GetKeyDown(KeyCode.Tab)) {
            showUI = !showUI;
        }
    }
    
    private void OnGUI() {
        GUI.skin.font = font;
        ButtonStyle = GUI.skin.button;
        
        if (showUI) {
            WindowRect = GUI.Window(1, WindowRect, AbilityMenu, "Abilties");
        }
    }

    private void AbilityMenu(int id) {
        Movement movement = gameManager.GetPlayer().GetComponent<Movement>();
        Trap trap = gameManager.GetPlayer().GetComponent<Trap>();
        GUILayout.BeginHorizontal();
        GUI.enabled = movement.moveRemaining > 0;
        if (GUI.Button(new Rect(15,15,80,80), "Move " + movement.moveRemaining)) {
            //Move Function
            movement.OnClick();
        }
        GUI.enabled = true;
        if (GUI.Button(new Rect(110, 15, 80, 80), "Root")) {
            //Root Function
        }
        if (GUI.Button(new Rect(205, 15, 80, 80), "Trap:Rng " + trap.maxDistance)) {
            //Trap Function
            trap.OnClick();
        }
        GUILayout.EndHorizontal();
    }

}
