using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.Core.IO.ScriptSerialization;

public class NeiroWeb
{
    // это по сути контейнер для массива нейронов neironArray
    // загружет его при создании из файла и сохраняет при выходе

    public const int neironInArrayWidth = 100; // количество по горизонтали
    public const int neironInArrayHeight = 100; // количество по вертикали
    private const string memory = "memory.txt"; // имя файла хранения сети
    private List<Neiron> neironArray = null; // массив нейронов

    // конструктор(нет)
    public NeiroWeb()
    {
        Debug.Log("InitWeb");
        neironArray = InitWeb();
    }

    // функция открывает текстовой файл и преобразовывает его в массив нейронов
    private static List<Neiron> InitWeb()
    {
        if (!File.Exists(memory))
        {
            Debug.Log("файл не найден, создаю новый");
            return new List<Neiron>();
        }
        Debug.Log("файл найден");
        string[] lines = File.ReadAllLines(memory);
        if (lines.Length == 0) return new List<Neiron>();
        string jStr = lines[0];

        JavaScriptSerializer json = new JavaScriptSerializer();

        return json.Deserialize<List<Neiron>>(jStr);
    }

    // преобразовать структуру данных в клас нейрона
    private static Neiron NeironCreate(Dictionary<string, object> o)
    {
        Neiron res = new Neiron();
        res.obj_name = (string)o["obj_name"];
        res.countTrainig = (int)o["countTrainig"];
        System.Object[] weightsData = (System.Object[])o["weights"];
        int arrSize = (int)Math.Sqrt(weightsData.Length);
        res.weight = new double[arrSize, arrSize];
        int index = 0;
        for (int n = 0; n < res.weight.GetLength(0); n++)
            for (int m = 0; m < res.weight.GetLength(1); m++)
            {
                res.weight[n, m] = Double.Parse(weightsData[index].ToString());
                index++;
            }
        return res;
    }

    // функция сравнивает входной массив с каждым нейроном из сети и 
    // возвращает имя нейрона наиболее похожего на него
    // именно эта функция отвечает за распознавание образа

    public string CheckLitera(int[,] arr)
    {
        string res = null;
        double max = 0;
        foreach (var n in neironArray)
        {
            double d = n.GetRes(arr);

            if (d > max)
            {
                max = d;
                res = n.GetName();
            }
        }
        return res;      
    }

    // функция сохраняет массив нейронов в файл
    public void SaveState()
    {
        JavaScriptSerializer json = new JavaScriptSerializer();
        string jStr = json.Serialize(neironArray);
        System.IO.StreamWriter file = new System.IO.StreamWriter(memory);
        file.WriteLine(jStr);
        file.Close();
    }

    // получить список имён образов, имеющихся в памяти
    public string[] GetLiteras()
    {
        var res = new List<string>();
        for (int i = 0; i < neironArray.Count; i++) res.Add(neironArray[i].GetName());
        res.Sort();
        return res.ToArray();
    }

    // эта функция заносит в память нейрона с именем trainingName
    // новый вариант образа data

    public void SetTraining(string trainingName, int[,] data)
    {
        Neiron neiron = neironArray.Find(v => v.obj_name.Equals(trainingName));
        if (neiron == null) // если нейрона с таким именем не существует, создадим новыи и добавим
        {                   // его в массив нейронов
            neiron = new Neiron();
            neiron.Clear(trainingName, neironInArrayWidth, neironInArrayHeight);
            neironArray.Add(neiron);
        }
        int countTrainig = neiron.Training(data); // обучим нейрон новому образу
        //string messageStr = "Имя образа - " + neiron.GetName() +
        //                    " вариантов образа в памяти - " + countTrainig.ToString();

        // покажем визуальное отображение памяти обученного нейрона
        //Form resultForm = new ShowMemoryVeight(neiron);
        //resultForm.Text = messageStr;
        //resultForm.Show();
    }
}
