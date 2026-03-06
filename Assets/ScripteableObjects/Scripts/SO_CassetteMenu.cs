using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[CreateAssetMenu(fileName = "NuevoCassette", menuName = "Menu/Datos de Cassette")]
public class SO_CassetteMenu : ScriptableObject
{
    public string nombreNivel;
    public Sprite imagenNombre; // La imagen para su nombre
    [Range(0, 3)] public int estrellasLogradas; // Máximo 3
    // public AudioClip cancionAsociada; // Su canción
    public EventReference cancionAsociada; // Su canción en FMOD
    public float parameterValue; // Valor para el parámetro de FMOD (puedes ajustarlo según tus necesidades)
}