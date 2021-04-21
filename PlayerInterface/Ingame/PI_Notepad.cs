using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_Notepad : MonoBehaviour
{
    //Активная вкладка.
    public Notepad_Tabs activeTab = Notepad_Tabs.Quests;//Активная вкладка.

    //Ссылки на другие объекты.
    private Object_Player playerData;//Данные игрока.
    private PC_Notepad notepadData;//Данные блокнота.
    public int startPage = 0;
    public void Start()
    {
        playerData = Camera.main.GetComponent<Object_Player>();//Данные игрока.
        notepadData = Camera.main.GetComponent<PC_Notepad>();//Данные блокнота.
    }

    public void NotepadGUI()
    {
        float buttonsPositionStartX = (Screen.width * 229) / 1920;//Самая левая позиция кнопок.
        float buttonsPositionY = (Screen.height*131)/1080;//Высота кнопок блокнота.

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), playerData.interfaceSkin.customStyles[13].normal.background);
        if (GUI.Button(new Rect(buttonsPositionStartX, buttonsPositionY-30, 30, 30), "Q"))
        {
            startPage = 0;
            activeTab = Notepad_Tabs.Quests;//Активная вкладка заданий.
        }
        if (GUI.Button(new Rect(buttonsPositionStartX + 30, buttonsPositionY - 30, 30, 30), "U"))
        {
            startPage = 0;
            activeTab = Notepad_Tabs.UnitTips;//Активная вкладка заметок.
        }
        switch (activeTab)
        {
            case Notepad_Tabs.Quests:
                DrawQuests(new Rect((Screen.width * 180) / 1920, (Screen.height * 150) / 1080, (Screen.width * 708) / 1920, (Screen.height * 800) / 1080));
                break;
            case Notepad_Tabs.UnitTips:
                DrawUnitTips(new Rect((Screen.width * 180) / 1920, (Screen.height * 150) / 1080, (Screen.width * 708) / 1920, (Screen.height * 800) / 1080));
                break;
        }
    }

    private void DrawUnitTips(Rect standartRect)
    {
        Rect page0Rect = standartRect;//Размеры и положение первой страницы.
        Rect page1Rect = new Rect((Screen.width * 933) / 1920, page0Rect.y, page0Rect.width, page0Rect.height);//Размеры и положение второй страницы.

        int maxLines = ((int)page0Rect.height / 20) - 5;//Получаем максимальное количество строк на странице.
        int currentTips = 0;//Количество занесенных подсказок.
        List<Source_NotepadPage_Tips> pages = new List<Source_NotepadPage_Tips>();//Страницы в блокноте.
        while(currentTips < notepadData.unitTips.Count)//Пока не обработаны все подсказки.
        {
            Source_NotepadPage_Tips page = new Source_NotepadPage_Tips();//Создаем новую страницу.
            for(int i = currentTips; i < notepadData.unitTips.Count; i++)
            {
                if(page.CurrentLines < maxLines)//Если барьер не преодолен.
                {
                    page.tipsList.Add(notepadData.unitTips[i]);//Добавляем подсказку на страницу.
                    currentTips += 1;//Прибавляем пройденную подсказку.
                }
                else
                {
                    break;
                }
            }
            pages.Add(page);//Добавляем новую страницу.
        }

        Debug.Log(pages.Count);

        for (int i = startPage; i <= startPage + 1; i++)
        {
            if (i >= 0 && i < pages.Count)//Для каждой страницы.
            {
                Rect currentPageRect = i == startPage ? page0Rect : page1Rect;
                int linesCounter = 0;//Счетчик линий на страницах блокнота.
                foreach (Source_Tip tip in pages[i].tipsList)//Для каждого задания на странице.
                {
                    GUI.Label(new Rect(currentPageRect.x, currentPageRect.y + 20 * linesCounter, currentPageRect.width, 20*tip.lineCount), tip.name + "\n   " + tip.description, playerData.interfaceSkin.customStyles[14]);//Отрисовываем название задания.
                    linesCounter += tip.lineCount;//Прибавляем счетчик линий.
                }
            }
        }

        if (startPage + 1 < pages.Count - 1)//Если следующая страничка существует.
        {
            if (GUI.Button(new Rect(page1Rect.x + page1Rect.width - 30, page1Rect.y + page1Rect.height - 30, 30, 30), ">"))
            {
                startPage += 1;
            }
        }
        if (startPage - 1 >= 0)//Если следующая страничка существует.
        {
            if (GUI.Button(new Rect(page0Rect.x + 10, page1Rect.y + page1Rect.height - 30, 30, 30), "<"))
            {
                startPage -= 1;
            }
        }
    }

    private void DrawQuests(Rect standartRect)//Отрисовка заданий.
    {
        Rect page0Rect = standartRect;//Размеры и положение первой страницы.
        Rect page1Rect = new Rect((Screen.width * 933) / 1920, page0Rect.y, page0Rect.width, page0Rect.height);//Размеры и положение второй страницы.

        int maxLines = ((int)page0Rect.height / 20) - 5;//Получаем максимальное количество строк на странице.
        int currentQuests = 0;//Количество занесенных квестов.
        List<Source_NotepadPage_Quests> pages = new List<Source_NotepadPage_Quests>();//Страницы в блокноте.
        Debug.Log("Максимальное кол-во строк =" + maxLines);
        while (currentQuests < notepadData.quests.Count)//Пока не обработаны все задания.
        {
            Source_NotepadPage_Quests page = new Source_NotepadPage_Quests();//Создаем новую страницу.
            for (int i = currentQuests; i < notepadData.quests.Count; i++)
            {
                if(page.CurrentLines < maxLines)//Если не преодолен барьер.
                {
                    page.questList.Add(notepadData.quests[i]);//Добавляем задание на страницу.
                    currentQuests += 1;//Добавляем количество пройденных квестов.
                }
                else//Если барьер преодолен.
                {
                    break;
                }
            }
            pages.Add(page);//Добавляем новую страницу.
        }

        Debug.Log(pages.Count);

        for(int i = startPage; i <= startPage + 1; i++)
        {
            if (i >= 0 && i < pages.Count)//Для каждой страницы.
            {
                Rect currentPageRect = i == startPage ? page0Rect : page1Rect;
                int linesCounter = 0;//Счетчик линий на страницах блокнота.
                foreach (Source_Quest quest in pages[i].questList)//Для каждого задания на странице.
                {
                    GUI.Label(new Rect(currentPageRect.x, currentPageRect.y + 20 * linesCounter, currentPageRect.width, 20), quest.Name, playerData.interfaceSkin.customStyles[14]);//Отрисовываем название задания.
                    linesCounter += 1;//Прибавляем счетчик линий.
                    foreach (Source_QuestRule rule in quest.rules)//Для каждой задачи в задании.
                    {
                        GUI.Label(new Rect(currentPageRect.x + 10, currentPageRect.y + 20 * linesCounter, currentPageRect.width - 10, 20), rule.Name, playerData.interfaceSkin.customStyles[14]);//Отрисовываем название задания.
                        linesCounter += 1;
                    }
                }
            }
        }
        if(startPage + 1 < pages.Count - 1)//Если следующая страничка существует.
        {
            if(GUI.Button(new Rect(page1Rect.x + page1Rect.width - 30, page1Rect.y + page1Rect.height - 30, 30, 30), ">"))
            {
                startPage += 1;
            }
        }
        if (startPage - 1 >= 0)//Если следующая страничка существует.
        {
            if (GUI.Button(new Rect(page0Rect.x + 10, page1Rect.y + page1Rect.height - 30, 30, 30), "<"))
            {
                startPage -= 1;
            }
        }
    }
}

public enum Notepad_Tabs { PlayerTips, UnitTips, Quests}
