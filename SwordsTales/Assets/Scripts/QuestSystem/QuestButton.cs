using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestButton : MonoBehaviour
    {
        public Button buttonComponent;
        public RawImage icon;
        public Text eventName;
        public Sprite currentImage;
        public Sprite waitingImage;
        public Sprite doneImage;
        public QuestEvent thisEvent;

        private QuestEvent.EventStatus _status;

        public void Setup(QuestEvent e, GameObject scrollList)
        {
            thisEvent = e;
            buttonComponent.transform.SetParent(scrollList.transform,false);
            eventName.text = "<b>" + thisEvent.name + "</b>\n" + thisEvent.description;
            _status = thisEvent.status;
            icon.texture = waitingImage.texture;
            buttonComponent.interactable = false;
        }

        public void UpdateButton(QuestEvent.EventStatus s)
        {
            _status = s;
            
            if( _status == QuestEvent.EventStatus.DONE)
            {
                
                icon.texture = doneImage.texture;
                buttonComponent.interactable = false;
            }
            else if (_status == QuestEvent.EventStatus.WAITING)
            {
                icon.texture = waitingImage.texture;
                buttonComponent.interactable = false;
            }
            else if (_status == QuestEvent.EventStatus.CURRENT)
            {
                icon.texture = currentImage.texture;
                buttonComponent.interactable = true;
            }
        }
    }
}
