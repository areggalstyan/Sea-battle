using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Board))]
public class ShipPlacer : PlayerRaycastInput
{
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private Ship[] shipTypes =
    {
        new Ship(5, 1),
        new Ship(4, 1),
        new Ship(3, 2),
        new Ship(2, 1)
    };

    private Board _board;
    private Stack<Vector3> _history;
    
    private bool _canUndo = true;
    private bool _canPlace = true;

    public IEnumerable<Ship> ShipTypes => shipTypes;
    public bool[] Ships { get; private set; }

    // TODO: Actually implement this.
    public bool IsValid()
    {
        return true;
    }

    private void Undo()
    {
        if (_history.Count == 0) return;
        var position = _history.Pop();
        
        _board.RemoveShip(position);
        Ships[_board.GetCellAt(position).GetRelativePosition(_board)] = false;
        StartCoroutine(StartUndoCooldown());
    }
    
    private IEnumerator StartUndoCooldown()
    {
        _canUndo = false;
        yield return new WaitForSeconds(cooldown);
        _canUndo = true;
    }

    protected override void Awake()
    {
        base.Awake();
        _board = GetComponent<Board>();
        Ships = new bool[100];
        _history = new Stack<Vector3>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Z) && _canUndo) Undo();
        if (_canPlace) Raycast(PlaceShip);
    }

    private void PlaceShip(Vector3 position)
    {
        if (!_board.IsCellAt(position)) return;
        _board.PlaceShip(position);
        Ships[_board.GetCellAt(position).GetRelativePosition(_board)] = true;
        _history.Push(position);
        StartCoroutine(StartPlaceCooldown());
    }

    private IEnumerator StartPlaceCooldown()
    {
        _canPlace = false;
        yield return new WaitForSeconds(cooldown);
        _canPlace = true;
    }
}