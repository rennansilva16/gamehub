# GameHub 🎮

O **GameHub** é uma plataforma pessoal para descobrir, organizar e acompanhar jogos de diferentes plataformas. A proposta é reunir um catálogo de jogos e permitir que cada usuário mantenha sua própria biblioteca, registrando o que deseja jogar, o que está jogando e o que já concluiu.

O projeto está sendo desenvolvido como uma aplicação full stack, utilizando Angular no frontend e ASP.NET Core no backend.

## Funcionalidades

### MVP — versão 1.0

- [x] Integração inicial do catálogo com a RAWG API no frontend.
- [x] Exibição de jogos em cards.
- [x] Estados iniciais de carregamento e erro.
- [ ] Criação da API própria para integração com a RAWG.
- [ ] Pesquisa de jogos por nome.
- [ ] Filtros por plataforma e gênero.
- [ ] Paginação do catálogo.
- [ ] Visualização dos detalhes de um jogo.
- [ ] Cadastro, login e logout.
- [ ] Biblioteca pessoal de jogos.
- [ ] Organização dos jogos por status.
- [ ] Dashboard com resumo da biblioteca.

## Tecnologias

### Frontend
- Angular
- TypeScript
- HTML e CSS

### Backend
- C#
- ASP.NET Core Web API

### Banco de dados
- PostgreSQL (planejado)
- Entity Framework Core (planejado)

### Integração externa
- RAWG Video Games Database API

## Arquitetura

O Angular será responsável pela interface e pela interação com o usuário. O backend em ASP.NET Core disponibilizará os endpoints da aplicação, fará a integração com a RAWG e controlará o acesso aos dados persistidos no PostgreSQL.

A chave da API da RAWG deverá ficar no backend, sem ser exposta no código público do frontend.

O banco de dados será utilizado inicialmente para armazenar usuários e suas bibliotecas pessoais. O catálogo completo da RAWG não será copiado para o banco nesta primeira versão.

## Estrutura do projeto

A estrutura atual separa o backend da documentação do projeto. Ela poderá evoluir conforme novas funcionalidades forem implementadas.

```text
GameHub/
├── backend/
│   ├── GameHub.Api/
│   │   │   └── ...
│   └── GameHub.slnx
├── docs/
│   ├── roadmap.md
│   └── backlog.md
├── src/
│   └── ...
└── README.md
```

> A estrutura apresentada é uma referência para a organização atual do projeto. Os arquivos e diretórios podem mudar conforme o desenvolvimento evoluir.

## Como executar o frontend

### Pré-requisitos

- Node.js
- npm

### Instalação

Na raiz do projeto Angular, execute:

```bash
npm install
```

### Execução

```bash
ng serve
```

Depois, acesse [http://localhost:4200/](http://localhost:4200/).

## Como executar o backend

### Pré-requisitos

- .NET SDK 10

### Execução

Acesse o diretório do projeto da API:

```bash
cd backend/GameHub.Api
```

Execute:

```bash
dotnet run
```

## Documentação

- [Roadmap](docs/roadmap.md): etapas planejadas para o desenvolvimento.
- [Backlog](docs/backlog.md): tarefas, funcionalidades e melhorias a implementar.

## Status

O projeto está em desenvolvimento.

A integração inicial do catálogo com a RAWG foi implementada diretamente no frontend. A estrutura inicial do backend ASP.NET Core Web API também foi criada, e sua configuração e validação local estão em andamento.

As próximas etapas incluem preparar a API própria para intermediar as consultas à RAWG, implementar a persistência de dados dos usuários e desenvolver as funcionalidades de autenticação e biblioteca pessoal.
