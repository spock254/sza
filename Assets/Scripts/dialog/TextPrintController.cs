using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextPrintController
{
    const string keys = "@|<%";

    string[] timingKeys = new string[]
    {
                "[0.1]",
                "[0.15]",
                "[0.2]",
                "[0.3]",
                "[0.4]",
                "[0.5]",
                "[0.6]",
                "[0.7]",
                "[0.8]",
                "[0.9]",
    };  /*   TODO    */

    public delegate IEnumerator Print(string text, float timer);

    Dictionary<char, Print> printDb = new Dictionary<char, Print>();
    Text textField;
    bool isRunning = false;

    MonoBehaviour instance;

    public TextPrintController(Text textField, MonoBehaviour monoBehaviour)
    {
        this.instance = monoBehaviour;
        this.textField = textField;

        printDb.Add('@', LetterByLetter);
        printDb.Add('|', WordByWordPrint);
        printDb.Add('<', FromRightToLeft);
        printDb.Add('%', RandomString);
    }

    public List<PrintKV> ParseText(string text)
    {
        List<PrintKV> printKVs = new List<PrintKV>();
        string currentText = string.Empty;

        bool isTimingString = false;
        string timer = string.Empty;

        foreach (char letter in text)
        {
            if (isTimingString == true && letter != ']')
            {
                timer += letter;
            }
            else if (IsKeyPresent(letter) == true)
            {
                printKVs.Add(new PrintKV(printDb[letter], currentText,
                    float.Parse(timer, CultureInfo.InvariantCulture.NumberFormat)));

                currentText = string.Empty;
                timer = string.Empty;
            }
            else if (letter == '[')
            {
                isTimingString = true;
            }
            else if (letter == ']')
            {
                isTimingString = false;
            }
            else
            {
                currentText += letter;
            }
        }

        return printKVs;
    }

    public string GetCleanText(String textToReplace)
    {

        foreach (var key in keys)
        {
            textToReplace = textToReplace.Replace(key.ToString(), string.Empty);
        }

        foreach (var key in timingKeys)
        {
            textToReplace = textToReplace.Replace(key, string.Empty);
        }

        return textToReplace;
    }

    bool IsKeyPresent(char key)
    {
        return keys.Contains(key.ToString());
    }


    public IEnumerator GetPrinter(char key, string text, float timer)
    {
        textField.text = string.Empty;
        return printDb[key](text, timer);
    }

    IEnumerator LetterByLetter(string text, float timer)
    {
        isRunning = true;

        foreach (var letter in text)
        {
            textField.text += letter;
            yield return new WaitForSeconds(timer);
        }

        isRunning = false;
    }

    IEnumerator WordByWordPrint(string text, float timer)
    {
        isRunning = true;

        string[] words = text.Split(' ');

        foreach (var word in words)
        {
            textField.text += word + " ";
            yield return new WaitForSeconds(timer);
        }

        isRunning = false;
    }

    IEnumerator RandomString(string text, float timer)
    {
        isRunning = true;

        string temp = StringRepiter(" ", text.Length);
        char[] tempArr = temp.ToArray();

        System.Random random = new System.Random();
        int[] textIndexes = Enumerable.Range(0, text.Length).OrderBy(c => random.Next()).ToArray();

        for (int i = 0; i < textIndexes.Length; i++)
        {
            tempArr[textIndexes[i]] = text[textIndexes[i]];
            textField.text = textField.text + new string(tempArr);

            yield return new WaitForSeconds(timer);

            textField.text = textField.text.Substring(0, textField.text.Length - text.Length);
        }

        textField.text += new string(tempArr);

        isRunning = false;
    }



    IEnumerator FromRightToLeft(string text, float timer)
    {
        isRunning = true;

        for (int i = text.Length - 1, j = 0; i > -1; i--, j++)
        {
            textField.text = StringRepiter(" ", text.Length - 1 - j) + text[i] + textField.text;
            textField.text = textField.text.Remove(0, i);

            yield return new WaitForSeconds(timer);
        }

        isRunning = false;
    }

    string StringRepiter(string src, int count)
    {
        return String.Concat(Enumerable.Repeat(src, count));
    }
    public void ProcessText(string text)
    {
        instance.StartCoroutine(Process(text));
    }

    public void CompleteText(string text)
    {
        string cleanText = GetCleanText(text);
        string extracted = cleanText.Substring(textField.text.Length,
            cleanText.Length - textField.text.Length);

        instance.StartCoroutine(LetterByLetter(extracted, 0.01f));
    }

    IEnumerator Process(string text)
    {
        List<TextPrintController.PrintKV> printKVs = ParseText(text);

        for (int i = 0; i < printKVs.Count;)
        {
            if (IsRunning() == false)
            {
                instance.StartCoroutine(printKVs[i].printFun(printKVs[i].text, printKVs[i].timer));
                i++;
            }
            else
            {
                yield return null;
            }
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void Stop()
    {
        isRunning = false;
        instance.StopAllCoroutines();
    }

    public struct PrintKV
    {
        public Print printFun;
        public string text;
        public float timer;

        public PrintKV(Print printFun, string text, float timer)
        {
            this.timer = timer;
            this.text = text;
            this.printFun = printFun;
        }
    }
}
