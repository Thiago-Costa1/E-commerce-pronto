document.addEventListener("DOMContentLoaded", function () {
    // Elementos principais
    const paymentPix = document.getElementById("payment-pix");
    const paymentBoleto = document.getElementById("payment-boleto");
    const paymentCard = document.getElementById("payment-card");
    const paymentSummary = document.getElementById("payment-summary");
    const dropdownOptions = document.getElementById("dropdown-options");
    const dropdownIcon = document.getElementById("dropdown-icon");
    const dropdownButton = document.querySelector(".custom-dropdown button");


    document.getElementById("finalizarPedido").addEventListener("click", function () {
        // Exibe a mensagem de sucesso
        Swal.fire({
            title: 'Pedido Finalizado!',
            text: 'Seu pedido foi realizado com sucesso. Obrigado pela compra!',
            icon: 'success',
            confirmButtonText: 'OK'
        }).then(() => {
            // Após o usuário clicar em "OK", submete o formulário
            document.querySelector("form").submit();
        });
    });


    // Função para alternar visibilidade do dropdown
    function toggleDropdown() {
        const isHidden = dropdownOptions.style.display === "none";
        dropdownOptions.style.display = isHidden ? "block" : "none";
        dropdownIcon.textContent = isHidden ? "arrow_drop_up" : "arrow_drop_down";
    }

    // Função para selecionar método de pagamento
    function selectPaymentMethod(method) {
        // Atualiza o texto do botão com o método selecionado
        dropdownButton.innerHTML = `
            ${method}
            <span id="dropdown-icon" class="material-icons">arrow_drop_down</span>
        `;

        // Atualiza o resumo do pagamento
        paymentSummary.textContent = method;

        // Oculta todos os elementos de pagamento
        paymentPix.style.display = "none";
        paymentBoleto.style.display = "none";
        paymentCard.style.display = "none";

        // Exibe o método correspondente
        if (method === "Pix") {
            paymentPix.style.display = "block";
        } else if (method === "Boleto Bancário") {
            paymentBoleto.style.display = "block";
        } else if (method === "Cartão de Crédito") {
            paymentCard.style.display = "block";
        }

        // Fecha o dropdown
        dropdownOptions.style.display = "none";
        dropdownIcon.textContent = "arrow_drop_down";
    }

    // Adicionar evento de clique para alternar o dropdown
    dropdownButton.addEventListener("click", toggleDropdown);

    // Adicionar eventos de clique às opções do dropdown
    const options = document.querySelectorAll(".dropdown-option");
    options.forEach(option => {
        option.addEventListener("click", function () {
            selectPaymentMethod(this.textContent);
        });
    });
});


$(document).ready(function () {
    $('#botaoCupom').click(function () {
        var codigoCupom = $('#codigoCupom').val();

        if (codigoCupom) {
            $.ajax({
                url: '/Home/ValidarCupom',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ Codigo: codigoCupom }),
                success: function (response) {
                    if (response.sucesso) {
                        // Atualiza o valor do cupom no resumo do pedido
                        $('#valorCupom').text(response.desconto.toFixed(2));

                        // Recalcula o total, se necessário
                        var subtotal = parseFloat($('#subtotal').text());
                        var garantia = parseFloat($('#garantiaTotal').text());
                        var total = subtotal + garantia - response.desconto;

                        $('#valorTotal').text(total.toFixed(2));
                    } else {
                        alert(response.mensagem);
                    }
                },
                error: function () {
                    alert('Erro ao validar o cupom.');
                }
            });
        }
    });
});




// Exibir e esconder as informações dos dados para compra
function toggleShippingInfo() {
    const shippingInfo = document.getElementById('shipping-info');
    shippingInfo.style.display = shippingInfo.style.display === 'none' ? 'block' : 'none';
    const dropdownIcon = document.getElementById('dropdown-icon');
    dropdownIcon.textContent = shippingInfo.style.display === 'block' ? 'arrow_drop_up' : 'arrow_drop_down';
}