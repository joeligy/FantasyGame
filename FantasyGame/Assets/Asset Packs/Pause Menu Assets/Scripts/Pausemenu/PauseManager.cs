using System;
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
        /// Audio Panel animator
        /// </summary>
        public Animator audioPanelAnimator;
        /// <summary>
        /// Video Panel animator  
        /// </summary>
        public Animator vidPanelAnimator;
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
        /// The terrain detail density float. It's only public because you may want to adjust it in editor
        /// </summary> 
        public  float detailDensity;

        /// <summary>
        /// Timescale value. The default is 1 for most games. You may want to change it if you are pausing the game in a slow motion situation 
        /// </summary> 
        public float timeScale = 1f;
        /// <summary>
        /// One terrain variable used if you have a terrain plugin like rtp. 
        /// </summary>
        public  Terrain terrain;
        /// <summary>
        /// Other terrain variable used if you want to have an option to target low end harware.
        /// </summary>
        public  Terrain simpleTerrain;
        /// <summary>
        /// Inital shadow distance 
        /// </summary>
        internal static float shadowDistINI;
        /// <summary>
        /// Inital render distance 
        /// </summary>
        internal static float renderDistINI;
        /// <summary>
        /// Inital AA quality 2, 4, or 8
        /// </summary>
        internal static float aaQualINI;
        /// <summary>
        /// Inital terrain detail density
        /// </summary>
        internal static float densityINI;
        /// <summary>
        /// Amount of trees that are acutal meshes
        /// </summary>
        internal static float treeMeshAmtINI;
        /// <summary>
        /// Inital fov 
        /// </summary>
        internal static float fovINI;
        /// <summary>
        /// Inital msaa amount 
        /// </summary>
        internal static int msaaINI;
        /// <summary>
        /// Inital vsync count, the Unity docs say,
        /// <code> 
        /// //This will set the game to have one VSync per frame
        /// QualitySettings.vSyncCount = 1;
        /// </code>
        /// <code>
        /// //This will disable vsync
        /// QualitySettings.vSyncCount = 0;
        /// </code>
        /// </summary>
        internal static int vsyncINI;
        /// <summary>
        /// AA drop down menu.
        /// </summary>
        public Dropdown aaCombo;
        /// <summary>
        /// Aniso drop down menu.
        /// </summary>
        public Dropdown afCombo;

        public Slider fovSlider;
        public Slider modelQualSlider;
        public Slider terrainQualSlider;
        public Slider highQualTreeSlider;
        public Slider renderDistSlider;
        public Slider terrainDensitySlider;
        public Slider shadowDistSlider;
        public Slider audioMasterSlider;
        public Slider audioMusicSlider;
        public Slider audioEffectsSlider;
        public Slider masterTexSlider;
        public Slider shadowCascadesSlider;
        public Toggle vSyncToggle;
        public Toggle aoToggle;
        public Toggle dofToggle;
        public Toggle fullscreenToggle;
        /// <summary>
        /// The preset text label.
        /// </summary>
        public Text presetLabel;
        /// <summary>
        /// Resolution text label.
        /// </summary>
        public Text resolutionLabel;
        /// <summary>
        /// Lod bias float array. You should manually assign these based on the quality level.
        /// </summary>
        public  float[] LODBias;
        /// <summary>
        /// Shadow distance array. You should manually assign these based on the quality level.
        /// </summary>
        public  float[] shadowDist;
        /// <summary>
        /// An array of music audio sources
        /// </summary>
        public  AudioSource[] music;
        /// <summary>
        /// An array of sound effect audio sources
        /// </summary>
        public  AudioSource[] effects;
        /// <summary>
        /// An array of the other UI elements, which is used for disabling the other elements when the game is paused.
        /// </summary>
        public GameObject[] otherUIElements;
        /// <summary>
        /// Editor boolean for hardcoding certain video settings. It will allow you to use the values defined in LOD Bias and Shadow Distance
        /// </summary>
        public Boolean hardCodeSomeVideoSettings;
        /// <summary>
        /// Boolean for turning on simple terrain
        /// </summary>
        public  Boolean useSimpleTerrain;
        public static Boolean readUseSimpleTerrain;
        /// <summary>
        /// Event system
        /// </summary>
        public EventSystem uiEventSystem;
        /// <summary>
        /// Default selected on the video panel
        /// </summary>
        public GameObject defaultSelectedVideo;
        /// <summary>
        /// Default selected on the video panel
        /// </summary>
        public GameObject defaultSelectedAudio;
        /// <summary>
        /// Default selected on the main panel
        /// </summary>
        public GameObject defaultSelectedMain;
        //last music multiplier; this should be a value between 0-1
        internal static float lastMusicMult;
        //last audio multiplier; this should be a value between 0-1
        internal static float lastAudioMult;
        //Initial master volume
        internal static float beforeMaster;
        //last texture limit 
        internal static int lastTexLimit;
        //int for amount of effects
        private int _audioEffectAmt = 0;
        //Inital audio effect volumes
        private float[] _beforeEffectVol;

        //Initial music volume
        private float _beforeMusic;
        //Preset level
        private int _currentLevel;
        //Resoutions
        private Resolution[] allRes;
        //Camera dof script
        private MonoBehaviour tempScript;
        //Presets 
        private String[] presets;
        //Fullscreen Boolean
        private Boolean isFullscreen;
        //current resoultion
        internal static Resolution currentRes;
        //Last resoultion 
        private Resolution beforeRes;

        //last shadow cascade value
        internal static int lastShadowCascade;
       
        public static Boolean aoBool;
        public static Boolean dofBool;
        private Boolean lastAOBool;
        private Boolean lastDOFBool;
        public static Terrain readTerrain;
        public static Terrain readSimpleTerrain;

        private SaveSettings saveSettings = new SaveSettings();
        /*
        //Color fade duration value
        //public float crossFadeDuration;
        //custom color
        //public Color _customColor;
        
         //Animation clips
         private AnimationClip audioIn;
         private AnimationClip audioOut;
         public AnimationClip vidIn;
         public AnimationClip vidOut;
         public AnimationClip mainIn;
         public AnimationClip mainOut; 
          */
        //Blur Variables
        //Blur Effect Script (using the standard image effects package) 
        //public Blur blurEffect;
        //Blur Effect Shader (should be the one that came with the package)
        //public Shader blurEffectShader;
        //Boolean for if the blur effect was originally enabled
        //public Boolean blurBool;

        /// <summary>
        /// The start method; you will need to place all of your inital value getting/setting here. 
        /// </summary>
        public void Start()
        {

            pauseMenuActive = false;
           
            readUseSimpleTerrain = useSimpleTerrain;
            if (useSimpleTerrain)
            {
                readSimpleTerrain = simpleTerrain;
            }
            else
            {
                readTerrain = terrain;
            }
           
            mainCamShared = mainCam;
            //Set the lastmusicmult and last audiomult
            //Set the first selected item
            uiEventSystem.firstSelectedGameObject = defaultSelectedMain;
            //Get the presets from the quality settings 
            presets = QualitySettings.names;
            _currentLevel = QualitySettings.GetQualityLevel();
            //Get the current resoultion, if the game is in fullscreen, and set the label to the original resolution
            allRes = Screen.resolutions;
            currentRes = Screen.currentResolution;
            //Debug.Log("ini res" + currentRes);
            isFullscreen = Screen.fullScreen;
            //get all specified audio source volumes
            _beforeEffectVol = new float[_audioEffectAmt];
            beforeMaster = AudioListener.volume;
            //get all ini values
            aaQualINI = QualitySettings.antiAliasing;
            renderDistINI = mainCam.farClipPlane;
            shadowDistINI = QualitySettings.shadowDistance;
            fovINI = mainCam.fieldOfView;
            msaaINI = QualitySettings.antiAliasing;
            vsyncINI = QualitySettings.vSyncCount;
            //enable titles
            TitleTexts.SetActive(true);
            //Find terrain
            terrain = Terrain.activeTerrain;
            //Disable other panels
            mainPanel.SetActive(false);
            //Enable mask
            mask.SetActive(false);
            //set last texture limit
            lastTexLimit = QualitySettings.masterTextureLimit;
            //set last shadow cascade 
            lastShadowCascade = QualitySettings.shadowCascades;
            //saveSettings.LoadGameSettings(File.ReadAllText(Application.persistentDataPath + "/" + saveSettings.fileName));
            try
            {
                densityINI = Terrain.activeTerrain.detailObjectDensity;
            }
            catch
            {
                if (terrain = null)
                {
                    Debug.Log("Terrain Not Assigned");
                }
            }

            //set the blur boolean to false;
            //blurBool = false;
            //Add the blur effect
            /*mainCamObj.AddComponent(typeof(Blur));
            blurEffect = (Blur)mainCamObj.GetComponent(typeof(Blur));
            blurEffect.blurShader = blurEffectShader;
            blurEffect.enabled = false;  */
        }
        /// <summary>
        /// Restart the level by loading the loaded level.
        /// </summary>
        public void Restart()
        {
            Application.LoadLevel(Application.loadedLevel);
            uiEventSystem.firstSelectedGameObject = defaultSelectedMain;
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
            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(true);
            }
            /* if (blurBool == false)
             {
                 blurEffect.enabled = false;
             }
             else
             {
                 //if you want to add in your own stuff do so here
                 return;
             } */
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
            readUseSimpleTerrain = useSimpleTerrain;
            useSimpleTerrain = readUseSimpleTerrain;
            //colorCrossfade();
            if (mainPanel.active == true)
            {
                pauseMenu.text = "Pause Menu";
            }

            if (CrossPlatformInputManager.GetButtonDown("Pause") && pauseMenuActive == false)
            {
                uiEventSystem.SetSelectedGameObject(defaultSelectedMain);

                pauseMenuActive = true;
                mainPanel.SetActive(true);

                TitleTexts.SetActive(true);
                mask.SetActive(true);
                Time.timeScale = 0;

                for (int i = 0; i < otherUIElements.Length; i++)
                {
                    otherUIElements[i].gameObject.SetActive(false);
                }
                /* if (blurBool == false)
                  {
                     blurEffect.enabled = true;
                 }  */
            }
            else if(CrossPlatformInputManager.GetButtonDown("Pause") && pauseMenuActive == true) {
                Time.timeScale = timeScale;

                pauseMenuActive = false;
                mainPanel.SetActive(false);

                TitleTexts.SetActive(false);
                mask.SetActive(false);

                for (int i = 0; i < otherUIElements.Length; i++)
                {
                    otherUIElements[i].gameObject.SetActive(true);
                }
            }



        }

    }
}
