using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class QDialog : MonoBehaviour
{
    public string file;

    public enum Status
    {
        ATTACK,
        IDLE,
        FOUND_PLAYER,
        PLAYER_LOST,
        PLAYER_IN_RANGE_AGAIN,
        PLAYER_OUT_OF_RANGE
    }


    public string LoadRandomLine(Status status)
    {
        string line;
        List<string> LoadedLines = new List<string>(); //List to load lines into

        var textFile = Resources.Load<TextAsset>(file);

        //StreamReader r = new StreamReader("Assets\\Dialog" + "\\" + file); //Streamreder for files

        if (textFile != null)
        {
            Debug.Log("textfile was not null");
            do
            {
                line = textFile.ToString();
                if (line != null)
                {
                    string[] lineData = line.Split(new[] { "\\n" }, StringSplitOptions.RemoveEmptyEntries); //Removes \n from string
                    if (lineData[0] == status.ToString())
                    {
                        string lineEntry = lineData[1];
                        LoadedLines.Add(lineEntry);
                    }
                }
            }
            while (line != null);
        }
        //line_ = File.ReadLines(file).Skip(line-1).Take(1).First(); //This skips to line
        //using (r)
        //{
        //    do
        //    {
        //        line = r.ReadLine();
        //        if (line != null)
        //        {
        //            string[] lineData = line.Split(new[] { "\\n" }, StringSplitOptions.RemoveEmptyEntries); //Removes \n from string
        //            if (lineData[0] == status.ToString())
        //            {
        //                string lineEntry = lineData[1];
        //                LoadedLines.Add(lineEntry);
        //            }
        //        }
        //    }
        //    while (line != null);
        //    r.Close();
        //}
        if (LoadedLines.Count == 0) return "I AM SILLY!"; //If all fails

        int lineNr = Random.Range(0, LoadedLines.Count);


        return LoadedLines[lineNr];
    }

}