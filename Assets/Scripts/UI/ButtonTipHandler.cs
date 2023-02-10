using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTipHandler : MonoBehaviour
{
    [SerializeField] private GameObject KeyboardMouseControls;
    [SerializeField] private GameObject GamepadControls;
    [SerializeField] private GameObject[] InventoryButtonTips;
    [SerializeField] private GameObject[] HoldingButtonTips;
    [SerializeField] private GameObject[] InteractionButtonTips;

    private static ButtonTipHandler _instance;

    public static ButtonTipHandler Instance
    {
        get { return _instance; }
    }

    private void Start()
    {
        _instance = this;
    }

    private void Update()
    {
        if (InputManager.Instance.inputSystem.currentControlScheme.Equals("Keyboard+Mouse"))
        {
            KeyboardMouseControls.SetActive(true);
            GamepadControls.SetActive(false);
        }
        else
        {
            KeyboardMouseControls.SetActive(false);
            GamepadControls.SetActive(true);
        }
    }

    public void InventoryTips()
    {
        foreach (GameObject tips in InventoryButtonTips)
        {
            tips.SetActive(true);
        }
        foreach (GameObject tips in HoldingButtonTips)
        {
            tips.SetActive(false);
        }
        foreach (GameObject tips in InteractionButtonTips)
        {
            tips.SetActive(false);
        }
    }

    public void HoldingTips()
    {
        foreach (GameObject tips in InventoryButtonTips)
        {
            tips.SetActive(false);
        }
        foreach (GameObject tips in HoldingButtonTips)
        {
            tips.SetActive(true);
        }
        foreach (GameObject tips in InteractionButtonTips)
        {
            tips.SetActive(false);
        }
    }

    public void InteractionTips()
    {
        foreach (GameObject tips in InventoryButtonTips)
        {
            tips.SetActive(false);
        }
        foreach (GameObject tips in HoldingButtonTips)
        {
            tips.SetActive(false);
        }
        foreach (GameObject tips in InteractionButtonTips)
        {
            tips.SetActive(true);
        }
    }
}
