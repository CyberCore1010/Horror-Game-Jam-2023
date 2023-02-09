using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private ItemMove ItemMove;

    private InputManager inputManager;
    private bool bodgeInventoryFix = false;

    void Start()
    {
        InventoryUI.SetActive(false);
        inputManager = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputManager.playerControls.Default.Inventory.performed += context =>
        {
            inputManager.playerControls.Default.Disable();
            inputManager.playerControls.UI.Enable();
            InventoryUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Inventory Opened");
        };

        inputManager.playerControls.UI.Inventory.performed += context =>
        {
            if (!bodgeInventoryFix || ItemMove.heldItem != null)
            {
                bodgeInventoryFix = true;
                return; //TODO: Remove this shit code
            }
            inputManager.playerControls.Default.Enable();
            inputManager.playerControls.UI.Disable();
            InventoryUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Inventory Closed");
        };
    }
}
