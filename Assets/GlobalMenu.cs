using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> categoriesDetailsGameObject = new List<GameObject>();
    [SerializeField] private GameObject currentCategory;

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
