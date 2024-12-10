document.addEventListener('DOMContentLoaded', function ()
    {
        // Atualiza o preço de um produto específico
        function atualizarPrecoProduto(codNotebook) {
            const input = document.querySelector(`input.quantity-input[data-id='${codNotebook}']`);
            if (!input) return;

            const precoNotebook = parseFloat(input.dataset.preco) || 0;
            const quantidade = parseInt(input.value) || 1;

            const garantiaSelect = document.querySelector(`.garantia-select[data-id='${codNotebook}']`);
            const precoGarantia = garantiaSelect ? parseFloat(garantiaSelect.value) || 0 : 0;

            const precoTotalProduto = (precoNotebook + precoGarantia) * quantidade;

            const precoProdutoElement = document.getElementById(`pPreco-${codNotebook}`);
            if (precoProdutoElement) {
                precoProdutoElement.innerText = `R$ ${precoTotalProduto.toFixed(2)}`;
            }
        }

        // Atualiza o preço total do carrinho
        function atualizarTotal() {
            let totalProdutos = 0;

            document.querySelectorAll('.quantity-input').forEach((input) => {
                const codNotebook = input.dataset.id;
                const precoNotebook = parseFloat(input.dataset.preco) || 0;
                const quantidade = parseInt(input.value) || 1;

                const garantiaSelect = document.querySelector(`.garantia-select[data-id='${codNotebook}']`);
                const precoGarantia = garantiaSelect ? parseFloat(garantiaSelect.value) || 0 : 0;

                totalProdutos += (precoNotebook + precoGarantia) * quantidade;
            });

            const precoTotalElement = document.getElementById('preco-total');
            if (precoTotalElement) {
                precoTotalElement.innerText = `R$ ${totalProdutos.toFixed(2)}`;
            }
        }

        // Atualiza a garantia e o preço correspondente
        function atualizarGarantia(codNotebook, valorGarantia) {
            fetch(`/Carrinho/AtualizarGarantia`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ codNotebook, valorGarantia })
            })
                .then(response => {
                    if (response.ok) {
                        atualizarPrecoProduto(codNotebook); // Atualiza o preço do produto
                        atualizarTotal(); // Atualiza o total do carrinho
                    } else {
                        console.error("Erro ao atualizar a garantia.");
                    }
                })
                .catch(error => console.error("Erro ao comunicar com o servidor:", error));
        }



        // Atualiza o carrinho no backend
        function atualizarCarrinho(codNotebook, quantidade) {
            fetch(`/Carrinho/AtualizarQuantidade?codNotebook=${codNotebook}&quantidade=${quantidade}`, {
                method: 'POST',
            })
                .then(() => {
                    console.log(`Carrinho atualizado para o produto ${codNotebook} com quantidade ${quantidade}`);
                    atualizarPrecoProduto(codNotebook); // Atualiza o preço do produto
                    atualizarTotal(); // Atualiza o total
                })
                .catch((err) => console.error('Erro ao atualizar o carrinho:', err));
        }

        // Diminui a quantidade de um produto

        function diminuirQuantidade(codNotebook) {
            const input = document.querySelector(`input.quantity-input[data-id='${codNotebook}']`);
            if (!input) return;

            let quantidade = parseInt(input.value) || 1;
            if (quantidade > 1) {
                quantidade -= 1;
                input.value = quantidade;
                atualizarCarrinho(codNotebook, quantidade);
            } 
        
        }

        // Aumenta a quantidade de um produto
        function aumentarQuantidade(codNotebook) {
            const input = document.querySelector(`input.quantity-input[data-id='${codNotebook}']`);
            if (!input) return;

            let quantidade = parseInt(input.value) || 1;
            quantidade += 1;
            input.value = quantidade;
            atualizarCarrinho(codNotebook, quantidade);
        }

        // Adiciona os eventos aos botões de quantidade
        document.querySelectorAll('.quantity-btn').forEach((button) => {
            button.addEventListener('click', function () {
                const codNotebook = this.dataset.id;
                const action = this.dataset.action;

                if (action === 'decrement') {
                    diminuirQuantidade(codNotebook);
                } else if (action === 'increment') {
                    aumentarQuantidade(codNotebook);
                }
            });
        });

        // Adiciona os eventos para o dropdown de garantia
        document.querySelectorAll('.garantia-select').forEach((select) => {
            select.addEventListener('change', function () {
                const codNotebook = this.dataset.id;
                atualizarGarantia(codNotebook, this.value);
            });
        });

        // Inicializa o total ao carregar a página
    atualizarTotal();

    }); 
        // Lógica para o cupom de desconto
    document.addEventListener('DOMContentLoaded', function () {
        const botaoCupom = document.getElementById('botaoCupom');
        const mensagemCupom = document.getElementById('mensagemCupom');
        const precoTotalElement = document.getElementById('preco-total');

        if (botaoCupom) {
            botaoCupom.addEventListener('click', function () {
                const codigoCupom = document.getElementById('codigoCupom').value.trim();

                // Limpa mensagens anteriores
                mensagemCupom.textContent = '';
                mensagemCupom.style.color = '';

                // Verifica se o campo está vazio
                if (!codigoCupom) {
                    mensagemCupom.textContent = "Por favor, insira um código de cupom.";
                    mensagemCupom.style.color = "red";
                    return;
                }

                // Faz a requisição para validar o cupom
                fetch('/Carrinho/ValidarCupom', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ Codigo: codigoCupom })
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Erro ao validar o cupom');
                        }
                        return response.json();
                    })
                    .then(data => {
                        if (data.sucesso) {
                            // Exibe mensagem de sucesso
                            mensagemCupom.textContent = `Cupom aplicado com sucesso! Desconto: R$ ${data.desconto.toFixed(2)}`;
                            mensagemCupom.style.color = "green";

                            // Atualiza o preço total
                            const precoTotal = parseFloat(precoTotalElement.textContent.replace('R$', '').trim());
                            const novoPreco = precoTotal - data.desconto;
                            precoTotalElement.textContent = `R$ ${novoPreco.toFixed(2)}`;
                        } else {
                            // Exibe mensagem de erro
                            mensagemCupom.textContent = data.mensagem;
                            mensagemCupom.style.color = "red";
                        }
                    })
                    .catch(error => {
                        console.error('Erro ao validar o cupom:', error);
                        mensagemCupom.textContent = "Ocorreu um erro ao validar o cupom. Tente novamente.";
                        mensagemCupom.style.color = "red";
                    });
            });
        }
    });            
      