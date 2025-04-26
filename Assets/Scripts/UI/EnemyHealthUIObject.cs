using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class EnemyHealthUIObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Slider healthSlider;
	[SerializeField] private Enemy _enemy;
	[SerializeField] private Image image;
	[SerializeField] TextMeshProUGUI cooldownText;
	[SerializeField] InfoPopUp hover;
	[SerializeField] GameObject OnCoolDownOverlay;
	public GameObject X;
	public bool isdead = false;

    public Enemy Enemy
	{
		get => _enemy;
		set
		{
			_enemy = value;

			healthSlider.maxValue = _enemy.MaxHealth;
			healthSlider.value = _enemy.CurrentHealth;
			image.sprite = _enemy.Icon;
			UpdateCoolDown();



		}
	}
    public InfoPopUp Hover
    {
        get => hover;
        set
        {
			hover = value;
        }
    }

    public void UpdateHealth ( ) {
		healthSlider.value = Enemy.CurrentHealth;
	}

	public void UpdateCoolDown ( ) {
		if (Enemy.CurrentCooldown == 0) {
			OnCoolDownOverlay.SetActive(false);

        } else if(!isdead) {
            OnCoolDownOverlay.SetActive(true);
        }

        cooldownText.text = Enemy.CurrentCooldown.ToString( );
	}

	public void Kill ( ) {
		image.color = Color.gray;
		healthSlider.value = 0;
        OnCoolDownOverlay.SetActive(false);
        isdead = true;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover.gameObject.SetActive(true);
		hover.SetUpEnemey(Enemy);
		if(Enemy.FinalAim!=null && Enemy.FinalAim.Count != 0 && Enemy.CurrentCooldown ==0)
		{
			HighlightAttack();

        }
    }
	
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Enemy.FinalAim != null && Enemy.FinalAim.Count != 0)
        {
            UnHighlight();

        }
        hover.gameObject.SetActive(false);

        Debug.Log("Mouse exit");
    }
	public void HighlightAttack()
	{
		foreach (GardenTile item in _enemy.FinalAim)
		{
			item.IsHighlighted = true;
		}
	}
	public void UnHighlight()
	{
        foreach (GardenTile item in _enemy.FinalAim)
        {
            item.IsHighlighted = false;

        }
    }
}
