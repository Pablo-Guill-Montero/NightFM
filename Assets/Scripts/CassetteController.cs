using UnityEngine;
using UnityEngine.UI;

public class CassetteController : MonoBehaviour
{
    [SerializeField] private SO_CassetteMenu[] _cassettesData; // Array de ScriptableObjects para cada cassette
    private SO_CassetteMenu _cassetteActual; 
    private int _currentLvl = 0;
    private AudioClip _audioClip;
    [SerializeField] private Image _tituloSprite;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Image[] _estrellasUI; // Array de imágenes para mostrar las estrellas
    [SerializeField] private Sprite _estrellaLlena; 
    [SerializeField] private Sprite _estrellaVacia; 
    private int _estrellas;

    // 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AplicarYActualizar();
    }

    public void Siguiente() {
        _currentLvl = (_currentLvl + 1) % _cassettesData.Length;
        AplicarYActualizar();
    }

    public void Anterior() {
        _currentLvl--;
        if (_currentLvl < 0) _currentLvl = _cassettesData.Length - 1;
        AplicarYActualizar();
    }

    void AplicarYActualizar() {
        _cassetteActual = _cassettesData[_currentLvl];
        _tituloSprite.sprite = _cassetteActual.imagenNombre;
        _audioClip = _cassetteActual.cancionAsociada;
        _estrellas = _cassetteActual.estrellasLogradas;
        Debug.Log($"Cassette: {_cassetteActual.nombreNivel}, Estrellas: {_estrellas}");
        ActualizarEstrellas();
        ActualizarAudio();
    }

    void ActualizarEstrellas() {
        for (int i = 0; i < _estrellasUI.Length; i++) {
            if(i < _estrellas) {
                _estrellasUI[i].sprite = _estrellaLlena;
            } else {
                _estrellasUI[i].sprite = _estrellaVacia;
            }
        }
    }

    void ActualizarAudio() {
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
}
