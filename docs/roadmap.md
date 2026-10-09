# Roadmap — GameHub

## Objetivo

Desenvolver o GameHub 1.0, uma plataforma para descobrir jogos e gerenciar uma biblioteca pessoal, com frontend Angular, backend ASP.NET Core e banco de dados PostgreSQL.

O desenvolvimento será incremental. Cada etapa deverá produzir uma entrega funcional antes de avançarmos para a próxima.

## Etapa 1 — Catálogo inicial

**Objetivo:** apresentar jogos da RAWG na interface Angular.

- [x] Criar a estrutura inicial do frontend.
- [x] Criar o serviço de integração com a RAWG.
- [x] Criar os modelos iniciais para os dados recebidos.
- [x] Exibir jogos em cards na página inicial.
- [x] Implementar estados de carregamento e erro.
- [x] Criar testes básicos para o serviço da RAWG.
- [x] Validar a compilação do frontend.
- [ ] Revisar a interface do catálogo e o estado vazio.
- [ ] Implementar pesquisa, filtros e paginação.
- [ ] Preparar a visualização de detalhes dos jogos.

**Critério de conclusão:** o usuário consegue explorar o catálogo com feedback adequado durante o carregamento, em caso de erro e quando não houver resultados.

## Etapa 2 — Backend e integração com a RAWG

**Objetivo:** criar a API própria do GameHub e retirar a dependência direta da RAWG no frontend.

- [ ] Criar a solução ASP.NET Core Web API.
- [ ] Configurar os ambientes de desenvolvimento.
- [ ] Criar a configuração segura da chave da RAWG.
- [ ] Implementar um serviço backend para consultar a RAWG.
- [ ] Criar endpoints para listar e pesquisar jogos.
- [ ] Implementar filtros e paginação compatíveis com o catálogo.
- [ ] Configurar o frontend para consumir a API do GameHub.
- [ ] Tratar erros e respostas da API.
- [ ] Criar testes para os serviços e endpoints principais.

**Critério de conclusão:** o frontend consulta o backend do GameHub, e o backend faz a integração com a RAWG sem expor a chave de acesso no navegador.

## Etapa 3 — Banco de dados e biblioteca pessoal

**Objetivo:** permitir que os dados da biblioteca sejam persistidos.

- [ ] Configurar o PostgreSQL.
- [ ] Integrar o Entity Framework Core.
- [ ] Criar a estrutura inicial das entidades e relacionamentos.
- [ ] Criar e aplicar migrations.
- [ ] Criar os endpoints para adicionar e remover jogos da biblioteca.
- [ ] Implementar os status: Quero jogar, Jogando, Concluído e Abandonado.
- [ ] Permitir selecionar a plataforma em que o usuário possui ou pretende jogar o título.
- [ ] Permitir avaliação pessoal e anotação opcional.
- [ ] Validar as operações de leitura e escrita.

**Critério de conclusão:** os jogos adicionados à biblioteca permanecem salvos no banco de dados e podem ser consultados posteriormente.

## Etapa 4 — Autenticação e segurança

**Objetivo:** garantir que cada usuário tenha acesso somente à própria biblioteca.

- [ ] Implementar cadastro de usuário.
- [ ] Implementar login e logout.
- [ ] Definir o mecanismo de autenticação da API.
- [ ] Proteger os endpoints da biblioteca.
- [ ] Validar os dados recebidos.
- [ ] Garantir a autorização por usuário.
- [ ] Criar testes para os principais cenários de autenticação e autorização.

**Critério de conclusão:** usuários autenticados conseguem gerenciar suas próprias bibliotecas, sem acessar ou alterar os dados de outras pessoas.

## Etapa 5 — Dashboard e experiência de uso

**Objetivo:** facilitar o acompanhamento da biblioteca pessoal.

- [ ] Exibir a quantidade total de jogos da biblioteca.
- [ ] Exibir totais por status.
- [ ] Criar acesso rápido aos jogos em andamento.
- [ ] Criar acesso rápido aos jogos que o usuário deseja jogar.
- [ ] Revisar responsividade e navegação.
- [ ] Padronizar mensagens de erro, sucesso, carregamento e ausência de resultados.

**Critério de conclusão:** o usuário consegue consultar rapidamente a situação da própria biblioteca e navegar pelas funcionalidades principais.

## Etapa 6 — Qualidade e publicação

**Objetivo:** preparar uma versão demonstrável do GameHub 1.0.

- [ ] Revisar os testes automatizados.
- [ ] Executar os testes de integração dos fluxos principais.
- [ ] Revisar segurança e configuração de segredos.
- [ ] Configurar variáveis de ambiente para publicação.
- [ ] Publicar frontend, backend e banco de dados.
- [ ] Atualizar as instruções de execução.
- [ ] Registrar limitações conhecidas e decisões técnicas.

**Critério de conclusão:** a aplicação pode ser acessada e demonstrada em um ambiente publicado, com as funcionalidades previstas para o MVP operacionais.

## Fora do escopo da versão 1.0

Os itens abaixo poderão ser avaliados depois da primeira versão:

- Integração com a PSN.
- Recursos sociais e perfis públicos.
- Recomendações personalizadas avançadas.
- Estatísticas detalhadas e histórico de gameplay.
- Metas, conquistas e acompanhamento de horas jogadas.
- Loja, preços e compras de jogos.
- Aplicativo mobile nativo.

## Regra de execução

Trabalhar em pequenas entregas. Antes de iniciar uma tarefa, verificar seus critérios de aceitação. Ao concluir, testar, atualizar a documentação necessária e registrar a mudança no Git.

O backlog detalha as tarefas executáveis de cada etapa.
