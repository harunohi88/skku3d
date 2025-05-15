﻿using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;

public partial class DataExporterEditor
{
    // WAK_ZA_DataTable 파일의 시트 ID. (주소창에 있음)
    private const string TABLE_SHEET_ID = "1uD8o5tl-Bg4YBNrxLhyChQ3IDPxKL41L6Z4kmDUdqlg";

    // WAK_ZA_Enum 파일의 시트 ID. (주소창에 있음)
    private const string ENUM_SHEET_ID = "1DZ51ehCDh07HNuT38TInhEAtgNv0UjwM6yVgMQuU-RE";

    // 클라이언트 Output 경로.
    private const string CLIENT_SCRIPT = "Assets/02.Scripts/DataTable";
    private const string CLIENT_BINATY = "Assets/Resources/DataTable";
    private const string CLIENT_HISTORY = "Assets/Editor Default Resources/DataTable";

#if UNITY_EDITOR_OSX
    private const string NEW_LINE = "\n";
#elif UNITY_EDITOR_WIN
    private const string NEW_LINE = "\r\n";
#endif

    // 테이블 정보.
    private List<TableInfo> TableInfos = new List<TableInfo>();

    // Enum 정보.
    private List<EnumInfo> EnumInfos = new List<EnumInfo>();

    private string ServerPath
    {
        get
        {
            return EditorPrefs.GetString("DATA_EXPORTER_SERVER_PATH");
        }
        set
        {
            EditorPrefs.SetString("DATA_EXPORTER_SERVER_PATH", value);
        }
    }

    private string AppointWord
    {
        get
        {
            return EditorPrefs.GetString("DATA_EXPORTER_APPOINT_WORD");
        }
        set
        {
            EditorPrefs.SetString("DATA_EXPORTER_APPOINT_WORD", value);
        }
    }

    private bool IsApplyClient
    {
        get
        {
            return EditorPrefs.GetBool("DATA_EXPORTER_APPLY_CLIENT");
        }
        set
        {
            EditorPrefs.SetBool("DATA_EXPORTER_APPLY_CLIENT", value);
        }
    }

    private bool IsApplyServer
    {
        get
        {
            return EditorPrefs.GetBool("DATA_EXPORTER_APPLY_SERVER");
        }
        set
        {
            EditorPrefs.SetBool("DATA_EXPORTER_APPLY_SERVER", value);
        }
    }

    private bool IsBinaryOnly
    {
        get
        {
            return EditorPrefs.GetBool("DATA_EXPORTER_BINARY_ONLY");
        }
        set
        {
            EditorPrefs.SetBool("DATA_EXPORTER_BINARY_ONLY", value);
        }
    }

    private bool IsVertical(string tableName)
    {
        return tableName.Equals("Constant");
    }

    private void Import()
    {
        var service = SheetConnect.Connect("ExporterEditor");

        var requestTable = service.Spreadsheets.Get(TABLE_SHEET_ID).Execute();
        TableInfos.Clear();
        foreach (Sheet sheet in requestTable.Sheets)
        {
            string sheetName = sheet.Properties.Title;
            string[] split = sheetName.Split('_');

            if (split.Length == 2 && split[1].Equals("X") == true)
            {
                continue;
            }

            if (split.Length == 2 && split[1].Equals("C") == false && split[1].Equals("S") == false && split[1].Equals(AppointWord) == false)
            {
                continue;
            }

            TableInfo tableInfo = CreateInstance<TableInfo>();
            tableInfo.TableName = split[0];
            tableInfo.SheetName = sheet.Properties.Title;
            tableInfo.IsUseAppointWord = split.Length == 2 && AppointWord.Length != 0 && split[1].Equals(AppointWord);
            TableInfos.Add(tableInfo);

            if (split.Length == 2 && split[1].Equals("C") == true)
            {
                tableInfo.IsUseServer = false;
            }
            if (split.Length == 2 && split[1].Equals("S") == true)
            {
                tableInfo.IsUseClient = false;
            }
        }
        RefreshTableNames();
        LogMessage($"[IMPORT] {requestTable.Sheets.Count}개의 table sheet가 존재합니다.");

        var requestEnum = service.Spreadsheets.Get(ENUM_SHEET_ID).Execute();
        EnumInfos.Clear();
        foreach (Sheet sheet in requestEnum.Sheets)
        {
            string sheetName = sheet.Properties.Title;
            string[] split = sheetName.Split('_');

            EnumInfo enumInfo = CreateInstance<EnumInfo>();
            enumInfo.EnumName = split[0];
            enumInfo.SheetName = sheet.Properties.Title;
            enumInfo.IsAddMax = split.Length == 2 && split[1].Equals("M") == true;
            EnumInfos.Add(enumInfo);
        }
        RefreshEnumNames();
        LogMessage($"[IMPORT] {requestEnum.Sheets.Count}개의 enum sheet가 존재합니다.");

        LogMessage("================================================", false);
    }

