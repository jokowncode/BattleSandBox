
public static class SceneTools {
    public static bool IsBattleScene(SceneType scene) {
        return scene == SceneType.BaseBattleScene;
    }
    
    public static bool IsDungeonScene(SceneType scene) {
        string sceneName = scene.ToString();
        return sceneName.StartsWith("Dungeons_");
    }
}

