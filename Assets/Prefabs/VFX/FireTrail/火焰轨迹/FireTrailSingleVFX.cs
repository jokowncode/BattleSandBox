using System;
using UnityEngine;
using UnityEngine.VFX;

public class FireTrailSingleVFX : MonoBehaviour{

    // TODO: If Enemy Can Set Fire Trail
    private TargetType Type = TargetType.Enemy;
    private float Damage;
    
    public float stopDelay = 3f;    // 延迟多久停止发射
    public float destroyDelay = 1f; // 停止后多久销毁物体

    private VisualEffect vfx;

    void Start(){
        vfx = GetComponentInChildren<VisualEffect>();
    }

    void OnEnable(){
        // stopDelay 秒后调用 StopVFX()
        Invoke(nameof(StopVFX), stopDelay);
    }

    void StopVFX(){
        if (vfx != null){
            vfx.SendEvent("OnStop"); // 触发 VFX Graph 停止事件
        }

        // destroyDelay 秒后销毁
        Destroy(gameObject, destroyDelay);
    }

    public void SetDamage(float damage){
        this.Damage = damage;
    }

    private void OnTriggerEnter(Collider other){
        if (other.TryGetComponent(out Fighter fighter) && fighter.gameObject.layer == LayerMask.NameToLayer(Type.ToString())) {
            fighter.BeDamaged(new EffectData{
                Value = Damage
            });
        }
    }
}