using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using System;

public class TargetAndSelectorHandler : MonoBehaviour
{

    [SerializeField] Target targetCursor;
    [SerializeField] SelectorUIHeader selectorUIHeader;

    //Swap curser per target type
    public Texture2D pointer; //Normal pointer
    public Texture2D target; //Cursor for clickable objects
    public Texture2D door; //Cursor for doorways
    public Texture2D combat; //Cursor for combat actions

    //Maximum distance to interact with objects
    float maxInteractionDistance = 4f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        if (Physics.Raycast(ray, out hit))
        {
            EnableOrDisableTargetCursor();
            SetTargetCursorImage(hit);

            SetSelectorUIHeader(hit);

            if (hit.distance < maxInteractionDistance)
            {
                HandleInteraction(hit);
            }
        }
    }

    private void EnableOrDisableTargetCursor()
    {
        if (PauseMenuEnabled() || ConversationActive())
        {
            targetCursor.gameObject.SetActive(false);
        }
        else
        {
            targetCursor.gameObject.SetActive(true);
        }
    }

    private void SetTargetCursorImage(RaycastHit hit)
    {
        GameObject targetObject = hit.collider.gameObject;
        if (targetObject.GetComponent<Door>() && hit.distance < maxInteractionDistance)
        {
            targetCursor.GetComponent<RawImage>().texture = door;
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        }
        else if (targetObject.GetComponent<NPC>() && hit.distance < maxInteractionDistance)
        {
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            targetCursor.GetComponent<RawImage>().texture = target;
        }
        else
        {
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
            targetCursor.GetComponent<RawImage>().texture = pointer;
        }
    }

    private void SetSelectorUIHeader(RaycastHit hit)
    {
        Text selectorTextComponent = selectorUIHeader.GetComponent<Text>();
        GameObject targetObject = hit.collider.gameObject;
        selectorUIHeader.gameObject.SetActive(true);

        if (targetObject.GetComponent<Door>() && hit.distance < maxInteractionDistance)
        {
            Door doorInfo = targetObject.GetComponent<Door>();
            string sceneName = doorInfo.GetSceneName();
            selectorTextComponent.text = sceneName;
        }
        else if (targetObject.GetComponent<NPC>() && hit.distance < maxInteractionDistance)
        {
            selectorTextComponent.text = targetObject.name;
        }
        else
        {
            selectorUIHeader.gameObject.SetActive(false);
        }

        if(PauseMenuEnabled() || ConversationActive())
        {
            selectorUIHeader.gameObject.SetActive(false);
        }
    }

    private bool PauseMenuEnabled()
    {
        GreatArcStudios.PauseManager pauseManager = FindObjectOfType<GreatArcStudios.PauseManager>();
        return pauseManager.pauseMenuActive;
    }

    private bool ConversationActive()
    {
        PixelCrushers.DialogueSystem.DialogueSystemController controller = FindObjectOfType<PixelCrushers.DialogueSystem.DialogueSystemController>();
        return controller.isConversationActive;
    }

    private void HandleInteraction(RaycastHit hit)
    {
        GameObject targetObject = hit.collider.gameObject;
        bool isDoor = targetObject.GetComponent<Door>();

        //We use GetButtonUp here instead of GetButtonDown because something bad happens if you use the latter.
        //Something about loading the new scene while the button is down messes things up.
        //And when you load the new scene, that button doesn't work the first time you try to use it.
        if (CrossPlatformInputManager.GetButtonUp("Interact")) {
            if(isDoor)
            {
                SaveSpawnPoint(targetObject);
                LoadScene(targetObject);
            }
        }
    }

    private void SaveSpawnPoint(GameObject targetObject)
    {
        Door doorInfo = targetObject.GetComponent<Door>();
        string spawnPoint = doorInfo.GetSpawnPointName();
        if(spawnPoint.Length > 0)
        {
            PlayerSpawnManager playerSpawnManager = FindObjectOfType<PlayerSpawnManager>();
            playerSpawnManager.spawnPoint = spawnPoint;

        }
    }

    private void LoadScene(GameObject targetObject)
    {
        Door doorInfo = targetObject.GetComponent<Door>();
        string doorSceneName = doorInfo.GetSceneName();
        if (doorSceneName != null)
        {
            SceneManager.LoadScene(doorSceneName);
        }
    }
}
