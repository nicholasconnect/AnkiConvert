using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string baseDir = Directory.GetCurrentDirectory();
        string inputPath = Path.Combine(baseDir, "input.txt");
        string outputDir = Path.Combine(baseDir, "output");
        string outputPath = Path.Combine(outputDir, "output.csv");

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("No input.txt found.");
            return;
        }

        Directory.CreateDirectory(outputDir);

        // Read all text from the input file to properly handle multi-line questions
        string allText = File.ReadAllText(inputPath, Encoding.UTF8);

        // Split the text into question blocks
        // We'll use a regex pattern to identify new questions
        string[] questionBlocks = Regex.Split(allText, @"(?=^(?!-\s).*?\?|^Question\s+\d+:)",
            RegexOptions.Multiline).Where(block => !string.IsNullOrWhiteSpace(block)).ToArray();

        // Process the question blocks and create the CSV output
        var outputLines = new List<string>();

        // Add CSV header
        outputLines.Add("Title,Question,Question type,Answer 1,Answer 2,Answer 3,Answer 4,Answer 5,Answer 6,Answer 7,Answer 8,Answer 9,Answer 10,Answer 11,Answer 12,Correct Answers,,");

        int questionNumber = 0;

        foreach (string block in questionBlocks)
        {
            if (block.Trim().StartsWith("Question") && !block.Contains("?"))
            {
                // This is just a question header, skip it
                continue;
            }

            questionNumber++;

            string[] lines = block.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder questionText = new StringBuilder();
            List<string> answers = new List<string>();
            List<int> correctIndices = new List<int>();

            bool inQuestion = true;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // If line starts with "Question X:", skip the prefix
                if (trimmedLine.StartsWith("Question") && trimmedLine.Contains(":"))
                {
                    string questionPart = trimmedLine.Substring(trimmedLine.IndexOf(":") + 1).Trim();
                    if (!string.IsNullOrEmpty(questionPart))
                    {
                        questionText.Append(questionPart + " ");
                    }
                    continue;
                }

                if (trimmedLine.StartsWith("-"))
                {
                    // We've reached an answer option
                    inQuestion = false;

                    string answerText = trimmedLine.Substring(1).Trim();
                    bool isCorrect = answerText.EndsWith("/");

                    if (isCorrect)
                    {
                        // Remove the trailing slash
                        answerText = answerText.Substring(0, answerText.Length - 1).Trim();
                        correctIndices.Add(answers.Count); // Store the index of the correct answer
                    }

                    answers.Add(answerText);
                }
                else if (inQuestion)
                {
                    // This is part of the question text
                    questionText.Append(trimmedLine + " ");
                }
            }

            // Process this question
            if (questionText.Length > 0 && answers.Count > 0)
            {
                ProcessQuestion(outputLines, questionText.ToString().Trim(), answers, correctIndices, questionNumber);
            }
        }

        // Write to CSV file
        File.WriteAllLines(outputPath, outputLines, Encoding.UTF8);
        Console.WriteLine($"Conversion complete. {questionNumber} questions saved to output/output.csv");
    }

    static void ProcessQuestion(List<string> outputLines, string question, List<string> answers, List<int> correctIndices, int questionNumber)
    {
        var csvLine = new StringBuilder();

        // Title - Just use "Question X" as title
        csvLine.Append($"Question {questionNumber},");

        // Question text - Remove any commas
        csvLine.Append($"\"{question.Replace(",", "").Replace("\"", "\"\"")}\",");

        // Question type - 2 for single choice, 1 for multiple choice
        int questionType = correctIndices.Count > 1 ? 1 : 2;
        csvLine.Append($"{questionType},");

        // Add answers (up to 12)
        for (int i = 0; i < 12; i++)
        {
            if (i < answers.Count)
                csvLine.Append($"\"{answers[i].Replace(",", "").Replace("\"", "\"\"")}\",");
            else
                csvLine.Append(","); // Empty answer slot
        }

        // Add correct answer positions (0s and 1s with spaces)
        var correctPositions = new int[answers.Count];
        foreach (var index in correctIndices)
        {
            correctPositions[index] = 1;
        }

        csvLine.Append(string.Join(" ", correctPositions));

        // Add two blank fields
        csvLine.Append(",,");

        outputLines.Add(csvLine.ToString());
    }
}