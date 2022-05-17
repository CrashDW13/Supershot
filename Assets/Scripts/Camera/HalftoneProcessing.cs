using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class HalftoneProcessing : MonoBehaviour
{
    //Shoutout to this tutorial which I used as a base: https://www.janplaehn.com/2020/07/halftone.html
    public Material postProcessingMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postProcessingMaterial); 
    }
}
