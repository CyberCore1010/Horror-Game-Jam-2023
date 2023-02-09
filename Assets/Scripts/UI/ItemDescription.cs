using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDescription : MonoBehaviour
{
    [SerializeField] private GameObject DescriptionContainer;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Category;
    [SerializeField] private TextMeshProUGUI Description;

    private static ItemDescription _instance;

    public static ItemDescription Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        Hide();
    }

    public void SetItem(string name, string category, string description)
    {
        Name.text = name;
        Category.text = category;
        Description.text = description;
        DescriptionContainer.SetActive(true);
    }

    public void Hide()
    {
        DescriptionContainer.SetActive(false);
    }
}
