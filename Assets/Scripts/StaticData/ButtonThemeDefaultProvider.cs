
using UnityEngine;

public static class ButtonThemeDefaultProvider {
    
    private static ButtonTheme _DefaultButtonTheme;
    public static ButtonTheme DefaultButtonTheme {
        get {
            if (_DefaultButtonTheme != null) return _DefaultButtonTheme;
            _DefaultButtonTheme = Resources.Load<ButtonTheme>("DefaultButtonTheme");
            return _DefaultButtonTheme;
        }
    }

}


