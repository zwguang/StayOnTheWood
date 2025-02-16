using UnityEngine.SceneManagement;

namespace GDK
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        public void ChangeSceneByName(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}