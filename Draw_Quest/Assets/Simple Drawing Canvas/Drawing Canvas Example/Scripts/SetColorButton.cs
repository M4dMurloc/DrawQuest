using UnityEngine;

/// <summary>
/// Set the color in which to draw
/// </summary>
public class SetColorButton : MonoBehaviour
{
    /// <summary>
    /// Color to set drawing to
    /// </summary>
    public Color color;
    /// <summary>
    /// Reference to drawingCanvas
    /// </summary>
    public DrawingCanvas drawingCanvas;

    /// <summary>
    /// On button cilck, set the drawing color to <see cref="color"/>
    /// </summary>
    public void OnClick()
    {
        if (drawingCanvas == null)
        {
            Debug.LogError("No DrawingCanvas assigned to button", this.gameObject);
            return;
        }
        drawingCanvas.SetColor(color);
    }
}
