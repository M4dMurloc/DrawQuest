using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Interface between UI and texture drawer
/// </summary>
public class DrawingCanvas : MonoBehaviour
{
    //--------------------------------------------------------m4d
    public InputField new_objInputField;
    public Dropdown objDropdown;
    public Toggle EnableTraningToggle;
    public Text resultText;
    public Text scoreText;
    public Text questText;
    public LinearTimer timer;

    private bool enableTraining = false;
    private int[,] arr;
    private NeiroWeb nw;
    private int score = 0;
    private string quest_word;
    //-----------------------------------------------------------
    /// <summary>
    /// File name for saving/loading the texture
    /// </summary>
    public const string FILENAME = "Drawing.png";

    /// <summary>
    /// Reference to the image object we want to draw on
    /// </summary>
    public Image imageCanvas;

    public Color eraseColor = Color.white;

    TextureDrawingAux textureDrawingAux;

    /// <summary>
    /// Action currently taken when touching the image
    /// </summary>
    Mode mode = Mode.Pencil;
    /// <summary>
    /// Diameter for drawing
    /// </summary>
    int drawingSize = 5;
    /// <summary>
    /// Color to draw in
    /// </summary>
    Color drawingColor = Color.black;
    /// <summary>
    /// Coordinates to draw from on next frame
    /// </summary>
    Vector2 pencilFrom;
    /// <summary>
    /// Coordinates to draw to on next frame
    /// </summary>
    Vector2 pencilTo;
    /// <summary>
    /// Should we draw on the next frame?
    /// </summary>
    bool drawPencil;
    /// <summary>
    /// The texture we should draw in sticker mode
    /// </summary>
    Texture2D stickerTexture;

    IEnumerator Start()
    {
        int width = (int)imageCanvas.rectTransform.rect.width;
        int height = (int)imageCanvas.rectTransform.rect.height;
        textureDrawingAux = new TextureDrawingAux(width, height, 1);

        textureDrawingAux.Clear(eraseColor);
        //Create a sprite from the texture and display it in imageCanvas
        Sprite sprite = Sprite.Create(textureDrawingAux.Texture, new Rect(0, 0, textureDrawingAux.Width, textureDrawingAux.Height), Vector2.one * 0.5f);
        imageCanvas.sprite = sprite;

        yield return null;

        //If a saved file exists, load it and draw it on the texture, else start a new one
        //if (System.IO.File.Exists(Application.persistentDataPath + "/" + FILENAME))
        //{
        //    byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + FILENAME); //Application.persistentDataPath + "/" + 

        //    Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        //    texture.LoadImage(bytes);

        //    textureDrawingAux.DrawTexture(textureDrawingAux.Width / 2 - texture.width / 2, textureDrawingAux.Height / 2 - texture.height / 2, texture);
        //}
    }

    void Update()
    {
        //Drawn during Update.
        //We're not doing this in the event callbacks to improve responsiveness
        if (drawPencil)
        {
            Color color = mode == Mode.Eraser ? eraseColor : drawingColor;
            drawPencil = false;
            textureDrawingAux.DrawPencil(pencilFrom, pencilTo, drawingSize, color, mode == Mode.Eraser);
        }
    }

    /// <summary>
    /// Set draw mode to pencil, and set the size of the pencil to <paramref name="size"/>
    /// </summary>
    /// <param name="size">the size of the pencil, in pixels</param>
    public void TogglePencilWithSize(int size)
    {
        mode = Mode.Pencil;
        drawingSize = size;
    }

    /// <summary>
    /// Set draw mode to sticker
    /// </summary>
    /// <param name="sticker">The sticker to draw on click/tap</param>
    public void ToggleSticker(Texture2D sticker)
    {
        mode = Mode.Sticker;
        this.stickerTexture = sticker;
    }

    /// <summary>
    /// Set draw mode to eraser, and set the size of the eraser to <paramref name="size"/>
    /// </summary>
    /// <param name="size">the size of the eraser, in pixels</param>
    public void ToggleEraserWithSize(int size)
    {
        mode = Mode.Eraser;
        drawingSize = size;
    }

