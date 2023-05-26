using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            OverlayTile currOverlayTile = openList.OrderBy(tile => tile.F).First();

            openList.Remove(currOverlayTile);
            closedList.Add(currOverlayTile);

            if (currOverlayTile == end)
            {
                return GetFinishedPath(start, end);
            }

            var neighbourTiles = GetNeighbourTiles(currOverlayTile);

            foreach (var neighbour in neighbourTiles)
            {
                if (neighbourTiles.Contains(neighbour))
                {
                    if (neighbour.isBlocked || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    neighbour.G = GetManhattanDistance(start, neighbour);
                    neighbour.H = GetManhattanDistance(end, neighbour);
                    neighbour.previous = currOverlayTile;

                    if(!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
            
        }
        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedPath(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> path = new List<OverlayTile>();
        OverlayTile currentTile = end;

        while (currentTile != start) {
            path.Add(currentTile);
            currentTile = currentTile.previous;
        }

        path.Reverse();
        return path;
    }

    private int GetManhattanDistance(OverlayTile start, OverlayTile neighbour)
    {
        return 
            Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + 
            Mathf.Abs(start.gridLocation.z - neighbour.gridLocation.z);
    }

    private List<OverlayTile> GetNeighbourTiles(OverlayTile currOverlayTile)
    {
        var map = MapManager.Instance.map;
        List<OverlayTile> neighbours = new List<OverlayTile>();

        // top
        var locationToCheck = new Vector3Int(
            currOverlayTile.gridLocation.x, 0,
            currOverlayTile.gridLocation.z+1);


        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        // right
        locationToCheck = new Vector3Int(
            currOverlayTile.gridLocation.x+1, 0,
            currOverlayTile.gridLocation.z);


        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        // bottom
        locationToCheck = new Vector3Int(
            currOverlayTile.gridLocation.x, 0,
            currOverlayTile.gridLocation.z-1);


        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        // top
        locationToCheck = new Vector3Int(
            currOverlayTile.gridLocation.x-1, 0,
            currOverlayTile.gridLocation.z);


        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        return neighbours;
    }
}