    private async void Export()
    {
        var service = SheetConnect.Connect("ExporterEditor");

        #region Enum 멤버 초기화
        for (int i = 0; i < EnumInfos.Count; ++i)
        {
            EnumInfos[i]?.EnumMembers.Clear();
        }
        #endregion

        #region 서버용 경로 확인
        // 서버용 데이터 매니저 생성.
        if (IsApplyServer == true)
        {
            if (Directory.Exists(ServerPath) == false)
            {
                LogErrorMessage($"[PHP] ({ServerPath}) 경로가 존재하지 않습니다.");
                return;
            }
            else
            {
                if (Directory.Exists($"{ServerPath}/Common") == false)
                {
                    Directory.CreateDirectory($"{ServerPath}/Common");
                }

                if (Directory.Exists($"{ServerPath}/DataTable") == false)
                {
                    Directory.CreateDirectory($"{ServerPath}/DataTable");
                }
            }
        }
        #endregion

        #region 매니저 파일 추출 (테이블 이름만 있어도 가능)
        // 바이너리만 추출시 매니저 파일은 생성하지 않음.
        if (IsBinaryOnly == false)
        {
            // 클라이언트용 데이터 매니저 생성.
            if (IsApplyClient == true)
            {
                ExportDataTableManager();
            }

            // 서버용 데이터 매니저 생성.
            if (IsApplyServer == true)
            {
                ExportDataTablePHP();
            }
        }
        #endregion

        #region 선택한 데이터만 로드
        for (int i = 0; i < TableInfos.Count; ++i)
        {
            if (TableInfos[i].IsApply == true)
            {
                SendRequestTable(service, TableInfos[i]);
                await Task.Delay(1);
            }
        }
        LogMessage($"[LOAD] 데이터 로드 완료");
        LogMessage("================================================", false);
        #endregion

        #region 데이터 스크립트 및 파일 추출
        for(int i = 0; i < EnumInfos.Count; ++i)
        {
            if(EnumInfos[i].EnumMembers.Count == 0)
            {
                SendRequestEnum(service, EnumInfos[i]);
            }
            RefreshEnumNames();
        }

        for (int i = 0; i < TableInfos.Count; ++i)
        {
            if (TableInfos[i].IsApply == true)
            {
                // #region Enum 로드 및 추출.
                // // 필요한 Enum만 로드.
                // for (int ii = 0; ii < TableInfos[i].Headers.Count; ++ii)
                // {
                //     Header header = TableInfos[i].Headers[ii];
                //     if (header.VariableType.StartsWith("e") == true)
                //     {
                //         EnumInfo enumInfo = EnumInfos.Find(x => x.EnumName.Equals(header.VariableType) == true);
                //         if (enumInfo == null)
                //         {
                //             LogErrorMessage($"[ENUM] {header.VariableType} 이름을 가진 Enum을 찾을 수 없습니다.");
                //             return;
                //         }

                //         // 이미 로드한 Enum일 경우 다시 로드하지 않음. (Enum 멤버 초기화는 Export 시도시마다 함)
                //         if (enumInfo.EnumMembers.Count == 0)
                //         {
                //             SendRequestEnum(service, enumInfo);
                //         }
                //     }
                // }
                // RefreshEnumNames();
                // #endregion

                if (IsApplyClient == true && TableInfos[i].IsUseClient == true)
                {
                    if (IsBinaryOnly == false)
                    {
                        ExportScript(TableInfos[i]);
                    }

                    if (IsVertical(TableInfos[i].TableName) == false)
                    {
                        ExportBinary_Horizontal(TableInfos[i]);
                    }
                    else
                    {
                        ExportBinary_Vertical(TableInfos[i]);
                    }
                }

                if (IsApplyServer == true && TableInfos[i].IsUseServer == true)
                {
                    ExportPHP(TableInfos[i]);
                }

                await Task.Delay(1);
            }
        }
        LogMessage("================================================", false);
        #endregion

        #region Enum 파일 추출
        if (IsApplyClient == true)
        {
            ExportEnumDef();
        }

        if (IsApplyServer == true)
        {
            ExportEnumDefPHP();
        }
        #endregion

        AssetDatabase.Refresh();
        LogMessage("================================================", false);
    }

    #region 로드

