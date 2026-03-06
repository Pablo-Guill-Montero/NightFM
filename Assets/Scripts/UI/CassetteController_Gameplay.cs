using UnityEngine;
using UnityEngine.UI;

public class CassetteController_Gameplay : MonoBehaviour
{
    [SerializeField] private SO_CassetteMenu[] _cassettesData; // Array de ScriptableObjects para cada cassette
    private SO_CassetteMenu _cassetteActual; 
    private int _currentLvl = 0;
    [SerializeField] private Image _tituloSprite;
    [SerializeField] private Image[] _estrellasUI; // Array de imágenes para mostrar las estrellas
    [SerializeField] private Sprite _estrellaLlena; 
    [SerializeField] private Sprite _estrellaVacia; 
    private int _estrellas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AplicarYActualizar();
    }

    void AplicarYActualizar() {
        _cassetteActual = _cassettesData[_currentLvl];
        _tituloSprite.sprite = _cassetteActual.imagenNombre;
        _estrellas = _cassetteActual.estrellasLogradas;
        ActualizarEstrellas();
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
}
