using UnityEngine;
using UnityEngine.SceneManagement;

namespace IdleCarService.Core
{
    public class Bootstrap : MonoBehaviour
    {
        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            LoadGameData();
            InitializeCoreSystems();
            SceneManager.LoadSceneAsync("GamePlay");
        }

        private void LoadGameData()
        {
            Debug.Log("Loading game data...");
        }

        private void InitializeCoreSystems()
        {
            Debug.Log("Initializing core systems...");
        }
    }

}