    private void SendRequestTable(SheetsService service, TableInfo tableInfo)
    {
        tableInfo.Table = null;
        tableInfo.Headers.Clear();

        try
        {
            var request = service.Spreadsheets.Values.Get(TABLE_SHEET_ID, tableInfo.SheetName + "!A1:CR");
            var requestValue = request.Execute().Values;


            if (IsVertical(tableInfo.TableName) == false)
            {
                ExporterEditor.SheetToDataTable(tableInfo, requestValue, true);
                SetHeaders_Horizontal(tableInfo);
            }
            else
            {
                ExporterEditor.SheetToDataTable(tableInfo, requestValue, false);
                SetHeaders_Vertical(tableInfo);
            }
            LogMessage($"[LOAD] {tableInfo.TableName}Data 로드.");
        }
        catch (Exception e)
        {
            LogErrorMessage($"[LOAD] {tableInfo.TableName}Data 로드에 실패하였습니다.");

            string message = e.Message;
            if (message.Length > 500)
            {
                message = e.Message.Substring(0, 500);
            }
            LogErrorMessage($"[LOAD]{message}");
        }
    }

    private void SendRequestEnum(SheetsService service, EnumInfo enumInfo)
    {
        try
        {
            var request = service.Spreadsheets.Values.Get(ENUM_SHEET_ID, enumInfo.SheetName + "!A1:C");
            var requestValue = request.Execute().Values;
            SheetToEnum(enumInfo, requestValue);
            LogMessage($"[LOAD] {enumInfo.EnumName} 로드.");
        }
        catch (Exception e)
        {
            LogErrorMessage($"[LOAD] {enumInfo.EnumName} 로드에 실패하였습니다.");
            string message = e.Message;
            if (message.Length > 500)
            {
                message = e.Message.Substring(0, 500);
            }
            LogErrorMessage($"[LOAD]{message}");
        }
    }

    private void SetHeaders_Horizontal(TableInfo tableInfo)
    {
        ExporterEditor.SetHeaders(tableInfo);
    }

    private void SetHeaders_Vertical(TableInfo tableInfo)
    {
        List<Header> headers = new List<Header>();
        List<Header> listHeaders = new List<Header>();

        for (int row = 1; row < tableInfo.Table.Rows.Count; ++row)
        {
            Header header = new Header();
            header.VariableType = tableInfo.Table.Rows[row][1].ToString();
            header.Description = tableInfo.Table.Rows[row][3].ToString();
            header.VariableName = tableInfo.Table.Rows[row][0].ToString();

            if (Regex.IsMatch(header.VariableName, "[0-9]+$") == true)
            {
                header.ListName = Regex.Replace(header.VariableName, "[0-9]+$", string.Empty);
            }
            else if (Regex.IsMatch(header.VariableName, @"\D+[0-9]+\D+") == true)
            {
                header.ListName = Regex.Replace(header.VariableName, "[0-9]+", string.Empty);
            }

            if (header.ListName.Equals(string.Empty) == false)
            {
                if (listHeaders.Find(x => x.ListName.Equals(header.ListName) == true) == null)
                {
                    listHeaders.Add(header);
                }
            }

            headers.Add(header);
        }

        tableInfo.Headers = headers;
        tableInfo.ListHeaders = listHeaders;
    }

    private void SheetToEnum(EnumInfo enumInfo, IList<IList<object>> value)
    {
        // 모든 데이터 멤버로 등록.
        for (int row = 0; row < value.Count; ++row)
        {
            EnumMember member = new EnumMember();
            member.Value = int.Parse(value[row][0].ToString());
            member.MemberName = value[row][1].ToString();
            member.Description = value[row][2].ToString();
            enumInfo.EnumMembers.Add(member);
        }
    }

    #endregion

    #region %%%Data.cs 파일

    private void ExportScript(TableInfo tableInfo)
    {
        var headers = tableInfo.Headers;
        var listHeaders = tableInfo.ListHeaders;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.IO;");
        sb.AppendLine("using System.Text;");
        sb.AppendLine();
        sb.AppendLine($"public class {tableInfo.TableName}Data");
        sb.AppendLine("{");

        // if(tableInfo.IsApply == false)
        // {
        //     sb.AppendLine();
        //     sb.AppendLine("}");
        //     File.WriteAllText($"{CLIENT_SCRIPT}/{tableInfo.TableName}Data.cs", sb.ToString());
        //     LogMessage($"[C#] {tableInfo.TableName}Data.cs 파일을 저장하였습니다.");
        //     return;
        // }

        #region 변수들

        for (int i = 0; i < headers.Count; ++i)
        {
            sb.AppendLine($"    ///<summary>{headers[i].Description}</summary>");
            if (headers[i].ListName.Equals(string.Empty) == true)
            {
                sb.AppendLine($"    public readonly {headers[i].VariableType} {headers[i].VariableName};");
            }
            else
            {
                sb.AppendLine($"    private readonly {headers[i].VariableType} {headers[i].VariableName};");
            }

            if (i < headers.Count - 1 || listHeaders != null)
            {
                sb.AppendLine();
            }
        }

        if (listHeaders != null)
        {
            for (int i = 0; i < listHeaders.Count; ++i)
            {
                string listName = listHeaders[i].ListName;
                string typeName = listHeaders[i].VariableType;
                sb.AppendLine($"    ///<summary>{listName} 리스트</summary>");
                sb.AppendLine($"    public readonly List<{typeName}> {listName}List = new List<{typeName}>();");

                if (i < listHeaders.Count - 1)
                {
                    sb.AppendLine();
                }
            }
        }

        #endregion

        #region 생성자

        sb.AppendLine($"    public {tableInfo.TableName}Data(BinaryReader reader)");
        sb.AppendLine("    {");

        for (int i = 0; i < headers.Count; ++i)
        {
            AppendText_BinaryReader(sb, headers[i]);
        }

        if (listHeaders != null && listHeaders.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("        LinkTable();");
        }

        sb.AppendLine("    }");
        #endregion

        #region LinkTable()

        if (listHeaders != null && listHeaders.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("    public void LinkTable()");
            sb.AppendLine("    {");

            for (int i = 0; i < headers.Count; ++i)
            {
                if (headers[i].ListName.Equals(string.Empty) == false)
                {
                    sb.AppendLine($"        {headers[i].ListName}List.Add({headers[i].VariableName});");
                }
            }

            sb.AppendLine("    }");
        }

        #endregion

        sb.AppendLine("}");

        File.WriteAllText($"{CLIENT_SCRIPT}/{tableInfo.TableName}Data.cs", sb.ToString());
        LogMessage($"[C#] {tableInfo.TableName}Data.cs 파일을 저장하였습니다.");
    }

