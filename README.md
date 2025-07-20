# QApabilities - Sistema de Cadastro de Alunos

## Visão Geral

Sistema de cadastro de alunos para a escola QApabilities implementado com arquitetura de microsserviços, seguindo as melhores práticas de Clean Code e princípios SOLID.

## Arquitetura

### Microsserviços

1. **QApabilities.Gateway** - API Gateway para roteamento e autenticação
2. **QApabilities.Students.API** - Microsserviço de gerenciamento de alunos
3. **QApabilities.Courses.API** - Microsserviço de gerenciamento de cursos
4. **QApabilities.Enrollments.API** - Microsserviço de matrículas
5. **QApabilities.Shared** - Biblioteca compartilhada com DTOs e utilitários

### Tecnologias

- **.NET 8** - Framework principal
- **Docker** - Containerização
- **Kubernetes** - Orquestração de containers
- **SQL Server** - Banco de dados principal
- **RabbitMQ** - Message broker
- **Redis** - Cache distribuído
- **Serilog** - Logging estruturado
- **Health Checks** - Monitoramento de saúde
- **Swagger** - Documentação da API

## Estrutura do Projeto

```
QApabilitiesSchool/
├── src/
│   ├── Gateway/
│   ├── Students/
│   ├── Courses/
│   ├── Enrollments/
│   └── Shared/
├── infrastructure/
│   ├── docker/
│   ├── k8s/
│   └── scripts/
├── tests/
└── docs/
```

## Como Executar

### Pré-requisitos

- .NET 8 SDK
- Docker Desktop
- Kubernetes (Docker Desktop ou Minikube)

### Execução Local

```bash
# Clone o repositório
git clone https://github.com/qapabilities/QApabilitiesSchool.git
cd QApabilitiesSchool

# Execute com Docker Compose
docker-compose up -d

# Ou execute individualmente
dotnet run --project src/Gateway/QApabilities.Gateway
dotnet run --project src/Students/QApabilities.Students.API
dotnet run --project src/Courses/QApabilities.Courses.API
dotnet run --project src/Enrollments/QApabilities.Enrollments.API
```

### Execução no Kubernetes

```bash
# Aplique os manifests
kubectl apply -f infrastructure/k8s/

# Verifique os pods
kubectl get pods
```

## APIs Disponíveis

- **Gateway**: http://localhost:5000
- **Students API**: http://localhost:5001
- **Courses API**: http://localhost:5002
- **Enrollments API**: http://localhost:5003

## Documentação

- Swagger UI disponível em cada API em `/swagger`
- Documentação completa em `/docs`

## Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT. 