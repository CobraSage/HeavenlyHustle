using System.IO;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    private static PersistentDataManager _instance;

    public static PersistentDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("PersistentDataManager");
                _instance = obj.AddComponent<PersistentDataManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public void SaveToFile<T>(string fileName, T data)
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public T LoadFromFile<T>(string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<T>(json);
        }
        return default(T);
    }

    public bool SaveFileExists(string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        return File.Exists(savePath);
    }

}