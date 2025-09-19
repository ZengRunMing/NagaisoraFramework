using UnityEngine;
using UnityEngine.EventSystems;

namespace NagaisoraFramework
{
	public class EventTriggerListener : EventTrigger
	{
		public delegate void VoidDelegate(GameObject go);

		public VoidDelegate onClick;

		public VoidDelegate onDown;

		public VoidDelegate onEnter;

		public VoidDelegate onExit;

		public VoidDelegate onUp;

		public VoidDelegate onSelect;

		public VoidDelegate onDeselect;

		public VoidDelegate onUpdateSelect;

		public VoidDelegate onSubmit;

		public VoidDelegate onCancel;

		public static EventTriggerListener Get(GameObject go)
		{
			EventTriggerListener eventTriggerListener = go.GetComponent<EventTriggerListener>();
			if (eventTriggerListener == null)
			{
				eventTriggerListener = go.AddComponent<EventTriggerListener>();
			}
			return eventTriggerListener;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			if (onClick != null)
			{
				onClick(base.gameObject);
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (onDown != null)
			{
				onDown(base.gameObject);
			}
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			if (onEnter != null)
			{
				onEnter(base.gameObject);
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			if (onExit != null)
			{
				onExit(base.gameObject);
			}
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			if (onUp != null)
			{
				onUp(base.gameObject);
			}
		}

		public override void OnSelect(BaseEventData eventData)
		{
			if (onSelect != null)
			{
				onSelect(base.gameObject);
			}
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			if (onDeselect != null)
			{
				onDeselect(base.gameObject);
			}
		}

		public override void OnUpdateSelected(BaseEventData eventData)
		{
			if (onUpdateSelect != null)
			{
				onUpdateSelect(base.gameObject);
			}
		}

		public override void OnSubmit(BaseEventData eventData)
		{
			if (onSubmit != null)
			{
				onSubmit(base.gameObject);
			}
		}

		public override void OnCancel(BaseEventData eventData)
		{
			if (onCancel != null)
			{
				onCancel(base.gameObject);
			}
		}
	}
}
