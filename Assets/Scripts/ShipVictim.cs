using System;
using UnityEngine;

[RequireComponent(typeof(Board))]
public class ShipVictim : MonoBehaviour
{
    private Board _board;
    
    public static event Action OnShipHit;
    public static event Action OnMiss;
    public static event Action OnShipKill;

    private void Awake()
    {
        _board = GetComponent<Board>();
    }

    private void OnEnable()
    {
        GameController.OnEnemyShoot += HandleEnemyShoot;
    }

    private void OnDisable()
    {
        GameController.OnEnemyShoot -= HandleEnemyShoot;
    }

    // TODO: Actually implement this.
    private bool IsKill(int position)
    {
        return false;
    }

    private void HandleEnemyShoot(int relative)
    {
        var position = _board.RelativePositionToAbsolute(relative);
        _board.KillOrMiss(position, () =>
        {
            if (IsKill(relative))
                OnShipKill?.Invoke();
            else
                OnShipHit?.Invoke();
        }, () => OnMiss?.Invoke());
    }
}
