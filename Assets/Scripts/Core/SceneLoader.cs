using UnityEngine.SceneManagement;

namespace IdleCarService.Core
{
    public static class SceneLoader
    {
        public enum Scene
        {
            Bootstrap,
            MainMenu,
            GamePlay
        }

        public static void LoadSceneAsync(Scene scene)
        {
            SceneManager.LoadSceneAsync(scene.ToString());
        }
    }
}