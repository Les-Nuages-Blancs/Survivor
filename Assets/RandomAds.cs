using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAds : MonoBehaviour
{
    [SerializeField] private AdsTextureSO adsTextureSO;
    [SerializeField] private MeshRenderer meshRenderer;

    private Material instanceMaterial; // Store a unique material instance

    void Start()
    {
        // Create an instance of the material if it doesn't already exist
        if (meshRenderer != null)
        {
            // Only create a new material if needed
            if (instanceMaterial == null)
            {
                instanceMaterial = new Material(meshRenderer.sharedMaterial);
                meshRenderer.material = instanceMaterial;
            }

            AssignTexture(adsTextureSO.GetRandomTexture());
        }
    }

    // Called when the script is loaded or a value is changed in the Inspector
    private void OnValidate()
    {
        // Ensure the random texture is applied in the editor as well
        if (adsTextureSO != null && meshRenderer != null)
        {
            // Create a unique material if not already created
            if (instanceMaterial == null)
            {
                instanceMaterial = new Material(meshRenderer.sharedMaterial);
                meshRenderer.material = instanceMaterial;
            }

            AssignTexture(adsTextureSO.GetRandomTexture());
        }
    }

    private void AssignTexture(Texture new_texture)
    {
        if (instanceMaterial != null && new_texture != null)
        {
            instanceMaterial.mainTexture = new_texture;
        }
    }
}
