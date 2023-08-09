using System;
using JetBrains.Annotations;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [CanBeNull] private GameObject _ship;
    
    public CellUnit Unit { get; private set; } = CellUnit.None;

    public void PlaceShip(GameObject prefab)
    {
        _ship = Spawn(prefab);
        Unit = CellUnit.Ship;
    }
    
    public void RemoveShip()
    {
        if (_ship is null) throw new NoShipException();
        DestroyShip();
        Unit = CellUnit.None;
    }
    
    public void KillShip(GameObject prefab)
    {
        Spawn(prefab);
        DestroyShip();
        Unit = CellUnit.Dead;
    }

    private void DestroyShip()
    {
        Destroy(_ship);
        _ship = null;
    }
    
    public void AddMiss(GameObject prefab)
    {
        Spawn(prefab);
        Unit = CellUnit.Miss;
    }
    
    public void KillOrMiss(GameObject deadPrefab, GameObject missPrefab)
    {
        if (Unit == CellUnit.Ship)
            KillShip(deadPrefab);
        else if (Unit == CellUnit.None)
            AddMiss(missPrefab);
    }
    
    public int GetRelativePosition(Board board)
    {
        var relative = transform.position - board.transform.position;
        return (int) (49.5f + relative.x - 10 * relative.z);
    }
    
    private GameObject Spawn(GameObject prefab)
    {
        return Instantiate(prefab, transform.position + Vector3.up, Quaternion.identity);
    }

    private class NoShipException : Exception {}
}