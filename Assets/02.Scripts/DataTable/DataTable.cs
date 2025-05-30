// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class DataTable
{
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
    #region Time
    private ReadOnlyList<TimeData> TimeList = null;
    private ReadOnlyDictionary<int, TimeData> TimeTable = null;

    public ReadOnlyList<TimeData> GetTimeDataList()
    {
        return TimeList;
    }

    public TimeData GetTimeData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (TimeTable.TryGetValue(key, out TimeData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of TimeData: <{key}>");
            return null;
        }
    }
    #endregion
    #region Stage
    private ReadOnlyList<StageData> StageList = null;
    private ReadOnlyDictionary<int, StageData> StageTable = null;

    public ReadOnlyList<StageData> GetStageDataList()
    {
        return StageList;
    }

    public StageData GetStageData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (StageTable.TryGetValue(key, out StageData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of StageData: <{key}>");
            return null;
        }
    }
    #endregion
    #region DropTable
    private ReadOnlyList<DropTableData> DropTableList = null;
    private ReadOnlyDictionary<int, DropTableData> DropTableTable = null;

    public ReadOnlyList<DropTableData> GetDropTableDataList()
    {
        return DropTableList;
    }

    public DropTableData GetDropTableData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (DropTableTable.TryGetValue(key, out DropTableData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of DropTableData: <{key}>");
            return null;
        }
    }
    #endregion
    #region Currency
    private ReadOnlyList<CurrencyData> CurrencyList = null;
    private ReadOnlyDictionary<int, CurrencyData> CurrencyTable = null;

    public ReadOnlyList<CurrencyData> GetCurrencyDataList()
    {
        return CurrencyList;
    }

    public CurrencyData GetCurrencyData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (CurrencyTable.TryGetValue(key, out CurrencyData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of CurrencyData: <{key}>");
            return null;
        }
    }
    #endregion
    #region ShopTable
    private ReadOnlyList<ShopTableData> ShopTableList = null;
    private ReadOnlyDictionary<int, ShopTableData> ShopTableTable = null;

    public ReadOnlyList<ShopTableData> GetShopTableDataList()
    {
        return ShopTableList;
    }

    public ShopTableData GetShopTableData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (ShopTableTable.TryGetValue(key, out ShopTableData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of ShopTableData: <{key}>");
            return null;
        }
    }
    #endregion
    #region PlayerExperience
    private ReadOnlyList<PlayerExperienceData> PlayerExperienceList = null;
    private ReadOnlyDictionary<int, PlayerExperienceData> PlayerExperienceTable = null;

    public ReadOnlyList<PlayerExperienceData> GetPlayerExperienceDataList()
    {
        return PlayerExperienceList;
    }

    public PlayerExperienceData GetPlayerExperienceData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (PlayerExperienceTable.TryGetValue(key, out PlayerExperienceData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of PlayerExperienceData: <{key}>");
            return null;
        }
    }
    #endregion
    #region PlayerStat
    private ReadOnlyList<PlayerStatData> PlayerStatList = null;
    private ReadOnlyDictionary<int, PlayerStatData> PlayerStatTable = null;

    public ReadOnlyList<PlayerStatData> GetPlayerStatDataList()
    {
        return PlayerStatList;
    }

    public PlayerStatData GetPlayerStatData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (PlayerStatTable.TryGetValue(key, out PlayerStatData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of PlayerStatData: <{key}>");
            return null;
        }
    }
    #endregion
    #region PlayerSkill
    private ReadOnlyList<PlayerSkillData> PlayerSkillList = null;
    private ReadOnlyDictionary<int, PlayerSkillData> PlayerSkillTable = null;

    public ReadOnlyList<PlayerSkillData> GetPlayerSkillDataList()
    {
        return PlayerSkillList;
    }

    public PlayerSkillData GetPlayerSkillData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (PlayerSkillTable.TryGetValue(key, out PlayerSkillData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of PlayerSkillData: <{key}>");
            return null;
        }
    }
    #endregion
    #region PlayerSkillUpgrade
    private ReadOnlyList<PlayerSkillUpgradeData> PlayerSkillUpgradeList = null;
    private ReadOnlyDictionary<int, PlayerSkillUpgradeData> PlayerSkillUpgradeTable = null;

    public ReadOnlyList<PlayerSkillUpgradeData> GetPlayerSkillUpgradeDataList()
    {
        return PlayerSkillUpgradeList;
    }

    public PlayerSkillUpgradeData GetPlayerSkillUpgradeData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (PlayerSkillUpgradeTable.TryGetValue(key, out PlayerSkillUpgradeData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of PlayerSkillUpgradeData: <{key}>");
            return null;
        }
    }
    #endregion

    public IEnumerator LoadRoutine()
    {
        int allCount = 0;
        int loadedCount = 0;

        allCount++;
        GetBytes_FromResources("Rune", (bytes) =>
        {
            LoadRuneData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("Time", (bytes) =>
        {
            LoadTimeData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("Stage", (bytes) =>
        {
            LoadStageData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("DropTable", (bytes) =>
        {
            LoadDropTableData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("Currency", (bytes) =>
        {
            LoadCurrencyData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("ShopTable", (bytes) =>
        {
            LoadShopTableData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("PlayerExperience", (bytes) =>
        {
            LoadPlayerExperienceData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("PlayerStat", (bytes) =>
        {
            LoadPlayerStatData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("PlayerSkill", (bytes) =>
        {
            LoadPlayerSkillData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("PlayerSkillUpgrade", (bytes) =>
        {
            LoadPlayerSkillUpgradeData(bytes);
            loadedCount++;
        });

        yield return new WaitUntil(() => allCount == loadedCount);
    }

    public void LoadForEditor()
    {
        byte[] runeBytes = GetBytes_ForEditor("RuneData");
        LoadRuneData(runeBytes);
        byte[] timeBytes = GetBytes_ForEditor("TimeData");
        LoadTimeData(timeBytes);
        byte[] stageBytes = GetBytes_ForEditor("StageData");
        LoadStageData(stageBytes);
        byte[] dropTableBytes = GetBytes_ForEditor("DropTableData");
        LoadDropTableData(dropTableBytes);
        byte[] currencyBytes = GetBytes_ForEditor("CurrencyData");
        LoadCurrencyData(currencyBytes);
        byte[] shopTableBytes = GetBytes_ForEditor("ShopTableData");
        LoadShopTableData(shopTableBytes);
        byte[] playerExperienceBytes = GetBytes_ForEditor("PlayerExperienceData");
        LoadPlayerExperienceData(playerExperienceBytes);
        byte[] playerStatBytes = GetBytes_ForEditor("PlayerStatData");
        LoadPlayerStatData(playerStatBytes);
        byte[] playerSkillBytes = GetBytes_ForEditor("PlayerSkillData");
        LoadPlayerSkillData(playerSkillBytes);
        byte[] playerSkillUpgradeBytes = GetBytes_ForEditor("PlayerSkillUpgradeData");
        LoadPlayerSkillUpgradeData(playerSkillUpgradeBytes);
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

    private void LoadTimeData(byte[] bytes)
    {
        List<TimeData> timeList = new List<TimeData>();
        Dictionary<int, TimeData> timeTable = new Dictionary<int, TimeData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            TimeData data = new TimeData(Reader);
            if (timeTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Time");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Time");
                continue;
            }

            timeList.Add(data);
            timeTable.Add(data.TID, data);
        }

        Reader.Close();

        TimeList = new ReadOnlyList<TimeData>(timeList);
        TimeTable = new ReadOnlyDictionary<int, TimeData>(timeTable);
    }

    private void LoadStageData(byte[] bytes)
    {
        List<StageData> stageList = new List<StageData>();
        Dictionary<int, StageData> stageTable = new Dictionary<int, StageData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            StageData data = new StageData(Reader);
            if (stageTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Stage");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Stage");
                continue;
            }

            stageList.Add(data);
            stageTable.Add(data.TID, data);
        }

        Reader.Close();

        StageList = new ReadOnlyList<StageData>(stageList);
        StageTable = new ReadOnlyDictionary<int, StageData>(stageTable);
    }

    private void LoadDropTableData(byte[] bytes)
    {
        List<DropTableData> dropTableList = new List<DropTableData>();
        Dictionary<int, DropTableData> dropTableTable = new Dictionary<int, DropTableData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            DropTableData data = new DropTableData(Reader);
            if (dropTableTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in DropTable");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in DropTable");
                continue;
            }

            dropTableList.Add(data);
            dropTableTable.Add(data.TID, data);
        }

        Reader.Close();

        DropTableList = new ReadOnlyList<DropTableData>(dropTableList);
        DropTableTable = new ReadOnlyDictionary<int, DropTableData>(dropTableTable);
    }

    private void LoadCurrencyData(byte[] bytes)
    {
        List<CurrencyData> currencyList = new List<CurrencyData>();
        Dictionary<int, CurrencyData> currencyTable = new Dictionary<int, CurrencyData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            CurrencyData data = new CurrencyData(Reader);
            if (currencyTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Currency");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Currency");
                continue;
            }

            currencyList.Add(data);
            currencyTable.Add(data.TID, data);
        }

        Reader.Close();

        CurrencyList = new ReadOnlyList<CurrencyData>(currencyList);
        CurrencyTable = new ReadOnlyDictionary<int, CurrencyData>(currencyTable);
    }

    private void LoadShopTableData(byte[] bytes)
    {
        List<ShopTableData> shopTableList = new List<ShopTableData>();
        Dictionary<int, ShopTableData> shopTableTable = new Dictionary<int, ShopTableData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            ShopTableData data = new ShopTableData(Reader);
            if (shopTableTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in ShopTable");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in ShopTable");
                continue;
            }

            shopTableList.Add(data);
            shopTableTable.Add(data.TID, data);
        }

        Reader.Close();

        ShopTableList = new ReadOnlyList<ShopTableData>(shopTableList);
        ShopTableTable = new ReadOnlyDictionary<int, ShopTableData>(shopTableTable);
    }

    private void LoadPlayerExperienceData(byte[] bytes)
    {
        List<PlayerExperienceData> playerExperienceList = new List<PlayerExperienceData>();
        Dictionary<int, PlayerExperienceData> playerExperienceTable = new Dictionary<int, PlayerExperienceData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            PlayerExperienceData data = new PlayerExperienceData(Reader);
            if (playerExperienceTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in PlayerExperience");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in PlayerExperience");
                continue;
            }

            playerExperienceList.Add(data);
            playerExperienceTable.Add(data.TID, data);
        }

        Reader.Close();

        PlayerExperienceList = new ReadOnlyList<PlayerExperienceData>(playerExperienceList);
        PlayerExperienceTable = new ReadOnlyDictionary<int, PlayerExperienceData>(playerExperienceTable);
    }

    private void LoadPlayerStatData(byte[] bytes)
    {
        List<PlayerStatData> playerStatList = new List<PlayerStatData>();
        Dictionary<int, PlayerStatData> playerStatTable = new Dictionary<int, PlayerStatData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            PlayerStatData data = new PlayerStatData(Reader);
            if (playerStatTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in PlayerStat");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in PlayerStat");
                continue;
            }

            playerStatList.Add(data);
            playerStatTable.Add(data.TID, data);
        }

        Reader.Close();

        PlayerStatList = new ReadOnlyList<PlayerStatData>(playerStatList);
        PlayerStatTable = new ReadOnlyDictionary<int, PlayerStatData>(playerStatTable);
    }

    private void LoadPlayerSkillData(byte[] bytes)
    {
        List<PlayerSkillData> playerSkillList = new List<PlayerSkillData>();
        Dictionary<int, PlayerSkillData> playerSkillTable = new Dictionary<int, PlayerSkillData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            PlayerSkillData data = new PlayerSkillData(Reader);
            if (playerSkillTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in PlayerSkill");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in PlayerSkill");
                continue;
            }

            playerSkillList.Add(data);
            playerSkillTable.Add(data.TID, data);
        }

        Reader.Close();

        PlayerSkillList = new ReadOnlyList<PlayerSkillData>(playerSkillList);
        PlayerSkillTable = new ReadOnlyDictionary<int, PlayerSkillData>(playerSkillTable);
    }

    private void LoadPlayerSkillUpgradeData(byte[] bytes)
    {
        List<PlayerSkillUpgradeData> playerSkillUpgradeList = new List<PlayerSkillUpgradeData>();
        Dictionary<int, PlayerSkillUpgradeData> playerSkillUpgradeTable = new Dictionary<int, PlayerSkillUpgradeData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            PlayerSkillUpgradeData data = new PlayerSkillUpgradeData(Reader);
            if (playerSkillUpgradeTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in PlayerSkillUpgrade");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in PlayerSkillUpgrade");
                continue;
            }

            playerSkillUpgradeList.Add(data);
            playerSkillUpgradeTable.Add(data.TID, data);
        }

        Reader.Close();

        PlayerSkillUpgradeList = new ReadOnlyList<PlayerSkillUpgradeData>(playerSkillUpgradeList);
        PlayerSkillUpgradeTable = new ReadOnlyDictionary<int, PlayerSkillUpgradeData>(playerSkillUpgradeTable);
    }

}
