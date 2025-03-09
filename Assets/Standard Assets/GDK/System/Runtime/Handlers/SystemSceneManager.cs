using UnityEngine.SceneManagement;

namespace GDK
{
    public class SystemSceneManager : Singleton<SystemSceneManager>
    {
        public void ChangeSceneByName(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}