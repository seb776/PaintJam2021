using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHelper : MonoBehaviour
{
    public List<Texture> BuildingTextures;
    public List<Texture> JapTextures;
    public List<Color> Colors;

    public MeshRenderer JapQuad;
    public MeshRenderer BuildingQuad;
    public Light Light;

    private void Start()
    {
        JapQuad.material.SetColor("_Color", Colors[Random.Range(0, Colors.Count)]*2.0f);
        Light.color = Colors[Random.Range(0, Colors.Count)];

        if (BuildingTextures != null && BuildingTextures.Count > 0)
            BuildingQuad.material.SetTexture("_MainTex", BuildingTextures[Random.Range(0, BuildingTextures.Count)]);
        if (JapTextures != null && JapTextures.Count > 0)
            JapQuad.material.SetTexture("_MainTex", JapTextures[Random.Range(0, JapTextures.Count)]);
    }
}
