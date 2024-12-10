using aspnetsite.Models;
using Newtonsoft.Json;

namespace aspnetsite.CarrinhoCompra
{
    public class CookieCarrinhoCompra
    {
        //criar uma chave para o cookie
        private string Key = "Carrinho.Compras";
        private Cookie.Cookie _cookie;

        public CookieCarrinhoCompra(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

       // Cadastrar item no carrinho
        public void Cadastrar(Notebook item)
        {
            List<Notebook> Lista;
            if (_cookie.Existe(Key)) // Verifica se o cookie já existe
            {
                Lista = Consultar(); // Consulta os itens atuais
                var ItemLocalizado = Lista.SingleOrDefault(a => a.codNotebook == item.codNotebook);

                if (ItemLocalizado == null) // Se o item não está no carrinho, adiciona
                {
                    item.quantidade = 1; // Defina a quantidade inicial como 1
                    Lista.Add(item);
                }
                else // Se já existe, atualiza a quantidade
                {
                    ItemLocalizado.quantidade = ItemLocalizado.quantidade + 1;
                }
            }
            else // Se o cookie não existe, cria uma nova lista
            {
                Lista = new List<Notebook>();
                Lista.Add(item);
            }
            // Salva a lista atualizada no cookie
            Salvar(Lista);
        }

        // Atualiza um item no carrinho
        public void Atualizar(Notebook item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.codNotebook == item.codNotebook);

            if (ItemLocalizado != null)
            {
                ItemLocalizado.quantidade = item.quantidade + 1; // Atualiza a quantidade
                Salvar(Lista); // Salva a lista atualizada
            }
        }
        // Remove um item do carrinho
        public void Remover(Notebook item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.codNotebook == item.codNotebook);

            if (ItemLocalizado != null)
            {
                Lista.Remove(ItemLocalizado); // Remove o item localizado
                Salvar(Lista);  // Salva a lista atualizada
            }
        }
        // Verifica se o cookie existe
        
        public bool Existe(string Key)
        {
            return _cookie.Existe(Key);
        }

        // Remove todos itens do carrinho
        public void RemoverTodos()
        {
            _cookie.Remover(Key);
        }

        // Consulta os itens do carrinho
        public List<Notebook> Consultar()
        {
            Console.WriteLine("Método Consultar() foi chamado."); // Para verificar se o método foi chamado
            if (_cookie.Existe(Key))
            {
                string valor = _cookie.Consultar(Key);
                Console.WriteLine($"Valor do cookie encontrado: {valor}"); // Para verificar o valor encontrado no cookie
                return JsonConvert.DeserializeObject<List<Notebook>>(valor); // Deserializa o valor do cookie para uma lista
            }
            else
            {
                Console.WriteLine("Nenhum cookie encontrado para a chave fornecida."); // Para confirmar quando nenhum cookie foi encontrado
                return new List<Notebook>(); // Retorna uma lista vazia se o cookie não existe
            }
        }

        // Salvar a lista de itens no cookie
        public void Salvar(List<Notebook> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Cadastrar(Key, Valor);
        }
        
        
    }
}
