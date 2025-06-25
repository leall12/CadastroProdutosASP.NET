drop database dbCadastroProdutos;
create database dbCadastroProdutos;
use dbCadastroProdutos;

create table tbUsuarios(
IdUser int primary key auto_increment,
Nome varchar(50) not null,
Email varchar(60) not null,
Senha varchar(50) not null
);

create table tbProdutos(
IdProd int primary key auto_increment,
Nome varchar(50) not null,
Descricao varchar(180) not null,
Preco decimal(6,2) not null,
Quantidade int not null
);

select * from tbUsuarios;
select * from tbProdutos;
