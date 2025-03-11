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
            SceneManager.LoadSceneAsync("GamePlay");
        }
    }

}
