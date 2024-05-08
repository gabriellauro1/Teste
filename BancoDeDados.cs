

using Microsoft.EntityFrameworkCore;

public class BancoDeDados : DbContext
{

    //Configuração da conexão
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseMySQL("server=localhost;port=3306;database=exemplo;user=root;password=positivo");

    }

    // Mapeamento das tabelas
    // dotnet ef migrations add "NomeDaMigracao"
    // dotnet ef database update
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Disciplina> Disciplinas { get; set; }

}