    private void AppendText_BinaryReader(StringBuilder sb, Header header)
    {
        if (header.VariableType.Equals("string") == true)
        {
            string lower = header.VariableName.ToLower();
            sb.AppendLine($"        int {lower} = reader.ReadInt32();");
            sb.AppendLine($"        {header.VariableName} = Encoding.UTF8.GetString(reader.ReadBytes({lower}));");
        }
        else if (header.VariableType.EndsWith("Type") == true)
        {
            sb.AppendLine($"        {header.VariableName} = ({header.VariableType})reader.ReadInt32();");
        }
        else
        {
            sb.AppendLine($"        {header.VariableName} = reader.Read{GetType(header.VariableType)}();");
        }
    }

    private TypeCode GetType(string typeName)
    {
        if (typeName.Equals("int") == true)
        {
            return TypeCode.Int32;
        }
        else if (typeName.Equals("long") == true)
        {
            return TypeCode.Int64;
        }
        else if (typeName.Equals("float") == true)
        {
            return TypeCode.Single;
        }
        else if (typeName.Equals("bool") == true)
        {
            return TypeCode.Boolean;
        }
        else if (typeName.Equals("string") == true)
        {
            return TypeCode.String;
        }
        else
        {
            return TypeCode.Empty;
        }
    }

    #endregion

    #region DataTable.cs 파일

    private void ExportDataTableManager()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!");
        sb.AppendLine("using System.Collections;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.IO;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine($"public partial class DataTable");
        sb.AppendLine("{");
        for (int i = 0; i < TableInfos.Count; ++i)
        {
            if (TableInfos[i].IsUseClient == false || TableInfos[i].IsApply == false)
            {
                continue;
            }
            AppendText_Variable(sb, TableInfos[i].TableName);
        }
        sb.AppendLine();
        sb.AppendLine("    public IEnumerator LoadRoutine()");
        sb.AppendLine("    {");
        sb.AppendLine("        int allCount = 0;");
        sb.AppendLine("        int loadedCount = 0;");
        sb.AppendLine();
        for (int i = 0; i < TableInfos.Count; ++i)
        {
            if (TableInfos[i].IsUseClient == false || TableInfos[i].IsApply == false)
            {
                continue;
            }
            AppendText_LoadRoutine(sb, TableInfos[i].TableName);
        }
        sb.AppendLine();
        sb.AppendLine("        yield return new WaitUntil(() => allCount == loadedCount);");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public void LoadForEditor()");
        sb.AppendLine("    {");
        for (int i = 0; i < TableInfos.Count; ++i)
        {
            if (TableInfos[i].IsUseClient == false || TableInfos[i].IsApply == false)
            {
                continue;
            }
            AppendText_LoadForEditor(sb, TableInfos[i].TableName);
        }
        sb.AppendLine("    }");
        sb.AppendLine();
        for (int i = 0; i < TableInfos.Count; ++i)
        {
            if (TableInfos[i].IsUseClient == false || TableInfos[i].IsApply == false)
            {
                continue;
            }
            AppendText_LoadFunction(sb, TableInfos[i].TableName);
        }
        sb.AppendLine("}");


        File.WriteAllText($"{CLIENT_SCRIPT}/DataTable.cs", sb.ToString());
        LogMessage($"[C#] DataTable.cs 파일을 저장하였습니다.");
    }

