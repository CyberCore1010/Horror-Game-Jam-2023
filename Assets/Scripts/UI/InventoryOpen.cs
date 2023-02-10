using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private ItemMove ItemMove;

    void Start()
    {
        InventoryUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputManager.Instance.playerControls.Default.Inventory.performed += context =>
        {
            InputManager.Instance.SwitchInputSystem(InputManager.InputMap.Inventory);
            InventoryUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Inventory Opened");
        };

        InputManager.Instance.playerControls.UI.Inventory.performed += context =>
        {
            if(ItemMove.heldItem == null)
            {
                InventoryHandler.Instance.OpenForInteraction = false;
                InputManager.Instance.SwitchInputSystem(InputManager.InputMap.Default);
                InventoryUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Debug.Log("Inventory Closed");
            }
        };
    }
}
