namespace aspnetsite.GerenciaArquivos
{
    public class GerenciadorArquivo
    {
        public static List<string> CadastrarImagemProduto(params IFormFile[] files)
        {
            var caminhos = new List<string>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0) // Verifica se o arquivo foi enviado
                {
                    var nomeArquivo = Path.GetFileName(file.FileName);
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imagens", nomeArquivo);

                    using (var stream = new FileStream(caminho, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var caminhoRelativo = Path.Combine("/Imagens", nomeArquivo).Replace("\\", "/");
                    caminhos.Add(caminhoRelativo);
                }
                else
                {
                    caminhos.Add(null); // Para manter a correspondência se algum arquivo não foi enviado
                }
            }
            return caminhos;
        }
    }
}