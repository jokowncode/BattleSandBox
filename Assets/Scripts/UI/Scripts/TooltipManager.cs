using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    private RectTransform tooltipRect;

    private void Awake()
    {
        Instance = this;
        tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        HideTooltip();
    }

    public void ShowTooltip(string message)
    {
        // if(!tooltipPanel.activeSelf)
        //     tooltipPanel.SetActive(true);
        tooltipPanel.SetActive(true);
        tooltipText.text = message;
        tooltipRect.anchoredPosition = new Vector2(Input.mousePosition.x - Screen.width / 2 - 250.0f,
                                                    Input.mousePosition.y - Screen.height / 2 + 50.0f);
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        if (tooltipPanel.activeSelf)
        {
            Vector2 localPos;
            //tooltipRect.anchoredPosition = Input.mousePosition/2;
            tooltipRect.position = new Vector2(Input.mousePosition.x - Screen.width/2 + 50.0f,
                Input.mousePosition.y - Screen.height/2  + 150.0f);
            tooltipRect.position = new Vector2(Screen.width / 2, Screen.height / 2);
            tooltipRect.position = new Vector2(Input.mousePosition.x + 150f, Input.mousePosition.y +80f);
            Debug.Log("MousePos" + Input.mousePosition);
            Debug.Log("Anchor" + tooltipRect.position);
        }
    }
}