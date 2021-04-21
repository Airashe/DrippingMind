using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source_NotepadPage_Tips
{
    public List<Source_Tip> tipsList;
    public int CurrentLines
    {
        get
        {
            int result = 0;
            foreach(Source_Tip tip in tipsList)
            {
                result += tip.lineCount;
            }
            return result;
        }
    }

    public Source_NotepadPage_Tips()
    {
        tipsList = new List<Source_Tip>();
    }
}
