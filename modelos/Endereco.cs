

public class Endereco {

    public int Id { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? CEP { get; set; }
    
    // Relacionamento muitos para 1
    public Cliente? Cliente { get; set; }

    /**
      Exemplo de POST   
      // Para vincular o cliente é 
      // necessário apenas o id.
      {
        "rua": "Rua z",
        "numero": "4",
        "bairro": "C",
        "cidade": "Curitiba",
        "cep": "656565756",
        "cliente": {
            "id": 2 
        }
      }

    */

    public override string ToString()
    {
        return $"Id: {Id}, Rua: {Rua}, Numero: {Numero}, Bairro: {Bairro}, Cidade: {Cidade}, CEP: {CEP}";
    }
}