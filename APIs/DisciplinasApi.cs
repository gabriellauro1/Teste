using Microsoft.EntityFrameworkCore;

public static class DisciplinasApi
{
  public static void MapDisciplinasApi(this WebApplication app)
  {
    var group = app.MapGroup("/disciplinas");

    group.MapGet("/", async (BancoDeDados db) =>
      {
        //select * from disciplinas
        return await db.Disciplinas.Include(c => c.Alunos).ToListAsync();
      }
    );

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
    group.MapPost("/", async (Disciplina disciplina, BancoDeDados db) =>
      {
        Console.WriteLine($"Disciplina: {disciplina}");

        // Tratamento para salvar alunos com e sem Ids.
        disciplina.Alunos = await SalvarAlunos(disciplina, db);
        
        db.Disciplinas.Add(disciplina);
        //insert into...
        await db.SaveChangesAsync();

        return Results.Created($"/disciplinas/{disciplina.Id}", disciplina);
      }
    );

    async Task<List<Aluno>> SalvarAlunos(Disciplina disciplina, BancoDeDados db)
    {
      List<Aluno> alunos = new();
      if (disciplina is not null && disciplina.Alunos is not null 
          && disciplina.Alunos.Count > 0){

        foreach (var aluno in disciplina.Alunos)
        {
          Console.WriteLine($"Aluno: {aluno}");
          if (aluno.Id > 0)
          {
            var aExistente = await db.Alunos.FindAsync(aluno.Id);
            if (aExistente is not null)
            {
              alunos.Add(aExistente);
            }
          }
          else
          {
            alunos.Add(aluno);
          }
        }
      }
      return alunos;
    }

    group.MapPut("/{id}", async (int id, Disciplina disciplinaAlterada, BancoDeDados db) =>
      {
        //select * from disciplinas where id = ?
        var disciplina = await db.Disciplinas.FindAsync(id);
        if (disciplina is null)
        {
            return Results.NotFound();
        }
        disciplina.Nome = disciplinaAlterada.Nome;
        disciplina.Descricao = disciplinaAlterada.Descricao;

        // Tratamento para salvar alunos com e sem Ids.
        disciplina.Alunos = await SalvarAlunos(disciplina, db);

        //update....
        await db.SaveChangesAsync();

        return Results.NoContent();
      }
    );

    group.MapDelete("/{id}", async (int id, BancoDeDados db) =>
      {
        if (await db.Disciplinas.FindAsync(id) is Disciplina disciplina)
        {
          //Operações de exclusão
          db.Disciplinas.Remove(disciplina);
          //delete from...
          await db.SaveChangesAsync();
          return Results.NoContent();
        }
        return Results.NotFound();
      }
    );
  }
}