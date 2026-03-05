using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems; // Necesario para detectar eventos del ratón (hover)

// Esta línea asegura que el GameObject tenga un componente Image. Si no lo tiene, lo añade automáticamente.
[RequireComponent(typeof(Image))]
public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configuración de Sprites")]
    [Tooltip("El sprite que se mostrará cuando el ratón esté ENCIMA.")]
    public Sprite spriteHover; 

    // Variables privadas para gestión interna
    private Image imagenComponente;
    private Sprite spriteOriginal;

    void Awake()
    {
        // Obtenemos la referencia al componente Image del mismo GameObject
        imagenComponente = GetComponent<Image>();

        // Guardamos el sprite original que tiene la Image en el editor para poder volver a él
        if (imagenComponente != null)
        {
            spriteOriginal = imagenComponente.sprite;
        }
    }

    // Se ejecuta automáticamente cuando el puntero del ratón ENTRA en el área del objeto
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (imagenComponente != null && spriteHover != null)
        {
            imagenComponente.sprite = spriteHover; // Cambiamos al sprite de hover
        }
    }

    // Se ejecuta automáticamente cuando el puntero del ratón SALE del área del objeto
    public void OnPointerExit(PointerEventData eventData)
    {
        if (imagenComponente != null)
        {
            imagenComponente.sprite = spriteOriginal; // Volvemos al sprite original guardado
        }
    }
}