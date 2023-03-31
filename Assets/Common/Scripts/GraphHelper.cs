using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphHelper : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI GraphLabel;
    [SerializeField] UnityEngine.UI.RawImage GraphImage;

    [SerializeField] float MinXValue = 0f;
    [SerializeField] float MaxXValue = 360f;
    [SerializeField] float MinYValue = -3f;
    [SerializeField] float MaxYValue = 3f;

    [SerializeField][ColorUsage(showAlpha: false)] Color BackgroundColour = Color.white;
    [SerializeField][ColorUsage(showAlpha: false)] Color DrawColour = Color.black;
    [SerializeField] int PointRadius = 10;

    Texture2D GraphTexture;

    private void Start()
    {
        ClearGraph();
    }

    public void AddPoint(float x, float y)
    {
        int textureX = Mathf.FloorToInt(Mathf.InverseLerp(MinXValue, MaxXValue, x) * GraphTexture.width);
        int textureY = Mathf.FloorToInt(Mathf.InverseLerp(MinYValue, MaxYValue, y) * GraphTexture.height);

        for (int offsetY = -PointRadius; offsetY <= PointRadius; offsetY++)
        {
            int drawY = textureY + offsetY;

            if (drawY >= GraphTexture.height)
                continue;

            for (int offsetX = -PointRadius; offsetX <= PointRadius; offsetX++)
            {
                int drawX = textureX + offsetX;

                if (drawX >= GraphTexture.width)
                    continue;

                int distSq = offsetX * offsetX + offsetY * offsetY;
                if (distSq > (PointRadius * PointRadius))
                    continue;

                GraphTexture.SetPixel(drawX, drawY, DrawColour);
            } 
        }

        GraphTexture.Apply();
    }

    public void ClearGraph()
    {
        Canvas owningCanvas = GetComponentInParent<Canvas>();
        RectTransform graphRT = GraphImage.rectTransform;

        int textureWidth  = Mathf.FloorToInt((graphRT.anchorMax.x - graphRT.anchorMin.x) * Screen.width + graphRT.sizeDelta.x * owningCanvas.scaleFactor);
        int textureHeight = Mathf.FloorToInt((graphRT.anchorMax.y - graphRT.anchorMin.y) * Screen.height + graphRT.sizeDelta.y * owningCanvas.scaleFactor);

        GraphTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        GraphTexture.wrapMode = TextureWrapMode.Clamp;

        Color32 clearColour = BackgroundColour;
        Color32[] pixelData = GraphTexture.GetPixels32();
        for(int index = 0; index < pixelData.Length; index++)
            pixelData[index] = clearColour;
        GraphTexture.SetPixels32(pixelData);
        GraphTexture.Apply();

        GraphImage.texture = GraphTexture;
    }
}
