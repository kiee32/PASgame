using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;   // Image bar merah yang akan memendek

    void Start()
    {
        if (playerHealth == null)
            playerHealth = FindAnyObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth tidak ditemukan di scene.", this);
            return;
        }

        // Pastikan Image berjenis Filled supaya bisa memendek dari kanan ke kiri
        if (fillImage != null)
        {
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        }

        playerHealth.OnHealthChanged += Refresh;
        Refresh(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= Refresh;
    }

    void Refresh(int current, int max)
    {
        if (fillImage == null || max <= 0) return;

        fillImage.fillAmount = (float)current / max;
    }
}