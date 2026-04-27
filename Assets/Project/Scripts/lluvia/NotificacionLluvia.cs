using UnityEngine;
using TMPro;

public class NotificacionLluvia : MonoBehaviour
{
    public CicloDiaNoche ciclo;
    public ControladorLluvia lluvia;

    public GameObject panel;
    public TextMeshProUGUI texto;

    private bool notificado = false;

    void Update()
    {
        if (ciclo == null || lluvia == null) return;

        float horaActual = ciclo.hora;

        // 1 hora antes = 1 minuto real
        float horaAviso = lluvia.horaInicioLluvia - 1f;
        if (horaAviso < 0) horaAviso += 24f;

        if (!notificado && Mathf.Abs(horaActual - horaAviso) < 0.05f)
        {
            Mostrar("⚠️ Lloverá en 1 minuto. Busca refugio.");
            notificado = true;
        }

        // Reset después de la lluvia
        if (horaActual > lluvia.horaInicioLluvia)
        {
            notificado = false;
        }
    }

    void Mostrar(string mensaje)
    {
        panel.SetActive(true);
        texto.text = mensaje;

        Invoke("Ocultar", 5f);
    }

    void Ocultar()
    {
        panel.SetActive(false);
    }
}