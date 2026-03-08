using UnityEngine;
using UnityEngine.UI;
using TMPro; // Para usar TextMeshPro en lugar de Text normal, si es lo que estás usando en tu canvas de depuración

public class Debug_RhythmCore : MonoBehaviour
{
// Active Beat: -1
// TargetPos: 
// TargetBeat: 
// PlayerPos: 
    [SerializeField] private MusicDataStore musicStore;
    [SerializeField] private Canvas debugCanvas;
    private TextMeshProUGUI _debugText;

    private int _currentTargetBeat = -1;
    private int _currentTargetCell = -1;
    private int _playerCurrentCell = -1;
    private int _fallos = 0;
    private int _aciertos = 0;
    private int _totalScore = 0;
    private int _combo = 1;
    private int _starCount = 0;

    private void OnEnable()
    {
        Composer.OnPuntuableBeat += HandlePuntuableBeat; 
        PlayerController.OnPlayerMove += HandlePlayerMove; 
        Judge.OnAcierto += HandleAcierto;
        Judge.OnFallo += HandleFallo;
        Referee.UpdateScoreEvent += HandleUpdateScore;
        Referee.StarAchievedEvent += HandleStarAchieved;
        Referee.UpdateComboEvent += HandleUpdateCombo;
    }

    private void OnDisable()
    {
        Composer.OnPuntuableBeat -= HandlePuntuableBeat;
        PlayerController.OnPlayerMove -= HandlePlayerMove;
        Judge.OnAcierto -= HandleAcierto;
        Judge.OnFallo -= HandleFallo;
    }

    private void Start()
    {
        _debugText = debugCanvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void HandleUpdateCombo(int combo)
    {
        _combo = combo;
    }

    private void HandleStarAchieved(int starCount)
    {
        _starCount = starCount;
    }

    private void HandleUpdateScore(int totalScore)
    {
        _totalScore = totalScore;
    }

    private void HandlePuntuableBeat(int beatNumber, int cellNumber)
    {
        _currentTargetBeat = beatNumber;
        _currentTargetCell = cellNumber;
    }

    private void HandlePlayerMove(int playerCellIndex)
    {
        _playerCurrentCell = playerCellIndex;
    }

    private void HandleAcierto(int aciertos)
    {
        _aciertos = aciertos;
    }

    private void HandleFallo(int fallos)
    {
        _fallos = fallos;
    }

    private void Update()
    {
        _debugText.text = $"Active Beat: {musicStore.GetActiveBeat()}\nLast Beat: {musicStore.GetLastBeat()}\nTarget Pos: {_currentTargetCell}\nTarget Beat: {_currentTargetBeat}\nPlayer Pos: {_playerCurrentCell}\n -Fallos: {_fallos}\n -Aciertos: {_aciertos}\n -Puntuación: {_totalScore}\n -Combo: {_combo}\n -Estrellas: {_starCount}";
    }

}
