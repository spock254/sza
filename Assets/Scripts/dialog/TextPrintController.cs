using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TextPrintController
{
    const string keys = "@|<";
    public delegate IEnumerator Print(string text, float timer);

    Dictionary<char, Print> printDb = new Dictionary<char, Print>();
    Text textField;
    bool isRunning = false;

    MonoBehaviour instance;

    //List<PrintKV> printKVs = new List<PrintKV>();
    // "wqeew fdsf @rere wewe qwqw |vfvdvd"
    public TextPrintController(Text textField, MonoBehaviour monoBehaviour) 
    {
        this.instance = monoBehaviour;
        this.textField = textField;
        
        printDb.Add('@', LetterByLetter);
        printDb.Add('|', WordByWordPrint);
        printDb.Add('<', FromRightToLeft);
    }

    public List<PrintKV> ParseText(string text)
    {
        List<PrintKV> printKVs = new List<PrintKV>();
        string currentText = string.Empty;

        foreach (char letter in text)
        {
            if (IsKeyPresent(letter) == true)
            {
                printKVs.Add(new PrintKV(printDb[letter], currentText));
                currentText = string.Empty;
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

    IEnumerator FromRightToLeft(string text, float timer) 
    {
        isRunning = true;
        Debug.Log(textField.text);
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

        instance.StartCoroutine(LetterByLetter(extracted, 0.005f));
    }

    IEnumerator Process(string text)
    {
        List<TextPrintController.PrintKV> printKVs = ParseText(text);

        for (int i = 0; i < printKVs.Count;)
        {
            if (IsRunning() == false)
            {
                instance.StartCoroutine(printKVs[i].printFun(printKVs[i].text, 0.05f));
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

        public PrintKV(Print printFun, string text) 
        {
            this.text = text;
            this.printFun = printFun;
        }
    }
}
