
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExploreArea : MonoBehaviour, IPointerClickHandler {

	private Image ExploreImage;
	private List<ExploreMapping> Mappings;
	private RectTransform ExploreAreaTrans;

	public Action<ExploreMapping> OnClickExplore;

	public bool IsEmpty => this.Mappings.Count == 0;
	
	private void Awake() {
		this.ExploreImage = this.GetComponent<Image>();
		this.ExploreAreaTrans = this.GetComponent<RectTransform>();
		this.Hide();
	}

	public void Show(ExploreNode node) {
		this.ExploreImage.sprite = node.ExploreCG;
		this.Mappings = new (node.Mappings);
		this.gameObject.SetActive(true);
	}

	public void Hide() {
		this.gameObject.SetActive(false);
	}

	public void OnPointerClick(PointerEventData eventData) {
		Vector2 position = eventData.position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ExploreAreaTrans, position, null, out var point);

		Vector2 areaSize = new Vector2(this.ExploreAreaTrans.rect.width, this.ExploreAreaTrans.rect.height);
		foreach (ExploreMapping mapping in this.Mappings) {
			Vector2 corner = mapping.LeftTop * areaSize;
			Vector2 size = mapping.Size * areaSize;
			corner.y = -corner.y - size.y;
			Rect rect = new Rect(corner, size);
			if (rect.Contains(point)) {
				this.Mappings.Remove(mapping);
				OnClickExplore?.Invoke(mapping);
				return;
			}
		}
	}
}

