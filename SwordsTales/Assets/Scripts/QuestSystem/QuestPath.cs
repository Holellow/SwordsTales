namespace QuestSystem
{
   public class QuestPath
   {
      public QuestEvent startEvent;
      public QuestEvent endEvent;

      public QuestPath(QuestEvent from, QuestEvent to)
      {
         startEvent = from;
         endEvent = to;
      }
   }
}
