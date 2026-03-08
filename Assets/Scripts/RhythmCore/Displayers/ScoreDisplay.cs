using UnityEngine;
using TMPro; // Para usar TextMeshPro en lugar de Text normal, si es lo que estás usando en tu canvas de puntuación
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private MusicDataStore musicStore;
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private TextMeshPro comboText;

    [SerializeField] private Canvas canvasEstrellas;
    [SerializeField] private Image[] _estrellasUI; // Array de imágenes para mostrar las estrellas
    [SerializeField] private Sprite _estrellaLlena; 
    [SerializeField] private Sprite _estrellaVacia; 
    // Slider para rellenarlo con la puntuación
    [SerializeField] private Slider scoreSlider;

    private void OnEnable()
    {
        Referee.UpdateScoreEvent += UpdateScore;
        Referee.UpdateComboEvent += UpdateCombo;
        Referee.StarAchievedEvent += UpdateStars;
        Referee.MaxScoreSettedEvent += InitScore; // Nos suscribimos al evento de max score para inicializar el slider en cuanto el referee lo calcule al inicio de la partida
    }

    private void OnDisable()
    {
        Referee.UpdateScoreEvent -= UpdateScore;
        Referee.UpdateComboEvent -= UpdateCombo;
        Referee.StarAchievedEvent -= UpdateStars;
        Referee.MaxScoreSettedEvent -= InitScore; // Nos desuscribimos del evento de max score al desactivar el script
    }

    private void InitScore(int maxScore)
    {
        UpdateScore(0);
        UpdateCombo(1);
        UpdateStars(0);
        // Delimitamos el rango máximo del slider
        scoreSlider.minValue = 0;
        scoreSlider.maxValue = maxScore; // El máximo score posible, que el referee puede calcular al inicio de la partida sumando el valor de cada beat puntuable
    }


    private void UpdateScore(int totalScore)
    {
        scoreText.text = totalScore.ToString();
        scoreSlider.value = totalScore; // Actualizamos el valor del slider para que se rellene según la puntuación actual
    }

    private void UpdateCombo(int combo)
    {
        comboText.text = $"x{combo}";
    }

    private void UpdateStars(int starCount)
    {
        for (int i = 0; i < _estrellasUI.Length; i++) {
            if(i < starCount) {
                _estrellasUI[i].sprite = _estrellaLlena;
            } else {
                _estrellasUI[i].sprite = _estrellaVacia;
            }
        }
    }



}