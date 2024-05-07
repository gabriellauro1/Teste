
public class Disciplina {

  public int Id { get; set; }
  public string? Nome { get; set; }
  public string? Descricao { get; set; }

  // Relacionamento muitos para muitos com alunos
  public List<Aluno>? Alunos { get; set; }

  /**
      Exemplo de POST
      // Pode ser cadastrado disciplina com 
      // alunos novos (sem Ids) ou 
      // alunos existentes (com Ids).
        
      {
        "id": 0,
        "nome": "Tópicos Especiais",
        "descricao": "Desenvolvimento web avançado",
        "alunos": [
          {
            "id": 0,
            "nome": "Adriano",
            "matricula": "789"
          },
          {
            "id": 1
          }
        ]
      }

  */

}