<h1 align="center" style="font-weight: bold;"> Clone Backend Twitter</h1>

<div align="center">

![c#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![.net](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![postman](https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=Postman&logoColor=white)
![sql](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)

</div>
<p align="center">Este é um projeto de backend inspirado no Twitter, 
desenvolvido com ASP.NET Core e Entity Framework Core. 
Ele fornece funcionalidades essenciais para uma rede social baseada em tweets, 
incluindo autenticação de usuários, postagens, curtidas, seguidores 
e interações em tempo real.</p>


## 📌 Funcionalidades
-[x] Autenticação usuários via **JWT**  
-[x] Postagem de tweets  
-[x] Sistema de curtidas (like/unlike)    
-[x] Sistema de seguidores (follow/unfollow)    
-[x] Feed personalizado de tweets  
-[x] Pesquisa de tweets e usuários  
-[x] Tendências baseadas em hashtags (**Trends**)  
-[x] **Validação de dados com FluentValidation**  
-[x] Integração com banco de dados SQL Server

## 🚀 Como Executar

### 💻Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) (Versão 6 ou superior)
- [Visual Studio](https://visualstudio.microsoft.com/) (Recomendado) ou [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/) (Opcional, para clonar o repositório)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Versão 2019 ou superior) - **Obrigatório para persistência dos dados**.
- [Postman](https://www.postman.com/downloads/) ou [Insomnia](https://insomnia.rest/download) (Para testar a API)


### Clone o repositório

```
git clone https://github.com/beckmanz/Clone-Backend-Twitter.git
```
### Variáveis de ambiente

Configure as variáveis de ambiente no arquivo appsettings.json

```json
"ConnectionStrings": {
    "DefaultConnection": "Your connection string"
  },
  "BaseURL": "http://localhost:8080",
  "Jwt": {
    "Key": "YourSecretKey",
    "Issuer": "YourIssue",
    "Audience": "YourAudience"
  }
```
  
### Crie e execute as migrações do banco de dados para criar as tabelas necessárias:

```
dotnet ef migrations add InitialMigration
dotnet ef database update
```
### Iniciando
Agora inicie o servidor

```
dotnet run
```

## 📍 API Endpoints

### Auth

| Rota                         | Descrição 
|------------------------------|----------
| <kbd>POST /Auth/Signup</kbd> | Registra um novo usuário [Detalhes da resposta](#signup)
| <kbd>POST /Auth/Signin</kbd> | Faz login do usuário [Detalhes da resposta](#signin)

<h3 id="signup">POST /Auth/Signup</h3>

**REQUISIÇÃO**
```json
{
  "name": "string",
  "email": "string",
  "password": "string"
}
```
**RESPOSTA**
```json
{
  "message": "Usuário cadastrado com sucesso!",
  "status": true,
  "data": {
    "name": "string",
    "slug": "string",
    "avatar": "string",
    "token": "string"
  }
}
```

<h3 id="signin">POST /Auth/Signin</h3>

**REQUISIÇÃO**
```json
{
  "email": "string",
  "password": "string"
}
```
**RESPOSTA**
```json
{
  "message": "Login realizado com sucesso!",
  "status": true,
  "data": {
    "name": "string",
    "slug": "string",
    "avatar": "string",
    "token": "string"
  }
}
```