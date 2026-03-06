using UnityEngine;
using UnityEngine.UI;
// Sustituimos UnityEngine.Audio por FMODUnity
using FMODUnity; 

public class KnovsController : MonoBehaviour
{
    [SerializeField] private DAWKnob[] _knobs;
    private Slider _masterSlider;
    private Slider _sfxSlider;
    private Slider _musicSlider;

    // Referencias a los Buses de FMOD
    private FMOD.Studio.Bus _masterBus;
    private FMOD.Studio.Bus _sfxBus;
    private FMOD.Studio.Bus _musicBus;

    private void Start()
    {
        _masterSlider = _knobs[0].linkedSlider;
        _sfxSlider = _knobs[1].linkedSlider;
        _musicSlider = _knobs[2].linkedSlider;

        // Obtener los buses (la ruta debe ser igual a la de FMOD Studio, ej: "bus:/Master")
        _masterBus = RuntimeManager.GetBus("bus:/"); 
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
    }

    public void UpdateMasterVolume() 
    {
        // FMOD usa 0.0 a 1.0. No necesitas Mathf.Log10 a menos que quieras curvas personalizadas.
        _masterBus.setVolume(_masterSlider.value);
    }   

    public void UpdateSFXVolume() 
    {
        _sfxBus.setVolume(_sfxSlider.value);
    }

    public void UpdateMusicVolume() 
    {
        _musicBus.setVolume(_musicSlider.value);
    }
}
