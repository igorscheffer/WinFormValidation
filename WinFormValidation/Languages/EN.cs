using System.Collections.Generic;

namespace WinFormValidation.Languages {
    internal class EN {
        internal static Dictionary<string, string> Messages = new Dictionary<string, string>() {
            { "required",       "{name} must be filled." },
            { "match",          "{name} must match the field {ruleValue}" },
            { "different",      "{name} must have a different value than the field {ruleValue}" },
            { "numeric",        "{name} must be set to a number" },
            { "checked",        "{name} must be checked" },
            { "unchecked",      "{name} should not be checked" },
            { "email",          "{name} must be a valid E-Mail" },
            { "cpf",            "{name} deve ser um número de CPF válido" },
            { "cnpj",           "{name} deve ser um número de CNPJ válido" },
            { "telefone",       "{name} deve ser um numero de telefone válido" },
            { "nfe",            "{name} deve ser um numero de nf-e válida" },
            { "reais",          "{name} deve ser um valor válido dentro do padrão 0.000,00" },
            { "cep",            "{name} deve ser um CEP válida" },
            { "placa",          "{name} deve ser uma Placa válida" },
            { "min_length",     "{name} must be at least {ruleValue} characters" },
            { "max_length",     "{name} cannot be longer than {ruleValue} characters" },
            { "exact_length",   "{name} it must be exactly {ruleValue} characters" },
            { "regex",          "{name} not formatted correctly" },
            { "in",             "{name} must have exactly {ruleValue}" },
            { "date",           "{name} must be a valid date" }
        };
    }
}
