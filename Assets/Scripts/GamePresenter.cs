using System.Linq;
using TMPro;
using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private ShipPlacer shipPlacer;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject myShotPanel;
    [SerializeField] private GameObject enemyShotPanel;
    [SerializeField] private TMP_Text eventBar;
    [SerializeField] private TMP_Text shipsText;
    [SerializeField] private TMP_Text turnText;

    private void OnEnable()
    {
        GameController.OnGameStart += HandleGameStart;
        GameController.OnInvalidGameStart += HandleInvalidGameStart;
        GameController.OnTurnChange += HandleTurnChange;
        GameController.OnFirstTurnSet += HandleTurnChange;
        GameController.OnInvalidEnemyShoot += HandleInvalidEnemyShoot;
        ShipAttacker.OnShipShoot += HandleShipShoot;
        ShipAttacker.OnShipHit += HandleShipHit;
        ShipAttacker.OnMiss += HandleMiss;
        ShipAttacker.OnShipKill += HandleShipKill;
        ShipVictim.OnShipHit += HandleEnemyShipHit;
        ShipVictim.OnMiss += HandleEnemyMiss;
        ShipVictim.OnShipKill += HandleEnemyShipKill;
    }

    private void OnDisable()
    {
        GameController.OnGameStart -= HandleGameStart;
        GameController.OnInvalidGameStart -= HandleInvalidGameStart;
        GameController.OnTurnChange -= HandleTurnChange;
        GameController.OnFirstTurnSet -= HandleTurnChange;
        GameController.OnInvalidEnemyShoot += HandleInvalidEnemyShoot;
        ShipAttacker.OnShipShoot -= HandleShipShoot;
        ShipAttacker.OnShipHit -= HandleShipHit;
        ShipAttacker.OnMiss -= HandleMiss;
        ShipAttacker.OnShipKill -= HandleShipKill;
        ShipVictim.OnShipHit -= HandleEnemyShipHit;
        ShipVictim.OnMiss -= HandleEnemyMiss;
        ShipVictim.OnShipKill -= HandleEnemyShipKill;
    }

    private void HandleEnemyShipHit()
    {
        eventBar.text = "Enemy hit!";
        eventBar.color = Color.red;
    }
    
    private void HandleEnemyShipKill()
    {
        eventBar.text = "Enemy kill!";
        eventBar.color = Color.red;
    }
    
    private void HandleEnemyMiss()
    {
        eventBar.text = "Enemy miss!";
        eventBar.color = Color.gray;
    }

    private void HandleInvalidEnemyShoot(string position)
    {
        eventBar.text = $"Invalid position {position}";
        eventBar.color = Color.red;
    }

    private void HandleGameStart()
    {
        startPanel.SetActive(false);
        playPanel.SetActive(true);
        eventBar.text = "Game started successfully!";
        eventBar.color = Color.green;
    }

    private void HandleInvalidGameStart()
    {
        eventBar.text = "Invalid ship configuration!";
        eventBar.color = Color.red;
    }

    private void HandleTurnChange(Turn turn)
    {
        turnText.text = turn == Turn.Me ? "Your turn" : "Enemy's turn";
        enemyShotPanel.SetActive(turn == Turn.Enemy);
    }

    private void HandleShipShoot(Vector3 position, int relative)
    {
        myShotPanel.SetActive(true);
        eventBar.text = GetLetter(relative / 10 + 1) + (relative % 10 + 1);
        eventBar.color = Color.yellow;
    }

    private string GetLetter(int value)
    {
        return ((char) ('A' - 1 + value)).ToString();
    }

    private void HandleShipHit()
    {
        myShotPanel.SetActive(false);
        eventBar.text = "Hit!";
        eventBar.color = Color.green;
    }
    
    private void HandleMiss()
    {
        myShotPanel.SetActive(false);
        eventBar.text = "Miss!";
        eventBar.color = Color.gray;
    }
    
    private void HandleShipKill()
    {
        myShotPanel.SetActive(false);
        eventBar.text = "Kill!";
        eventBar.color = Color.green;
    }

    private void Awake()
    {
        PresentShipsText();
    }

    private void PresentShipsText()
    {
        shipsText.text = "Ships:\n" + string.Join(", ", shipPlacer.ShipTypes.Select(Ship.FormatInfo));
    }
}