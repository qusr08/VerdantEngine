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
	[SerializeField] EnemyHover hover;
    [SerializeField] PlantHover phover;

    public Enemy Enemy {
		get => _enemy;
		set {
			_enemy = value;

			healthSlider.maxValue = _enemy.MaxHealth;
			healthSlider.value = _enemy.CurrentHealth;
			image.sprite = _enemy.Icon;
		}
	}
    public EnemyHover Hover
    {
        get => hover;
        set
        {
			hover = value;
        }
    }
    public PlantHover PHover
    {
        get => phover;
        set
        {
            phover = value;
        }
    }
    public void UpdateHealth ( ) {
		healthSlider.value = Enemy.CurrentHealth;
	}

	public void UpdateCoolDown ( ) {
		if (Enemy.CurrentCooldown == 0) {
			cooldownText.color = Color.red;
		} else {
			cooldownText.color = Color.white;
		}

		cooldownText.text = Enemy.CurrentCooldown.ToString( );
	}

	public void Kill ( ) {
		image.color = Color.gray;
		healthSlider.value = 0;
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        phover.gameObject.SetActive(false);
        hover.gameObject.SetActive(true);
		hover.UpdateText(_enemy);
		if(Enemy.FinalAim!=null && Enemy.FinalAim.Count != 0)
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
        phover.gameObject.SetActive(true);
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
