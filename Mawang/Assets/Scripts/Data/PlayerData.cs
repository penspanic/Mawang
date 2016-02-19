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


    public int obsidian { get; private set; }
    public string lastClearedStage { get; set; }     // 마지막으로 깬 스테이지( 스테이지 클리어시, 조건검사를 통해 finalStage에 값 수정 )
    public string selectedStage { get; set; }  // StageSelect 씬에서 골라줌

    public bool isFirst { get; private set; }

    public void CheckInstance()
    {

    }

    void Awake()
    {

        DontDestroyOnLoad(gameObject);
        Debug.Log("First Data Init");

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
            Debug.Log("First, Skeleton Added");
            AddUnit("Skeleton");

        }

        selectedStage = "C0S1"; // Temp
        lastClearedStage = "C2S3";
        obsidian = 100; //Temp
    }


    private void LoadData()
    {
        var data = PlayerPrefs.GetString("playerUnitList");

        if (!DataLoadSave.HasKey("isFirst")) // 유닛 데이터가없을경우
        {
            Debug.Log("First");
            isFirst = true;

            upgradePoint.Add("Hp", 0);
            upgradePoint.Add("Damage", 0);
            upgradePoint.Add("Cool Time", 0);

            itemStorage.Add("Fix", 0);
            itemStorage.Add("Freeze", 0);
            itemStorage.Add("Defense", 0);
            return;
        }

        // 처음이 아닌경우

        upgradePoint.Add("Hp", DataLoadSave.GetInt("Hp"));
        upgradePoint.Add("Damage", DataLoadSave.GetInt("Damage"));
        upgradePoint.Add("Cool Time", DataLoadSave.GetInt("Cool Time"));

        itemStorage.Add("Fix", DataLoadSave.GetInt("Fix"));
        itemStorage.Add("Freeze", DataLoadSave.GetInt("Freeze"));
        itemStorage.Add("Defense", DataLoadSave.GetInt("Defense"));

        
        lastClearedStage = DataLoadSave.GetString("lastClearedStage");
        isFirst = DataLoadSave.GetInt("isFirst") == 0 ? true : false;
        obsidian = DataLoadSave.GetInt("obsidian");

        // Deserialize
        var b = new BinaryFormatter();
        var m = new MemoryStream(Convert.FromBase64String(data));

        playerUnitList = (List<string>)b.Deserialize(m);

        data = PlayerPrefs.GetString("satan");
        m = new MemoryStream(Convert.FromBase64String(data));

    }

    public void OnApplicationQuit()
    {
        Debug.Log("Save Data");
        SaveData();
    }


    public void SaveData()
    {

        DataLoadSave.SetString("lastClearedStage", lastClearedStage);
        DataLoadSave.SetInt("isFirst", 1);
        DataLoadSave.SetInt("obsidian", obsidian);

        DataLoadSave.SetInt("Hp", upgradePoint["Hp"]);
        DataLoadSave.SetInt("Damage", upgradePoint["Damage"]);
        DataLoadSave.SetInt("Cool Time", upgradePoint["Cool Time"]);

        DataLoadSave.SetInt("Fix", itemStorage["Fix"]);
        DataLoadSave.SetInt("Freeze", itemStorage["Freeze"]);
        DataLoadSave.SetInt("Defense", itemStorage["Defense"]);

        // Deserialize - Serialize
        var b = new BinaryFormatter();
        var m = new MemoryStream();
        b.Serialize(m, playerUnitList);
        PlayerPrefs.SetString("playerUnitList", Convert.ToBase64String(m.GetBuffer()));


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

    public void AddUnit(string name)
    {
        if (playerUnitList.Contains(name))
            return;
        else
            playerUnitList.Add(name);
    }
}