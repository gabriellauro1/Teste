using Microsoft.EntityFrameworkCore;

public static class EnderecosApi
{
  public static void MapEnderecosApi(this WebApplication app)
  {
    var group = app.MapGroup("/enderecos");

    group.MapGet("/", async (BancoDeDados db) =>
      //select * from enderecos
      await db.Enderecos.ToListAsync()
    );

    group.MapPost("/", async (Endereco endereco, BancoDeDados db) =>
      {
        // Tratamento para salvar endereços incluindo cliente.
        if (endereco.Cliente is not null)
        {
          var cliente = await db.Clientes.FindAsync(endereco.Cliente.Id);
          if (cliente is not null)
          {
            endereco.Cliente = cliente;
          }
        }
        else
        {
          return Results.BadRequest("Cliente com Id é obrigatório");
        }

        db.Enderecos.Add(endereco);
        //insert into...
        await db.SaveChangesAsync();

        return Results.Created($"/enderecos/{endereco.Id}", endereco);
      }
    );

    group.MapPut("/{id}", async (int id, Endereco enderecoAlterado, BancoDeDados db) =>
      {
        //select * from enderecos where id = ?
        var endereco = await db.Enderecos.FindAsync(id);
        if (endereco is null)
        {
            return Results.NotFound();
        }
        endereco.Rua = enderecoAlterado.Rua;
        endereco.Numero = enderecoAlterado.Numero;
        endereco.Bairro = enderecoAlterado.Bairro;
        endereco.Cidade = enderecoAlterado.Cidade;
        endereco.CEP = enderecoAlterado.CEP;

        // Tratamento para salvar endereços incluindo cliente.
        if (endereco.Cliente is not null)
        {
          var cliente = await db.Clientes.FindAsync(endereco.Cliente.Id);
          if (cliente is not null)
          {
            endereco.Cliente = cliente;
          }
        }
        else
        {
          return Results.BadRequest("Cliente com Id é obrigatório");
        }

        //update....
        await db.SaveChangesAsync();

        return Results.NoContent();
      }
    );

    group.MapDelete("/{id}", async (int id, BancoDeDados db) =>
      {
        if (await db.Enderecos.FindAsync(id) is Endereco endereco)
        {
          //Operações de exclusão
          db.Enderecos.Remove(endereco);
          //delete from...
          await db.SaveChangesAsync();
          return Results.NoContent();
        }
        return Results.NotFound();
      }
    );
  }
}