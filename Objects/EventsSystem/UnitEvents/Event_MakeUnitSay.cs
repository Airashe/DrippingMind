using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_MakeUnitSay : Object_Event
{
    public int unitUniqueId = -1;//id юнита, который должен произнести фразу.
    public Source_UnitQuote quote;//Какая фраза будет сказана.

    public override void Initialize()
    {
        base.Initialize();
        Source_GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Ссылка на гейм менеджер.

        Object_Unit unit = gameManager.GetUnitDataByUniqueId(unitUniqueId);//Данные искомого юнита.
        if (unit != null && quote)//Если юнит был найден.
        {
            PI_IngameGUI playerIngameGUI = Camera.main.GetComponent<PI_IngameGUI>();//Ссылка на контроллер внутриигрового интерфейса.

            unit.Say(quote);//Отправляем запрос на произнесение фразы.
            playerIngameGUI.AddSayTrackForUnit(unit);//Добавляем юнит в список слежки для интерфейса.
        }
    }
}
