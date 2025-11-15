using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour,IInteractive
{
    [SerializeField] private int sceneID;
    public void Interaction()
    {
        SceneManager.LoadScene(sceneID);
    }
}