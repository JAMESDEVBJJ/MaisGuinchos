# 📌 +Guinchos BE (Mais Guinchos Backend)

Backend da aplicação **Mais Guinchos**, responsável por gerenciar solicitações de guincho em tempo real entre clientes e motoristas.

O sistema atende **motoristas, clientes e empresas**, permitindo:

- Definir localização e destino  
- Buscar guinchos próximos  
- Calcular rota e viagem  
- Criar solicitações de reboque  
- Motoristas aceitarem ou enviarem contra propostas  

---

## 📌 Tecnologias utilizadas

- **C#**
- **ASP.NET Core**
- **Entity Framework Core**
- **PostgreSQL**
- **JWT (autenticação e autorização)**
- **SignalR (WebSockets)**
- **Middlewares**
- **Arquitetura em camadas (Controller / Service / Repository)**

---

## ⚙️ Pré-requisitos

Antes de rodar o projeto, você precisa ter instalado:

- **.NET SDK** (recomendado versão 8 ou superior)
- **PostgreSQL**
- **Git**

---

## 🚀 Como rodar o projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/JAMESDEVBJJ/MaisGuinchos

cd MaisGuinchos
```

2. Restaurar dependências
dotnet restore
3. Configurar o banco de dados

No arquivo appsettings.json, configure sua string de conexão:

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=MaisGuinchos;Username=seu_user;Password=sua_senha"
}
4. Rodar migrations (Entity Framework)
dotnet ef database update
5. Executar o projeto
dotnet run
🔄 Fluxo da aplicação

Cliente define localização e destino

Sistema calcula a rota e viagem

Cliente cria uma solicitação de guincho

Motoristas próximos recebem a solicitação em tempo real (SignalR)

O motorista pode:

✅ Aceitar

❌ Recusar

💰 Enviar contra proposta (ajuste de valor com justificativa)

👉 O cliente recebe todas as atualizações instantaneamente

🔐 Autenticação (JWT)

A aplicação utiliza JWT (JSON Web Token) para autenticação.

Usuários precisam realizar login para acessar rotas protegidas

O token deve ser enviado no header:

Authorization: Bearer {seu_token}
⚙️ Configuração JWT

No arquivo appsettings.json:

"Jwt": {
  "Key": "sua-chave-secreta",
  "Issuer": "MaisGuinchos"
}
🔄 Comunicação em tempo real

O sistema utiliza SignalR para:

Envio de solicitações de guincho em tempo real

Recebimento de propostas e contra propostas

Atualização instantânea entre cliente e motorista

🧱 Arquitetura

O projeto segue o padrão de arquitetura em camadas:

Controllers → entrada das requisições HTTP

Services → regras de negócio

Repositories → acesso ao banco de dados

DTOs → transporte de dados

Entities → modelos do banco

Interfaces → desacoplamento

DI (Dependency Injection) → injeção de dependência

Middlewares → tratamento de exceções e pipeline

📦 Funcionalidades

Cadastro e login de usuários

Autenticação com JWT

Criação de solicitações de guincho personalizadas

Envio de propostas por motoristas

Contra propostas em tempo real (SignalR)

Atualização de status das solicitações

Histórico de localizações por usuários

Filtros de busca

Consulta de dados de usuários

Persistência de imagens no disco

📌 Observações

Projeto em desenvolvimento

Pode conter melhorias futuras como:

Logs estruturados

Testes automatizados

Dockerização

👨‍💻 Autor

Desenvolvido por James Daniel Xavier
