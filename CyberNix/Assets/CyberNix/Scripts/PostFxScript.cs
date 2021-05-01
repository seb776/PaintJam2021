using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostFxScript : MonoBehaviour
{
    public Material PostFXMat;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, PostFXMat);
    }
}
