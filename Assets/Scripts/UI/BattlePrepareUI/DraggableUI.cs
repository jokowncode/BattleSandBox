using System.Collections;
using System.Collections.Generic;
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
    
    private void Start(){
        image = this.GetComponent<Image>();
    }
    
    public void OnBeginDrag(PointerEventData eventData){
        if (StartDragSfx) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, StartDragSfx);
        }

        Hero go = HeroWarehouseManager.Instance.GetHeroByRef(prefabReference);
        if (go !=null){
            previewInstance = Instantiate(go);
            previewInstance.transform.position = Vector3.one * 100.0f;
            previewInstance.name = prefabReference;
            SetAlpha(0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData){
        if (!previewInstance) return;
        Ray ray = CameraManager.Instance.MainCamera.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Deploy"))){
            Vector3 worldPos = hit.point;
            previewInstance.transform.position = worldPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        if (!previewInstance) return;
        SetAlpha(1f);
        if (!BattleManager.Instance.IsWithinArea(previewInstance.transform.position)){
            Destroy(previewInstance.gameObject);
        }else{
            GetNavMeshPosition(previewInstance.transform.position, 1.0f, out Vector3 finalPos);
            previewInstance.transform.position = finalPos;
            previewInstance.Deploy();
            BattleManager.Instance.AddHero(previewInstance);
            Destroy(this.gameObject);
        }
    }

    private void SetAlpha(float alpha){
        image.color = new Color(image.color.r,image.color.g,image.color.b, alpha);
    }

    private void GetNavMeshPosition(Vector3 currentPos, float maxDistance, out Vector3 navMeshPos){
        if (NavMesh.SamplePosition(currentPos, out var hit, maxDistance, NavMesh.AllAreas)){
            navMeshPos = hit.position;
            return;
        }
        navMeshPos = currentPos;
    }
}
