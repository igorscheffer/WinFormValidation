using System.Collections.Generic;

namespace WinFormValidation.Languages {
    internal class PT_BR {
        internal static Dictionary<string, string> Messages = new Dictionary<string, string>(){
            { "required",       "{name} deve ser preenchido." },
            { "match",          "{name} deve combinar com o campo {ruleValue}" },
            { "different",      "{name} deve ter um valor diferente do campo {ruleValue}" },
            { "numeric",        "{name} deve ser definido para um número" },
            { "checked",        "{name} deve ser marcado" },
            { "unchecked",      "{name} deve ser não deve ser marcado" },
            { "email",          "{name} deve ser um E-Mail válido" },
            { "cpf",            "{name} deve ser um número de CPF válido" },
            { "cnpj",           "{name} deve ser um número de CNPJ válido" },
            { "telefone",       "{name} deve ser um numero de telefone válido" },
            { "nfe",            "{name} deve ser um numero de nf-e válida" },
            { "reais",          "{name} deve ser um valor válido dentro do padrão 0.000,00" },
            { "cep",            "{name} deve ser um CEP válida" },
            { "placa",          "{name} deve ser uma Placa válida" },
            { "min_length",     "{name} deve ser pelo menos {ruleValue} caracteres" },
            { "max_length",     "{name} não pode ser mais longo do que {ruleValue} caracteres" },
            { "exact_length",   "{name} deve ser exatamente {ruleValue} caracteres" },
            { "regex",          "{name} não está formatado corretamente" },
            { "in",             "{name} deve ter exatamente {ruleValue}" },
            { "date",           "{name} deve ser uma data válida" }
        };
    }
}
