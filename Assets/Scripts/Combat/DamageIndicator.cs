using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI numberText;
	[SerializeField] private float duration = 1f; // Time before it disappears
	[SerializeField] private float floatSpeed = 1f; // Speed at which it rises

	private float elapsedTime = 0f;

	public void SetDamage (float damage) {
		numberText.color = Color.red;
		numberText.text = damage.ToString( );
		Destroy(gameObject, duration); // Destroy after `lifetime` seconds
	}
	public void SetHeal(float heal)
	{
		numberText.color = Color.green;

		numberText.text = heal.ToString();
		Destroy(gameObject, duration); // Destroy after `lifetime` seconds
	}

	private void Update ( ) {
		elapsedTime += Time.deltaTime;

		// Move upward
		transform.position += new Vector3(0, floatSpeed * Time.deltaTime, 0);

		// Fade out over time
		float fadeProgress = elapsedTime / duration;

		Color textColor = numberText.color;
		textColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
		numberText.color = textColor;
	}
}
