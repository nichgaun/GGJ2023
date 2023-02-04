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
    [SerializeField] Texture2D buttonTexture;

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
        ButtonStyle.normal.background = buttonTexture;
        
        if (showUI) {
            WindowRect = GUI.Window(1, WindowRect, AbilityMenu, "Abilties", ButtonStyle);
        }
    }

    private void AbilityMenu(int id) {
        Movement movement = gameManager.GetPlayer().GetComponent<Movement>();
        GUILayout.BeginHorizontal();
        GUI.enabled = movement.moveRemaining > 0;
        if (GUI.Button(new Rect(15,15,80,80), "Move " + movement.moveRemaining, ButtonStyle)) {
            //Move Function
            movement.OnClick();
        }
        GUI.enabled = true;
        if (GUI.Button(new Rect(110, 15, 80, 80), "Root")) {
            //Root Function
        }
        GUILayout.EndHorizontal();
    }

}
