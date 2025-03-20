using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTip : MonoBehaviour
{
    private static ToolTip instance;

    [SerializeField] Camera uiCamera;
    private TMP_Text toolTipText;
    private RectTransform backgrounRectTransform;

    private void Awake()
    {
        instance = this;
        backgrounRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        toolTipText = transform.Find("Text").GetComponent<TMP_Text>();
        HideToolTip();
        //ShowToolTip("Random text");
    }
    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
    }
    private void ShowToolTip(string toolTipString)
    {
        gameObject.SetActive(true);

        toolTipText.text = toolTipString;
        float textPaddingSize = 5;
        Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + textPaddingSize * 2f, toolTipText.preferredHeight + textPaddingSize * 2f);
        backgrounRectTransform.sizeDelta = backgroundSize;
        
    }
    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }
    public static void ShowToolTip_Static(string toolTipString)
    {
        instance.ShowToolTip(toolTipString);
    }
    public static void HideToolTip_Static()
    {
        instance.HideToolTip();
    }

}
