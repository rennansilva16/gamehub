# GameHub 🎮

O **GameHub** é uma plataforma pessoal para descobrir, organizar e acompanhar jogos de diferentes plataformas. A proposta é reunir um catálogo de jogos e permitir que cada usuário mantenha sua própria biblioteca, registrando o que deseja jogar, o que está jogando e o que já concluiu.

O projeto está sendo desenvolvido como uma aplicação full stack, utilizando Angular no frontend e ASP.NET Core no backend.

## Funcionalidades

### MVP — versão 1.0

- [x] Integração inicial do catálogo com a RAWG API.
- [x] Exibição de jogos em cards.
- [x] Estados iniciais de carregamento e erro.
- [ ] Pesquisa de jogos por nome.
- [ ] Filtros por plataforma e gênero.
- [ ] Paginação do catálogo.
- [ ] Visualização dos detalhes de um jogo.
- [ ] Cadastro, login e logout.
- [ ] Biblioteca pessoal de jogos.
- [ ] Organização dos jogos por status.
- [ ] Avaliação pessoal e anotações opcionais.
- [ ] Dashboard com resumo da biblioteca.

A lista representa o escopo planejado para a versão 1.0. Os itens marcados como concluídos correspondem à implementação inicial do catálogo no frontend, não à conclusão de toda a funcionalidade no produto final.

## Tecnologias

### Frontend
- Angular
- TypeScript
- HTML e CSS

### Backend
- C#
- ASP.NET Core Web API
- Entity Framework Core

### Banco de dados
- PostgreSQL

### Integração externa
- RAWG Video Games Database API

### Testes
- Vitest e ferramentas de teste do Angular para o frontend.
- Ferramentas de testes automatizados do .NET para o backend, a definir durante sua implementação.

## Arquitetura planejada

O Angular será responsável pela interface e pela interação com o usuário. O backend em ASP.NET Core disponibilizará os endpoints da aplicação, fará a integração com a RAWG e controlará o acesso aos dados persistidos no PostgreSQL.

A API key da RAWG deverá ficar no backend, sem ser exposta no código público do frontend.

O banco de dados será utilizado inicialmente para armazenar usuários e suas bibliotecas pessoais. O catálogo completo da RAWG não será copiado para o banco nesta primeira versão.

## Como executar o frontend

### Pré-requisitos

- Node.js e npm.
- Uma chave de API da RAWG para a integração local atual.

### Instalação

```bash
npm install
```

Configure a chave da RAWG no arquivo de ambiente de desenvolvimento utilizado pelo projeto. Não compartilhe nem versione esse arquivo com a chave real.

### Executar

```bash
ng serve
```

Depois, acesse `http://localhost:4200/`.

> **Nota:** os comandos acima se referem ao frontend Angular atual. O backend e o banco de dados terão instruções próprias quando estiverem implementados.

## Organização do projeto

A estrutura pode evoluir conforme o desenvolvimento:

```text
GameHub/
├── docs/
│   ├── roadmap.md
│   └── backlog.md
├── src/
│   └── ...
└── README.md
```

## Escopo da versão 1.0

O foco é criar um catálogo funcional, permitir autenticação e oferecer uma biblioteca pessoal persistida no banco de dados.

Recursos como integração com a PSN, funcionalidades sociais, recomendações avançadas, estatísticas detalhadas e compras não fazem parte desta versão.

## Documentação

- [Roadmap](docs/roadmap.md): etapas planejadas para o desenvolvimento.
- [Backlog](docs/backlog.md): tarefas e melhorias a implementar.

## Status

O projeto está em desenvolvimento. A integração inicial do catálogo com a RAWG foi implementada no frontend. As próximas etapas incluem evoluir o catálogo e iniciar o backend para centralizar a integração externa e preparar a persistência dos dados.
