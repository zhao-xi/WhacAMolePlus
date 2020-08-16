using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    public float score = 0f;
    public bool hasRed = false;
    public Material greenMaterial;
    [HideInInspector] public float[][] posScores;

    [HideInInspector] public bool lastDisappeared = false;
    float redAppearTime = 0f;

    private void Awake()
    {
        posScores = new float[12][];
        for(int i = 0; i < 12; i++)
        {
            posScores[i] = new float[21];
        }
    }

    private void Update()
    {
        if (hasRed) redAppearTime += Time.deltaTime;
        else redAppearTime = 0f;

        if (lastDisappeared) EndGame();

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            var ray = Camera.main.ScreenPointToRay(touch.position);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "white")
            {
                hit.collider.gameObject.GetComponent<Renderer>().material = greenMaterial;
                hit.collider.gameObject.tag = "green";

                int index = hit.collider.transform.parent.GetSiblingIndex();
                int row = index / 21;
                int col = index % 21;
                Debug.Log("row : " + row + " col : " + col + " score : " + posScores[row][col] + " (before)");

                if (hasRed)
                {
                    score -= 1;
                    score -= redAppearTime;

                    posScores[row][col] -= 1;
                    posScores[row][col] -= redAppearTime;
                }
                else
                {
                    score += 1;
                    score += hit.collider.gameObject.GetComponent<DisappearAfterTime>().life;

                    posScores[row][col] += 1;
                    posScores[row][col] += hit.collider.gameObject.GetComponent<DisappearAfterTime>().life;
                }

                Debug.Log("row : " + row + " col : " + col + " score : " + posScores[row][col] + " (after)");
            }
        }

        //GameObject.Find("scoreText").GetComponent<Text>().text = score.ToString();
    }

    bool notSaved = true;

    private void EndGame()
    {
        if (notSaved) Save();
        Debug.Log("game ended.");
    }

    private List<string[]> rowData = new List<string[]>();

    void Save()
    {
        for (int i = 0; i < 12; i++)
        {
            string[] rowDataTemp = new string[21];
            for(int j = 0; j < 21; j++)
            {
                rowDataTemp[j] = posScores[i][j].ToString();
            }
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = Application.dataPath + "/CSV/" + "Saved_data.csv";

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        notSaved = false;
    }
}
