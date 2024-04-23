

public class Cliente
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    
    // Relacionamento 1 para muitos
    public List<Endereco>? Enderecos { get; set; }

    /**
      Exemplo de POST
      // Pode ser cadastrado cliente com 
      // endereços novos (sem Ids) ou 
      // endereços existente (com Ids).
        
      {
        "nome": "João",
        "cpf": "22222",
        "telefone": "2222",
        "email": "e@e.com",
        "enderecos": [
          {
            "rua": "Rua a",
            "numero": "1",
            "bairro": "C",
            "cidade": "Curitiba",
            "cep": "44444"
          },
          {
            "id": 6,
            "rua": "Rua a",
            "numero": "1",
            "bairro": "C",
            "cidade": "Curitiba",
            "cep": "44444"
          }
        ]
      }

    */

    public override string ToString()
    {
        return $"Id: {Id}, Nome: {Nome}, CPF: {CPF}, Telefone: {Telefone}, Email: {Email}";
    }

}