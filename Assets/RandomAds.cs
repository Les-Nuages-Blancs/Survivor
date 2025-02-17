using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAds : MonoBehaviour
{
    [SerializeField] private AdsTextureSO adsTextureSO;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private bool useGivenIndex = false;
    [SerializeField] private int givenIndex = 0;

    void Start()
    {
        ApplyTexture();
    }

    private void OnValidate()
    {
        ApplyTexture();
    }

    private void ApplyTexture()
    {
        Texture new_texture;

        if (useGivenIndex)
        {
            new_texture = adsTextureSO.GetTextureByIndex(givenIndex);
        }
        else
        {
            new_texture = adsTextureSO.GetRandomTexture();
        }

        if (new_texture != null)
        {
            gameObject.SetActive(true);

            AssignTexture(new_texture);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void AssignTexture(Texture new_texture)
    {
        meshRenderer.material.mainTexture = new_texture;
    }
}
