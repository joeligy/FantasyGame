using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SFB_DisplayPolycount : MonoBehaviour {

	[MenuItem("Window/SFBayStudios/Polycount %#p")]
	static void ShowPolycount(){
		int totalTriCount = 0;
		// Get count of parnet object if applicable
		if (Selection.activeGameObject.GetComponent<MeshFilter>()){
			Mesh parentMesh = Selection.activeGameObject.GetComponent<MeshFilter>().sharedMesh;
			int parentTri = parentMesh.triangles.Length / 3;
			totalTriCount += parentTri;
		}
		//Get count of all children of object
		Transform[] allChildren = Selection.activeGameObject.GetComponentsInChildren<Transform>();
		foreach(Transform child in allChildren)
		{
			if (child.gameObject.GetComponent<MeshFilter>()){
				Mesh objMesh = child.gameObject.GetComponent<MeshFilter>().sharedMesh;
				int triCount = objMesh.triangles.Length / 3;
				totalTriCount += triCount;
			} else if (child.gameObject.GetComponent<SkinnedMeshRenderer>()){
				Mesh objMesh2 = child.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
				int triCount2 = objMesh2.triangles.Length / 3;
				totalTriCount += triCount2;
			}
		}
		Debug.Log("There are " + totalTriCount + " triangles in the selection.");
	}
}
