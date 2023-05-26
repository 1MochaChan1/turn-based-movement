using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private const float characterY = 1.5f;
    public Camera cam;
    public GameObject characterPrefab;
    public LayerMask mask;
    public int speed = 5;


    private PathFinder pathFinder;
    private List<OverlayTile> path;
    private Character character;

    void Start()
    {
        cam = Camera.main;
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
    }

    // Update is called once per frame
    void Update()
    {
        var focusedTileHit = GetFocusTile();


        if (focusedTileHit.HasValue)
        {
            GameObject overlayTile = focusedTileHit.Value.collider.gameObject;
            transform.position = overlayTile.transform.position;

            if (Input.GetMouseButtonDown(0))
            {
                if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<Character>();
                    PositionCharacter(overlayTile.GetComponent<OverlayTile>());
                }
                else
                {
                    path = pathFinder.FindPath(character.activeTile, overlayTile.GetComponent<OverlayTile>());
                }

            }
        }

        if (path.Count > 0)
        {
            MoveAlongPath();
        }

    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;
        var targetPos = path[0].transform.position;
        targetPos.y = characterY;
        character.transform.position = Vector3.MoveTowards(character.transform.position, targetPos, step);

        if (Vector2.Distance(
            new Vector2(character.transform.position.x, character.transform.position.z),
            new Vector2(targetPos.x, targetPos.z))< 0.001f)
        {
            PositionCharacter(path[0]);
            path.RemoveAt(0);
        }
    }

    public RaycastHit? GetFocusTile()
    {
        // Visualize the raycast
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = 1000f;
        //mousePos = cam.ScreenToWorldPoint(mousePos);
        //Debug.DrawRay(transform.position, mousePos-transform.position, Color.blue);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            if (hit.collider != null)
            {
                return hit;
            }
        }

        return null;
    }

    public void PositionCharacter(OverlayTile overlayTile)
    {
        character.transform.position = new Vector3(overlayTile.transform.position.x, characterY, overlayTile.transform.position.z);
        character.activeTile = overlayTile;
    }
}
