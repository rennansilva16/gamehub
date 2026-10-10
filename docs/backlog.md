# Backlog — GameHub

## Objetivo

Manter uma lista organizada das tarefas necessárias para desenvolver o GameHub 1.0. As tarefas devem ser pequenas o suficiente para serem implementadas, testadas e revisadas individualmente.

## Prioridades

- **P0 — Essencial:** necessário para o funcionamento do MVP.
- **P1 — Importante:** melhora uma funcionalidade principal.
- **P2 — Futuro:** pode ficar para depois da primeira versão.

## Etapa 1 — Catálogo Angular

- [x] **P0 — Integrar catálogo com a RAWG:** consultar a API e exibir os jogos na página inicial.
- [x] **P0 — Criar modelos da RAWG:** tipar os dados utilizados pelo frontend.
- [x] **P0 — Exibir jogos em cards:** apresentar capa e nome dos jogos.
- [x] **P0 — Tratar carregamento e erro:** informar ao usuário quando os jogos estiverem carregando ou quando ocorrer uma falha.
- [x] **P0 — Testar o serviço da RAWG:** verificar a requisição HTTP e os parâmetros enviados.
- [x] **P0 — Validar compilação:** confirmar que o frontend compila após a integração inicial.

**Critério de conclusão:** aplicação Angular executando de forma estável, consultando a RAWG e exibindo os jogos na página inicial, com estados de carregamento e tratamento de erros.

## Etapa 2 — Backend e integração com a RAWG

- [ ] **P0 — Criar projeto ASP.NET Core Web API:** estabelecer a estrutura inicial do backend.
- [ ] **P0 — Configurar a aplicação:** preparar a execução local e a comunicação entre frontend e backend.
- [ ] **P0 — Configurar a chave da RAWG com segurança:** manter a chave fora do código público.
- [ ] **P0 — Implementar integração com a RAWG:** criar um serviço backend responsável pelas consultas externas.
- [ ] **P0 — Criar endpoint de catálogo:** disponibilizar os jogos da RAWG para o frontend.
- [ ] **P0 — Integrar Angular à API do GameHub:** substituir as chamadas diretas à RAWG pelas chamadas à API própria.
- [ ] **P0 — Tratar falhas do backend:** permitir que o frontend apresente mensagens adequadas quando ocorrerem erros.
- [ ] **P1 — Criar testes do backend:** verificar os principais comportamentos dos serviços e endpoints.

**Critério de conclusão:** o Angular continua exibindo os jogos como atualmente, mas passa a consultar a API do GameHub, que será responsável por buscar os dados na RAWG sem expor a chave de acesso no navegador.

## Etapa 3 — PostgreSQL e biblioteca

- [ ] **P0 — Configurar PostgreSQL:** preparar a conexão com o banco de dados.
- [ ] **P0 — Configurar Entity Framework Core:** integrar o ORM ao backend.
- [ ] **P0 — Modelar a biblioteca pessoal:** definir como os jogos salvos serão representados.
- [ ] **P0 — Criar migrations:** versionar a estrutura do banco.
- [ ] **P0 — Adicionar jogo à biblioteca:** permitir salvar um jogo escolhido no catálogo.
- [ ] **P0 — Remover jogo da biblioteca:** permitir excluir um registro pessoal.
- [ ] **P0 — Atualizar status:** permitir escolher entre Quero jogar, Jogando, Concluído e Abandonado.
- [ ] **P0 — Registrar plataforma:** armazenar a plataforma escolhida para cada registro.
- [ ] **P1 — Adicionar avaliação pessoal:** permitir avaliar um jogo.
- [ ] **P1 — Adicionar anotações:** permitir salvar observações opcionais.
- [ ] **P0 — Testar persistência:** verificar se os registros continuam disponíveis após novas consultas à aplicação.

## Etapa 4 — Autenticação e autorização

- [ ] **P0 — Definir o modelo de usuário:** estabelecer os dados necessários para cadastro e identificação.
- [ ] **P0 — Implementar cadastro:** permitir criar uma conta.
- [ ] **P0 — Implementar login e logout:** gerenciar o acesso à aplicação.
- [ ] **P0 — Proteger a biblioteca:** exigir autenticação nas operações pessoais.
- [ ] **P0 — Restringir acesso por usuário:** impedir a consulta ou alteração da biblioteca de outra pessoa.
- [ ] **P0 — Validar autorização:** testar tentativas de acesso indevido.
- [ ] **P1 — Implementar recuperação de acesso:** avaliar a funcionalidade após o fluxo básico de autenticação.

## Etapa 5 — Dashboard e usabilidade

- [ ] **P0 — Criar dashboard pessoal:** apresentar um resumo da biblioteca.
- [ ] **P0 — Exibir totais por status:** mostrar a distribuição dos jogos.
- [ ] **P0 — Criar acesso rápido aos jogos em andamento.**
- [ ] **P0 — Criar acesso rápido aos jogos que o usuário deseja jogar.**
- [ ] **P1 — Revisar responsividade:** adaptar as telas a diferentes tamanhos de dispositivo.
- [ ] **P1 — Padronizar feedbacks:** revisar mensagens de sucesso, erro e carregamento.

## Etapa 6 — Testes e publicação

- [ ] **P0 — Revisar testes automatizados:** garantir cobertura dos fluxos principais.
- [ ] **P0 — Testar o fluxo completo:** catálogo, autenticação e biblioteca.
- [ ] **P0 — Revisar segredos e configurações:** evitar exposição de chaves e credenciais.
- [ ] **P0 — Preparar ambiente de produção:** configurar frontend, backend e banco.
- [ ] **P0 — Publicar a aplicação:** disponibilizar uma versão demonstrável.
- [ ] **P0 — Atualizar documentação:** registrar como executar e configurar o projeto.

## Melhorias futuras — fora do MVP

- [ ] **P2 — Integrar com a PSN.**
- [ ] **P2 — Criar recursos sociais.**
- [ ] **P2 — Adicionar recomendações personalizadas.**
- [ ] **P2 — Criar estatísticas detalhadas.**
- [ ] **P2 — Adicionar metas e conquistas pessoais.**
- [ ] **P2 — Avaliar uma versão mobile nativa.**
- [ ] **P2 — Desenvolver um projeto separado para loja e preços.**

## Como utilizar o backlog

1. Escolher uma tarefa da etapa atual.
2. Criar uma issue no GitHub com descrição e critérios de aceitação.
3. Implementar somente o necessário para concluir a tarefa.
4. Executar os testes e validar o resultado.
5. Atualizar o status e registrar a alteração no Git.

As tarefas concluídas devem permanecer marcadas para manter o histórico do projeto. Novas tarefas podem ser adicionadas quando surgirem necessidades reais durante a implementação.
