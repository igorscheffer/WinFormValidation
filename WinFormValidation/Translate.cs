using System;
using System.Collections.Generic;
using WinFormValidation.Languages;

namespace WinFormValidation {
    public class Translate {
        private string _Message;
        private static Dictionary<string, string> _Language = PT_BR.Messages;

        public dynamic Rules { get; set; }
        public string Rule { get; set; }
        public string RuleValue { get; set; }
        public string Message {
            get {
                GenerateMessage();
                return _Message;
            }
        }

        private static Dictionary<string, dynamic> Languages = new Dictionary<string, dynamic>() {
            { "PT_BR", PT_BR.Messages },
            { "EN", EN.Messages }
        };

        public static void setLanguage(string language) {
            if (!string.IsNullOrWhiteSpace(language)) {
                if (Languages.ContainsKey(language)) {
                    _Language = Languages[language];
                }
                else {
                    throw new Exception("Linguagem não encontrada.");
                }
            }
        }

        public void GenerateMessage() {
            string name = Rules.Name;
            string rule = Rule;
            string ruleValue = RuleValue ?? string.Empty;

            string Message = _Language[rule];
            Message = Message.Replace("{name}", name);
            Message = Message.Replace("{ruleValue}", ruleValue);

            _Message = Message;
        }
    }
}
