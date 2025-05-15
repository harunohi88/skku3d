// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class DataTable
{
    #region Stat
    private ReadOnlyList<StatData> StatList = null;
    private ReadOnlyDictionary<int, StatData> StatTable = null;

    public ReadOnlyList<StatData> GetStatDataList()
    {
        return StatList;
    }

    public StatData GetStatData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (StatTable.TryGetValue(key, out StatData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of StatData: <{key}>");
            return null;
        }
    }
    #endregion
    #region Rune
    private ReadOnlyList<RuneData> RuneList = null;
    private ReadOnlyDictionary<int, RuneData> RuneTable = null;

    public ReadOnlyList<RuneData> GetRuneDataList()
    {
        return RuneList;
    }

    public RuneData GetRuneData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (RuneTable.TryGetValue(key, out RuneData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of RuneData: <{key}>");
            return null;
        }
    }
    #endregion
    #region FireLight
    private ReadOnlyList<FireLightData> FireLightList = null;
    private ReadOnlyDictionary<int, FireLightData> FireLightTable = null;

    public ReadOnlyList<FireLightData> GetFireLightDataList()
    {
        return FireLightList;
    }

    public FireLightData GetFireLightData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (FireLightTable.TryGetValue(key, out FireLightData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of FireLightData: <{key}>");
            return null;
        }
    }
    #endregion
    #region FireUnit
    private ReadOnlyList<FireUnitData> FireUnitList = null;
    private ReadOnlyDictionary<int, FireUnitData> FireUnitTable = null;

    public ReadOnlyList<FireUnitData> GetFireUnitDataList()
    {
        return FireUnitList;
    }

    public FireUnitData GetFireUnitData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (FireUnitTable.TryGetValue(key, out FireUnitData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of FireUnitData: <{key}>");
            return null;
        }
    }
    #endregion

    public IEnumerator LoadRoutine()
    {
        int allCount = 0;
        int loadedCount = 0;

        allCount++;
        GetBytes_FromResources("Stat", (bytes) =>
        {
            LoadStatData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("Rune", (bytes) =>
        {
            LoadRuneData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("FireLight", (bytes) =>
        {
            LoadFireLightData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("FireUnit", (bytes) =>
        {
            LoadFireUnitData(bytes);
            loadedCount++;
        });

        yield return new WaitUntil(() => allCount == loadedCount);
    }

    public void LoadForEditor()
    {
        byte[] statBytes = GetBytes_ForEditor("StatData");
        LoadStatData(statBytes);
        byte[] runeBytes = GetBytes_ForEditor("RuneData");
        LoadRuneData(runeBytes);
        byte[] fireLightBytes = GetBytes_ForEditor("FireLightData");
        LoadFireLightData(fireLightBytes);
        byte[] fireUnitBytes = GetBytes_ForEditor("FireUnitData");
        LoadFireUnitData(fireUnitBytes);
    }

    private void LoadStatData(byte[] bytes)
    {
        List<StatData> statList = new List<StatData>();
        Dictionary<int, StatData> statTable = new Dictionary<int, StatData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            StatData data = new StatData(Reader);
            if (statTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Stat");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Stat");
                continue;
            }

            statList.Add(data);
            statTable.Add(data.TID, data);
        }

        Reader.Close();

        StatList = new ReadOnlyList<StatData>(statList);
        StatTable = new ReadOnlyDictionary<int, StatData>(statTable);
    }

    private void LoadRuneData(byte[] bytes)
    {
        List<RuneData> runeList = new List<RuneData>();
        Dictionary<int, RuneData> runeTable = new Dictionary<int, RuneData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            RuneData data = new RuneData(Reader);
            if (runeTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Rune");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Rune");
                continue;
            }

            runeList.Add(data);
            runeTable.Add(data.TID, data);
        }

        Reader.Close();

        RuneList = new ReadOnlyList<RuneData>(runeList);
        RuneTable = new ReadOnlyDictionary<int, RuneData>(runeTable);
    }

    private void LoadFireLightData(byte[] bytes)
    {
        List<FireLightData> fireLightList = new List<FireLightData>();
        Dictionary<int, FireLightData> fireLightTable = new Dictionary<int, FireLightData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            FireLightData data = new FireLightData(Reader);
            if (fireLightTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in FireLight");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in FireLight");
                continue;
            }

            fireLightList.Add(data);
            fireLightTable.Add(data.TID, data);
        }

        Reader.Close();

        FireLightList = new ReadOnlyList<FireLightData>(fireLightList);
        FireLightTable = new ReadOnlyDictionary<int, FireLightData>(fireLightTable);
    }

    private void LoadFireUnitData(byte[] bytes)
    {
        List<FireUnitData> fireUnitList = new List<FireUnitData>();
        Dictionary<int, FireUnitData> fireUnitTable = new Dictionary<int, FireUnitData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            FireUnitData data = new FireUnitData(Reader);
            if (fireUnitTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in FireUnit");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in FireUnit");
                continue;
            }

            fireUnitList.Add(data);
            fireUnitTable.Add(data.TID, data);
        }

        Reader.Close();

        FireUnitList = new ReadOnlyList<FireUnitData>(fireUnitList);
        FireUnitTable = new ReadOnlyDictionary<int, FireUnitData>(fireUnitTable);
    }

}
