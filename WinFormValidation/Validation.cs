﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WinFormValidation.Properties;

namespace WinFormValidation {
    public class Validation {
        Form ValidateForm;
        ErrorProvider ErrorProvider;

        private List<Rules> Rules = new List<Rules>();
        private List<Valid> Valid = new List<Valid>();
        private List<Errors> Errors = new List<Errors>();

        #region métodos
        /// <summary>
        /// Cria uma nova instância da classe de Validação
        /// </summary>
        /// <param name="Form">Formulario de Entrada</param>
        /// <param name="ErrorProvider">ErrorProvider do Formulario de Entrada</param>
        public Validation(Form Form = null, ErrorProvider ErrorProvider = null) {
            ValidateForm = Form;
            this.ErrorProvider = ErrorProvider;
        }
        #endregion

        #region
        /// <summary>
        /// Cria uma regra de Validação
        /// </summary>
        /// <param name="Component">Componente a ser validado</param>
        /// <param name="Name">Nome do componente (será exibido na string do erro)</param>
        /// <param name="Rule">Regras a seram aplicadas no Component</param>
        public void AddRule(dynamic Component, string Name, string Rule) {
            Rules.Add(new Rules { Component = Component, Name = Name, Rule = Rule });
        }
        #endregion

        #region atributo
        /// <summary>
        /// Atribui a Linguagem das mensagens de erros.
        /// </summary>
        public string Language {
            set {
                if (!string.IsNullOrWhiteSpace(value)) {
                    Translate.setLanguage(value);
                }
            }
        }
        #endregion

        private void ValidateMinLength(Rules Rules, string Rule, string RuleValue) {
            if (!Rules.Optional) {
                if (Rules.Value.Length >= Convert.ToInt16(RuleValue)) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
            }
        }

        private void ValidateMaxLength(Rules Rules, string Rule, string RuleValue) {
            if (Rules.Value.Length <= Convert.ToInt16(RuleValue)) {
                Valid.Add(new Valid { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
            }
            else {
                Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
            }
        }

        private void ValidateExactLength(Rules Rules, string Rule, string RuleValue) {
            if (!Rules.Optional) {
                if (Rules.Value.Length == Convert.ToInt16(RuleValue)) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
            }
        }

        private void ValidateMinValue(Rules Rules, string Rule, string RuleValue) {
            if (!Rules.Optional) {
                int Value = 0;
                bool MatchValue = int.TryParse(Rules.Value, out Value);
                if (Value >= Convert.ToInt32(RuleValue)) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
            }
        }

        private void ValidateMaxValue(Rules Rules, string Rule, string RuleValue) {
            if (!Rules.Optional) {
                int Value = 0;
                bool MatchValue = int.TryParse(Rules.Value, out Value);
                if (Value <= Convert.ToInt32(RuleValue)) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
            }
        }

        private void ValidateRegExp(Rules Rules, string Rule, string RuleValue) {
            if (!Rules.Optional) {
                var regexp = RuleValue;
                var match = Regex.Match(RuleValue, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
            }
        }

        private void ValidateIn(Rules Rules, string Rule, string RuleValue) {
            if (!Rules.Optional) {
                string[] inRules = RuleValue.Split(',');

                if (Array.Exists(inRules, find => find.ToLower() == Rules.Value.ToLower())) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
            }
        }

        private void ValidateDate(Rules Rules, string Rule, string RuleValue) {
            if (!Rules.Optional) {
                DateTime date;

                bool validate = DateTime.TryParseExact(Rules.Value, RuleValue, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date);

                if (validate) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = RuleValue });
                }
            }
        }

        private void ValidateRequired(Rules Rules, string Rule) {
            if (!string.IsNullOrWhiteSpace(Rules.Value)) {
                Valid.Add(new Valid { Rules = Rules, Rule = Rule });
            }
            else {
                Errors.Add(new Errors { Rules = Rules, Rule = Rule });
            }
        }

