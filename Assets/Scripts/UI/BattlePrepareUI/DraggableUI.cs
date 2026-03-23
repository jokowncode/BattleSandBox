using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DraggableUI : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler{

    [SerializeField] private AudioClip StartDragSfx;
    
    private Hero previewInstance;
    public string prefabReference;
    private Image image;

    private Hero CurrentHero;
    
    private void Start(){
        image = this.GetComponent<Image>();
        this.CurrentHero = this.GetComponent<Hero>();
    }
    
    public void OnBeginDrag(PointerEventData eventData){
        if (!CurrentHero) {
            if (BattleManager.Instance.IsFullHero){
                SceneChangeManager.Instance.AddGameTip("人数已满！");
                AudioManager.Instance.PlayErrorSfx();
                return;
            }    
        }else if (BattleManager.Instance.IsBattleStart) {
            return;
        }
        
        if (StartDragSfx) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, StartDragSfx);
        }

        Vector3 position = Vector3.one * 100.0f;
        if (CurrentHero) {
            position = this.CurrentHero.transform.position;
            this.CurrentHero.TransitionShow(false);
            BattleManager.Instance.RecallHero(this.CurrentHero, false);
        }
        Hero go = HeroWarehouseManager.Instance.GetHeroByRef(prefabReference);
        if (go) {
            previewInstance = Instantiate(go);
            if (CurrentHero && CurrentHero.IsOriginExist) {
                previewInstance.SetOriginExist();
                if (CurrentHero.TryGetComponent(out FighterDeadBattleDefeat _)) {
                    previewInstance.AddComponent<FighterDeadBattleDefeat>();
                }
            }
            previewInstance.transform.position = position;
        }
    }

    public void OnDrag(PointerEventData eventData){
        if (!previewInstance) return;
        Ray ray = CameraManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Deploy"))
            && BattleManager.Instance.IsWithinArea(hit.point) != -1){
            previewInstance.transform.position = hit.point;
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        if (!previewInstance) return;
        SetAlpha(1f);
        DeployHero(previewInstance, previewInstance.transform.position);
    }

    public void DeployHero(Vector3 heroPos) {
        Hero prefab = HeroWarehouseManager.Instance.GetHeroByRef(prefabReference);
        DeployHero(Instantiate(prefab), heroPos);
    }

    private void DeployHero(Hero hero, Vector3 heroPos) {
        hero.transform.position = heroPos;
        GetNavMeshPosition(hero.transform.position, 1.0f, out Vector3 finalPos);
        int deploAreaIndex = BattleManager.Instance.IsWithinArea(finalPos);
        if (deploAreaIndex != -1){
            hero.transform.position = finalPos;
            DraggableUI dragHero = hero.AddComponent<DraggableUI>();
            dragHero.prefabReference = hero.Name;
            hero.Deploy(deploAreaIndex);
            Destroy(this.gameObject);
        }else {
            if (this.CurrentHero) {
                this.CurrentHero.TransitionShow(true);
                this.CurrentHero.Deploy(this.CurrentHero.DeployAreaIndex);
            }
            Destroy(hero.gameObject);
            previewInstance = null;
        }
    }

    private void SetAlpha(float alpha){
        // image.color = new Color(image.color.r,image.color.g,image.color.b, alpha);
    }

    private void GetNavMeshPosition(Vector3 currentPos, float maxDistance, out Vector3 navMeshPos){
        if (NavMesh.SamplePosition(currentPos, out var hit, maxDistance, NavMesh.AllAreas)){
            navMeshPos = hit.position;
            return;
        }
        navMeshPos = currentPos;
    }
}
