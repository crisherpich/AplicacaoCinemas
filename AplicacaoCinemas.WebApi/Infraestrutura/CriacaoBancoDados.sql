drop table if exists [AplicacaoCinemas].[dbo].[Sessoes];

drop table if exists [AplicacaoCinemas].[dbo].[Filmes]; 

drop database if exists AplicacaoCinemas;

Create database AplicacaoCinemas;

CREATE TABLE [AplicacaoCinemas].[dbo].[Filmes] (
    [Id]      UNIQUEIDENTIFIER NOT NULL,
    [Titulo]  VARCHAR (50)     NOT NULL,
    [Sinopse] VARCHAR (MAX)    NULL,
    [Duracao] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [AplicacaoCinemas].[dbo].[Sessoes] (
    [Id]                      UNIQUEIDENTIFIER NOT NULL,
    [Dia]                     DATE             NOT NULL,
    [HoraInicio]              INT              NOT NULL,
    [MinutoInicio]            INT              NOT NULL,
    [QuantidadeLugares]       INT              NOT NULL,
    [QuantidadeLugaresLivres] INT              NOT NULL,
    [Preco]                   FLOAT (53)       NOT NULL,
    [IdFilme]                 UNIQUEIDENTIFIER NOT NULL,
    [token_concorrencia]      VARCHAR (MAX)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Sessoes_ToTable] FOREIGN KEY ([IdFilme]) REFERENCES [dbo].[Filmes] ([Id])
);