using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject HoverGlow;
    [SerializeField] private GameObject ItemContainer;

    private ItemMove itemMove;
    private InputManager inputManager;
    private bool isHovering;

    private void Start()
    {
        itemMove = transform.parent.parent.gameObject.GetComponentInChildren<ItemMove>();

        inputManager = InputManager.Instance;

        inputManager.playerControls.UI.Move.performed += context =>
        {
            if(isHovering)
            {
                itemMove.HandleMovement(GetComponentInChildren<Item>(), ItemContainer.transform, true);
            }
        };

        inputManager.playerControls.UI.Interact.performed += context =>
        {
            if (isHovering)
            {
                itemMove.HandleMovement(GetComponentInChildren<Item>(), ItemContainer.transform, false);
            }
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        HoverGlow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        HoverGlow.SetActive(false);
    }
}
