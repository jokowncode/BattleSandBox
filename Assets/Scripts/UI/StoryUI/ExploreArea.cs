
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExploreArea : MonoBehaviour, IPointerClickHandler {

	private Image ExploreImage;
	private List<ExploreMapping> Mappings;
	private RectTransform ExploreAreaTrans;

	public Action OnExploreAllGoods;
	public Action<ExploreMapping> OnClickExplore;
	
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
		
		foreach (ExploreMapping mapping in this.Mappings) {
			Vector2 location = mapping.Location;
			location *= new Vector2(this.ExploreAreaTrans.rect.width, this.ExploreAreaTrans.rect.height);
			if (Vector2.SqrMagnitude(point - location) <= 40.0f * 40.0f) {
				OnClickExplore?.Invoke(mapping);
				this.Mappings.Remove(mapping);
				break;
			}
		}

		if (this.Mappings.Count == 0) {
			OnExploreAllGoods?.Invoke();
			this.Hide();
		}
	}
}

