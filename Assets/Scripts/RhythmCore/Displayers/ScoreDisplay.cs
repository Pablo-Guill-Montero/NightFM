using UnityEngine;
using TMPro; // Para usar TextMeshPro en lugar de Text normal, si es lo que estás usando en tu canvas de puntuación
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private MusicDataStore musicStore;
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private TextMeshPro comboText;
    [SerializeField] private Slider comboSlider;

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
        Referee.UpdateComboSliderMAxEvent += (maxCombo) => comboSlider.maxValue = maxCombo; // Nos suscribimos al evento para actualizar el máximo del slider de combo cada vez que el referee lo actualice
        Judge.OnRachaAumentada += (rachaActual) => comboSlider.value = rachaActual; // Nos suscribimos al evento de racha aumentada para actualizar el valor del slider de combo cada vez que el jugador aumente su racha
        Judge.OnFallo += (fallos) => comboSlider.value = 0; // Reseteamos el slider de combo a 0 cada vez que el jugador falle
    }

    private void OnDisable()
    {
        Referee.UpdateScoreEvent -= UpdateScore;
        Referee.UpdateComboEvent -= UpdateCombo;
        Referee.StarAchievedEvent -= UpdateStars;
        Referee.MaxScoreSettedEvent -= InitScore; // Nos desuscribimos del evento de max score al desactivar el script
        Referee.UpdateComboSliderMAxEvent -= (maxCombo) => comboSlider.maxValue = maxCombo; // Nos desuscribimos del evento para actualizar el máximo del slider de combo al desactivar el script
        Judge.OnRachaAumentada -= (rachaActual) => comboSlider.value = rachaActual; // Nos desuscribimos del evento de racha aumentada al desactivar el script
        Judge.OnFallo -= (fallos) => comboSlider.value = 0; // Nos desuscribimos del evento de fallo al desactivar el script
    }

    private void InitScore(int maxScore)
    {
        UpdateScore(0);
        UpdateCombo(1);
        UpdateStars(0);
        // Delimitamos el rango máximo del slider
        scoreSlider.minValue = 0;
        scoreSlider.maxValue = maxScore; // El máximo score posible, que el referee puede calcular al inicio de la partida sumando el valor de cada beat puntuable
        comboSlider.minValue = 1;
        comboSlider.maxValue = 4; // Ajusta este valor según el máximo combo posible
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