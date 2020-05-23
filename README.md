WinFormValidation - C# Validação de Formulários
========================
Biblioteca autônoma para validação de formulários do WinForm.

## Instalação

 1. Faça o Download da DLL do WinFormValidation. [Clique aqui para fazer download](https://github.com/igorscheffer/WinFormValidation/blob/master/WinFormValidation/bin/Release/WinFormValidation.dll)
 2. Abra seu projeto no **Visual Studio**.
 3. Clique em **Projeto**.
 4. Clique em **Adicionar Referencias**.
 5. Clique em **procurar** e a **DLL**.
 6. Clique em **OK**.

## Exemplo de Uso
Existem três maneiras de exibir uma mensagem de erro com esta biblioteca. Usando `ErrorMessageBoxShow`, `ErrorProviderShow`, `ErrorMessageLineShow`.
Usando **ErrorProviderShow**:
```
using WinFormValidation;

...
	ErrorProvider ErrorProvider = new ErrorProvider();
...

private void OnClickCadastrar(object sender, EventArgs e){
 	 try{
      		Validation validation = new Validation(this, ErrorProvider);

      		validation.AddRule(textBoxNome, "Nome Completo", "required|max_length:40");
      		validation.AddRule(textBoxEmail, "E-Mail", "required|email");
		validation.AddRule(textBoxTelefone, "Telefone", "required|telefone");
      		validation.AddRule(TimePickerNascimento, "Data de Nascimento", "required|date:dd/MM/yyyy");
      		validation.Validate();

      		if (validation.IsValid()) {
			MessageBox.Show("Validado com Sucesso.");
  		}
		else {
			validation.ErrorProviderShow();
		}
	}
  	catch(Exception ex){
    		MessageBox.Show(ex.Message);
  	}
...
```
## Regras Disponíveis.

> Clique para mostrar detalhes.

<details><summary><strong>required</strong></summary>
O componente sob está regra, deve estar presente e não 'vazio'.
</details>

<details><summary><strong>required_if</strong>:outro_componente,valor_1,valor_2,...</summary>
O componente sob esta regra, deve estar presente e não estar vazio se o campo outro componente for igual a qualquer valor.
Por exemplo `required_if:outro_component,1,sim,ativo` será necessário se o valor de `outro_campo` for `1`, `'1'`, `'sim'`, ou `'ativo'`.
</details>

<details><summary><strong>numeric</strong></summary>
O componente sob esta regra, deve ser numérico.
</details>

<details><summary><strong>checked</strong></summary>
O componente sob esta regra, deve estar selecionado.
</details>

<details><summary><strong>unchecked</strong></summary>
O componente sob esta regra, não deve estar selecionado selecionado.
</details>

<details><summary><strong>email</strong></summary>
O componente sob esta regra, deve ter um endereço de e-mail valido.
</details>

<details><summary><strong>match</strong>:outro_componente</summary>
O componente sob esta regra, deve estar presente e ter o valor igual ao outro campo selecionado.
</details>

<details><summary><strong>different</strong>:outro_componente</summary>
O componente sob esta regra, deve estar presente e não ter o valor igual ao outro campo selecionado.
</details>

<details><summary><strong>min_length</strong>:número</summary>
O componente sob esta regra, deve ter um tamanho maior ou igual ao número fornecido.
</details>

<details><summary><strong>min_length</strong>:número</summary>
O componente sob esta regra, deve ter um tamanho menor ou igual ao número fornecido.
</details>

<details><summary><strong>exact_length</strong>:número</summary>
O componente sob esta regra, deve ter um tamanho igual ao número fornecido.
</details>

<details><summary><strong>regexp</strong>:sua_expressao_regular</summary>
O campo sob esta regra, deve corresponder a expressão regular especificada.
</details>

<details><summary><strong>in</strong>:valor_1,valor_2...</summary>
O campo sob esta regra, deve ser igual a um dos valores fornecidos.
</details>

<details><summary><strong>date</strong>:formato</summary>
O campo sob esta regra, deve ser um formato de data válido.
</details>

## Formato de Mensagem de erro
Você pode optar por exibir as mensagens de erros de 3 formas. Usando `ErrorMessageBoxShow`, `ErrorProviderShow`, `ErrorMessageLineShow`.
```
	...
		if (validation.IsValid()) {
			...
  		}
		else {
			// Exibe uma MessageBox com as mensagens de erro.
			validation.ErrorMessageBoxShow();
			
			// Exibe um icone com a mensagem de cada componente.
			validation.ErrorProviderShow();

			// Exibe uma labael com a mensagem de cada componente.
			validation.ErrorMessageLineShow();
		}
	...
```

## Selecionar Linguagem
Você pode selecionar a linguagem das mensagens de erro usando o `Language` antes da validação (Padrão "PT_BR").
```
		...
		Validation validation = new Validation(this, ErrorProvider);
		...
  		validation.Language = "EN";
  		...
  		validation.Validate();
		...
```
