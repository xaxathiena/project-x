#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class BYDataTableMaker : MonoBehaviour
{
    [MenuItem("Assets/BY/StandAlone/Make Data by CSV")]
    public static void CreateStandAloneAsset()
    {
        foreach (UnityEngine.Object obj in Selection.objects)
        {
            TextAsset csvFile = (TextAsset)obj;
            string tableName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(csvFile));
            ScriptableObject scriptableTable = ScriptableObject.CreateInstance(tableName);


            if (scriptableTable == null)
                return;

            AssetDatabase.CreateAsset(scriptableTable, "Assets/_Master/_Data/_StandAlone/DataTable/" + tableName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            BYDataTableOgrin by = (BYDataTableOgrin)scriptableTable;
            by.ImportDataUsingNewtonJson(csvFile.text);

            EditorUtility.SetDirty(by);
        }
    }
    [MenuItem("Assets/BY/Mobile/Make Data by CSV")]
    public static void CreateMobileAsset()
    {
        foreach (UnityEngine.Object obj in Selection.objects)
        {
            TextAsset csvFile = (TextAsset)obj;
            string tableName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(csvFile));
            ScriptableObject scriptableTable = ScriptableObject.CreateInstance(tableName);


            if (scriptableTable == null)
                return;

            AssetDatabase.CreateAsset(scriptableTable, "Assets/_Master/_Data/_Mobile/DataTable/" + tableName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            BYDataTableOgrin by = (BYDataTableOgrin)scriptableTable;
            by.ImportDataUsingNewtonJson(csvFile.text);

            EditorUtility.SetDirty(by);
        }
    }
    [MenuItem("Assets/BY/Create CSV File form ScriptableTable (Binary file)", false, 1)]
    private static void CreateCSVFile()
    {
        foreach (UnityEngine.Object obj in Selection.objects)
        {
            BYDataTableOgrin dataFile = (BYDataTableOgrin)obj;
            string tableName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(dataFile));
            string data = dataFile.GetCSVData();
            string filePath = "Assets/Data/DataTable/" + tableName + ".csv";
            FileUtil.DeleteFileOrDirectory(filePath);

            FileStream fs = new FileStream(filePath, FileMode.Create);//Tạo file mới tên là test.txt            
            // request.RawData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataSend));
            StreamWriter sWriter = new StreamWriter(fs);//fs là 1 FileStream 
            sWriter.Write(data);
            // Ghi và đóng file
            sWriter.Flush();
            fs.Close();
            AssetDatabase.Refresh();
        }
    }
}
#endif