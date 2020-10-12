using UnityEngine;
using UnityEngine.UI;

namespace Dialogue_System
{
    public class DialogueLine : DialogueSystemClass
    {
        private Text _textHolder;
        
        [Header ("Text Options")]
        [SerializeField] private Color textColor;
        [SerializeField] private Font textFont;
        
        [SerializeField] private float delay;
        [SerializeField] private float delayBetweenLines;
        
        [SerializeField] private string input;

        [Header("Sound")] 
        [SerializeField] private AudioClip sound;

        [Header("Character Image")] 
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;
       
        private void Awake()
        {
            _textHolder = GetComponent<Text>();
            _textHolder.text = "";
            
            imageHolder.sprite = characterSprite;
            imageHolder.preserveAspect = true;
        }

        private void Start()
        {
            StartCoroutine(WriteText(input, _textHolder,delay,textColor,textFont,sound,delayBetweenLines));
        }
    }
}
