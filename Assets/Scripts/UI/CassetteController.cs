using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class CassetteController : MonoBehaviour
{
    [SerializeField] private SO_CassetteMenu[] _cassettesData; // Array de ScriptableObjects para cada cassette
    private SO_CassetteMenu _cassetteActual; 
    private int _currentLvl = 0;
    [SerializeField] private Image _tituloSprite;
    // [SerializeField] private AudioSource _audioSource;
    
    [SerializeField] private Image[] _estrellasUI; // Array de imágenes para mostrar las estrellas
    [SerializeField] private Sprite _estrellaLlena; 
    [SerializeField] private Sprite _estrellaVacia; 
    private int _estrellas;

    [Header("Configuración de FMOD")]
    [SerializeField] private EventReference musicEvent; // Arrastra aquí tu evento de música
    [SerializeField] private string parameterName = "Song"; // Nombre exacto del parámetro en FMOD Studio
    [SerializeField] private float _parameterValue = 0f; // Valor para el parámetro (puedes ajustarlo según tus necesidades)
    private FMOD.Studio.EventInstance musicInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 1. CREAR la instancia (No es PlayOneShot)
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        // 2. REPRODUCIR
        musicInstance.start();
        // 3. LIBERAR (Indica a FMOD que limpie la memoria cuando el sonido se detenga)
        musicInstance.release();

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
        _estrellas = _cassetteActual.estrellasLogradas;
        _parameterValue = _cassetteActual.parameterValue;
        Debug.Log($"Cassette: {_cassetteActual.nombreNivel}, Estrellas: {_estrellas}, Parámetro: {_parameterValue}");
        ActualizarEstrellas();
        CambiarEstadoMusica(_parameterValue);
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

    // Método para cambiar la música (puedes llamarlo desde un trigger o evento)
    public void CambiarEstadoMusica(float valor)
    {
        // Modifica el parámetro en tiempo real para saltar entre secciones o hacer crossfades
        musicInstance.setParameterByName(parameterName, valor);
    }

    void OnDestroy()
    {
        // 4. DETENER la música si el objeto se destruye
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
