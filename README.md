Projeto: ControleTeste
=====================

Visão Geral
-----------
Aplicação web em ASP.NET Core Razor Pages (.NET 8) para controle de alterações e gestão de usuários. Usa padrão Repository/Service, autenticação via JWT (token em cookie HttpOnly) e autorização baseada em claim isAdmin para operações administrativas.

Público-alvo do README
----------------------
Desenvolvedores que vão: rodar localmente, criar novas features (Razor Pages), manter serviços/repositórios e entender o fluxo de autenticação/autorizações.

Stack Tecnológico
-----------------
- .NET 8 (Razor Pages)
- ASP.NET Core Authentication (JWT em cookie)
- Padrão Repository + Service
- Views: Razor Pages (Pages/**/*.cshtml + PageModel .cshtml.cs)
- Front-end: Bootstrap 5, Bootstrap Icons, JS/CSS em wwwroot
- Banco: SQL Server (configurável via appsettings) — pode usar EF Core ou outro provedor (ver Services/Repositories)

Estrutura importante do repositório
-----------------------------------
- Pages/Users/           -> CRUD de usuários (Create.cshtml, Index.cshtml, Edit.cshtml, Delete.cshtml e respectivos PageModels .cshtml.cs)
- Pages/Alteracoes/      -> Páginas do sistema principal (dashboard)
- Pages/Account/         -> Login/Logout handlers
- Pages/Shared/_Layout.cshtml -> Layout principal, navbar, inclusão de css/js
- Models/                -> Entidades (ex.: Usuario.cs)
- Services/              -> Regras de negócio (IUserService, IAlteracaoService, etc.)
- Repositories/          -> Acesso a dados (IUserRepository etc.)
- wwwroot/css/site.css   -> Estilos customizados
- wwwroot/js/            -> Scripts customizados
- Middleware/            -> Middlewares (ex.: ExceptionHandlingMiddleware.cs)
- Program.cs / Startup   -> Configuração de DI, autenticação e pipeline HTTP

Rodando o projeto localmente
---------------------------
1. Restaurar pacotes e build:
   - dotnet restore
   - dotnet build
2. Atualizar connection string em appsettings.Development.json (se aplicável)
3. Se o projeto usa EF Core (ver pastas Repositories/ e file de DbContext):
   - dotnet ef database update
   - Ou: dotnet ef migrations add "Init" && dotnet ef database update
4. Executar:
   - dotnet run
   - Ou abrir a solução no Visual Studio e executar (F5)
5. Acesse: https://localhost:5001/ (ou a porta configurada)

Autenticação e autorização
--------------------------
- Fluxo: Login gera JWT assinado e armazenado em Cookie HttpOnly. Middleware extrai e valida token para construir ClaimsPrincipal.
- Autorização: existe uma policy "IsAdmin" usada em páginas sensíveis (ex.: Pages/Users/Create.cshtml.cs)
- Para criar testes de role, crie um usuário com IsAdmin=true no banco ou use scripts/seeders existentes.

Como adicionar uma nova feature (fluxo recomendado)
-------------------------------------------------
1. Planeje a responsabilidade: UI (Razor Page) vs API vs Serviço.
2. Model/Entidade: adicione em Models/ se necessário.
3. Repositório:
   - Interface: Repositories/IYourEntityRepository.cs
   - Implementação: Repositories/YourEntityRepository.cs (injeção de DbContext/connection)
4. Serviço:
   - Interface: Services/IYourEntityService.cs
   - Implementação: Services/YourEntityService.cs
   - Métodos: coloque lógica de negócio e chamadas para o repositório
5. Registrar DI:
   - Em Program.cs, registre: services.AddScoped<IYourEntityRepository, YourEntityRepository>(); services.AddScoped<IYourEntityService, YourEntityService>();
6. Razor Page (UI):
   - Crie Pages/YourFeature/Index.cshtml e Index.cshtml.cs (ou Create/Edit/Delete conforme CRUD)
   - No PageModel (cshtml.cs), injete o service via construtor
   - Use [Authorize] / Policies conforme necessário
   - Use Tag Helpers e asp-for para model binding
7. Validação:
   - Use data annotations (System.ComponentModel.DataAnnotations) no InputModel e asp-validation-for na view
   - Habilite validação do lado cliente com scripts já incluídos (jquery-validate + unobtrusive)
8. Testar manualmente e criar/atualizar migrations se alterar esquema
9. Commit: siga o fluxo git do projeto (branch feature/..., PR para main)

Exemplo mínimo para adicionar uma página Razor (Create):
- Pages/YourFeature/Create.cshtml (form com inputs usando asp-for)
- Pages/YourFeature/Create.cshtml.cs:
  - definir InputModel com [BindProperty]
  - injetar IYourService
  - OnPostAsync chama service.CreateAsync(...) e redireciona

Boas práticas e convenções adotadas
----------------------------------
- Padrão Repository/Service: nada de lógica de negócio em PageModels
- PageModels devem ser "thin" (validar modelstate e chamar serviços)
- Nomes: serviços começam com I, implementações sem I
- Autorização por Policy sempre que for operação administrativa
- Validação com DataAnnotations
- Scripts e CSS centralizados em wwwroot

Dicas rápidas sobre Razor Pages (o que estudar)
-----------------------------------------------
- Documentação oficial: https://learn.microsoft.com/aspnet/core/razor-pages
- Conceitos: PageModel, handlers (OnGet/OnPost/OnPostAsync), Tag Helpers (asp-for, asp-action), Partial Views, View Components
- Model Binding e Validation (DataAnnotations e client-side validation)
- Autenticação/Authorization no ASP.NET Core (claims, policies, JWT bearer)
- DI no ASP.NET Core

Debugging e checagens úteis
---------------------------
- Verificar carregamento de arquivos estáticos: abra DevTools → Network para ver /css/site.css e /js/*.js
- Verificar token JWT no cookie (nome configurado em Program.cs) para autenticação
- Logs: configurar logging mínimo em appsettings.Development.json para depurar
- Erros no servidor: ver Middleware/ExceptionHandlingMiddleware.cs (existe um middleware customizável)

Comandos Git/Workflow sugeridos
------------------------------
- Criar branch para feature: git checkout -b feature/minha-feature
- Commit atômico com mensagem clara
- Push e abra PR: git push origin feature/minha-feature
- Rebase/merge conforme regra do repositório

Testes
------
- Se houver projeto de testes, use dotnet test
- Caso não haja, recomendo criar testes unitários para Services (mocks de repositório) e testes de integração para páginas críticas

Deploy
------
- Build: dotnet publish -c Release
- Configurar variáveis de ambiente (ConnectionStrings, JWT signing key, Cookie options)
- Host no IIS, Azure App Service, ou container Docker (adicionar Dockerfile se necessário)

Contato e referências internas
-----------------------------
- Arquivos chave para inspeção rápida:
  - Pages/Shared/_Layout.cshtml (layout e inclusão de CSS/JS)
  - Pages/Account/Login.cshtml(.cs) (fluxo de autenticação)
  - Services/IUserService + Repositories/IUserRepository
  - Middleware/ExceptionHandlingMiddleware.cs

Observações finais e checklist ao criar uma feature
--------------------------------------------------
- [ ] Atualizar Models/Services/Repositories conforme necessidade
- [ ] Registrar DI em Program.cs
- [ ] Criar/Atualizar Razor Pages com validação e autorizações
- [ ] Rodar migrations (se alterar esquema)
- [ ] Testar fluxo manualmente (login -> acessar página)
- [ ] Commitar em branch e abrir PR

