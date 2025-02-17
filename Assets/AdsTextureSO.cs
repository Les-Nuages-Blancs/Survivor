using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdsTextureSO", menuName = "ScriptableObject/AdsTextureSO")]
public class AdsTextureSO : ScriptableObject
{
    [SerializeField] private List<Texture> textures = new List<Texture>();

    public Texture GetRandomTexture()
    {
        if (textures == null || textures.Count == 0)
            return null;

        return textures[Random.Range(0, textures.Count)];
    }

    public Texture GetTextureByIndex(int index)
    {
        if (textures == null || index < 0 || index >= textures.Count)
            return null;

        return textures[index];
    }

    public List<Texture> GetAllTextures() => textures;
}
