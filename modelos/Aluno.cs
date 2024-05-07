
public class Aluno {

  public int Id { get; set; }
  public string? Nome { get; set; }
  public string? Matricula { get; set; }

  // Relacionamento muitos para muitos com disciplinas
  public List<Disciplina>? Disciplinas { get; set; }

  /**
      Exemplo de POST
      // Pode ser cadastrado aluno com 
      // disciplinas novas (sem Ids) ou 
      // disciplinas existentes (com Ids).
        
      {
        "id": 0,
        "nome": "Carlos",
        "matricula": "567",
        "disciplinas": [
          {
            "id": 0,
            "nome": "Web básico",
            "descricao": "Desenvolvimento Web Básico"
          },
          {
            "id": 1
          }
        ]
      }

  */

}