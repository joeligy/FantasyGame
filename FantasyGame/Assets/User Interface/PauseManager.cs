﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;
using System.IO;
//using UnityStandardAssets.ImageEffects;
/// <summary>
///  Copyright (c) 2016 Eric Zhu 
/// </summary>
namespace GreatArcStudios
{
    /// <summary>
    /// The pause menu manager. You can extend this to make your own. Everything is pretty modular, so creating you own based off of this should be easy. Thanks for downloading and good luck! 
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        /// <summary>
        /// This is a bool indicating whether or not the pause menu is active
        /// </summary> 
        public bool pauseMenuActive;
        /// <summary>
        /// This is the main panel holder, which holds the main panel and should be called "main panel"
        /// </summary> 
        public GameObject mainPanel;
        /// <summary>
        /// This is the video panel holder, which holds all of the controls for the video panel and should be called "vid panel"
        /// </summary>
        /// <summary>
        /// These are the game objects with the title texts like "Pause menu" and "Game Title" 
        /// </summary>
        public GameObject TitleTexts;
        /// <summary>
        /// The mask that makes the scene darker  
        /// </summary>
        public GameObject mask;
        /// <summary>
        /// Quit Panel animator  
        /// </summary>
        public Animator quitPanelAnimator;
        /// <summary>
        /// Pause menu text 
        /// </summary>
        public Text pauseMenu;

        /// <summary>
        /// Main menu level string used for loading the main menu. This means you'll need to type in the editor text box, the name of the main menu level, ie: "mainmenu";
        /// </summary>
        public String mainMenu;

        //DOF script name
        /// <summary>
        /// The Depth of Field script name, ie: "DepthOfField". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String DOFScriptName;

        /// <summary>
        /// The Ambient Occlusion script name, ie: "AmbientOcclusion". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String AOScriptName;

        /// <summary>
        /// The main camera, assign this through the editor. 
        /// </summary>        
        public Camera mainCam;
        internal static Camera mainCamShared;
        /// <summary>
        /// The main camera game object, assign this through the editor. 
        /// </summary> 
        public GameObject mainCamObj;

        /// <summary>
        /// Timescale value. The default is 1 for most games. You may want to change it if you are pausing the game in a slow motion situation 
        /// </summary> 
        public float timeScale = 1f;

        /// <summary>
        /// Event system
        /// </summary>
        public EventSystem uiEventSystem;
        /// <summary>
        /// Default selected on the main panel
        /// </summary>
        public GameObject defaultSelectedMain;

        /// <summary>
        /// The start method; you will need to place all of your inital value getting/setting here. 
        /// </summary>
        public void Start()
        {
            pauseMenuActive = false;
            mainCamShared = mainCam;

            //enable titles
            TitleTexts.SetActive(true);

            //Disable other panels
            mainPanel.SetActive(false);
            //Enable mask
            mask.SetActive(false);

            //saveSettings.LoadGameSettings(File.ReadAllText(Application.persistentDataPath + "/" + saveSettings.fileName));
        }

        /// <summary>
        /// Method to resume the game, so disable the pause menu and re-enable all other ui elements
        /// </summary>
        public void Resume()
        {
            pauseMenuActive = false;
            Time.timeScale = timeScale;

            mainPanel.SetActive(false);
            TitleTexts.SetActive(false);
            mask.SetActive(false);
        }

        /// <summary>
        /// All the methods relating to qutting should be called here.
        /// </summary>
        public void quitOptions()
        {
            quitPanelAnimator.enabled = true;
            quitPanelAnimator.Play("QuitPanelIn");
        }

        /// <summary>
        /// Method to quit the game. Call methods such as auto saving before qutting here.
        /// </summary>
        public void quitGame()
        {
            Application.Quit();
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        /// <summary>
        /// Cancels quittting by playing an animation.
        /// </summary>
        public void quitCancel()
        {
            quitPanelAnimator.Play("QuitPanelOut");
        }

        /// <summary>
        ///Loads the main menu scene.
        /// </summary>
        public void returnToMenu()
        {
            Application.LoadLevel(mainMenu);
            uiEventSystem.SetSelectedGameObject(defaultSelectedMain);
        }

        // Update is called once per frame
        /// <summary>
        /// The update method. This mainly searches for the user pressing the escape key.
        /// </summary>
        public void Update()
        {
            QuestLogOpen();
            if (mainPanel.active == true)
            {
                pauseMenu.text = "Options";
            }

            if (pauseMenuActive == true && CrossPlatformInputManager.GetButtonDown("Interact"))
            {
                ProcessMenuSelection();
            }

            if (CrossPlatformInputManager.GetButtonDown("Pause") && pauseMenuActive == false && !ConversationActive() && !QuestLogOpen())
            {
                //Has to be in a coroutine because of a Unity bug.
                //If you don't skip the initial frame where the menu is rendered, the selected button won't highlight.
                StartCoroutine(SetInitialMenuOption());

                Time.timeScale = 0;

                pauseMenuActive = true;
                mainPanel.SetActive(true);
                TitleTexts.SetActive(true);
                mask.SetActive(true);
            }
            else if(CrossPlatformInputManager.GetButtonDown("Pause") && pauseMenuActive == true) {
                Time.timeScale = timeScale;

                pauseMenuActive = false;
                mainPanel.SetActive(false);

                TitleTexts.SetActive(false);
                mask.SetActive(false);
            }
        }

        private bool ConversationActive()
        {
            PixelCrushers.DialogueSystem.DialogueSystemController controller = FindObjectOfType<PixelCrushers.DialogueSystem.DialogueSystemController>();
            return controller.isConversationActive;
        }

        private bool QuestLogOpen()
        {
            PixelCrushers.DialogueSystem.QuestLogWindow[] questLogWindows = FindObjectsOfType<PixelCrushers.DialogueSystem.QuestLogWindow>();
            foreach (PixelCrushers.DialogueSystem.QuestLogWindow window in questLogWindows)
            {
                if (window.isOpen)
                {
                    return true;
                }
            }
            return false;
        }

        IEnumerator SetInitialMenuOption()
        {
            uiEventSystem.SetSelectedGameObject(null);
            yield return null;
            uiEventSystem.SetSelectedGameObject(defaultSelectedMain);
        }

        private void ProcessMenuSelection()
        {
            string selectedButton = uiEventSystem.currentSelectedGameObject.name;
            if (selectedButton == "Resume")
            {
                Resume();
            } else if (selectedButton == "Quit")
            {
                quitGame();
            }
        }

    }
}
