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
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
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
        if (hit.collider.gameObject.tag == "Door" && hit.distance < maxInteractionDistance)
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
        if (CrossPlatformInputManager.GetButtonDown("Interact")) {
            if(targetObject.tag == "Door")
            {
                LoadScene("Shuli's House");
            }
        }
    }

    private void LoadScene(string sceneName)
    {
        PlayerLocationCache cache = FindObjectOfType<PlayerLocationCache>();
        cache.SavePreviousSceneData(SceneManager.GetActiveScene().name, transform.position);
        SceneManager.LoadScene(sceneName);
    }
}
