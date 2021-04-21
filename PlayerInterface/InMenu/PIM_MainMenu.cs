using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIM_MainMenu : MonoBehaviour
{
    public GUISkin mainSkin;//Основной скин главного меню.
    private Source_GameManager gameManager;//Ссылка на менеджер игры.

    //Контроллеры анимированного GUI.
    public Source_AnimatedGUI test;
    public Source_AnimationController heartAC;//Контроллер анимации сердца.

    public bool awaitsInput;//Ожидаем ли изменение клавиши.
    public string awaitsInputKey;//Ключ которому будет назначена новая клавиша.
    Resolution[] resolutions;
    public int selectedResolution;//Выбранное разрешение экрана.
    public int anisotropicFiltering = 1;
    public int shadows = 0;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Получаем ссылку на менеджер игры.
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                selectedResolution = i;
            }
        }
        anisotropicFiltering = QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable ? 0 : QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable ? 1 : 2;
        shadows = QualitySettings.shadows == ShadowQuality.Disable ? 0 : QualitySettings.shadows == ShadowQuality.HardOnly ? 1 : 2;
        gameManager.gammaCorrection = 0;
    }

    private void OnGUI()
    {
        if(awaitsInput && awaitsInputKey.Length > 0)
        {
            if(Event.current.keyCode != KeyCode.None)//Если есть клавиша которую наживаем.
            {
                Source_Constants.userInputs[awaitsInputKey] = Event.current.keyCode;
                awaitsInput = false;
            }
            if(Input.GetMouseButtonDown(0))
            {
                Source_Constants.userInputs[awaitsInputKey] = KeyCode.Mouse0;
                awaitsInput = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Source_Constants.userInputs[awaitsInputKey] = KeyCode.Mouse1;
                awaitsInput = false;
            }
        }

        switch(gameManager.stage)//В зависимости от стадии игры.
        {
            case GameStage.MainMenu://Если находимся в главном меню.
                switch(gameManager.mainMenu)//В зависимости от вкладки главного меню.
                {
                    case MainMenu.MainMenu://Если вкладка главного меню.
                        Interface_MainMenu();//Отрисовываем главное меню.
                        break;
                    case MainMenu.Settings://Если вкладка настроек.
                        Interface_SettingsMenu();//Отрисовываем интерфейс настроек.
                        break;
                }
                break;
            case GameStage.SceneLoading://Если загрузка сцены.
                Interface_Loading();//Отрисовываем интерфейс загрузки.
                break;
            case GameStage.Loading://Если загрузка сцены.
                Interface_Loading();//Отрисовываем интерфейс загрузки.
                break;
            case GameStage.AwaitingEventsSystem:
                Interface_Loading();//Отрисовываем интерфейс загрузки.
                break;
            case GameStage.Game://Если в реальности 1.
                break;
        }
    }

    private void Interface_MainMenu()//Интерфейс главного меню.
    {
        Vector2 buttonSize = new Vector2(240, 60);
        Vector2 buttonPosition = new Vector2(0, 0);//Позиция кнопок на экране.

        float sizeQuadrantY = (Screen.height / 6);//Размер равной части.
        buttonPosition.y = sizeQuadrantY * 5 + (sizeQuadrantY / 2) - (buttonSize.y/2);//Позиция по высоте.
        float sizeQuadrantX = (Screen.width/3);//.
        buttonSize.x = sizeQuadrantX;//Ширина кнопки в квадрат.

        if (GUI.Button(new Rect(0, buttonPosition.y, buttonSize.x, buttonSize.y), "New story", mainSkin.customStyles[0]))//Кнопка новая игра.
        {
            gameManager.StartGame(1);//Начало игры.
        }
        if (GUI.Button(new Rect(sizeQuadrantX * 1, buttonPosition.y, buttonSize.x, buttonSize.y), "Settings", mainSkin.customStyles[0]))//Кнопка настроек.
        {
            gameManager.mainMenu = MainMenu.Settings;
            gameManager.settingsTab = SettingsTab.Hotkeys;
        }
        if (GUI.Button(new Rect(sizeQuadrantX * 2, buttonPosition.y, buttonSize.x, buttonSize.y), "Exit", mainSkin.customStyles[0]))//Выход.
        {
            Application.Quit();//Выход из игры.
        }
    }

    private void Interface_SettingsMenu()//Интерфейс меню настроек.
    {
        Vector2 buttonSize = new Vector2(240, 60);
        Vector2 buttonPosition = new Vector2(0, 0);//Позиция кнопок на экране.

        float sizeQuadrantY = (Screen.height / 6);//Размер равной части.
        buttonPosition.y = sizeQuadrantY * 5 + (sizeQuadrantY / 2) - (buttonSize.y / 2);//Позиция по высоте.
        float sizeQuadrantX = (Screen.width / 3);//.
        buttonSize.x = sizeQuadrantX;//Ширина кнопки в квадрат.

        GUI.Label(new Rect(0, 0, Screen.width, buttonSize.y), "Settings:", mainSkin.customStyles[3]);//Заголовок меню.

        if(GUI.Button(new Rect(10, buttonSize.y, 180, 30), "Hotkeys", mainSkin.customStyles[4]))//Настройки хоткеев.
        {
            gameManager.settingsTab = SettingsTab.Hotkeys;
        }
        if (GUI.Button(new Rect(200, buttonSize.y, 180, 30), "Video", mainSkin.customStyles[4]))//Настройки видео.
        {
            gameManager.settingsTab = SettingsTab.Video;
        }
        if (GUI.Button(new Rect(390, buttonSize.y, 180, 30), "Audio", mainSkin.customStyles[4]))//Настройки аудио.
        {
            Debug.Log("Nigaa");
            gameManager.settingsTab = SettingsTab.Audio;
        }

        switch(gameManager.settingsTab)
        {
            case SettingsTab.Hotkeys:
                Interface_SettingsMenu_Hotkeys();//Рисуем вкладку с клавишами.
                break;
            case SettingsTab.Video:
                Interface_SettingsMenu_VideoSettings();//Рисуем вкладку с видео.
                break;
            case SettingsTab.Audio:
                Interface_SettingsMenu_AudioSettings();
                break;
        }

        if (GUI.Button(new Rect(sizeQuadrantX * 2, buttonPosition.y, buttonSize.x, buttonSize.y), "Back", mainSkin.customStyles[0]))//Выход.
        {
            gameManager.mainMenu = MainMenu.MainMenu;
        }
    }

    private void Interface_SettingsMenu_Hotkeys()//Интерфейс вводимых клавиш.
    {
        int hotKeyCounter = 0;//Счетчик клавиш.
        foreach (KeyValuePair<string, KeyCode> hotkeyData in Source_Constants.userInputs)//Для каждой горячей клавиши.
        {
            GUI.Box(new Rect(10, 100 + hotKeyCounter*32, Screen.width - 10, 30), hotkeyData.Key, mainSkin.customStyles[5]);//Рамка.
            GUI.Label(new Rect(10, 100 + hotKeyCounter * 32, Screen.width - 10, 30), Source_Constants.HotkeyTranslator(hotkeyData.Key), mainSkin.customStyles[6]);//Текст горячей клавиши.
            if(GUI.Button(new Rect(Screen.width/2, 100 + hotKeyCounter * 32, 180, 30), hotkeyData.Value.ToString(), mainSkin.customStyles[4]))
            {
                awaitsInput = true;
                awaitsInputKey = hotkeyData.Key;
            }
            hotKeyCounter += 1;//Прибавляем счетчик.
        }
    }

    private void Interface_SettingsMenu_VideoSettings()
    {
        GUI.Box(new Rect(10, 100, Screen.width - 10, 30), "", mainSkin.customStyles[5]);//Рамка.
        GUI.Label(new Rect(10, 100, Screen.width - 10, 30), "Window mode:", mainSkin.customStyles[6]);//Текст горячей клавиши.
        if(GUI.Button(new Rect(Screen.width / 2, 100, 180, 30), Screen.fullScreen ? "Fullscreen" : "Windowed", mainSkin.customStyles[4]))
        {
            Screen.fullScreenMode = Screen.fullScreen ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;//Установка полноэкранного режима.
        }
        GUI.Box(new Rect(10, 132, Screen.width - 10, 30), "", mainSkin.customStyles[5]);//Рамка.
        GUI.Label(new Rect(10, 132, Screen.width - 10, 30), "Resolution:", mainSkin.customStyles[6]);//Текст горячей клавиши.
        if (GUI.Button(new Rect(Screen.width / 2, 132, 180, 30), resolutions[selectedResolution].width + "x" + resolutions[selectedResolution].height, mainSkin.customStyles[4]))
        {
            selectedResolution += 1;
            selectedResolution = selectedResolution >= resolutions.Length ? 0 : selectedResolution;
            Screen.SetResolution(resolutions[selectedResolution].width, resolutions[selectedResolution].height, Screen.fullScreen);
        }

        GUI.Box(new Rect(10, 164, Screen.width - 10, 30), "", mainSkin.customStyles[5]);//Рамка.
        GUI.Label(new Rect(10, 164, Screen.width - 10, 30), "Anisotropic filtering:", mainSkin.customStyles[6]);//Текст горячей клавиши.
        if (GUI.Button(new Rect(Screen.width / 2, 164, 180, 30), anisotropicFiltering == 0?"Disable" : anisotropicFiltering == 1 ? "Enable" : "ForceEnable", mainSkin.customStyles[4]))
        {
            anisotropicFiltering += 1;
            anisotropicFiltering = anisotropicFiltering > 2 ? 0 : anisotropicFiltering;
            switch(anisotropicFiltering)
            {
                case 0:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 2:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
            }
        }

        GUI.Box(new Rect(10, 196, Screen.width - 10, 30), "", mainSkin.customStyles[5]);//Рамка.
        GUI.Label(new Rect(10, 196, Screen.width - 10, 30), "Shadows:", mainSkin.customStyles[6]);//Текст горячей клавиши.
        if (GUI.Button(new Rect(Screen.width / 2, 196, 180, 30), shadows == 0 ? "Disable" : shadows == 1 ? "Hard only" : "All", mainSkin.customStyles[4]))
        {
            shadows += 1;
            shadows = shadows > 2 ? 0 : shadows;
            switch (shadows)
            {
                case 0:
                    QualitySettings.shadows = ShadowQuality.Disable;
                    break;
                case 1:
                    QualitySettings.shadows = ShadowQuality.HardOnly;
                    break;
                case 2:
                    QualitySettings.shadows = ShadowQuality.All;
                    break;
            }
        }

        GUI.Box(new Rect(10, 228, Screen.width - 10, 30), "", mainSkin.customStyles[5]);//Рамка.
        GUI.Label(new Rect(10, 228, Screen.width - 10, 30), "Gamma (not stable):", mainSkin.customStyles[6]);//Текст горячей клавиши.
        gameManager.gammaCorrection = GUI.HorizontalSlider(new Rect(Screen.width / 2, 238, 180, 20), gameManager.gammaCorrection, 0, 1.0f, mainSkin.horizontalSlider, mainSkin.horizontalSliderThumb);
    }

    private void Interface_SettingsMenu_AudioSettings()
    {
        GUI.Box(new Rect(10, 100, Screen.width - 10, 30), "", mainSkin.customStyles[5]);//Рамка.
        GUI.Label(new Rect(10, 100, Screen.width - 10, 30), "Audio volume:", mainSkin.customStyles[6]);//Текст горячей клавиши.
        AudioListener.volume = GUI.HorizontalSlider(new Rect(Screen.width / 2, 110, 180, 20), AudioListener.volume, 0, 1.0f, mainSkin.horizontalSlider, mainSkin.horizontalSliderThumb);
    }

    private void Interface_Loading()//Интерфейс загрузки.
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainSkin.customStyles[1].normal.background);//Черная маска на весь экран.

        if (heartAC != null)//Если есть контроллер анимации.
        {
            heartAC.Play();//Проигрываем анимацию сердца.
        }
        else
        {
            heartAC = new Source_AnimationController(test, 15, true);//Создаем контроллер анимации сердца.
        }
        GUI.DrawTexture(new Rect(Screen.width - Screen.height/2, 0, Screen.height/2, Screen.height), heartAC.CurrentFrame);//Рисуем сердце.
        GUI.DrawTexture(new Rect(Screen.width - 660, Screen.height-110, 650, 100), mainSkin.customStyles[2].normal.background);
        GUI.Label(new Rect(Screen.width - 180, Screen.height - 130, 180, 30), gameManager.loadingOperationName);//Отображаем информацию об операции.
    }
}
