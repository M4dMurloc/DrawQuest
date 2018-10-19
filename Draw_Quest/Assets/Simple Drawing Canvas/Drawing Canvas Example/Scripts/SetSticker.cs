using UnityEngine;
using System.Collections;

/// <summary>
/// Set the texture to draw
/// </summary>
public class SetSticker : MonoBehaviour {
    /// <summary>
    /// Texture to draw on the canvas
    /// </summary>
    public Texture2D texture;
    /// <summary>
    /// Reference to drawingCanvas
    /// </summary>
    public DrawingCanvas drawingCanvas;

    /// <summary>
    /// On button cilck, set the texture you want to draw to <see cref="texture"/>
    /// </summary>
    public void OnClick()
    {
        if (drawingCanvas == null)
        {
            Debug.LogError("No DrawingCanvas assigned to button", this.gameObject);
            return;
        }
        drawingCanvas.ToggleSticker(texture);
    }
}
