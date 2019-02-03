using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TargetHandler : MonoBehaviour
{

    //Swap curser per target type
    public Texture2D pointer; //Normal pointer
    public Texture2D target; //Cursor for clickable objects
    public Texture2D door; //Cursor for doorways
    public Texture2D combat; //Cursor for combat actions

    

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
            Transform objectHit = hit.transform;
            print(objectHit);
        }
        if (hit.collider.gameObject.tag == "Door" && hit.distance < 3)
        {
            Target targetCursor = FindObjectOfType<Target>();
            targetCursor.GetComponent<RawImage>().texture = door;
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            SceneManager.LoadScene("Shuli's House");
        }
        else
        {
            Target targetCursor = FindObjectOfType<Target>();
            targetCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
            targetCursor.GetComponent<RawImage>().texture = pointer;
        }
    }
}
