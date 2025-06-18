using System.Data;
using CadastroProdutos.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace CadastroProdutos.Repositorio
{
    // Define a classe UsuarioRepositorio, responsável por operações de acesso a dados para a entidade Usuario.
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        // Declara um campo privado somente leitura para armazenar a string de conexão com o MySQL.
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");
       
        // Define um método público para adicionar um novo usuário ao banco de dados. Recebe um objeto 'Usuario' como parâmetro.
        public void AdicionarProduto(Produto produto)
        {
            /* Cria uma nova instância da conexão MySQL dentro de um bloco 'using'.
             Isso garante que a conexão será fechada e descartada corretamente após o uso, mesmo em caso de erro.*/
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL.

                conexao.Open();
                /* Cria um novo comando SQL para inserir dados na tabela 'Usuario'. Os valores para Nome, Email e Senha são passados como parâmetros
                 (@Nome, @Email, @Senha) para evitar SQL Injection.*/

                MySqlCommand cmd = new("INSERT INTO tbProdutos (Nome, Descricao, Preco, Quantidade) VALUES (@Nome,@Descricao,@Preco,@Quantidade)", conexao);
                // Adiciona um parâmetro ao comando SQL para o campo 'Nome', utilizando o valor da propriedade 'Nome' do objeto 'usuario'.
                cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                // Adiciona um parâmetro ao comando SQL para o campo 'Email', utilizando o valor da propriedade 'Email' do objeto 'usuario'.
                cmd.Parameters.AddWithValue("@Descricao", produto.Descricao);
                // Adiciona um parâmetro ao comando SQL para o campo 'Senha', utilizando o valor da propriedade 'Senha' do objeto 'usuario'.
                cmd.Parameters.AddWithValue("@Preco", produto.Preco);
                cmd.Parameters.AddWithValue("@Quantidade", produto.Quantidade);
                // Executa o comando SQL INSERT no banco de dados. Retorna o número de linhas afetadas.
                cmd.ExecuteNonQuery();
                // Fecha a conexão com o banco de dados (embora o 'using' já faria isso, só garante o fechamento).
                conexao.Close();
            }
        }
     
        // Método para listar todos os Produtos do banco de dados
        public IEnumerable<Produto> TodosProdutos()
        {
            // Cria uma nova lista para armazenar os objetos Produto
            List<Produto> Productlist = new List<Produto>();

            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar todos os registros da tabela 'tbProdutos'
                MySqlCommand cmd = new MySqlCommand("SELECT * from tbProdutos", conexao);

                // Cria um adaptador de dados para preencher um DataTable com os resultados da consulta
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // Cria um novo DataTable
                DataTable dt = new DataTable();
                // metodo fill- Preenche o DataTable com os dados retornados pela consulta
                da.Fill(dt);
                // Fecha explicitamente a conexão com o banco de dados 
                conexao.Close();

                // interage sobre cada linha (DataRow) do DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Cria um novo objeto Cliente e preenche suas propriedades com os valores da linha atual
                    Productlist.Add(
                                new Produto
                                {
                                    IdProd = Convert.ToInt32(dr["idprod"]), // Converte o valor da coluna "codigo" para inteiro
                                    Nome = ((string)dr["nome"]), // Converte o valor da coluna "nome" para string
                                    Descricao = ((string)dr["descricao"]), // Converte o valor da coluna "telefone" para string
                                    Preco = Convert.ToDecimal(dr["preco"]), // Converte o valor da coluna "email" para string
                                    Quantidade = Convert.ToInt32(dr["quantidade"]), // Converte o valor da coluna "email" para string
                                });
                }
                // Retorna a lista de todos os produtos
                return Productlist;
            }
        }

            // Método para buscar um Produto específico pelo seu código (Id)
            public Produto ObterProduto(int IdProd)
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para selecionar um registro da tabela 'Produto' com base no código
                    MySqlCommand cmd = new MySqlCommand("SELECT * from tbProdutos where idprod=@idprod ", conexao);

                    // Adiciona um parâmetro para o código a ser buscado, definindo seu tipo e valor
                    cmd.Parameters.AddWithValue("@idprod", IdProd);

                    // Cria um adaptador de dados (não utilizado diretamente para ExecuteReader)
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                    // Declara um leitor de dados do MySQL
                    MySqlDataReader dr;
                    // Cria um novo objeto Produto para armazenar os resultados
                    Produto Produto = new Produto();

                    /* Executa o comando SQL e retorna um objeto MySqlDataReader para ler os resultados
                    CommandBehavior.CloseConnection garante que a conexão seja fechada quando o DataReader for fechado*/

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    // Lê os resultados linha por linha
                    while (dr.Read())
                    {
                        // Preenche as propriedades do objeto Produto com os valores da linha atual
                        Produto.IdProd = Convert.ToInt32(dr["id"]);//propriedade Id e convertendo para int
                        Produto.Nome = (string)(dr["nome"]); // propriedade Nome e passando string
                        Produto.Descricao = (string)(dr["descricao"]); //propriedade descricao e passando string
                        Produto.Preco = Convert.ToDecimal(dr["preco"]); //propriedade preco e passando int
                        Produto.Quantidade = Convert.ToInt32(dr["quantidade"]);
                    }
                    // Retorna o objeto Produto encontrado (ou um objeto com valores padrão se não encontrado)
                    return Produto;
                }
            }

        
    }
}
