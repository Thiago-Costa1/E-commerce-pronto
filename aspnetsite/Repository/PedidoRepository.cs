using aspnetsite.Models;
using aspnetsite.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;

namespace aspnetsite.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _conexaoMySQL;

        // Construtor que injeta a string de conexão
        public PedidoRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public void CriarPedido(Pedido pedido)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                using (var transaction = conexao.BeginTransaction())
                {
                    try
                    {
                        // Inserir o pedido na tabela Pedido
                        MySqlCommand cmdPedido = new MySqlCommand(
                            "INSERT INTO Pedido (IdCliente, DataPedido, ValorTotal) " +
                            "VALUES (@IdCliente, @DataPedido, @ValorTotal); SELECT LAST_INSERT_ID();",
                            conexao,
                            transaction);

                        cmdPedido.Parameters.AddWithValue("@IdCliente", pedido.IdCliente);
                        cmdPedido.Parameters.AddWithValue("@DataPedido", pedido.DataPedido);
                        cmdPedido.Parameters.AddWithValue("@ValorTotal", pedido.ValorTotal);

                        // Obter o ID gerado para o pedido
                        int idPedido = Convert.ToInt32(cmdPedido.ExecuteScalar());

                        // Inserir os itens do pedido na tabela Itens
                        foreach (var item in pedido.Itens)
                        {
                            // Obter o nome do produto da tabela `tbNotebook`
                            MySqlCommand cmdProduto = new MySqlCommand(
                                "SELECT nomeNotebook FROM tbNotebook WHERE codNotebook = @IdProduto",
                                conexao,
                                transaction);
                            cmdProduto.Parameters.AddWithValue("@IdProduto", item.IdProduto);
                            var nomeNotebook = cmdProduto.ExecuteScalar()?.ToString();

                            if (string.IsNullOrEmpty(nomeNotebook))
                            {
                                throw new Exception($"O nome do notebook com o ID {item.IdProduto} não foi encontrado.");
                            }

                            // Inserir o item na tabela `Itens`
                            MySqlCommand cmdItem = new MySqlCommand(
                                "INSERT INTO Itens (IdPedido, IdProduto, ValorParcial, Preco, Garantia, ValorTotal, QtdItens, NomeProduto) " +
                                "VALUES (@IdPedido, @IdProduto, @ValorParcial, @Preco, @Garantia, @ValorTotal, @QtdItens, @NomeProduto)",
                                conexao,
                                transaction);

                            cmdItem.Parameters.AddWithValue("@IdPedido", idPedido);
                            cmdItem.Parameters.AddWithValue("@IdProduto", item.IdProduto);
                            cmdItem.Parameters.AddWithValue("@ValorParcial", item.ValorParcial);
                            cmdItem.Parameters.AddWithValue("@Preco", item.Preco);
                            cmdItem.Parameters.AddWithValue("@Garantia", item.Garantia);
                            cmdItem.Parameters.AddWithValue("@ValorTotal", item.ValorTotal);
                            cmdItem.Parameters.AddWithValue("@QtdItens", item.QtdItens);
                            cmdItem.Parameters.AddWithValue("@NomeProduto", nomeNotebook);

                            cmdItem.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                conexao.Close();
            }
        }

        public List<Pedido> ObterPedidosPorCliente(int idCliente)
        {
            List<Pedido> pedidos = new List<Pedido>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // Consultar os pedidos do cliente
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM Pedido WHERE IdCliente = @IdCliente ORDER BY DataPedido DESC",
                    conexao);
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var pedido = new Pedido
                    {
                        IdPedido = Convert.ToInt32(dr["IdPedido"]),
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        DataPedido = Convert.ToDateTime(dr["DataPedido"]),
                        ValorTotal = Convert.ToDecimal(dr["ValorTotal"]),
                        Itens = new List<Itens>() // Inicializa a lista de itens
                    };

                    pedidos.Add(pedido);
                }
                dr.Close();

                // Consultar os itens de cada pedido com NomeProduto

                foreach (var pedido in pedidos)
                {
                    MySqlCommand cmdItens = new MySqlCommand(
                            @"SELECT * FROM Itens WHERE IdPedido = @IdPedido; ", conexao);
                    cmdItens.Parameters.AddWithValue("@IdPedido", pedido.IdPedido);

                    dr = cmdItens.ExecuteReader();
                    while (dr.Read())
                    {
                        pedido.Itens.Add(new Itens
                        {
                            IdItem = Convert.ToInt32(dr["IdItem"]),
                            IdProduto = Convert.ToInt32(dr["IdProduto"]),
                            NomeProduto = dr["NomeProduto"].ToString(),
                            ValorParcial = Convert.ToDecimal(dr["ValorParcial"]), // Novo campo
                            Preco = Convert.ToDecimal(dr["Preco"]),
                            Garantia = Convert.ToDecimal(dr["Garantia"]),
                            ValorTotal = Convert.ToDecimal(dr["ValorTotal"]),
                            QtdItens = Convert.ToInt32(dr["QtdItens"])
                        });
                    }
                    dr.Close();
                }
                conexao.Close();
            }
            return pedidos;
        }
    }
}