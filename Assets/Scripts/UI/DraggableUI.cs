using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    //public Sprite idleSprite;
    public GameObject previewPrefab;
    public GameObject previewInstance;
    
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
    }
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if (previewPrefab != null)
        {
            //Debug.Log("Instantiated preview");
            previewInstance = Instantiate(previewPrefab);
            SetAlpha(0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        
        if (previewInstance != null)
        {
            // TODO:Solve WorldPos Calculate
            Debug.Log("Tracing preview");
            Vector3 worldPos = Vector3.one;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                worldPos = hit.point; // 更精确点击地面或目标物体
            }
            worldPos.y = 0f;
            //Debug.Log(worldPos);
            previewInstance.transform.position = worldPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        SetAlpha(1f);
        if (previewInstance != null && !ValidPosition(previewInstance.transform.position))
        {
            Destroy(previewInstance);
        }
        else
        {
            Debug.Log(BattleManager.Instance);
            Debug.Log(previewInstance);
            BattleManager.Instance.AddHero(previewInstance);
            //BattleManager.Instance.AddHero(previewInstance.GetComponentInChildren<Hero>());
            Destroy(this.gameObject);
        }
        
    }

    void SetAlpha(float alpha)
    {
        image.color = new Color(image.color.r,image.color.g,image.color.b, alpha);
    }

    bool ValidPosition(Vector3 pos)
    {
        if(pos.x<5f && pos.x>-5f && pos.z<5f && pos.z>-5f)
        {
           return true; 
        }
        return false;
    }

    Vector3 GetFinalPosition(Vector3 position)
    {
        return Vector3.one;
    }
    
    
}
