using System;
using UnityEngine;

[RequireComponent(typeof(Board))]
public class ShipAttacker : PlayerRaycastInput
{
    private Board _board;
    private Vector3 _lastPosition;

    public static event Action<Vector3, int> OnShipShoot;
    public static event Action OnShipHit;
    public static event Action OnMiss;
    public static event Action OnShipKill;

    public void Hit()
    {
        _board.KillShip(_lastPosition);
        OnShipHit?.Invoke();
    }

    public void Miss()
    {
        _board.AddMiss(_lastPosition);
        OnMiss?.Invoke();
    }

    public void Kill()
    {
        _board.KillShip(_lastPosition);
        OnShipKill?.Invoke();
    }
    
    private void TryShoot()
    {
        if (OnShipShoot is null) return;
        Raycast(position =>
        {
            if (!_board.IsCellAt(position)) return;
            _lastPosition = position;
            OnShipShoot.Invoke(position, _board.GetCellAt(position).GetRelativePosition(_board));
        });
    }

    private void OnEnable()
    {
        GameController.OnTryShoot += TryShoot;
    }

    private void OnDestroy()
    {
        GameController.OnTryShoot -= TryShoot;
    }

    protected override void Awake()
    {
        base.Awake();
        _board = GetComponent<Board>();
    }
}