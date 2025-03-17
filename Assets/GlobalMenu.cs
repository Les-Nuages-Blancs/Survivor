using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> categoriesDetailsGameObject = new List<GameObject>();
    [SerializeField] private List<Button> accordingButtonForCategories = new List<Button>();
    [SerializeField] private GameObject currentCategory;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color activeColor;

    private void Start()
    {
        InitCurrentCategory();
    }

    public void InitCurrentCategory()
    {
        foreach (GameObject category in categoriesDetailsGameObject)
        {
            category.SetActive(false);
        }

        currentCategory.SetActive(true);

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        foreach (Button button in accordingButtonForCategories)
        {
            ColorBlock colorBlock = button.colors; // Get the current color block
            colorBlock.normalColor = normalColor; // Modify the normalColor
            button.colors = colorBlock; // Assign the modified color block back
        }

        // Now modify the color for the selected category
        int index = categoriesDetailsGameObject.IndexOf(currentCategory);

        ColorBlock activeColorBlock = accordingButtonForCategories[index].colors;
        activeColorBlock.normalColor = activeColor;
        accordingButtonForCategories[index].colors = activeColorBlock;
    }

    public GameObject CurrentCategory
    {
        get => currentCategory;
        set
        {
            if (currentCategory != value)
            {
                currentCategory.SetActive(false);

                currentCategory = value;
                
                currentCategory.SetActive(true);

                UpdateButtons();
            }
        }
    }

    public void SetCurrentCategory(GameObject newCurrentCategory)
    {
        if (categoriesDetailsGameObject.Contains(newCurrentCategory))
        {
            CurrentCategory = newCurrentCategory;
        }
        else
        {
            Debug.LogWarning(newCurrentCategory + " is not an available category");
        }
    }

    private void AddToCurrentIndex(int valueToAdd)
    {
        int currentIndex = categoriesDetailsGameObject.IndexOf(currentCategory);

        int newCurrentIndex = (valueToAdd + currentIndex + categoriesDetailsGameObject.Count) % categoriesDetailsGameObject.Count;

        CurrentCategory = categoriesDetailsGameObject[newCurrentIndex];
    }

    public void SetNextCategoryAsCurrent()
    {
        AddToCurrentIndex(1);
    }

    public void SetPreviousCategoryAsCurrent()
    {
        AddToCurrentIndex(-1);
    }
}
