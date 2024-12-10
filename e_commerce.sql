CREATE DATABASE e_commerce;
USE e_commerce;



CREATE TABLE tbNotebook (
    codNotebook INT AUTO_INCREMENT PRIMARY KEY,
    nomeNotebook VARCHAR(100) NOT NULL,
    quantidade int null,
    imagemNotebook1 VARCHAR(255) not null,
	imagemNotebook2 VARCHAR(255) not null,
    imagemNotebook3 VARCHAR(255) not null,
    imagemNotebook4 VARCHAR(255) not null,
    precoNotebook DECIMAL(10, 2) NOT NULL,
    descontoNotebook DECIMAL(10, 2) DEFAULT 0,
    valorGarantiaNotebook DECIMAL(10, 2) null,
    corNotebook VARCHAR(40) NOT NULL,
    sistemaOperacionalNotebook VARCHAR(100) NOT NULL,
    processadorNotebook VARCHAR(100) NOT NULL,
	placaVideoNotebook VARCHAR(100) NOT NULL,
	telaNotebook VARCHAR(100) NOT NULL,
	memoriaNotebook VARCHAR(100) NOT NULL,
	armazenamentoNotebook VARCHAR(100) NOT NULL,
	portasConexaoNotebook VARCHAR(100) NOT NULL,
	cameraNotebook VARCHAR(100) NOT NULL,
	audioNotebook VARCHAR(100) NOT NULL,
	redeComunicacaoNotebook VARCHAR(100) NOT NULL,
	bateriaNotebook VARCHAR(100) NOT NULL,
	fonteAlimentacaoNotebook VARCHAR(100) NOT NULL,
	pesoNotebook VARCHAR(50) NOT NULL,
	incluidoNaCaixaNotebook VARCHAR(100) NOT NULL
);

-- 1. Tabela Pedido
CREATE TABLE Pedido (
    IdPedido INT AUTO_INCREMENT PRIMARY KEY,
    IdCliente INT NOT NULL, -- Relaciona com o cliente
    DataPedido DATETIME NOT NULL,
    ValorTotal DECIMAL(10, 2) NOT NULL
);
-- 2. Tabela Itens
CREATE TABLE Itens (
    IdItem INT AUTO_INCREMENT PRIMARY KEY,
    IdPedido INT NOT NULL, -- Relaciona com Pedido
    IdProduto INT NOT NULL, -- Relaciona com Produto
    QtdItens INT NOT NULL, -- Quantidade de produtos no pedido
    Preco DECIMAL(10, 2) NOT NULL, -- Preço unitário no momento da compra
    Garantia DECIMAL(10, 2) DEFAULT 0, -- Garantia adicional (opcional)
    ValorParcial DECIMAL(10, 2) NOT NULL DEFAULT 0, -- (QtdItens * (Preco + Garantia))
    ValorTotal DECIMAL(10, 2) NOT NULL, -- (QtdItens * (Preco + Garantia))
    NomeProduto VARCHAR(255) NOT NULL,
    FOREIGN KEY (IdPedido) REFERENCES Pedido(IdPedido) ON DELETE CASCADE
);




CREATE TABLE Cliente (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Nascimento DATE NOT NULL,
    Sexo CHAR(1),
    CPF VARCHAR(14) NOT NULL,
    Telefone VARCHAR(14) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    Senha VARCHAR(30) NOT NULL,
    Situacao CHAR(1) NOT NULL,
    Endereco VARCHAR(100),
    Cidade VARCHAR(50),
    Cep VARCHAR(10)
);


create table Colaborador(
Id int auto_increment primary key,
Nome varchar(50) not null,
Email varchar(50) not null,
Senha varchar(30) not null,
Tipo varchar(8) not null
);



INSERT INTO Cliente (Nome, Nascimento, Sexo, CPF, Telefone, Email, Senha, Situacao, Endereco, Cidade, Cep)
VALUES ('Rafael', '2003-08-04', 'M', '12332156776', '11 4002-8922', 'rafael@gmail.com', '12345',  'A', 'Rua São João', 'São Paulo', '02312-120');


select * from Cliente;


INSERT INTO Colaborador (Nome, Email, Senha, Tipo)
VALUES ('Joao', 'joao@gmail.com', '123', 'C');

INSERT INTO Colaborador (Nome, Email, Senha, Tipo)
VALUES ('Alexandre', 'alexandre@gmail.com', '12345', 'G');

	

select * from Cliente;
select * from Colaborador;
select * from tbNotebook;