    /// <summary>
    /// Set the color to draw in to <paramref name="color"/>
    /// </summary>
    /// <param name="color">the color to draw in</param>
    public void SetColor(Color color)
    {
        drawingColor = color;
    }

    /// <summary>
    /// Erase everything on the canvas
    /// </summary>
    public void ClearCanvas()
    {
        textureDrawingAux.Clear(eraseColor);
    }

    //Callback. Draws a single circle where we touched the texture
    public void OnPointerDown(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        //calculate the coordinates of the cursor on the texture
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(imageCanvas.rectTransform, pointerData.position, pointerData.pressEventCamera, out localCursor))
            return;
        localCursor.x += imageCanvas.rectTransform.pivot.x * imageCanvas.rectTransform.rect.width;
        localCursor.y += imageCanvas.rectTransform.pivot.y * imageCanvas.rectTransform.rect.height;

        if (mode == Mode.Sticker)
        {
            if (stickerTexture == null)
            {
                Debug.LogError("Sticker texture not set!");
                return;
            }
            textureDrawingAux.DrawTexture(Mathf.RoundToInt(localCursor.x) - stickerTexture.width / 2, Mathf.RoundToInt(localCursor.y) - stickerTexture.height / 2, stickerTexture);
            return;
        }
        //if it's eraser, set the color to white, else use drawing color
        Color color = mode == Mode.Eraser ? eraseColor : drawingColor;
        //draw a single circle where we touched the image
        textureDrawingAux.DrawCircleFill(Mathf.RoundToInt(localCursor.x), Mathf.RoundToInt(localCursor.y), drawingSize, color, mode == Mode.Eraser);
    }

    //Callback. Traces a pencil line between the 2 points
    public void OnPointerDrag(BaseEventData eventData)
    {
        if (mode == Mode.Sticker)
        {
            return;
        }
        PointerEventData pointerData = eventData as PointerEventData;

        //calculate the coordinates of the cursor on the texture, both when we begin the drag and end it
        Vector2 crtPos;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(imageCanvas.rectTransform, pointerData.position, pointerData.pressEventCamera, out crtPos))
            return;
        Vector2 prevPos;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(imageCanvas.rectTransform, pointerData.position - pointerData.delta, pointerData.pressEventCamera, out prevPos))
            return;

        //if all went ok, set to draw the line on the next frame
        pencilFrom = prevPos;
        pencilTo = crtPos;

        pencilFrom.x += imageCanvas.rectTransform.pivot.x * imageCanvas.rectTransform.rect.width;
        pencilFrom.y += imageCanvas.rectTransform.pivot.y * imageCanvas.rectTransform.rect.height;

        pencilTo.x += imageCanvas.rectTransform.pivot.x * imageCanvas.rectTransform.rect.width;
        pencilTo.y += imageCanvas.rectTransform.pivot.y * imageCanvas.rectTransform.rect.height;

        drawPencil = true;
    }

    /// <summary>
    /// Drawing mode for the texture
    /// </summary>
    public enum Mode
    {
        //Set pixels traced over to set color
        Pencil = 0,
        //Set pixels traced over to white
        Eraser = 1,
        //Draw the selected sticker
        Sticker = 2
    }

    void Awake()
    {
        nw = new NeiroWeb();

        List<string> items = new List<string>(nw.GetLiteras());

        if (items.Count > 0)
        {
            objDropdown.AddOptions(items);
        }

        UpdateQuest();
    }

    void OnApplicationQuit()
    {
        nw.SaveState();
    } 

    public void Learn()
    {
#if ADMIN
        int[,] clipArr = NeiroGraphUtils.CutImageToArray(textureDrawingAux.Texture, new Vector2(textureDrawingAux.Texture.width, textureDrawingAux.Texture.height));

        if (clipArr == null) return;
        arr = NeiroGraphUtils.LeadArray(clipArr, new int[NeiroWeb.neironInArrayWidth, NeiroWeb.neironInArrayHeight]);
        string s = nw.CheckLitera(arr);
        if (s == null) s = "null";

        //Debug.Log("Результат распознавания: " + s);
        resultText.text = "Результат распознавания: " + s;

        if (enableTraining)
        {
            nw.SetTraining(s, arr, resultText);
            //Debug.Log("Обучение прошло успешно");
            resultText.text += ". Обучение прошло успешно.";

            ClearCanvas();
        }

        //IMG1.material.mainTexture = (Texture2D)imageCanvas.material.mainTexture;//NeiroGraphUtils.GetBitmapFromArr(clipArr);
        //IMG2.material.mainTexture = NeiroGraphUtils.GetBitmapFromArr(arr);
        //DialogResult askResult = MessageBox.Show("Результат распознавания - " + s + " ?", "", MessageBoxButtons.YesNo);
        //if (askResult != DialogResult.Yes || !enableTraining || MessageBox.Show("Добавить этот образ в память нейрона '" + s + "'", "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        //nw.SetTraining(s, arr);
#elif USER
        int[,] clipArr = NeiroGraphUtils.CutImageToArray(textureDrawingAux.Texture, new Vector2(textureDrawingAux.Texture.width, textureDrawingAux.Texture.height));

        if (clipArr == null)
        {
            resultText.text = "Вы ничего не нарисовали!";
            return;
        }

        arr = NeiroGraphUtils.LeadArray(clipArr, new int[NeiroWeb.neironInArrayWidth, NeiroWeb.neironInArrayHeight]);
        string s = nw.CheckLitera(arr);
        if (s == null)
        {
            resultText.text = "Ничего не понимаю...";
            return;
        }
        else if (s == quest_word)
        {
            resultText.text = "Боже мой, это действительно " + s;
            UpdateQuest();

            ClearCanvas();
        }
        else
        {
            resultText.text = "Не похоже это на " + quest_word + ". Больше похоже на " + s + "...";
            return;
        }

        score += nw.GenerateScore();
        scoreText.text = score.ToString();
#endif
    }

    public void AddObj()
    {
        AddObjectToList(new_objInputField.text);
        new_objInputField.text = "";
    }

    //Метод помещает строку в список значений
    private void AddObjectToList(string symbol)
    {
        if (symbol == null || symbol.Length == 0)
        {
            //Debug.Log("Значение не может иметь длину 0 символов.");
            return;
        }

        objDropdown.options.Add(new Dropdown.OptionData(symbol));
        objDropdown.value = objDropdown.options.Count;

        //Debug.Log("Сейчас значение '" + symbol + "' в списке, теперь можно научить нейросеть сеть его распознавать.");
    }

    public void SetTrainingValue()
    {
        enableTraining = EnableTraningToggle.isOn;

        //if(enableTraining)
        //{
        //    Debug.Log("Режим обучения включен, теперь нейросеть сможет запоминать введённый образ.");
        //}
        //else
        //{
        //    Debug.Log("Режим обучения выключен.");
        //}
    }

    public void ObjectToMemory()
    {
        if (enableTraining)
        {
            string litera = objDropdown.options[objDropdown.value].text;

            if (litera.Length == 0)
            {
                //Debug.Log("Не выбран ни один символ для занесения в память.");
                resultText.text = "Не выбран ни один символ для занесения в память.";
                return;
            }
            nw.SetTraining(litera, arr, resultText);

            //Debug.Log("Выбранный символ '" + litera + "' успешно добавлен в память сети");
            resultText.text = "Выбранный символ '" + litera + "' успешно добавлен в память сети.";

            ClearCanvas();
        }
        else
        {
            //Debug.Log("Режим обучения не включен, ничего не произошло.");
            resultText.text = "Режим обучения не включен, ничего не произошло.";
        }
    }

    public void AppQuit()
    {
        Application.Quit();
    }

    private void UpdateQuest()
    {
        quest_word = nw.SetQuest();
        questText.text = "Нарисуйте: " + quest_word;
        //resultText.text = "";

        timer.SetTime(15);
    }
}
