using UnityEngine;

[CreateAssetMenu(menuName = "PhantomSpirit/TaskData", fileName = "TaskData")]
public class TaskData : ScriptableObject {
    public string TaskName;
    public string[] TaskDescs;
    public SceneType BindDungeon = SceneType.None;
}