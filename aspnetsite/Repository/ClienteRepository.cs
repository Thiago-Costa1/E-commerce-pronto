using aspnetsite.Models;
using aspnetsite.Models.Constant;
using aspnetsite.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;

namespace aspnetsite.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        // Propriedade Privada para injetar a conexao com o banco de dados;
        private readonly string _conexaoMySQL;
        IConfiguration _config;

        //Método construtor da classe ClienteRepository
        public ClienteRepository(IConfiguration conf)
        {
            //Injeção de dependência do banco de dados
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;

        }

        public Cliente BuscaEmailCliente(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select Email from Cliente WHERE Email=@Email", conexao);
                cmd.Parameters.AddWithValue("@Email", email);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.Email = (string)(dr["Email"]);
                }
                return cliente;
            }
        }


        public Cliente BuscaCpfCliente(string CPF)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select CPF from Cliente WHERE CPF=@CPF", conexao);
                cmd.Parameters.AddWithValue("@CPF", CPF);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.CPF = (string)(dr["CPF"]);
                }
                return cliente;
            }
        }

        public Cliente ObterCliente(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Cliente WHERE Id=@Id", conexao);
                cmd.Parameters.AddWithValue("@Id", Id);

                MySqlDataReader dr;
                Cliente cliente = new Cliente();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.Id = Convert.ToInt32(dr["Id"]);
                    cliente.Nome = Convert.ToString(dr["Nome"]);
                    cliente.Nascimento = Convert.ToDateTime(dr["Nascimento"]);
                    cliente.Sexo = Convert.ToString(dr["Sexo"]);
                    cliente.CPF = Convert.ToString(dr["CPF"]);
                    cliente.Telefone = Convert.ToString(dr["Telefone"]);
                    cliente.Email = Convert.ToString(dr["Email"]);
                    cliente.Senha = Convert.ToString(dr["Senha"]);
                    cliente.Situacao = Convert.ToString(dr["Situacao"]);
                    cliente.Endereco = Convert.ToString(dr["Endereco"]);
                    cliente.Cidade = Convert.ToString(dr["Cidade"]);
                    cliente.Cep = Convert.ToString(dr["Cep"]);
                }
                return cliente;
            }
        }


        public void Ativar(int Id)
        {
            string Situacao = SituacaoConstant.Ativo;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Cliente set Situacao=@Situacao WHERE Id=@Id ", conexao);

                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@Situacao", MySqlDbType.VarChar).Value = Situacao;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Desativar(int Id)
        {
            string Situacao = SituacaoConstant.Desativado;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Cliente set Situacao=@Situacao WHERE Id=@Id ", conexao);

                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@Situacao", MySqlDbType.VarChar).Value = Situacao;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from Cliente WHERE Id=@Id", conexao);

                cmd.Parameters.AddWithValue("Id", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Atualizar(Cliente cliente)
        {
            string Situacao = SituacaoConstant.Ativo;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE Cliente SET Nome=@Nome, Nascimento=@Nascimento, Sexo=@Sexo, CPF=@CPF, " +
                    "Telefone=@Telefone, Email=@Email, Senha=@Senha, Situacao=@Situacao, " +
                    "Endereco=@Endereco, Cidade=@Cidade, Cep=@Cep WHERE Id=@Id", conexao);

                cmd.Parameters.AddWithValue("@Id", cliente.Id);
                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Nascimento", cliente.Nascimento.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                cmd.Parameters.AddWithValue("@CPF", cliente.CPF);
                cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@Senha", cliente.Senha);
                cmd.Parameters.AddWithValue("@Situacao", Situacao);
                cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                cmd.Parameters.AddWithValue("@Cidade", cliente.Cidade);
                cmd.Parameters.AddWithValue("@Cep", cliente.Cep);

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Cadastrar(Cliente cliente)
        {
            string Situacao = SituacaoConstant.Ativo;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO Cliente (Nome, Nascimento, Sexo, CPF, Telefone, Email, Senha, Situacao, Endereco, Cidade, Cep) " +
                    "VALUES (@Nome, @Nascimento, @Sexo, @CPF, @Telefone, @Email, @Senha, @Situacao, @Endereco, @Cidade, @Cep)", conexao);

                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Nascimento", cliente.Nascimento.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                cmd.Parameters.AddWithValue("@CPF", cliente.CPF);
                cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@Senha", cliente.Senha);
                cmd.Parameters.AddWithValue("@Situacao", Situacao);
                cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                cmd.Parameters.AddWithValue("@Cidade", cliente.Cidade);
                cmd.Parameters.AddWithValue("@Cep", cliente.Cep);

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public IEnumerable<Cliente> ObterTodosClientes()
        {
            List<Cliente> cliList = new List<Cliente>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Cliente", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    cliList.Add(new Cliente
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nome = Convert.ToString(dr["Nome"]),
                        Nascimento = Convert.ToDateTime(dr["Nascimento"]),
                        Sexo = Convert.ToString(dr["Sexo"]),
                        CPF = Convert.ToString(dr["CPF"]),
                        Telefone = Convert.ToString(dr["Telefone"]),
                        Email = Convert.ToString(dr["Email"]),
                        Senha = Convert.ToString(dr["Senha"]),
                        Situacao = Convert.ToString(dr["Situacao"]),
                        Endereco = Convert.ToString(dr["Endereco"]),
                        Cidade = Convert.ToString(dr["Cidade"]),
                        Cep = Convert.ToString(dr["Cep"]),
                    });
                }
                return cliList;
            }
        }

        // Login Cliente
        public Cliente Login(string Email, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Cliente WHERE Email = @Email AND Senha = @Senha", conexao);

                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Senha", Senha);

                MySqlDataReader dr;
                Cliente cliente = new Cliente();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.Id = Convert.ToInt32(dr["Id"]);
                    cliente.Nome = Convert.ToString(dr["Nome"]);
                    cliente.Nascimento = Convert.ToDateTime(dr["Nascimento"]);
                    cliente.Sexo = Convert.ToString(dr["Sexo"]);
                    cliente.CPF = Convert.ToString(dr["CPF"]);
                    cliente.Telefone = Convert.ToString(dr["Telefone"]);
                    cliente.Email = Convert.ToString(dr["Email"]);
                    cliente.Senha = Convert.ToString(dr["Senha"]);
                    cliente.Situacao = Convert.ToString(dr["Situacao"]);
                    cliente.Endereco = Convert.ToString(dr["Endereco"]);
                    cliente.Cidade = Convert.ToString(dr["Cidade"]);
                    cliente.Cep = Convert.ToString(dr["Cep"]);
                }
                return cliente;
            }
        }
    }
}