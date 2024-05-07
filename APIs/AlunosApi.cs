using Microsoft.EntityFrameworkCore;

public static class AlunosApi
{
  public static void MapAlunosApi(this WebApplication app)
  {
    var group = app.MapGroup("/alunos");

    group.MapGet("/", async (BancoDeDados db) =>
      {
        //select * from alunos
        return await db.Alunos.Include(c => c.Disciplinas).ToListAsync();
      }
    );

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
    group.MapPost("/", async (Aluno aluno, BancoDeDados db) =>
      {
        Console.WriteLine($"Aluno: {aluno}");

        // Tratamento para salvar disciplinas com e sem Ids.
        aluno.Disciplinas = await SalvarDisciplinas(aluno, db);
        
        db.Alunos.Add(aluno);
        //insert into...
        await db.SaveChangesAsync();

        return Results.Created($"/alunos/{aluno.Id}", aluno);
      }
    );

    async Task<List<Disciplina>> SalvarDisciplinas(Aluno aluno, BancoDeDados db)
    {
      List<Disciplina> disciplinas = new();
      if (aluno is not null && aluno.Disciplinas is not null 
          && aluno.Disciplinas.Count > 0){

        foreach (var disciplina in aluno.Disciplinas)
        {
          Console.WriteLine($"Disciplina: {disciplina}");
          if (disciplina.Id > 0)
          {
            var dExistente = await db.Disciplinas.FindAsync(disciplina.Id);
            if (dExistente is not null)
            {
              disciplinas.Add(dExistente);
            }
          }
          else
          {
            disciplinas.Add(disciplina);
          }
        }
      }
      return disciplinas;
    }

    group.MapPut("/{id}", async (int id, Aluno alunoAlterado, BancoDeDados db) =>
      {
        //select * from alunos where id = ?
        var aluno = await db.Alunos.FindAsync(id);
        if (aluno is null)
        {
            return Results.NotFound();
        }
        aluno.Nome = alunoAlterado.Nome;
        aluno.Matricula = alunoAlterado.Matricula;

        // Tratamento para salvar disciplinas com e sem Ids.
        aluno.Disciplinas = await SalvarDisciplinas(aluno, db);

        //update....
        await db.SaveChangesAsync();

        return Results.NoContent();
      }
    );

    group.MapDelete("/{id}", async (int id, BancoDeDados db) =>
      {
        if (await db.Alunos.FindAsync(id) is Aluno aluno)
        {
          //Operações de exclusão
          db.Alunos.Remove(aluno);
          //delete from...
          await db.SaveChangesAsync();
          return Results.NoContent();
        }
        return Results.NotFound();
      }
    );
  }
}