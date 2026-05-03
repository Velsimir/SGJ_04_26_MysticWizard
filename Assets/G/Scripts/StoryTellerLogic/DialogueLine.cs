namespace G.Scripts.StoryTellerLogic
{
    [System.Serializable]
    public class DialogueLine
    {
        public Speaker Speaker;
        public string Text;

        public DialogueLine(Speaker speaker, string text)
        {
            Speaker = speaker;
            Text = text;
        }
    }

    public enum Speaker
    {
        Fairy, 
        Wizard 
    }
}