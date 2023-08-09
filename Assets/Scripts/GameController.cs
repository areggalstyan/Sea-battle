using System;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private TMP_InputField cellInput;
    [SerializeField] private ShipPlacer shipPlacer;

    private bool _isGameStarted;
    private Turn _turn;
    
    public static event Action OnGameStart;
    public static event Action OnInvalidGameStart;
    public static event Action OnTryShoot;
    public static event Action<Turn> OnTurnChange;
    public static event Action<Turn> OnFirstTurnSet;
    public static event Action<string> OnInvalidEnemyShoot;
    public static event Action<int> OnEnemyShoot;

    public void SetFirstTurnMe()
    {
        _turn = Turn.Me;
        OnFirstTurnSet?.Invoke(_turn);
    }

    public void SetFirstTurnEnemy()
    {
        _turn = Turn.Enemy;
        OnFirstTurnSet?.Invoke(_turn);
    }
    
    public void TryStartGame()
    {
        if (shipPlacer.IsValid())
        {
            OnGameStart?.Invoke();
            shipPlacer.enabled = false;
            _isGameStarted = true;
        }
        else
        {
            OnInvalidGameStart?.Invoke();
        }
    }

    public void EnemyShoot()
    {
        var text = cellInput.text;
        
        if (GetRelativePositionFromText(text, out var position))
            OnEnemyShoot?.Invoke(position);
        else
            OnInvalidEnemyShoot?.Invoke(text);
    }

    private static bool GetRelativePositionFromText(string text, out int position)
    {
        var letter = text[0];
        var number = text.Substring(1);

        var row = char.ToUpper(letter) - 65;
        var isColumnValid = int.TryParse(number, out position);

        position += row * 10 - 1;
        return isColumnValid && position >= 0 && position < 100;
    }

    private void OnEnable()
    {
        ShipAttacker.OnMiss += ChangeTurn;
        ShipVictim.OnMiss += ChangeTurn;
    }
    
    private void OnDisable()
    {
        ShipAttacker.OnMiss -= ChangeTurn;
        ShipVictim.OnMiss -= ChangeTurn;
    }
    
    private void ChangeTurn()
    {
        _turn = _turn == Turn.Me ? Turn.Enemy : Turn.Me;
        OnTurnChange?.Invoke(_turn);
    }

    private void Update()
    {
        if (_isGameStarted && _turn == Turn.Me) OnTryShoot?.Invoke();
    }
}