        private void ValidateRequiredIf(Rules Rules, string Rule, string RuleValue) {
            string[] inRules = RuleValue.Split(',');

            Rules RequiredIfComponent = this.Rules.Find(find => find.Component.Name == inRules[0]);

            if (RequiredIfComponent != null) {
                if (RuleValue.Contains(",")) {
                    if (Array.Exists(inRules, find => find.ToLower() == RequiredIfComponent.Value.ToLower())) {
                        ValidateRule(Rules, true);
                    }
                }
                else {
                    if (!string.IsNullOrWhiteSpace(RequiredIfComponent.Value)) {
                        ValidateRule(Rules, true);
                    }
                }
            }
        }

        private void ValidateMatch(Rules Rules, string Rule, string RuleValue) {
            Rules Match = this.Rules.Find(find => find.Component.Name == RuleValue);

            if (Rules.Value == Match.Value) {
                Valid.Add(new Valid { Rules = Rules, Rule = Rule });
            }
            else {
                Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = Match.Name });
            }
        }

        private void ValidateDifferent(Rules Rules, string Rule, string RuleValue) {
            Rules Different = this.Rules.Find(find => find.Component.Name == RuleValue);

            if (Rules.Value != Different.Value) {
                Valid.Add(new Valid { Rules = Rules, Rule = Rule });
            }
            else {
                Errors.Add(new Errors { Rules = Rules, Rule = Rule, RuleValue = Different.Name });
            }
        }

        private void ValidateEmail(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
                var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }

        private void ValidateNumeric(Rules Rules, string Rule) {
            var regexp = @"^([0-9]*)$";
            var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);
            if (match.Success) {
                Valid.Add(new Valid { Rules = Rules, Rule = Rule });
            }
            else {
                Errors.Add(new Errors { Rules = Rules, Rule = Rule });
            }
        }

        private void ValidateInteger(Rules Rules, string Rule) {
            int Val = 0;
            bool match = int.TryParse(Rules.Value, out Val);
            if (match) {
                Valid.Add(new Valid { Rules = Rules, Rule = Rule });
            }
            else {
                Errors.Add(new Errors { Rules = Rules, Rule = Rule });
            }
        }

        private void ValidateChecked(Rules Rules, string Rule) {
            if (Rules.Value.ToLower() == Convert.ToString(true).ToLower()) {
                Valid.Add(new Valid { Rules = Rules, Rule = Rule });
            }
            else {
                Errors.Add(new Errors { Rules = Rules, Rule = Rule });
            }
        }

        private void ValidateUnChecked(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                if (Rules.Value.ToLower() == Convert.ToString(false).ToLower()) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }

        private void ValidateCPF(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"^(\d{3})([\.]\d{3})([\.]\d{3})(\-\d{2})$";
                var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }

        private void ValidateCNPJ(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"^(\d{2})([\.]\d{3}){2}(\/\d{4})(\-\d{2})$";
                var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }

        private void ValidateTelefone(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"\(\d{2,}\) \d{4,}\-\d{4}$";
                var match = Regex.Match(Rules.Value.Trim(), regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }

        private void ValidateNFE(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"^(\d{0,3}([\.]\d{3}){2})$";
                var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }

        private void ValidateReais(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"^(\d{1,3}(\.\d{3})*)(\,\d{2})$";
                var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }
        /*
        private void ValidateQuantidade(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"^\d+(\,\d{2})$";
                var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }

        private void ValidatePeso(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"^\d+(\.\d{3})$";
                var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }
        */
        private void ValidateCEP(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"^(\d{5})(\-\d{3})$";
                var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }

        private void ValidatePlaca(Rules Rules, string Rule) {
            if (!Rules.Optional) {
                var regexp = @"^([A-Z]{3})(\-\d\w\d{2})$";
                var match = Regex.Match(Rules.Value, regexp, RegexOptions.IgnoreCase);

                if (match.Success) {
                    Valid.Add(new Valid { Rules = Rules, Rule = Rule });
                }
                else {
                    Errors.Add(new Errors { Rules = Rules, Rule = Rule });
                }
            }
        }

        private void ValidateRule(Rules Rule, bool ForceRequired = false) {
            if (!string.IsNullOrWhiteSpace(Rule.Rule)) {
                if (ForceRequired) ValidateRequired(Rule, "required");

                string[] split_rules = Rule.Rule.Split('|');

                foreach (string split_rule in split_rules) {
                    if (split_rule.Contains(':')) {
                        string[] sub_split_rules = split_rule.Split(new char[] { ':' }, 2, StringSplitOptions.None);

                        switch (sub_split_rules[0]) {
                            case "required_if": if (!ForceRequired) { ValidateRequiredIf(Rule, sub_split_rules[0], sub_split_rules[1]); } break;
                            case "match": ValidateMatch(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                            case "different": ValidateDifferent(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                            case "min_length": ValidateMinLength(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                            case "max_length": ValidateMaxLength(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                            case "min_value": ValidateMinValue(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                            case "max_value": ValidateMaxValue(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                            case "exact_length": ValidateExactLength(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                            case "regexp": ValidateRegExp(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                            case "in": ValidateIn(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                            case "date": ValidateDate(Rule, sub_split_rules[0], sub_split_rules[1]); break;
                        }
                    }
                    else {
                        switch (split_rule) {
                            case "required": ValidateRequired(Rule, split_rule); break;
                            case "numeric": ValidateNumeric(Rule, split_rule); break;
                            case "checked": ValidateChecked(Rule, split_rule); break;
                            case "unchecked": ValidateUnChecked(Rule, split_rule); break;
                            case "email": ValidateEmail(Rule, split_rule); break;
                            case "cpf": ValidateCPF(Rule, split_rule); break;
                            case "cnpj": ValidateCNPJ(Rule, split_rule); break;
                            case "telefone": ValidateTelefone(Rule, split_rule); break;
                            case "nfe": ValidateNFE(Rule, split_rule); break;
                            case "reais": ValidateReais(Rule, split_rule); break;
                            //case "quantidade": ValidateQuantidade(Rule, split_rule); break;
                            //case "peso": ValidatePeso(Rule, split_rule); break;
                            case "cep": ValidateCEP(Rule, split_rule); break;
                            case "placa": ValidatePlaca(Rule, split_rule); break;
                        }
                    }
                }
            }
        }

        private void FocusFirstError() {
            if (ValidateForm != null) {
                Errors error = Errors.First<Errors>();
                Control Component = ValidateForm.Controls.Find(error.Rules.Component.Name, true)[0];
                Component.Focus();
            }
        }

        #region
        /// <summary>
        /// Valida as Regras
        /// </summary>
        public void Validate() {
            foreach (Rules Rule in Rules) {
                if (!string.IsNullOrWhiteSpace(Rule.Rule)) {
                    ValidateRule(Rule);
                }
            }
        }
        #endregion

        #region
        /// <summary>
        /// Verifica se os Components são Validos
        /// </summary>
        /// <returns>true ou false</returns>
        public bool IsValid() {
            if (Errors.Count() == 0) {
                if (ErrorProvider != null) {
                    ErrorProvider.Clear();
                }

                return true;
            }
            else {
                return false;
            }
        }
        #endregion

        #region
        /// <summary>
        /// Retorna os Components validos
        /// </summary>
        /// <returns></returns>
        public List<Valid> GetValid() {
            return Valid;
        }
        #endregion

        #region
        /// <summary>
        /// Retorna os Components não validos
        /// </summary>
        /// <returns></returns>
        public List<Errors> GetErrors() {
            return Errors;
        }
        #endregion

        #region
        /// <summary>
        /// Limpa todos os dados do Formulario
        /// </summary>
        public void CleanAllComponents() {
            if (ErrorProvider != null) {
                ErrorProvider.Clear();
            }

            foreach (Rules Rule in Rules) {
                if (Rule.Component.GetType().Name == "ComboBox") {
                    Rule.Component.SelectedValue = -1;
                }
                else {
                    Rule.Component.Text = "";
                }
            }
        }
        #endregion

        #region
        /// <summary>
        /// Exibe um icone em todos Components não validos
        /// </summary>
        /// <param name="newIcon">Icone que será usado para mostrar o erro</param>
        /// <param name="Width">Largura do Icone</param>
        /// <param name="Height">Altura do Icone</param>
        /// <param name="Padding">Preenchimento do Icone</param>
        /// <param name="RightToLeft">Exibir Icone do lado esquerdo do Component</param>
        public void ErrorProviderShow(dynamic newIcon = null, int Width = 20, int Height = 20, int Padding = 0, bool RightToLeft = false) {
            if (ErrorProvider != null) {
                Bitmap DefaultIcon = Resources.error_1.ToBitmap();

                if (newIcon != null) {
                    if (newIcon.GetType().Name == "Bitmap") {
                        DefaultIcon = newIcon;
                    }
                    else if (newIcon.GetType().Name == "Icon") {
                        DefaultIcon = newIcon.ToBitMap();
                    }
                }

                Bitmap ErrorIconBit = new Bitmap(DefaultIcon, Width, Height);
                System.Drawing.Icon ErrorIcon = System.Drawing.Icon.FromHandle(ErrorIconBit.GetHicon());

                ErrorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                ErrorProvider.RightToLeft = RightToLeft;
                ErrorProvider.Icon = ErrorIcon;
                ErrorProvider.Clear();

                foreach (Errors Erro in GetErrors()) {
                    this.ErrorProvider.SetIconPadding(Erro.Rules.Component, Padding);
                    this.ErrorProvider.SetError(Erro.Rules.Component, Erro.Message);
                }
            }
        }
        #endregion

        #region
        /// <summary>
        /// Exibe uma MessageBox com os Campos não validos e erros
        /// </summary>
        /// <param name="Title">Titulo da MessageBox</param>
        public void ErrorMessageBoxShow(string Title = "Oooops...") {
            string ShowMessage = "";
            Errors Last = GetErrors().Last();

            foreach (Errors Erro in GetErrors()) {
                ShowMessage += Erro.Message + "\n";
                if (!Erro.Equals(Last)) ShowMessage += "\n";
            }

            MessageBox.Show(ShowMessage, Title);
        }
        #endregion

        #region
        /// <summary>
        /// Exibe uma label embaixo todos Components não validos
        /// </summary>
        /// <param name="MarginTop">Margem Superior</param>
        /// <param name="MarginLeft">Margem Esquerda</param>
        /// <param name="ForeColor">Cor do Texto Padrão(Color.Red)</param>
        public void ErrorMessageLineShow(int MarginTop = 0, int MarginLeft = 0, Color? ForeColor = null) {
            if (ValidateForm != null) {
                Label ErrorLabel;
                List<Control> labelRemove = ValidateForm.Controls.Cast<Control>().Where(find => (string)find.Tag == "WFVEISF").ToList();

                if (labelRemove.Count > 0) {
                    foreach (Control label in labelRemove) {
                        ValidateForm.Controls.Remove(label);
                    }
                }

                foreach (Errors Error in GetErrors()) {
                    int X = Error.Rules.Component.Location.X;
                    int Y = Error.Rules.Component.Location.Y;
                    int WComp = Error.Rules.Component.Width;
                    int HComp = Error.Rules.Component.Height;

                    ErrorLabel = new Label();
                    ErrorLabel.Tag = "WFVEISF";
                    ErrorLabel.Text = Error.Message;
                    ErrorLabel.ForeColor = ForeColor ?? Color.Red;
                    ErrorLabel.Location = new Point((X + (MarginLeft)), (Y + HComp + 2 + (MarginTop)));
                    ErrorLabel.BackColor = Color.Transparent;
                    ErrorLabel.Size = new Size(WComp, ErrorLabel.PreferredHeight);
                    ErrorLabel.AutoEllipsis = true;

                    ValidateForm.Controls.Add(ErrorLabel);

                    ErrorLabel.BringToFront();
                }

                FocusFirstError();
            }
        }
        #endregion
    }
}
