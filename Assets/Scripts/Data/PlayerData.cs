using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// binaryformatter를 하기위해서 namespace 추가
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class PlayerData : MonoBehaviour
{

    #region Singleton
    static PlayerData _instance;

    public static PlayerData instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject newObj = new GameObject("Player Data");
                _instance = newObj.AddComponent<PlayerData>();
            }
            return _instance;
        }
    }
    #endregion


    public List<string> playerUnitList { get; private set; }
    public List<string> selectedUnitList { get; private set; }
    public Dictionary<string, int> upgradePoint { get; private set; }
    public Dictionary<string, int> itemStorage { get; private set; }


    public int obsidian { get; set; }
    public string lastClearedStage { get; set; }     // 마지막으로 깬 스테이지( 스테이지 클리어시, 조건검사를 통해 finalStage에 값 수정 )
    public string selectedStage { get; set; }  // StageSelect 씬에서 골라줌
    public bool isFirst { get; private set; }
    public int stageSelectPosIndex { get; set; }
    public bool appRated { get; set; }
    public void CheckInstance()
    {

    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 1
        playerUnitList = new List<string>();
        selectedUnitList = new List<string>();
        upgradePoint = new Dictionary<string, int>();
        itemStorage = new Dictionary<string, int>();

        // 2
        LoadData();
        // 3
        if (isFirst) // 제일 처음일때
        {
            AddUnit("Skeleton");
            obsidian = 100; //Temp

        }

        selectedStage = "C0S1"; // Temp
    }


    private void LoadData()
    {
        var data = PlayerPrefs.GetString("playerUnitList");

        if (!PlayerPrefs.HasKey("isFirst")) // 유닛 데이터가없을경우
        {
            isFirst = true;
            stageSelectPosIndex = 0;
            upgradePoint.Add("Hp", 0);
            upgradePoint.Add("Damage", 0);
            upgradePoint.Add("Cool Time", 0);

            itemStorage.Add("Fix", 0);
            itemStorage.Add("Freeze", 0);
            itemStorage.Add("Defense", 0);
            return;
        }

        // 처음이 아닌경우

        if (PlayerPrefs.HasKey("stageSelectPosIndex"))
            stageSelectPosIndex = PlayerPrefs.GetInt("stageSelectPosIndex");

        upgradePoint.Add("Hp", PlayerPrefs.GetInt("Hp"));
        upgradePoint.Add("Damage", PlayerPrefs.GetInt("Damage"));
        upgradePoint.Add("Cool Time", PlayerPrefs.GetInt("Cool Time"));

        itemStorage.Add("Fix", PlayerPrefs.GetInt("Fix"));
        itemStorage.Add("Freeze", PlayerPrefs.GetInt("Freeze"));
        itemStorage.Add("Defense", PlayerPrefs.GetInt("Defense"));

        if (PlayerPrefs.HasKey("lastClearedStage"))
            lastClearedStage = PlayerPrefs.GetString("lastClearedStage");

        isFirst = PlayerPrefs.GetInt("isFirst") == 0 ? true : false;
        appRated = PlayerPrefs.GetInt("appRated") == 1 ? true : false;
        obsidian = PlayerPrefs.GetInt("obsidian");

        // Deserialize
        var b = new BinaryFormatter();
        using (var m = new MemoryStream(Convert.FromBase64String(data)))
        {
            playerUnitList = (List<string>)b.Deserialize(m);
        }
    }

    public void OnApplicationQuit()
    {
        SaveData();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveData();
    }

    public void SaveData()
    {
        if (lastClearedStage != null)
            PlayerPrefs.SetString("lastClearedStage", lastClearedStage);

        PlayerPrefs.SetInt("isFirst", 1);
        PlayerPrefs.SetInt("appRated", appRated ? 1 : 0);
        PlayerPrefs.SetInt("obsidian", obsidian);

        PlayerPrefs.SetInt("stageSelectPosIndex", stageSelectPosIndex);

        PlayerPrefs.SetInt("Hp", upgradePoint["Hp"]);
        PlayerPrefs.SetInt("Damage", upgradePoint["Damage"]);
        PlayerPrefs.SetInt("Cool Time", upgradePoint["Cool Time"]);

        PlayerPrefs.SetInt("Fix", itemStorage["Fix"]);
        PlayerPrefs.SetInt("Freeze", itemStorage["Freeze"]);
        PlayerPrefs.SetInt("Defense", itemStorage["Defense"]);

        // Deserialize - Serialize
        var b = new BinaryFormatter();
        using (var m = new MemoryStream())
        {
            b.Serialize(m, playerUnitList);
            PlayerPrefs.SetString("playerUnitList", Convert.ToBase64String(m.GetBuffer()));
        }

        PlayerPrefs.Save();

    }

    public void StageClear(string stage)
    {

        obsidian += 30;
        int c = int.Parse(stage[1].ToString());
        int s = int.Parse(stage[3].ToString());
        CheckAddUnit(c, s);

        if (lastClearedStage == null)
        {
            lastClearedStage = stage;
            return;
        }

        int LastC = int.Parse(lastClearedStage[1].ToString());
        int LastS = int.Parse(lastClearedStage[3].ToString());


        if (c > LastC)
        {
            lastClearedStage = stage;
        }
        else if (c == LastC)
        {
            if (s > LastS)
            {
                lastClearedStage = stage;
            }
            else
                return;
        }
        else
            return;

    }

    void CheckAddUnit(int c, int s)
    {
        if (c == 0 && s == 1)
            AddUnit("Goblin");

        if (c == 0 && s == 2)
            AddUnit("Orc");

        if (c == 1 && s == 3)
        {
            AddUnit("Harpy");
            AddUnit("Dullahan");
        }
        if (c == 2 && s == 3)
            AddUnit("Grim");
    }

    public bool IsStageCleared(string stage)
    {
        int c = int.Parse(stage[1].ToString());
        int s = int.Parse(stage[3].ToString());

        int LastC = int.Parse(lastClearedStage[1].ToString());
        int LastS = int.Parse(lastClearedStage[3].ToString());

        if (c > LastC)
            return true;
        else if (c == LastC)
            if (s < LastS)
                return true;
            else
                return false;
        else
            return false;
    }

    public int GetSelectedChapter()
    {
        return int.Parse(selectedStage[1].ToString());
    }

    public int GetClearedStageCount()
    {
        if (lastClearedStage == null)
            return 0;

        int lastChapter = int.Parse(lastClearedStage[1].ToString());
        int lastStage = int.Parse(lastClearedStage[3].ToString());

        //C0S1 부터 시작

        return lastChapter * 3 + lastStage;
    }

    // 하나도 깬 챕터가 없을 경우 999 리턴
    public int GetClearedLastChapter()
    {
        if (lastClearedStage == null)
            return 999;
        int lastChapter = int.Parse(lastClearedStage[1].ToString());
        int lastStage = int.Parse(lastClearedStage[3].ToString());
        if (lastStage == 3)
        {
            return lastChapter;
        }
        else
        {
            if (lastChapter == 0)
                return 999;
            else
                return lastChapter - 1;
        }
    }

    public static string GetNextStageName(string stageName)
    {
        int chapter = int.Parse(stageName[1].ToString());
        int stage = int.Parse(stageName[3].ToString());

        if (stageName == "C3S3")
            return "C3S3";

        if (stage == 3)
        {
            return "C" + (chapter + 1).ToString() + "S1";
        }
        else
        {
            return "C" + chapter.ToString() + "S" + (stage + 1).ToString();
        }
    }

    public bool PurchaseItem(string name, int cost)
    {
        if (obsidian - cost >= 0)
        {
            obsidian -= cost;
            itemStorage[name]++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseItem(string name)
    {
        itemStorage[name]--;
    }

    public void AddUnit(string name)
    {
        if (playerUnitList.Contains(name))
            return;
        else
            playerUnitList.Add(name);
    }
}
