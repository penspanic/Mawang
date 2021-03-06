﻿// binaryformatter를 하기위해서 namespace 추가
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    #region Singleton

    private static PlayerData _instance;

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

    #endregion Singleton

    public List<string> playerUnitList { get; private set; }
    public List<string> selectedUnitList { get; set; }
    public List<string> selectedItemList { get; set; }
    public Dictionary<string, int> upgradePoint { get; private set; }
    public Dictionary<string, int> itemStorage { get; private set; }

    public int obsidian { get; set; }
    public string lastClearedStage { get; set; }     // 마지막으로 깬 스테이지( 스테이지 클리어시, 조건검사를 통해 finalStage에 값 수정 )
    public string selectedStage { get; set; }  // StageSelect 씬에서 골라줌
    public bool isFirst { get; private set; }
    public bool appRated { get; set; }

    public void CheckInstance()
    {

    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 1
        playerUnitList = new List<string>();
        selectedUnitList = new List<string>();
        selectedItemList = new List<string>();
        upgradePoint = new Dictionary<string, int>();
        itemStorage = new Dictionary<string, int>();

        // 2
        LoadData();

        // 3
        if (isFirst) // 제일 처음일때
        {
            Init();
        }

        selectedStage = "C0S1"; // Temp
    }

    void Init()
    {
        AddUnit("Skeleton");
        obsidian = 100;
    }

    private void LoadData()
    {
        upgradePoint.Add("Hp", (int)GetValue<int>("Hp", 0));
        upgradePoint.Add("Damage", (int)GetValue<int>("Damage", 0));
        upgradePoint.Add("Cool Time", (int)GetValue<int>("Cool Time", 0));

        itemStorage.Add("Fix", (int)GetValue<int>("Fix", 0));
        itemStorage.Add("Freeze", (int)GetValue<int>("Freeze",0));
        itemStorage.Add("Defense", (int)GetValue<int>("Defense",0));
        itemStorage.Add("Gold Upgrade", (int)GetValue<int>("Gold Upgrade",0));
        itemStorage.Add("Time Scale Up", (int)GetValue<int>("Time Scale Up", 0));

        if (!PlayerPrefs.HasKey("isFirst")) // 유닛 데이터가없을경우
        {
            isFirst = true;
            return;
        }

        // 처음이 아닌경우

        lastClearedStage = (string)GetValue<string>("lastClearedStage", null);

        isFirst = PlayerPrefs.GetInt("isFirst") == 0 ? true : false;
        appRated = PlayerPrefs.GetInt("appRated") == 1 ? true : false;
        obsidian = PlayerPrefs.GetInt("obsidian");

        var data = PlayerPrefs.GetString("playerUnitList");
        // Deserialize
        var b = new BinaryFormatter();
        using (var m = new MemoryStream(Convert.FromBase64String(data)))
        {
            playerUnitList = (List<string>)b.Deserialize(m);
        }

        data = PlayerPrefs.GetString("selectedUnitList");
        if (data != null && data.Length != 0)
        {
            using (var m = new MemoryStream(Convert.FromBase64String(data)))
            {
                selectedUnitList = (List<string>)b.Deserialize(m);
            }
        }

        data = PlayerPrefs.GetString("selectedItemList");
        if(data != null && data.Length != 0)
        {
            using (var m = new MemoryStream(Convert.FromBase64String(data)))
            {
                selectedItemList = (List<string>)b.Deserialize(m);
            }
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
        #region Stage All Clear

        //for (int c = 0; c < 8; c++)
        //{
        //    for (int s = 1; s < 5; s++)
        //    {
        //        if (c < 4 && s == 4)
        //            continue;
        //        StageClear("C" + c.ToString() + "S" + s.ToString());
        //    }
        //}

        //AddUnit("Goblin");
        //AddUnit("Dullahan");
        //AddUnit("Harpy");
        //AddUnit("Orc");
        //AddUnit("Grim");
        //AddUnit("Werewolf");
        //AddUnit("Marionette");
        //AddUnit("Aragog");
        //AddUnit("Witch");

        #endregion

        if (lastClearedStage != null)
            PlayerPrefs.SetString("lastClearedStage", lastClearedStage);

        PlayerPrefs.SetInt("isFirst", 1);
        PlayerPrefs.SetInt("appRated", appRated ? 1 : 0);
        PlayerPrefs.SetInt("obsidian", obsidian);

        PlayerPrefs.SetInt("Hp", upgradePoint["Hp"]);
        PlayerPrefs.SetInt("Damage", upgradePoint["Damage"]);
        PlayerPrefs.SetInt("Cool Time", upgradePoint["Cool Time"]);

        PlayerPrefs.SetInt("Fix", itemStorage["Fix"]);
        PlayerPrefs.SetInt("Freeze", itemStorage["Freeze"]);
        PlayerPrefs.SetInt("Defense", itemStorage["Defense"]);
        PlayerPrefs.SetInt("Gold Upgrade", itemStorage["Gold Upgrade"]);
        PlayerPrefs.SetInt("Time Scale Up", itemStorage["Time Scale Up"]);


        // Deserialize - Serialize
        var b = new BinaryFormatter();
        using (var m = new MemoryStream())
        {
            b.Serialize(m, playerUnitList);
            PlayerPrefs.SetString("playerUnitList", Convert.ToBase64String(m.GetBuffer()));
        }

        if (selectedUnitList != null && selectedUnitList.Count != 0) // 선택된 유닛 저장
        {
            using (var m = new MemoryStream())
            {
                b.Serialize(m, selectedUnitList);
                PlayerPrefs.SetString("selectedUnitList", Convert.ToBase64String(m.GetBuffer()));
            }
        }

        if (selectedItemList != null && selectedItemList.Count != 0) // 선택된 아이템 저장
        {
            using (var m = new MemoryStream())
            {
                b.Serialize(m, selectedItemList);
                PlayerPrefs.SetString("selectedItemList", Convert.ToBase64String(m.GetBuffer()));
            }
        }

        PlayerPrefs.Save();
    }

    public void StageClear(string stage)
    {
        obsidian += 30;
        int c = int.Parse(stage[1].ToString());
        int s = int.Parse(stage[3].ToString());
        CheckAddUnit(c, s);

        if (lastClearedStage == null || lastClearedStage == "")
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

    private void CheckAddUnit(int c, int s)
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

        if (c == 4 && s == 1)
            AddUnit("Werewolf");

        if (c == 4 && s == 3)
            AddUnit("Marionette");

        if (c == 5 && s == 1)
            AddUnit("Aragog");

        if (c == 5 && s == 3)
            AddUnit("Witch");
    }

    public bool IsStageCleared(string stage)
    {
        int c = int.Parse(stage[1].ToString());
        int s = int.Parse(stage[3].ToString());

        Debug.Log("Last Stagee : " + lastClearedStage);
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

        if (lastChapter < 4 && lastStage == 3 || lastChapter >= 4 && lastStage == 4)
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

        if (stageName == "C7S4")
            return "C7S4";

        if (stage == 3)
        {
            return "C" + (chapter + 1).ToString() + "S1";
        }
        else
        {
            return "C" + chapter.ToString() + "S" + (stage + 1).ToString();
        }
    }

    public string GetInGameSceneName()
    {
        if (GetSelectedChapter() < 4)
            return "World1";
        else
            return "World2";
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

    #region Util

    public object GetValue<T>(string key, T defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            if (typeof(T) == typeof(string))
            {
                return PlayerPrefs.GetString(key);
            }
            else if (typeof(T) == typeof(int))
            {
                return PlayerPrefs.GetInt(key);
            }
            else if (typeof(T) == typeof(float))
            {
                return PlayerPrefs.GetFloat(key);
            }
            else
            {
                throw new ArgumentException();
            }
        }
        else
            return defaultValue;
    }

    #endregion
}