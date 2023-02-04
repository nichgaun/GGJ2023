using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{

    public GUISkin guiSkin;
    public Texture2D background, LOGO;
    public bool DragWindow = false;
    public bool showUI = false;

    private string clicked = "";
    private Rect WindowRect = new Rect((Screen.width / 2) - 400, Screen.height -120, 800, 100);
    GameManager gameManager;
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
        if (showUI) {
            if (background != null) 
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

            GUI.skin = guiSkin;

            WindowRect = GUI.Window(1, WindowRect, AbilityMenu, "Abilties");
        }
    }

    private void AbilityMenu(int id) {
        GUILayout.BeginHorizontal();
        if (GUI.Button(new Rect(15,15,80,80), "Move")) {
            //Move Function
            gameManager.GetPlayer().GetComponent<Movement>().OnClick();
        }
        if (GUI.Button(new Rect(110, 15, 80, 80), "Root")) {
            //Root Function
        }
        GUILayout.EndHorizontal();
    }

}
