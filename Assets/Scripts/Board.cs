using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private GameObject deadPrefab;
    [SerializeField] private GameObject missPrefab;
    
    private Dictionary<Vector3, Cell> _cells;
    private Dictionary<int, Cell> _relativeCells;
    
    private void Awake()
    {
        _cells = new Dictionary<Vector3, Cell>();
        foreach (var cell in GetComponentsInChildren<Cell>())
            _cells[cell.transform.position] = cell;
        _relativeCells = _cells
            .Select(pair => new KeyValuePair<int, Cell>(pair.Value.GetRelativePosition(this), pair.Value))
            .ToDictionary(it => it.Key, it => it.Value);
    }

    public Cell GetCellAt(Vector3 position)
    {
        return _cells[position];
    }

    public Vector3 RelativePositionToAbsolute(int position)
    {
        return _relativeCells[position].transform.position;
    }

    public bool IsCellAt(Vector3 position)
    {
        return _cells.ContainsKey(position);
    }

    public void PlaceShip(Vector3 position)
    {
        if (!IsCellAt(position)) return;
        _cells[position].PlaceShip(shipPrefab);
    }
    
    public void RemoveShip(Vector3 position)
    {
        if (!IsCellAt(position)) return;
        _cells[position].RemoveShip();
    }
    
    public void KillShip(Vector3 position)
    {
        if (!IsCellAt(position)) return;
        _cells[position].KillShip(deadPrefab);
    }
    
    public void AddMiss(Vector3 position)
    {
        if (!IsCellAt(position)) return;
        _cells[position].AddMiss(missPrefab);
    }

    public void KillOrMiss(Vector3 position, Action kill = null, Action miss = null)
    {
        if (!IsCellAt(position)) return;
        _cells[position].KillOrMiss(deadPrefab, missPrefab);

        var unit = GetUnitAt(position);
        if (unit == CellUnit.Dead)
            kill?.Invoke();
        else if (unit == CellUnit.Miss)
            miss?.Invoke();
    }

    public CellUnit GetUnitAt(Vector3 position)
    {
        return GetCellAt(position).Unit;
    }
}