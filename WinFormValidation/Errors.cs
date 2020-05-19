namespace WinFormValidation {
    public class Errors {
        public Rules Rules { get; set; }
        public string Rule { get; set; }
        public string RuleValue { get; set; }
        public string Message {
            get {
                Translate Translate = new Translate { Rules = Rules, Rule = Rule, RuleValue = RuleValue };
                return Translate.Message;
            }
        }
    }
}
