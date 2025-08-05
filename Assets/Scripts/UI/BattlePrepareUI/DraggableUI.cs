using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DraggableUI : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler{

    [SerializeField] private AudioClip StartDragHeroSfx;
    [SerializeField] private AudioClip DeployHeroSfx;
    
    //public Sprite idleSprite;
    private Hero previewInstance;
    public string prefabReference;
    
    private Image image;
    // Start is called before the first frame update
    void Start(){
        image = this.GetComponent<Image>();
    }
    
    
    public void OnBeginDrag(PointerEventData eventData){
        Vector3 instantiatedPosition;
        FindNearestNavMeshPoint(Vector3.zero,10.0f,out instantiatedPosition);
        Hero hero = HeroWarehouseManager.Instance.GetHeroByRef(prefabReference);
        if (hero !=null) {
            previewInstance = Instantiate(hero, instantiatedPosition, Quaternion.identity);
            previewInstance.name=prefabReference;
            SetAlpha(0.5f);
            
            if(StartDragHeroSfx)
                AudioManager.Instance.PlaySfxAtPoint(this.transform.position, StartDragHeroSfx);
            
        }
    }

    public void OnDrag(PointerEventData eventData){
        if (previewInstance != null){
            Vector3 worldPos = Vector3.one;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)){
                worldPos = hit.point; // 更精确点击地面或目标物体
            }
            worldPos.y = 0f;
            previewInstance.transform.position = worldPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        SetAlpha(1f);
        if (previewInstance != null && !ValidPosition(previewInstance.transform.position)){
            Destroy(previewInstance);
        }else{
            FindNearestNavMeshPoint(previewInstance.transform.position, 1.0f, out Vector3 finalPos);
            previewInstance.transform.position = finalPos;
            previewInstance.Deploy();
            BattleManager.Instance.AddHero(previewInstance.GetComponent<Hero>());
            Destroy(this.gameObject);
            
            if(DeployHeroSfx)
                AudioManager.Instance.PlaySfxAtPoint(this.transform.position, DeployHeroSfx);
        }
    }

    void SetAlpha(float alpha){
        image.color = new Color(image.color.r,image.color.g,image.color.b, alpha);
    }

    bool ValidPosition(Vector3 pos){
        if(BattleManager.Instance.IsWithinArea(pos)){
           return true; 
        }
        return false;
    }
    
    // 辅助函数：查找最近的NavMesh点
    private bool FindNearestNavMeshPoint(Vector3 source, float maxDistance, out Vector3 result){
        NavMeshHit hit;
        if (NavMesh.SamplePosition(source, out hit, maxDistance, NavMesh.AllAreas)){
            result = hit.position;
            return true;
        }
        result = source;
        return false;
    }
    
}
