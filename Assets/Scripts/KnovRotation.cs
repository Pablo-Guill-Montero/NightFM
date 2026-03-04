using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DAWKnob : MonoBehaviour, IDragHandler
{
    [Header("Configuración de Valor")]
    [Range(0, 1)] public float value = 0.5f;
    public float sensitivity = 0.003f; // Ajusta la velocidad de giro

    [Header("Referencias UI")]
    public RectTransform visualKnob; // El objeto que rotará
    public Slider linkedSlider;     // El slider que solo mostrará el progreso

    [Header("Límites de Rotación")]
    public float minAngle = 140f;   // Ángulo en valor 0
    public float maxAngle = -140f;  // Ángulo en valor 1

    public void OnDrag(PointerEventData eventData)
    {
        // Detecta movimiento vertical (y) y horizontal (x)
        float delta = eventData.delta.y + eventData.delta.x;
        
        // Actualiza el valor interno
        value = Mathf.Clamp01(value + delta * sensitivity);
        
        UpdateUI();
    }

    void UpdateUI()
    {
        if (visualKnob == null) return;

        // 1. Rotar el Knob
        float angle = Mathf.Lerp(minAngle, maxAngle, value);
        visualKnob.localRotation = Quaternion.Euler(0, 0, angle);

        // 2. Actualizar el Slider (aunque esté desactivado para el usuario)
        if (linkedSlider != null)
        {
            linkedSlider.value = value;
        }
    }

    // Se ejecuta al cambiar valores en el Inspector
    void OnValidate() => UpdateUI();
}
