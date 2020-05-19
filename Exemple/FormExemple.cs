using Exemple.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormValidation;

namespace Exemple {
    public partial class FormExemple : Form {
        ErrorProvider ErrorProvider = new ErrorProvider();

        public FormExemple() {
            InitializeComponent();
        }

        private void OnClickTest(object sender, EventArgs e) {
            try {
                Validation validation = new Validation(this, ErrorProvider);
                
                validation.AddRule(textRequired, "Required", "required");
                validation.AddRule(textNumeric, "Numeric", "numeric");
                validation.AddRule(textEmail, "Email", "email");
                validation.AddRule(cbkChecked, "Checked", "checked");
                validation.AddRule(cbkUnChecked, "UnCkecked", "unchecked");

                validation.AddRule(textMinLength, "Min", "min_length:4");
                validation.AddRule(textMaxLength, "Max", "max_length:4");
                validation.AddRule(textExactLength, "Exact", "exact_length:5");
                //validation.AddRule(textRequired, "Regex", @"regex:");
                validation.AddRule(textIn, "In", "in:any amount, test");
                validation.AddRule(textDate, "Date", "date:dd/MM/yyyy");

                validation.AddRule(textMatch, "Match", "required");
                validation.AddRule(textMatch2, "Match (Repeat)", "match:textMatch|min:3|max:50");

                validation.AddRule(textDifferent, "Different", "");
                validation.AddRule(textDifferent2, "Different (No Repeat)", "different:textDifferent");

                validation.AddRule(textRequiredIf, "RequiredIf", "");
                validation.AddRule(textRequiredIf2, "RequiredIf", "required_if:textRequiredIf");

                validation.Validate();

                if (validation.IsValid()) {
                    MessageBox.Show("Validado com Sucesso.");
                }
                else {
                    validation.ErrorProviderShow();
                    validation.ErrorMessageLineShow();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
