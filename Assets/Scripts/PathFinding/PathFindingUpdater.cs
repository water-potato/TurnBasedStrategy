using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PathFindingUpdater : MonoBehaviour
{
    public void Start()
    {
        DestructibleCrate.OnAnyCrateDestroyed += DestructibleCrate_OnAnyCrateDestroyed;
    }

    private void DestructibleCrate_OnAnyCrateDestroyed(object sender, EventArgs e)
    {
        // crate�� �ı��Ǹ� , walkable�ϰ� �����
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GridPosition, true); 
    }
}
