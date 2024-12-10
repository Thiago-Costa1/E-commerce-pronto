using aspnetsite.Models;
using aspnetsite.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace aspnetsite.Repository
{
    public class NotebookRepository : INotebookRepository
    {
        private readonly string _conexaoMySQL;

        public NotebookRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public IEnumerable<Notebook> ObterTodosNotebooks()
        {
            List<Notebook> Notebooklist = new List<Notebook>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbNotebook", conexao);
                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sd.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Notebooklist.Add(
                        new Notebook()
                        {
                            codNotebook                = Convert.ToInt32(dr["codNotebook"]),
                            nomeNotebook               = (String) (dr["nomeNotebook"]),
                            quantidade                 = Convert.ToInt32(dr["quantidade"]),
                            imagemNotebook1            = (String) (dr["imagemNotebook1"]),
                            imagemNotebook2            = (String) (dr["imagemNotebook2"]),
                            imagemNotebook3            = (String) (dr["imagemNotebook3"]),
                            imagemNotebook4            = (String) (dr["imagemNotebook4"]),
                            precoNotebook              = Convert.ToDecimal(dr["precoNotebook"]),
                            descontoNotebook           = dr["descontoNotebook"]      != DBNull.Value ? Convert.ToDecimal(dr["descontoNotebook"]) : 0,
                            valorGarantiaNotebook      = dr["valorGarantiaNotebook"] != DBNull.Value ? Convert.ToDecimal(dr["valorGarantiaNotebook"]) : 0,
                            corNotebook                = (String) (dr["corNotebook"]),
                            sistemaOperacionalNotebook = (String) (dr["sistemaOperacionalNotebook"]),
                            processadorNotebook        = (String) (dr["processadorNotebook"]),
                            placaVideoNotebook         = (String) (dr["placaVideoNotebook"]),                            
                            telaNotebook               = (String) (dr["telaNotebook"]),
                            memoriaNotebook            = (String) (dr["memoriaNotebook"]),
                            armazenamentoNotebook      = (String) (dr["armazenamentoNotebook"]),
                            portasConexaoNotebook      = (String) (dr["portasConexaoNotebook"]),
                            cameraNotebook             = (String) (dr["cameraNotebook"]),                            
                            audioNotebook              = (String) (dr["audioNotebook"]),
                            redeComunicacaoNotebook    = (String) (dr["redeComunicacaoNotebook"]),
                            bateriaNotebook            = (String) (dr["bateriaNotebook"]),
                            fonteAlimentacaoNotebook   = (String) (dr["fonteAlimentacaoNotebook"]),
                            pesoNotebook               = (String) (dr["pesoNotebook"]),
                            incluidoNaCaixaNotebook    = (String) (dr["incluidoNaCaixaNotebook"]),
                        });
                }
                return Notebooklist;
            }
        }

        public Notebook ObterNotebooks(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tbnotebook where codNotebook=@cod", conexao);
                cmd.Parameters.Add("@cod", MySqlDbType.VarChar).Value = Id;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Notebook notebook = new Notebook();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    
                    notebook.codNotebook                = Convert.ToInt32(dr["codNotebook"]);
                    notebook.nomeNotebook               = (String)(dr["nomeNotebook"]);
                    notebook.quantidade                 = Convert.ToInt32(dr["quantidade"]);
                    notebook.imagemNotebook1            = (String)(dr["imagemNotebook1"]);
                    notebook.imagemNotebook2            = (String)(dr["imagemNotebook2"]);
                    notebook.imagemNotebook3            = (String)(dr["imagemNotebook3"]);
                    notebook.imagemNotebook4            = (String)(dr["imagemNotebook4"]);
                    notebook.precoNotebook              = Convert.ToInt32(dr["precoNotebook"]);
                    notebook.descontoNotebook           = Convert.ToInt32(dr["descontoNotebook"]);
                    notebook.valorGarantiaNotebook      = dr["valorGarantiaNotebook"] != DBNull.Value ? Convert.ToDecimal(dr["valorGarantiaNotebook"]) : (decimal?)null;
                    notebook.corNotebook                = (String)(dr["corNotebook"]);
                    notebook.sistemaOperacionalNotebook = (String)(dr["sistemaOperacionalNotebook"]);
                    notebook.processadorNotebook        = (String)(dr["processadorNotebook"]);
                    notebook.placaVideoNotebook         = (String)(dr["placaVideoNotebook"]);
                    notebook.telaNotebook               = (String)(dr["telaNotebook"]);
                    notebook.memoriaNotebook            = (String)(dr["memoriaNotebook"]);
                    notebook.armazenamentoNotebook      = (String)(dr["armazenamentoNotebook"]);
                    notebook.portasConexaoNotebook      = (String)(dr["portasConexaoNotebook"]);
                    notebook.cameraNotebook             = (String)(dr["cameraNotebook"]);
                    notebook.audioNotebook              = (String)(dr["audioNotebook"]);
                    notebook.redeComunicacaoNotebook    = (String)(dr["redeComunicacaoNotebook"]);
                    notebook.bateriaNotebook            = (String)(dr["bateriaNotebook"]);
                    notebook.fonteAlimentacaoNotebook   = (String)(dr["fonteAlimentacaoNotebook"]);
                    notebook.pesoNotebook               = (String)(dr["pesoNotebook"]);
                    notebook.incluidoNaCaixaNotebook    = (String)(dr["incluidoNaCaixaNotebook"]);
                }
                return notebook;                    
            }
        }

        public void Cadastrar(Notebook notebook)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                Console.WriteLine(notebook.nomeNotebook);
                MySqlCommand cmd = new MySqlCommand("insert into tbNotebook values(@CodNotebook, @NomeNotebook, @Quantidade, @ImagemNotebook1, @ImagemNotebook2," +
                    " @ImagemNotebook3, @ImagemNotebook4, @PrecoNotebook, @DescontoNotebook, @valorGarantiaNotebook, " +
                    " @CorNotebook, @SistemaOperacionalNotebook, @ProcessadorNotebook, @PlacaVideoNotebook, @TelaNotebook, @MemoriaNotebook," +
                    " @ArmazenamentoNotebook, @PortasConexaoNotebook, @CameraNotebook, @AudioNotebook, @RedeComunicacaoNotebook, @BateriaNotebook," +
                    " @FonteAlimentacaoNotebook, @PesoNotebook, @IncluidoNaCaixaNotebook)", conexao);

                cmd.Parameters.Add("@CodNotebook", MySqlDbType.VarChar).Value                = notebook.codNotebook;
                cmd.Parameters.Add("@NomeNotebook", MySqlDbType.VarChar).Value               = notebook.nomeNotebook;
                cmd.Parameters.Add("@Quantidade", MySqlDbType.Int32).Value           = (int?)notebook.quantidade ?? (object)DBNull.Value;
                cmd.Parameters.Add("@ImagemNotebook1", MySqlDbType.VarChar).Value            = notebook.imagemNotebook1;
                cmd.Parameters.Add("@ImagemNotebook2", MySqlDbType.VarChar).Value            = notebook.imagemNotebook2;
                cmd.Parameters.Add("@ImagemNotebook3", MySqlDbType.VarChar).Value            = notebook.imagemNotebook3;
                cmd.Parameters.Add("@ImagemNotebook4", MySqlDbType.VarChar).Value            = notebook.imagemNotebook4;
                cmd.Parameters.Add("@PrecoNotebook", MySqlDbType.Int32).Value                = notebook.precoNotebook;

                cmd.Parameters.Add("@DescontoNotebook", MySqlDbType.Int32).Value             = notebook.descontoNotebook;
                cmd.Parameters.Add("@ValorGarantiaNotebook", MySqlDbType.Int32).Value        = notebook.valorGarantiaNotebook;
                cmd.Parameters.Add("@CorNotebook", MySqlDbType.VarChar).Value                = notebook.corNotebook;
                cmd.Parameters.Add("@SistemaOperacionalNotebook", MySqlDbType.VarChar).Value = notebook.sistemaOperacionalNotebook;
               
                cmd.Parameters.Add("@ProcessadorNotebook", MySqlDbType.VarChar).Value        = notebook.processadorNotebook;
                cmd.Parameters.Add("@PlacaVideoNotebook", MySqlDbType.VarChar).Value         = notebook.placaVideoNotebook;
                cmd.Parameters.Add("@TelaNotebook", MySqlDbType.VarChar).Value               = notebook.telaNotebook;
                cmd.Parameters.Add("@MemoriaNotebook", MySqlDbType.VarChar).Value            = notebook.memoriaNotebook;
                cmd.Parameters.Add("@ArmazenamentoNotebook", MySqlDbType.VarChar).Value      = notebook.armazenamentoNotebook;
                cmd.Parameters.Add("@PortasConexaoNotebook", MySqlDbType.VarChar).Value      = notebook.portasConexaoNotebook;
                cmd.Parameters.Add("@CameraNotebook", MySqlDbType.VarChar).Value             = notebook.cameraNotebook;
                cmd.Parameters.Add("@AudioNotebook", MySqlDbType.VarChar).Value              = notebook.audioNotebook;

                cmd.Parameters.Add("@RedeComunicacaoNotebook", MySqlDbType.VarChar).Value    = notebook.redeComunicacaoNotebook;
                cmd.Parameters.Add("@BateriaNotebook", MySqlDbType.VarChar).Value            = notebook.bateriaNotebook;
                cmd.Parameters.Add("@FonteAlimentacaoNotebook", MySqlDbType.VarChar).Value   = notebook.fonteAlimentacaoNotebook;
                cmd.Parameters.Add("@PesoNotebook", MySqlDbType.VarChar).Value               = notebook.pesoNotebook;
                cmd.Parameters.Add("@IncluidoNaCaixaNotebook", MySqlDbType.VarChar).Value    = notebook.incluidoNaCaixaNotebook;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Atualizar(Notebook notebook)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE tbNotebook SET nomeNotebook=@nomeNotebook, quantidade=@quantidade, " +
                    "ImagemNotebook1=@ImagemNotebook1, ImagemNotebook2=@ImagemNotebook2, " +
                    "imagemNotebook3=@imagemNotebook3, imagemNotebook4=@imagemNotebook4, " +
                    "precoNotebook=@precoNotebook, descontoNotebook=@descontoNotebook, " +
                    "valorGarantiaNotebook=@valorGarantiaNotebook, corNotebook=@corNotebook, " +
                    "sistemaOperacionalNotebook=@sistemaOperacionalNotebook, " +
                    "processadorNotebook=@processadorNotebook, placaVideoNotebook=@placaVideoNotebook, " +
                    "telaNotebook=@telaNotebook, memoriaNotebook=@memoriaNotebook, " +
                    "armazenamentoNotebook=@armazenamentoNotebook, portasConexaoNotebook=@portasConexaoNotebook, " +
                    "cameraNotebook=@cameraNotebook, audioNotebook=@audioNotebook, " +
                    "redeComunicacaoNotebook=@redeComunicacaoNotebook, bateriaNotebook=@bateriaNotebook, " +
                    "fonteAlimentacaoNotebook=@fonteAlimentacaoNotebook, pesoNotebook=@pesoNotebook, " +
                    "incluidoNaCaixaNotebook=@incluidoNaCaixaNotebook WHERE codNotebook = @codNotebook",
                    conexao
                );

                // Adicionando os parâmetros com os nomes consistentes
                cmd.Parameters.Add("@codNotebook", MySqlDbType.VarChar).Value = notebook.codNotebook;
                cmd.Parameters.Add("@nomeNotebook", MySqlDbType.VarChar).Value = notebook.nomeNotebook;
                cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = (int?)notebook.quantidade ?? (object)DBNull.Value;
                cmd.Parameters.Add("@ImagemNotebook1", MySqlDbType.VarChar).Value = notebook.imagemNotebook1;
                cmd.Parameters.Add("@ImagemNotebook2", MySqlDbType.VarChar).Value = notebook.imagemNotebook2;
                cmd.Parameters.Add("@ImagemNotebook3", MySqlDbType.VarChar).Value = notebook.imagemNotebook3;
                cmd.Parameters.Add("@ImagemNotebook4", MySqlDbType.VarChar).Value = notebook.imagemNotebook4;
                cmd.Parameters.Add("@precoNotebook", MySqlDbType.Int32).Value = notebook.precoNotebook;
                cmd.Parameters.Add("@descontoNotebook", MySqlDbType.Int32).Value = notebook.descontoNotebook;
                cmd.Parameters.Add("@valorGarantiaNotebook", MySqlDbType.Int32).Value = notebook.valorGarantiaNotebook;
                cmd.Parameters.Add("@corNotebook", MySqlDbType.VarChar).Value = notebook.corNotebook;
                cmd.Parameters.Add("@sistemaOperacionalNotebook", MySqlDbType.VarChar).Value = notebook.sistemaOperacionalNotebook;
                cmd.Parameters.Add("@processadorNotebook", MySqlDbType.VarChar).Value = notebook.processadorNotebook;
                cmd.Parameters.Add("@placaVideoNotebook", MySqlDbType.VarChar).Value = notebook.placaVideoNotebook;
                cmd.Parameters.Add("@telaNotebook", MySqlDbType.VarChar).Value = notebook.telaNotebook;
                cmd.Parameters.Add("@memoriaNotebook", MySqlDbType.VarChar).Value = notebook.memoriaNotebook;
                cmd.Parameters.Add("@armazenamentoNotebook", MySqlDbType.VarChar).Value = notebook.armazenamentoNotebook;
                cmd.Parameters.Add("@portasConexaoNotebook", MySqlDbType.VarChar).Value = notebook.portasConexaoNotebook;
                cmd.Parameters.Add("@cameraNotebook", MySqlDbType.VarChar).Value = notebook.cameraNotebook;
                cmd.Parameters.Add("@audioNotebook", MySqlDbType.VarChar).Value = notebook.audioNotebook;
                cmd.Parameters.Add("@redeComunicacaoNotebook", MySqlDbType.VarChar).Value = notebook.redeComunicacaoNotebook;
                cmd.Parameters.Add("@bateriaNotebook", MySqlDbType.VarChar).Value = notebook.bateriaNotebook;
                cmd.Parameters.Add("@fonteAlimentacaoNotebook", MySqlDbType.VarChar).Value = notebook.fonteAlimentacaoNotebook;
                cmd.Parameters.Add("@pesoNotebook", MySqlDbType.VarChar).Value = notebook.pesoNotebook;
                cmd.Parameters.Add("@incluidoNaCaixaNotebook", MySqlDbType.VarChar).Value = notebook.incluidoNaCaixaNotebook;

                // Executar a consulta de atualização
                cmd.ExecuteNonQuery();
                conexao.Close();
            }     
        }
        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from tbNotebook where codNotebook=@codNotebook", conexao);
                cmd.Parameters.AddWithValue("@codNotebook", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
    }
}