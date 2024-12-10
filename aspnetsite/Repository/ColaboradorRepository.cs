using aspnetsite.Models;
using aspnetsite.Models.Constant;
using aspnetsite.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;

namespace aspnetsite.Repository
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        // Propriedade readonly para injetar a conexão com o banco de dados
        private readonly string _conexaoMySQL;

        // metodo construtor da classe ColaboradorRepository
        public ColaboradorRepository(IConfiguration conf)
        {
            // Injeção de dependencia do banco de dados
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }
            public void Atualizar(Colaborador colaborador)
            {
                string Tipo = ColaboradorTipoConstant.Comum;
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("update Colaborador set Nome=@Nome, " +
                        " Email=@Email, Senha=@Senha, Tipo=@Tipo Where Id=@Id ", conexao);

                    cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = colaborador.Id;
                    cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = colaborador.Nome;
                    cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = colaborador.Email;
                    cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = colaborador.Senha;
                    cmd.Parameters.Add("@Tipo", MySqlDbType.VarChar).Value = Tipo;
                    cmd.ExecuteNonQuery();
                    conexao.Close();
                }
            }
        public Colaborador ObterColaborador(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                try
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM Colaborador WHERE Id = @Id", conexao);
                    cmd.Parameters.AddWithValue("@Id", Id);

                    // Executa o comando e obtém o DataReader
                    using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        // Verifica se encontrou algum registro
                        if (dr.Read())
                        {
                            // Preenche o objeto Colaborador com os dados do banco
                            return new Colaborador
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = dr["Nome"].ToString(),
                                Email = dr["Email"].ToString(),
                                Senha = dr["Senha"].ToString(),
                                Tipo = dr["Tipo"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Adicione logs ou tratamento de erro aqui, se necessário
                    Console.WriteLine($"Erro ao obter colaborador: {ex.Message}");
                    throw;
                }
            }

            // Caso nenhum registro seja encontrado, retorna null
            return null;
        }

        public IEnumerable<Colaborador> ObterTodosColaboradores()
        {
            List<Colaborador> colabList = new List<Colaborador>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM COLABORADOR", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    colabList.Add(
                        new Colaborador
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = (string)(dr["Nome"]),
                            Email = (string)(dr["Email"]),
                            Senha = (string)(dr["Senha"]),
                            Tipo = (string)(dr["Tipo"])
                        });
                }
                return colabList;
            }
        }
        public void Cadastrar(Colaborador colaborador)
        {
            string Comum = ColaboradorTipoConstant.Comum;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("insert into Colaborador(Nome, Email, Senha, Tipo) " +
                                                    " Values (@Nome, @Email, @Senha, @Tipo)", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = colaborador.Nome;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = colaborador.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = colaborador.Senha;
                cmd.Parameters.Add("@Tipo", MySqlDbType.VarChar).Value = Comum;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public Colaborador Login(string Email, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from Colaborador where Email = @Email and Senha = @Senha", conexao);

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = Senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Colaborador colaborador = new Colaborador();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    colaborador.Id = (Int32)(dr["Id"]);
                    colaborador.Nome = (string)(dr["Nome"]);
                    colaborador.Email = (string)(dr["Email"]);
                    colaborador.Senha = (string)(dr["Senha"]);
                    colaborador.Tipo = (string)(dr["Tipo"]);
                }
                return colaborador;
            }
        }
        public void AtualizarSenha(Colaborador colaborador)
        {
            throw new NotImplementedException();
        }

        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL)) // Usa sua variável de conexão
            {
                conexao.Open();

                // Cria o comando para exclusão
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Colaborador WHERE Id = @Id", conexao);
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = Id;

                // Executa o comando
                int rowsAffected = cmd.ExecuteNonQuery();

                // Valida se alguma linha foi afetada
                if (rowsAffected == 0)
                {
                    throw new Exception($"Nenhum colaborador com o ID {Id} foi encontrado para exclusão.");
                }
            }
        }

        public List<Colaborador> ObterColaboradorPorEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
