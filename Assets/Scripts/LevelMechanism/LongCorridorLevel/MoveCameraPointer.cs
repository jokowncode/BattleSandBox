
using System;
using UnityEngine;
using UnityEngine.UI;

public class MoveCameraPointer : MonoBehaviour {

    [SerializeField] private Camera MainCam;
    [SerializeField] private Button LeftPointer;
    [SerializeField] private Button RightPointer;
    [SerializeField] private Transform Areas;

    private Vector3 CameraStartPos;

    private int AreaCount => Areas.childCount;
    private int CurrentAreaIndex = 0;
    
    private void Awake() {
        this.CameraStartPos = this.MainCam.transform.position;
        this.LeftPointer.gameObject.SetActive(false);
        this.LeftPointer.onClick.AddListener(() => {
            this.ClickPointer(-1);
        });
        
        this.RightPointer.onClick.AddListener(() => {
            this.ClickPointer(1);
        });
    }

    private void ClickPointer(int dir) {
        this.CurrentAreaIndex += dir;
        if (this.CurrentAreaIndex == 0) {
            this.MainCam.transform.position = this.CameraStartPos;
        } else {
            Vector3 pos = this.MainCam.transform.position;
            pos.x = this.Areas.GetChild(this.CurrentAreaIndex - 1).position.x;
            this.MainCam.transform.position = pos;
        }
        this.LeftPointer.gameObject.SetActive(this.CurrentAreaIndex != 0);
        this.RightPointer.gameObject.SetActive(this.CurrentAreaIndex != this.AreaCount - 1);
    }

    private void Start() {
        BattleManager.Instance.OnBattleStart += () => {
            this.MainCam.transform.position = this.CameraStartPos;
            if (this.MainCam.TryGetComponent(out FollowCamera follow)) {
                follow.enabled = true;
            }
            this.LeftPointer.gameObject.SetActive(false);
            this.RightPointer.gameObject.SetActive(false);
        };
    }
}