    private void AppendText_Variable(StringBuilder sb, string tableName)
    {
        if (IsVertical(tableName) == false)
        {
            sb.AppendLine($"    #region {tableName}");
            sb.AppendLine($"    private ReadOnlyList<{tableName}Data> {tableName}List = null;");
            sb.AppendLine($"    private ReadOnlyDictionary<int, {tableName}Data> {tableName}Table = null;");
            sb.AppendLine();
            sb.AppendLine($"    public ReadOnlyList<{tableName}Data> Get{tableName}DataList()");
            sb.AppendLine("    {");
            sb.AppendLine($"        return {tableName}List;");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine($"    public {tableName}Data Get{tableName}Data(int key)");
            sb.AppendLine("    {");
            sb.AppendLine("        if (key == 0)");
            sb.AppendLine("        {");
            sb.AppendLine("            return null;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        if ({tableName}Table.TryGetValue(key, out {tableName}Data retVal) == true)");
            sb.AppendLine("        {");
            sb.AppendLine("            return retVal;");
            sb.AppendLine("        }");
            sb.AppendLine("        else");
            sb.AppendLine("        {");
            string logText = "<{key}>";
            sb.AppendLine($"            Debug.LogError($\"Can not find UniqueID of {tableName}Data: {logText}\");");
            sb.AppendLine("            return null;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("    #endregion");
        }
        else
        {
            sb.AppendLine($"    #region {tableName}");
            sb.AppendLine($"    public {tableName}Data {tableName}Value = null;");
            sb.AppendLine($"    public {tableName}Data Get{tableName}Data()");
            sb.AppendLine("    {");
            sb.AppendLine($"        return {tableName}Value;");
            sb.AppendLine("    }");
            sb.AppendLine("    #endregion");
        }
    }

    private void AppendText_LoadRoutine(StringBuilder sb, string tableName)
    {
        sb.AppendLine("        allCount++;");
        sb.AppendLine($"        GetBytes_FromResources(\"{tableName}\", (bytes) =>");
        sb.AppendLine("        {");
        sb.AppendLine($"            Load{tableName}Data(bytes);");
        sb.AppendLine("            loadedCount++;");
        sb.AppendLine("        });");
    }

    private void AppendText_LoadForEditor(StringBuilder sb, string tableName)
    {
        string lowerCamel = ExporterEditor.GetLowerString(tableName) + "Bytes";
        sb.AppendLine($"        byte[] {lowerCamel} = GetBytes_ForEditor(\"{tableName}Data\");");
        sb.AppendLine($"        Load{tableName}Data({lowerCamel});");
    }

    private void AppendText_LoadFunction(StringBuilder sb, string tableName)
    {
        if (IsVertical(tableName) == false)
        {
            string lowerCamel = ExporterEditor.GetLowerString(tableName);
            sb.AppendLine($"    private void Load{tableName}Data(byte[] bytes)");
            sb.AppendLine("    {");
            sb.AppendLine($"        List<{tableName}Data> {lowerCamel}List = new List<{tableName}Data>();");
            sb.AppendLine($"        Dictionary<int, {tableName}Data> {lowerCamel}Table = new Dictionary<int, {tableName}Data>();");
            sb.AppendLine();
            sb.AppendLine("        Reader = new BinaryReader(new MemoryStream(bytes));");
            sb.AppendLine();
            sb.AppendLine("        while (Reader.BaseStream.Position < bytes.Length)");
            sb.AppendLine("        {");
            sb.AppendLine($"            {tableName}Data data = new {tableName}Data(Reader);");
            sb.AppendLine($"            if ({lowerCamel}Table.ContainsKey(data.TID) == true)");
            sb.AppendLine("            {");
            sb.AppendLine($"                Debug.LogError(\"The duplicate TID: \" + data.TID + \" in {tableName}\");");
            sb.AppendLine("                continue;");
            sb.AppendLine("            }");
            sb.AppendLine("            else if (data.TID == 0)");
            sb.AppendLine("            {");
            sb.AppendLine($"                Debug.LogError(\"TID is 0 in {tableName}\");");
            sb.AppendLine("                continue;");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine($"            {lowerCamel}List.Add(data);");
            sb.AppendLine($"            {lowerCamel}Table.Add(data.TID, data);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        Reader.Close();");
            sb.AppendLine();
            sb.AppendLine($"        {tableName}List = new ReadOnlyList<{tableName}Data>({lowerCamel}List);");
            sb.AppendLine($"        {tableName}Table = new ReadOnlyDictionary<int, {tableName}Data>({lowerCamel}Table);");
            sb.AppendLine("    }");
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine($"    private void Load{tableName}Data(byte[] bytes)");
            sb.AppendLine("    {");
            sb.AppendLine("        Reader = new BinaryReader(new MemoryStream(bytes));");
            sb.AppendLine($"        {tableName}Value = new {tableName}Data(Reader);");
            sb.AppendLine("        Reader.Close();");
            sb.AppendLine("    }");
            sb.AppendLine();
        }
    }

    #endregion

    #region .bytes 파일

    private void ExportBinary_Horizontal(TableInfo tableInfo)
    {
        string binary_path = $"{CLIENT_BINATY}/{tableInfo.TableName}Data.bytes";
        string text_path = $"{CLIENT_HISTORY}/{tableInfo.TableName}Data.txt";

        FileStream fsbw = new FileStream(binary_path, FileMode.Create, FileAccess.Write, FileShare.None);
        FileStream fssw = new FileStream(text_path, FileMode.Create, FileAccess.Write, FileShare.None);
        BinaryWriter bw = new BinaryWriter(fsbw, new UTF8Encoding(false));
        StreamWriter sw = new StreamWriter(fssw, new UTF8Encoding(false));

        Dictionary<string, List<int>> duplicatedTIDs = new Dictionary<string, List<int>>();

        for (int row = 3; row < tableInfo.Table.Rows.Count; ++row)
        {
            for (int col = 0; col < tableInfo.Table.Columns.Count; ++col)
            {
                string data = tableInfo.Table.Rows[row][col].ToString();

                if (col == 0)
                {
                    if (duplicatedTIDs.ContainsKey(data) == false)
                    {
                        List<int> value = new List<int>();
                        value.Add(row);
                        duplicatedTIDs.Add(data, value);
                    }
                    else
                    {
                        duplicatedTIDs[data].Add(row);
                    }
                }
                bw.Write(GetBytes(tableInfo.Headers[col].VariableType, data));
                sw.Write(data.Replace("\n", " ") + "\t");
            }
            sw.WriteLine();
        }

        bw.Close();
        sw.Close();
        fsbw.Close();
        fssw.Close();

        LogMessage($"[BINARY] {tableInfo.TableName}Data.bytes 파일을 저장하였습니다.");
        LogMessage($"[HISTORY] {tableInfo.TableName}Data.txt 파일을 저장하였습니다.");

        CheckDuplicatedTID(duplicatedTIDs, tableInfo.TableName);
    }

    private void ExportBinary_Vertical(TableInfo tableInfo)
    {
        string binary_path = $"{CLIENT_BINATY}/{tableInfo.TableName}Data.bytes";
        string text_path = $"{CLIENT_HISTORY}/{tableInfo.TableName}Data.txt";

        FileStream fsbw = new FileStream(binary_path, FileMode.Create, FileAccess.Write, FileShare.None);
        FileStream fssw = new FileStream(text_path, FileMode.Create, FileAccess.Write, FileShare.None);
        BinaryWriter bw = new BinaryWriter(fsbw, new UTF8Encoding(false));
        StreamWriter sw = new StreamWriter(fssw, new UTF8Encoding(false));

        for (int row = 1; row < tableInfo.Table.Rows.Count; ++row)
        {
            string data = tableInfo.Table.Rows[row][2].ToString();
            bw.Write(GetBytes(tableInfo.Headers[row - 1].VariableType, data));
            sw.Write(data);
            sw.WriteLine();
        }

        bw.Close();
        sw.Close();
        fsbw.Close();
        fssw.Close();

        LogMessage($"[BINARY] {tableInfo.TableName}Data.bytes 파일을 저장하였습니다.");
        LogMessage($"[HISTORY] {tableInfo.TableName}Data.txt 파일을 저장하였습니다.");
    }

    private void CheckDuplicatedTID(Dictionary<string, List<int>> duplicatedTIDs, string tableName)
    {
        if (duplicatedTIDs.Count == 0)
        {
            return;
        }

        bool check = false;
        foreach (KeyValuePair<string, List<int>> pair in duplicatedTIDs)
        {
            int overlap = pair.Value.Count;
            if (overlap > 1)
            {
                if (check == false)
                {
                    LogErrorMessage($"[EXPORT] 중복된 라인이 있는 테이블 = {tableName}");
                    check = true;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append($"키가 {pair.Key}이고 중복된 라인은? ");
                for (int i = 0; i < overlap; ++i)
                {
                    sb.Append(pair.Value[i] + 1);
                }
                LogErrorMessage($"[EXPORT] {sb}");
            }
        }
    }

    private byte[] GetBytes(string typeName, string value)
    {
        TypeCode type = ExporterEditor.GetTypeCode(typeName);

        if (type == TypeCode.Empty)
        {
            var enumData = EnumInfos.Find((x) => x.EnumName.Equals(typeName) == true);
            if (enumData == null)
            {
                LogErrorMessage($"[CONVERT] Enum:{typeName} 찾을 수 없습니다.");
                return null;
            }

            var memberData = enumData.EnumMembers.Find((x) => x.MemberName.Equals(value) == true);
            if (memberData == null)
            {
                LogErrorMessage($"[CONVERT] Enum:{typeName}의 {value} 항목을 찾을 수 없습니다.");
                return null;
            }

            value = memberData.Value.ToString();
            type = TypeCode.Int32;
        }

        return ExporterEditor.GetBytes(type, value);
    }

    #endregion

    #region EnumDef.cs 파일
    private void ExportEnumDef()
    {
        StringBuilder sb = new StringBuilder();
        string fileText = ""; // "// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!";

        // Enum 파일이 없다면 신규 파일.
        /*
        if (File.Exists($"{CLIENT_SCRIPT}/EnumDef.cs") == true)
        {
            UnityEngine.Debug.Log("<color=blue>EnumDef.cs 가 존재합니다</color>");
            fileText = File.ReadAllText($"{CLIENT_SCRIPT}/EnumDef.cs");
        }
        else
        {
            UnityEngine.Debug.Log("<color=red>EnumDef.cs 를 새로 생성</color>");
            fileText.Insert(0, "// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!");
        }
        */

        sb.Clear();
        sb.AppendLine("// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!");

        for (int i = 0; i < EnumInfos.Count; ++i)
        {
            // 멤버 정보가 있는 Enum만 업데이트.
            if (EnumInfos[i].EnumMembers.Count == 0)
            {
                continue;
            }

            EnumInfo enumInfo = EnumInfos[i];

            /*
            for (int a = 0; a < enumInfo.EnumMembers.Count; a++)
            {
                UnityEngine.Debug.Log(string.Format("{0}", EditorJsonUtility.ToJson(enumInfo.EnumMembers[a])));
            }

            
            // EnumDef 파일에 해당 Enum이 있는지 확인했는데 없다면 추가.
            if (fileText.Contains(enumInfo.EnumName) == false)
            {
                // 첫 Enum일 경우 제일 앞에 추가.
                if (i == 0)
                {
                    // 수정하지 마세요!의 느낌표 위치. (3 : 1 + 줄바꿈1)
                    start = fileText.IndexOf("!") + (1 + NEW_LINE.Length);
                }
                // 그 이후일 경우 이전 인덱스의 Enum의 끝 위치에 추가. (5 : 1 + 줄바꿈2)
                else
                {
                    int beforeStart = fileText.IndexOf("public enum " + EnumInfos[i - 1].EnumName);
                    int beforeEnd = fileText.Substring(beforeStart).IndexOf("}");
                    start = beforeStart + beforeEnd + (1 + NEW_LINE.Length * 2);
                }
            }
            // 이미 있는 Enum이라면 업데이트. (5 : 1 + 줄바꿈2)
            else
            {
                int currentStart = fileText.IndexOf("public enum " + EnumInfos[i].EnumName);
                int currentEnd = fileText.Substring(currentStart).IndexOf("}");
                fileText = fileText.Remove(currentStart, currentEnd + (1 + NEW_LINE.Length * 2));

                start = currentStart;
            }
            */
            sb.AppendLine($"public enum {EnumInfos[i].EnumName}");
            sb.AppendLine("{");
            for (int ii = 0; ii < EnumInfos[i].EnumMembers.Count; ++ii)
            {
                EnumMember member = EnumInfos[i].EnumMembers[ii];
                sb.AppendLine($"    ///<summary>{member.Description}</summary>");
                sb.AppendLine($"    {member.MemberName} = {member.Value},");
            }

            if (EnumInfos[i].IsAddMax == true)
            {
                sb.AppendLine();
                sb.AppendLine("    Max");
            }

            sb.AppendLine("}");
            sb.AppendLine();

            fileText = sb.ToString();
        }

        UnityEngine.Debug.Log(string.Format("{0}", fileText));
        File.WriteAllText($"{CLIENT_SCRIPT}/EnumDef.cs", fileText);
        LogMessage($"[C#] EnumDef.cs 파일을 저장하였습니다.");
    }
    #endregion

    #region .php 파일
    private void ExportEnumDefPHP()
    {
        StringBuilder sb = new StringBuilder();
        string fileText = string.Empty;

        // Enum 파일이 없다면 신규 파일.
        if (File.Exists($"{ServerPath}/DataTable/EnumDef.php") == true)
        {
            fileText = File.ReadAllText($"{ServerPath}/DataTable/EnumDef.php");
        }
        else
        {
            fileText.Insert(0, "<?php\r\n// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!\r\n\r\nclass Enum\r\n{");
        }

        for (int i = 0; i < EnumInfos.Count; ++i)
        {
            // 멤버 정보가 있는 Enum만 업데이트.
            if (EnumInfos[i].EnumMembers.Count == 0)
            {
                continue;
            }

            EnumInfo enumInfo = EnumInfos[i];
            int start;

            // EnumDef 파일에 해당 Enum이 있는지 확인했는데 없다면 추가.
            if (fileText.Contains(enumInfo.EnumName.ToSnakeCase().ToUpper()) == false)
            {
                // 첫 Enum일 경우 제일 앞에 추가.
                if (i == 0)
                {
                    // class Enum의 { 위치. (15 : 1 + "class Enum"길이 + { 길이 + 줄바꿈2)
                    start = fileText.IndexOf("class Enum") + (1 + "class Enum".Length + "{".Length + NEW_LINE.Length * 2);
                }
                // 그 이후일 경우 이전 인덱스의 Enum의 끝 위치에 추가. (5 : 1 + 줄바꿈2)
                else
                {
                    int beforeStart = fileText.IndexOf("    static public $" + EnumInfos[i - 1].EnumName.ToSnakeCase().ToUpper());
                    int beforeEnd = fileText.Substring(beforeStart).IndexOf(";");
                    start = beforeStart + beforeEnd + (1 + NEW_LINE.Length * 2);
                }
            }
            // 이미 있는 Enum이라면 업데이트. (5 : 1 + 줄바꿈2)
            else
            {
                int currentStart = fileText.IndexOf("    static public $" + EnumInfos[i].EnumName.ToSnakeCase().ToUpper());
                int currentEnd = fileText.Substring(currentStart).IndexOf(";");
                fileText = fileText.Remove(currentStart, currentEnd + (1 + NEW_LINE.Length * 2));

                start = currentStart;
            }

            sb.Clear();
            sb.AppendLine($"    static public ${EnumInfos[i].EnumName.ToSnakeCase().ToUpper()} = array(");

            int maxValue = 0;
            for (int ii = 0; ii < EnumInfos[i].EnumMembers.Count; ++ii)
            {
                EnumMember member = EnumInfos[i].EnumMembers[ii];
                sb.AppendLine($"        '{member.MemberName}' => {member.Value},");
                maxValue = member.Value;
            }

            if (EnumInfos[i].IsAddMax == true)
            {
                sb.AppendLine($"        'Max' => {maxValue + 1},");
            }

            sb.AppendLine("    );");
            sb.AppendLine();

            fileText = fileText.Insert(start, sb.ToString());
        }

        File.WriteAllText($"{ServerPath}/DataTable/EnumDef.php", fileText);
        LogMessage($"[PHP] EnumDef.php 파일을 저장하였습니다.");
    }

    private void ExportDataTablePHP()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<?php");
        sb.AppendLine("// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!");
        sb.AppendLine();
        sb.AppendLine($"require_once __DIR__ . '/../DataTable/EnumDef.php';");
        sb.AppendLine();
        for (int i = 0; i < TableInfos.Count; ++i)
        {
            if (TableInfos[i].IsUseServer == false)
            {
                continue;
            }
            sb.AppendLine($"require_once __DIR__ . '/../DataTable/{TableInfos[i].TableName}Data.php';");
        }
        sb.AppendLine();
        sb.AppendLine("?>");

        File.WriteAllText($"{ServerPath}/Common/DataTable.php", sb.ToString());
        LogMessage($"[PHP] DataTable.php 파일을 저장하였습니다.");
    }

    private void ExportPHP(TableInfo tableInfo)
    {
        if (tableInfo.Table == null)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<?php");
        sb.AppendLine("// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!");
        sb.AppendLine();
        sb.AppendLine($"class {tableInfo.TableName.ToSnakeCase().ToUpper()}_DATA");
        sb.AppendLine("{");
        sb.AppendLine("    static public $DATA = array (");
        for (int row = 3; row < tableInfo.Table.Rows.Count; ++row)
        {
            sb.AppendLine($"        {tableInfo.Table.Rows[row][0]} => array (");
            for (int col = 0; col < tableInfo.Table.Columns.Count; ++col)
            {
                Header header = tableInfo.Headers[col];

                string data = tableInfo.Table.Rows[row][col].ToString();

                if (header.VariableType.Equals("string") == true)
                {
                    data = data.Replace("\n", "\\n");
                    data = $"'{data}'";
                }
                else if (header.VariableType.StartsWith("e") == true)
                {
                    var enumData = EnumInfos.Find(x => x.EnumName.Equals(header.VariableType) == true);
                    if (enumData == null)
                    {
                        data = "0";
                    }
                    else
                    {
                        var memberData = enumData.EnumMembers.Find(x => x.MemberName.Equals(data) == true);
                        if (memberData == null)
                        {
                            data = "0";
                        }
                        else
                        {
                            data = memberData.Value.ToString();
                        }
                    }
                }

                sb.AppendLine($"            '{header.VariableName.ToSnakeCase()}' => {data},");
            }
            sb.AppendLine("        ),");
        }
        sb.AppendLine("    );");
        sb.AppendLine("}");
        sb.AppendLine("?>");

        File.WriteAllText($"{ServerPath}/DataTable/{tableInfo.TableName}Data.php", sb.ToString());
        LogMessage($"[PHP] {tableInfo.TableName}Data.php 파일을 저장하였습니다.");
    }

    #endregion
}
