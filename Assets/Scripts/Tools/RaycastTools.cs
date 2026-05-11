
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class RaycastTools {

    public static List<RaycastResult> GetRaycastResult(this EventSystem es) {
        // TODO: MAYBE NEED OPTIMIZE, BUT ONLY INSTRUCTION USE, NOT VERY EXPENSIVE?
        PointerEventData ped = new PointerEventData(es);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        es.RaycastAll(ped, results);
        return results;
    }

}



