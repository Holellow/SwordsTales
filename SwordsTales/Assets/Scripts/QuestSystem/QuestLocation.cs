using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

public class QuestLocation : MonoBehaviour
{
    public QuestManager qManager;
    public QuestEvent qEvent;
    public QuestButton qButton;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(qEvent.status);
        if (!collision.gameObject.CompareTag("Player")) return;
        if (qEvent.status != QuestEvent.EventStatus.CURRENT) return;
        
        qEvent.UpdateQuestEvent(QuestEvent.EventStatus.DONE);
        qButton.UpdateButton(QuestEvent.EventStatus.DONE);
        qManager.UpdateQuestsOnCompletion(qEvent);
    }

    public void Setup(QuestManager qm, QuestEvent qe, QuestButton qb)
    {
        qManager = qm;
        qEvent = qe;
        qButton = qb;
        qe.button = qButton;
    }
}
