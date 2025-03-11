using UnityEngine;
using System.IO;

namespace IdleCarService.Utils
{
    public class SaveSystem
    {
        private string _saveFilePath;

        public SaveSystem()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, "game_save.json");
        }
        
        public void SaveGame(SaveData saveData)
        {
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(_saveFilePath, json);
            Debug.Log("Game Saved!");
        }
        
        public SaveData LoadGame()
        {
            if (File.Exists(_saveFilePath))
            {
                string json = File.ReadAllText(_saveFilePath);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);
                Debug.Log("Game Loaded!");
                return saveData;
            }
            else
            {
                Debug.LogWarning("Save file not found!");
                return null;
            }
        }
    }
}