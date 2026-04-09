using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10];

    string currentDirectory;

    public string scoreFileName = "highscores.txt";


    // Start is called before the first frame update
    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log ("OUr current directory is: " + currentDirectory);

        LoadScoresFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScoresFromFile()
    {
        // Before we try to read a file, we should check that
        // it exists. If it doesn't exist, we'll log a message and
        // abort.
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (fileExists == true)
        {
            Debug.Log("Found high score file " + scoreFileName);
        }
        else
        {
            Debug.Log("The file " + scoreFileName +
            " does not exist. No scores will be loaded.", this);
            return;
        }
        // Make a new array of default values. This ensures that
        // no old values stick around if we've loaded a scores file
        // in the past.
        scores = new int[scores.Length];
        // Now we read the file in. We do this using a "StreamReader",
        // which we give our full file path to. Don't forget the directory
        // separator between the directory and the filename!
        StreamReader fileReader = new StreamReader(currentDirectory +
        "\\" + scoreFileName);
        // A counter to make sure we don't go past the end of our scores
        int scoreCount = 0;
        // A while loop, which runs as long as there is data to be
        // read AND we haven't reached the end of our scores array.
        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            // Read that line into a variable
            string fileLine = fileReader.ReadLine();
            // Try to parse that variable into an int
            // First, make a variable to put it in
            int readScore = -1;
            // Try to parse it
            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse)
            {
                // If we successfully read a number, put it in the array.
                scores[scoreCount] = readScore;
            }
            else
            {
                // If the number couldn't be parsed then we probably had
                // junk in our file. Lets print an error, and then use
                // a default value.
                Debug.Log("Invalid line in scores file at " + scoreCount +
                ", using default value.", this);
                scores[scoreCount] = 0;
            }
            // Don't forget to incrememt the counter!
            scoreCount++;
        }
        // Make sure to close the stream!
        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
    }

    public void SaveScoresToFile()
    {
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);
        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }

        fileWriter.Close();

        Debug.Log("High scores written to " + scoreFileName);
    }

    public void AddScore(int newScore)
    {
        int desiredIndex = -1;
        for (int i = 0;i < scores.Length;i++)
        {
            if (scores[i] < newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }

        if (desiredIndex < 0)
        {
            Debug.Log("Scores of " + newScore + "not high enough for high scores list.");
            return;   
        }

        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
        }

        scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore + " entered the high scores at position " + desiredIndex, this);

    }
}
