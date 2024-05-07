using Microsoft.EntityFrameworkCore;

public static class ClientesApi
{
  public static void MapClientesApi(this WebApplication app)
  {
    var group = app.MapGroup("/clientes");

    group.MapGet("/", async (BancoDeDados db) =>
      {
        //select * from clientes
        return await db.Clientes.Include(c => c.Enderecos).ToListAsync();
      }
    );

    /**
      Exemplo de POST
      // Pode ser cadastrado cliente com 
      // endereços novos (sem Ids) ou 
      // endereços existentes (com Ids).
        
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
            "id": 6
          }
        ]
      }

    */
    group.MapPost("/", async (Cliente cliente, BancoDeDados db) =>
      {
        Console.WriteLine($"Cliente: {cliente}");

        // Tratamento para salvar endereços com e sem Ids.
        cliente.Enderecos = await SalvarEnderecos(cliente, db);
        
        db.Clientes.Add(cliente);
        //insert into...
        await db.SaveChangesAsync();

        return Results.Created($"/clientes/{cliente.Id}", cliente);
      }
    );

    async Task<List<Endereco>> SalvarEnderecos(Cliente cliente, BancoDeDados db)
    {
      List<Endereco> enderecos = new();
      if (cliente is not null && cliente.Enderecos is not null 
          && cliente.Enderecos.Count > 0){

        foreach (var endereco in cliente.Enderecos)
        {
          Console.WriteLine($"Endereço: {endereco}");
          if (endereco.Id > 0)
          {
            var eExistente = await db.Enderecos.FindAsync(endereco.Id);
            if (eExistente is not null)
            {
              enderecos.Add(eExistente);
            }
          }
          else
          {
            enderecos.Add(endereco);
          }
        }
      }
      return enderecos;
    }

    group.MapPut("/{id}", async (int id, Cliente clienteAlterado, BancoDeDados db) =>
      {
        //select * from clientes where id = ?
        var cliente = await db.Clientes.FindAsync(id);
        if (cliente is null)
        {
            return Results.NotFound();
        }
        cliente.Nome = clienteAlterado.Nome;
        cliente.Telefone = clienteAlterado.Telefone;
        cliente.Email = clienteAlterado.Email;
        cliente.CPF = clienteAlterado.CPF;

        // Tratamento para salvar endereços com e sem Ids.
        cliente.Enderecos = await SalvarEnderecos(cliente, db);

        //update....
        await db.SaveChangesAsync();

        return Results.NoContent();
      }
    );

    group.MapDelete("/{id}", async (int id, BancoDeDados db) =>
      {
        if (await db.Clientes.FindAsync(id) is Cliente cliente)
        {
          //Operações de exclusão
          db.Clientes.Remove(cliente);
          //delete from...
          await db.SaveChangesAsync();
          return Results.NoContent();
        }
        return Results.NotFound();
      }
    );
  }
}