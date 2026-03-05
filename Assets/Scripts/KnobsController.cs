using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class KnovsController : MonoBehaviour
{
    [SerializeField] private DAWKnob[] _knobs;
    private Slider _masterSlider;
    private Slider _sfxSlider;
    private Slider _musicSlider;

    [SerializeField] private AudioMixer _audioMixer; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _masterSlider = _knobs[0].linkedSlider;
        _sfxSlider = _knobs[1].linkedSlider;
        _musicSlider = _knobs[2].linkedSlider;
    }

    public void UpdateMasterVolume() 
    {
        if(_masterSlider.value != 0)
            _audioMixer.SetFloat("MasterVol", Mathf.Log10(_masterSlider.value) * 20);    
        else
            _audioMixer.SetFloat("MasterVol", -80); // Valor mínimo para silenciar completamente
    }   

    public void UpdateSFXVolume() 
    {
        if(_sfxSlider.value != 0)
            _audioMixer.SetFloat("SFXVol", Mathf.Log10(_sfxSlider.value) * 20);    
        else
            _audioMixer.SetFloat("SFXVol", -80); // Valor mínimo para silenciar completamente
    }

    public void UpdateMusicVolume() 
    {
        if(_musicSlider.value != 0)
            _audioMixer.SetFloat("MusicVol", Mathf.Log10(_musicSlider.value) * 20);    
        else
            _audioMixer.SetFloat("MusicVol", -80); // Valor mínimo para silenciar completamente
    }
}
