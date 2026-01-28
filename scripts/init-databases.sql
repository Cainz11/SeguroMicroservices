-- Script de inicialização dos bancos de dados SQL Server
-- Execute este script após iniciar o container SQL Server

-- Criar banco de dados para PropostaService
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PropostaDB')
BEGIN
    CREATE DATABASE PropostaDB;
END
GO

-- Criar banco de dados para ContratacaoService
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ContratacaoDB')
BEGIN
    CREATE DATABASE ContratacaoDB;
END
GO

-- As tabelas serão criadas automaticamente pelo Entity Framework através das migrations
