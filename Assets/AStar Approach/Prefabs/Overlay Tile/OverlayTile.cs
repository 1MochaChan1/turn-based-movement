using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OverlayTile : MonoBehaviour
{
    // Update is called once per frame
    public int H;
    public int G;

    public int F { get {  return H+G; } }

    public bool isBlocked;
    public OverlayTile previous;

    public Vector3Int gridLocation;

    void Update()
    {
        
    }

    public void ShowOverlay()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void HideOverlay()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
