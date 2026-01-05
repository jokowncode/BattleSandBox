
public static class SceneTools {
    public static bool IsBattleScene(SceneType scene) {
        string sceneName = scene.ToString();
        return sceneName.StartsWith("Battle_");
    }
    
    public static bool IsDungeonScene(SceneType scene) {
        string sceneName = scene.ToString();
        return sceneName.StartsWith("Dungeons_");
    }
}

