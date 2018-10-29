using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// в этом классе собраны функции для преобразования изображений
public class NeiroGraphUtils
{
    // очищает рисунок
    public static void ClearImage(Texture2D texture)
    {
        texture = new Texture2D(texture.width, texture.height);
    }

    //// преобразовать рисунок в массив, все цвета кроме белого заносятся как 1, белый  - 0
    //public static int[,] GetArrayFromTexture(Texture2D image)
    //{
    //    int[,] res = new int[image.width, image.height];
    //    for (int n = 0; n < res.GetLength(0); n++)
    //        for (int m = 0; m < res.GetLength(1); m++)
    //        {
    //            int color = ((int)image.GetPixel(n, m).r + (int)image.GetPixel(n, m).g + (int)image.GetPixel(n, m).b) / 3;
    //            res[n, m] = color > 0 ? 1 : 0;
    //        }
    //    return res;
    //}

    //// преобразовать массив в рисунок
    //public static Texture2D GetTextureFromArr(int[,] array)
    //{
    //    Texture2D bitmap = new Texture2D(array.GetLength(0), array.GetLength(1));
    //    for (int x = 0; x < array.GetLength(0); x++)
    //        for (int y = 0; y < array.GetLength(1); y++)
    //            if (array[x, y] == 0)
    //                bitmap.SetPixel(x, y, Color.white);
    //            else
    //                bitmap.SetPixel(x, y, Color.black);
    //    return bitmap;
    //}

    //обрезать рисунок по краям и преобразовать в массив
    public static int[,] CutImageToArray(Texture2D b, Vector2 max)
    {
        int x1 = 0;
        int y1 = 0;
        int x2 = (int)max.x;
        int y2 = (int)max.y;
        
        for (int y = 0; y < b.height && y1 == 0; y++)
            for (int x = 0; x < b.width && y1 == 0; x++)
                if (b.GetPixel(x, y) != Color.white) y1 = y;
        for (int y = b.height - 1; y >= 0 && y2 == max.y; y--)
            for (int x = 0; x < b.width && y2 == max.y; x++)
                if (b.GetPixel(x, y) != Color.white) y2 = y;
        for (int x = 0; x < b.width && x1 == 0; x++)
            for (int y = 0; y < b.height && x1 == 0; y++)
                if (b.GetPixel(x, y) != Color.white) x1 = x;
        for (int x = b.width - 1; x >= 0 && x2 == max.x; x--)
            for (int y = 0; y < b.height && x2 == max.x; y++)
                if (b.GetPixel(x, y) != Color.white) x2 = x;

        if (x1 == 0 && y1 == 0 && x2 == max.x && y2 == max.y) return null;

        int size = x2 - x1 > y2 - y1 ? x2 - x1 + 1 : y2 - y1 + 1;
        int dx = y2 - y1 > x2 - x1 ? ((y2 - y1) - (x2 - x1)) / 2 : 0;
        int dy = y2 - y1 < x2 - x1 ? ((x2 - x1) - (y2 - y1)) / 2 : 0;

        int[,] res = new int[size, size];
        for (int x = 0; x < res.GetLength(0); x++)
            for (int y = 0; y < res.GetLength(1); y++)
            {
                int pX = x + x1 - dx;
                int pY = y + y1 - dy;
                if (pX < 0 || pX >= max.x || pY < 0 || pY >= max.y)
                    res[x, y] = 0;
                else
                    res[x, y] = b.GetPixel(x + x1 - dx, y + y1 - dy) == Color.white ? 0 : 1;
            }
        return res;
    }

    // пересчитать массив source в массив res - используется для 
    // приведения произвольного массива данных к массиву стандартных размеров
    public static int[,] LeadArray(int[,] source, int[,] res)
    {
        for (int n = 0; n < res.GetLength(0); n++)
            for (int m = 0; m < res.GetLength(1); m++) res[n, m] = 0;

        double pX = (double)res.GetLength(0) / (double)source.GetLength(0);
        double pY = (double)res.GetLength(1) / (double)source.GetLength(1);

        for (int n = 0; n < source.GetLength(0); n++)
            for (int m = 0; m < source.GetLength(1); m++)
            {
                int posX = (int)(n * pX);
                int posY = (int)(m * pY);
                if (res[posX, posY] == 0) res[posX, posY] = source[n, m];
            }
        return res;
    }
}

