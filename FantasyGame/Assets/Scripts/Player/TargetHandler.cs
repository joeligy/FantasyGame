using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using System;

public class TargetHandler : MonoBehaviour
{

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
            UpdateTargetCursorIcon(hit);
            if(hit.distance < maxInteractionDistance)
            {
                HandleInteraction(hit);
            }
        }
    }

    private void UpdateTargetCursorIcon(RaycastHit hit)
    {
        GameObject targetObject = hit.collider.gameObject;
        bool isDoor = targetObject.GetComponent<Door>();

        if (isDoor && hit.distance < maxInteractionDistance)
        {
            Target targetCursor = FindObjectOfType<Target>();
            targetCursor.GetComponent<RawImage>().texture = door;
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        }
        else if(hit.collider.gameObject.tag == "NPC" && hit.distance < maxInteractionDistance)
        {
            Target targetCursor = FindObjectOfType<Target>();
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            targetCursor.GetComponent<RawImage>().texture = target;
        }
        else
        {
            Target targetCursor = FindObjectOfType<Target>();
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
            targetCursor.GetComponent<RawImage>().texture = pointer;
        }
    }

    private void HandleInteraction(RaycastHit hit)
    {
        GameObject targetObject = hit.collider.gameObject;
        bool isDoor = targetObject.GetComponent<Door>();

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
