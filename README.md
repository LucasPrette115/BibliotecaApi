# 📚 Biblioteca API

API REST desenvolvida em **ASP.NET Core** para gerenciamento de uma biblioteca, contemplando cadastro de usuários, livros e controle de empréstimos com cálculo de multas.

---

## 🗂 Estrutura do Projeto

```
BibliotecaApi/
├── Application/        # Controllers e Responses
├── Domain/Entities/    # Entidades do domínio
├── Infrastructure/     # Repositórios, Autenticação e IoC
├── OptionsSetup/       # Configuração do JWT
├── UseCases/           # Casos de uso por domínio
├── Program.cs
└── appsettings.json
```

---

## 🚀 Como Executar

### ✅ Sem Docker

**Pré-requisitos:**
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

```bash
# Clone o repositório
git clone https://github.com/LucasPrette115/BibliotecaApi.git
cd BibliotecaApi

# Execute o projeto
dotnet run
```

A API estará disponível em: `http://localhost:5023`

Acesse o Swagger em: `https://localhost:5023/swagger`

---

### 🐳 Com Docker

**Pré-requisitos:**
- [Docker](https://www.docker.com/)

- > ℹ️ O container já está configurado com `ASPNETCORE_ENVIRONMENT=Development`,
> habilitando o Swagger automaticamente.
```bash
# Clone o repositório
git clone https://github.com/LucasPrette115/BibliotecaApi.git
cd BibliotecaApi

# Build da imagem
docker build -t biblioteca-api .

# Execute o container
docker run -p 8080:8080 biblioteca-api
```

A API estará disponível em: `http://localhost:8080`

Acesse o Swagger em: `http://localhost:8080/swagger`

---

## 🗄️ Banco de Dados

O projeto utiliza **SQLite** com o banco de dados já incluído no repositório (`Biblioteca.db`) com dados iniciais para facilitar os testes.

**Schema da tabela Usuarios:**
```sql
CREATE TABLE "Usuarios" (
    "id"           INTEGER NOT NULL,
    "nome"         TEXT(100),
    "cpf"          TEXT(11),
    "email"        TEXT(100),
    "possui_atraso" INTEGER DEFAULT 0,
    "senha_hash"   TEXT,
    CONSTRAINT "Usuarios_PK" PRIMARY KEY("id")
);
```

> ⚠️ O banco já contém dados de exemplo.

---

## 🔐 Autenticação

A API utiliza **JWT Bearer Token** para proteger endpoints sensíveis.

### Endpoints protegidos:
- `POST /Livro/Cadastrar`
- `POST /Emprestimo/Cadastrar`
- `POST /Emprestimo/Devolver`

### Como autenticar:

**1. Faça login com o usuário de teste:**

```http
POST /Usuario/Login
Content-Type: application/json

{
    "email": "avaliador@biblioteca.com",
    "senha": "Admin@123"
}
```

**2. Use o token retornado no header:**

```http
Authorization: Bearer {token}
```

> ⚠️ A `SecretKey` do JWT está no `appsettings.json` por se tratar de um ambiente de desenvolvimento/avaliação. Em produção, recomenda-se o uso de variáveis de ambiente ou um cofre de segredos (ex: Azure Key Vault).

---

## 👤 Usuário de Teste

| Campo | Valor |
|-------|-------|
| Email | avaliador@biblioteca.com |
| Senha | Admin@123 |

---

## ✅ Desafios Implementados

| # | Tipo | Descrição | Status |
|---|------|-----------|--------|
| 1 | ⭐ FEATURE | Validar CPF duplicado no cadastro de usuário | ✅ Implementado |
| 2 | ⭐ FEATURE | Validar ISBN com exatamente 13 dígitos numéricos | ✅ Implementado |
| 3 | 🐛 BUG | Corrigir geração de multa para devoluções no prazo | ✅ Implementado |
| 4 | 🐛 BUG | Impedir empréstimo de livro já emprestado | ✅ Implementado |
| 5 | ⭐ FEATURE | Endpoint GET /Livro/Listar | ✅ Implementado |
| 6 | ⭐ FEATURE | Impedir novo empréstimo se usuário tiver atraso ativo | ✅ Implementado |
| 7 | ⭐ FEATURE | Cálculo de multa escalonado com limite máximo de R$ 50,00 | ✅ Implementado |
| 8 | ⭐ FEATURE | Autenticação JWT Bearer nos endpoints sensíveis | ✅ Implementado |

### Detalhes das implementações:

**Desafio 1 — CPF duplicado:**
- Validação realizada no `CadastrarUsuarioUC` antes de persistir
- Método `CpfExiste(string cpf)` no `UsuarioRepository` consulta via `SELECT COUNT(1)`
- Retorna `400 Bad Request` com mensagem: `"Usuário com este CPF já está cadastrado."`

**Desafio 2 — ISBN:**
- Validação realizada na entidade `LivroEntity` no método `ValidarDados`
- Verifica se contém apenas dígitos numéricos e exatamente 13 caracteres
- Retorna `400 Bad Request` com mensagem de erro apropriada

**Desafio 3 — Regra de multa corrigida:**
- Calcula `TimeSpan` entre `DataDevolucao` e `DataPrevistaDevolucao`
- Multa gerada somente quando `atraso.Days > 0`
- Devoluções no prazo ou antecipadas retornam multa `R$ 0,00`

**Desafio 4 — Livro já emprestado:**
- Método `EstaDisponivel(int idLivro)` no `LivroRepository` consulta coluna `disponivel` via `SELECT disponivel FROM Livros WHERE id = @id`
- Coluna `disponivel` é marcada como `FALSE` após registrar empréstimo e `TRUE` após devolução
- No `CadastrarEmprestimoUC`, se `!EstaDisponivel` lança exceção antes de persistir
- Retorna `400 Bad Request` com mensagem: `"Este livro já está emprestado e ainda não foi devolvido."`

**Desafio 5 — Listar livros:**
- Endpoint `GET /Livro/Listar` implementado no `LivroController`
- Método `Listar()` no `LivroRepository` retorna todos os registros via `SELECT id, titulo, autor, isbn`
- Retorna lista vazia `[]` quando não há registros

**Desafio 6 — Usuário com atraso:**
- Método `PossuiEmprestimoEmAtraso(int idUsuario)` no `EmprestimoRepository`
- Consulta empréstimos onde `data_devolucao IS NULL AND data_prevista_devolucao < DateTime.Now`
- Se possui atraso, marca `possui_atraso = TRUE` na tabela `Usuarios` e bloqueia o empréstimo
- Marcação é limpa automaticamente no `DevolverEmprestimoUC` após devolução
- Retorna `400 Bad Request` com mensagem: `"Usuário com empréstimo em atraso não pode realizar novo empréstimo."`

**Desafio 7 — Cálculo escalonado:**
- Até 3 dias → `atraso.Days * R$ 2,00`
- A partir do 4º dia → `(3 × R$ 2,00) + ((dias - 3) × R$ 3,50)`
- Limite máximo aplicado via `Math.Min(multa, 50.00m)`
- Exemplos:
  - 2 dias → R$ 4,00
  - 5 dias → R$ 13,00
  - 20 dias → R$ 50,00 (limitado)

**Desafio 8 — JWT Bearer Authentication:**
- Endpoint público `POST /Usuario/Login` recebe `email` e `senha`
- Senha armazenada com hash via `PBKDF2 + SHA512` com salt aleatório
- Token gerado pelo `JwtProvider` usando algoritmo `HmacSha256`
- Claims do token: `sub` (id do usuário) e `email`
- Expiração: 60 minutos
- Endpoints protegidos com `[Authorize]`: `POST /Livro/Cadastrar`, `POST /Emprestimo/Cadastrar`, `POST /Emprestimo/Devolver`
- Retorna `401 Unauthorized` para token inválido ou ausente