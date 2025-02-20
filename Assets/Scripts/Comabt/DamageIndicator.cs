using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public float lifetime = 1f; // Time before it disappears
    public float floatSpeed = 1f; // Speed at which it rises
    private TextMeshProUGUI textMesh;
    public Color textColor;
    private float elapsedTime = 0f;

    private void Awake()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetDamage(float damage)
    {
        textMesh.text = damage.ToString();
        Destroy(gameObject, lifetime); // Destroy after `lifetime` seconds
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // Move upward
        transform.position += new Vector3(0, floatSpeed * Time.deltaTime, 0);

        // Fade out over time
        float fadeProgress = elapsedTime / lifetime;
        textColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
        textMesh.color = textColor;
    }
}
