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
        if (Random.Range(0.0f,1.0f) < 0.5f)
        {
            this.gameObject.transform.localScale = Vector3.Scale(this.gameObject.transform.localScale, new Vector3(-1.0f, 1.0f, 1.0f));
        }
        var pos = this.gameObject.transform.position;
        this.transform.position = new Vector3(pos.x, pos.y + Random.Range(-1.0f, 0.5f), pos.z);
        JapQuad.material.SetColor("_Color", Colors[Random.Range(0, Colors.Count)]*2.0f);
        var jpos = JapQuad.transform.position;
        JapQuad.transform.position = new Vector3(jpos.x, jpos.y + Random.Range(-0.5f, 1.0f), jpos.z);
        Light.color = Colors[Random.Range(0, Colors.Count)];

        if (BuildingTextures != null && BuildingTextures.Count > 0)
            BuildingQuad.material.SetTexture("_MainTex", BuildingTextures[Random.Range(0, BuildingTextures.Count)]);
        if (JapTextures != null && JapTextures.Count > 0)
            JapQuad.material.SetTexture("_MainTex", JapTextures[Random.Range(0, JapTextures.Count)]);
    }
}
