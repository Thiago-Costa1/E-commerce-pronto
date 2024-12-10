// Início da lógica das miniaturas para trocar a imagem grande
document.addEventListener('DOMContentLoaded', function()
{
    const mainImage = document.getElementById('img-grande');
    const zoomImg = document.getElementById('zoom-img');
    const thumbnails = document.querySelectorAll('.imagem-pequena');

    thumbnails.forEach(thumbnail =>
        {
        thumbnail.addEventListener('mouseover', function()
        {
            const newSrc = this.src;
            mainImage.src = newSrc; // Substitui a imagem principal pela imagem em miniatura
            zoomImg.src = newSrc; // Atualiza a imagem de zoom também
        });
    });
});
// Fim da lógica das miniaturas para trocar a imagem grande

// Início da lógica para aplicar o zoom quando o mouse estiver sob a imagem grande

document.addEventListener("DOMContentLoaded", () =>
    {
    const box = document.getElementById("box");
    const img = document.getElementById("img-grande");
    const zoomContainer = document.getElementById("zoom-container");
    const zoomImg = document.getElementById("zoom-img");

    box.addEventListener("mousemove", (e) =>
        {
        zoomContainer.style.display = "block";

        const rect = e.currentTarget.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        // Posiciona o contêiner de zoom ao lado da imagem principal
        zoomContainer.style.top = `${rect.top}px`;
        zoomContainer.style.left = `${rect.right}px`; // Adicionar margem

        const imgWidth = img.offsetWidth;
        const imgHeight = img.offsetHeight;

        const zoomWidth = zoomContainer.offsetWidth;
        const zoomHeight = zoomContainer.offsetHeight;

        const zoomX = (x / imgWidth) * zoomImg.width - (zoomWidth / 2);
        const zoomY = (y / imgHeight) * zoomImg.height - (zoomHeight / 2);

        zoomImg.style.left = `-${zoomX}px`;
        zoomImg.style.top = `-${zoomY}px`;
    });

    box.addEventListener("mouseleave", () =>
    {
        zoomContainer.style.display = "none";
    });
});

// Fim da lógica para aplicar o zoom quando o mouse estiver sob a imagem grande

/* Início da tabela das parcelas*/ 

    document.addEventListener('DOMContentLoaded', function () {
        const parcelasButton = document.getElementById('parcelas-button');
        const parcelasTable = document.getElementById('parcelas-table');
        const dropdownIcon = parcelasButton.querySelector('.dropdown-icon');

        parcelasButton.addEventListener('click', function (event) {
            event.stopPropagation(); // Evita a propagação do evento para elementos pais
            const isVisible = getComputedStyle(parcelasTable).display !== 'none';

            if (isVisible) {
                parcelasTable.style.display = 'none';
                dropdownIcon.classList.remove('rotate');
            } else {
                parcelasTable.style.display = 'block';
                dropdownIcon.classList.add('rotate');
            }
        });
    });

    // Função auxiliar para formatar o número
    function formatNumber(num) {
        return num.toFixed(2).replace('.', ',').replace(/\B(?=(\d{3})+(?!\d))/g, '.');
    }


/* Fim da tabela das parcelas */

document.addEventListener('DOMContentLoaded', function()
{
    const containerInf = document.querySelector('.container-inf');
    const containerInterno = document.querySelector('.container-interno');
    const dropdownIcon = document.querySelector('#span1');

    containerInf.addEventListener('click', function() {
        const isVisible = containerInterno.style.display === 'flex';

        if (isVisible) {
            containerInterno.style.display = 'none';
            dropdownIcon.classList.remove('rotacao-cima');
            dropdownIcon.classList.add('rotate-down');
        } else {
            containerInterno.style.display = 'flex';
            dropdownIcon.classList.remove('rotacao-baixo');
            dropdownIcon.classList.add('rotacao-cima');
        }
    });
});

// Definindo a função de alteração de imagem fora do 'DOMContentLoaded'
function alterarImagem(novaImagem) {
    document.getElementById('img-grande').src = novaImagem;
}

document.addEventListener('DOMContentLoaded', function () {
    // Se necessário, pode adicionar outras funções dentro deste bloco
    console.log("Página carregada!");
});



