# Guia de Início Rápido - QApabilities

Este guia irá ajudá-lo a configurar e executar o projeto QApabilities em seu ambiente de desenvolvimento.

## Pré-requisitos

### Software Necessário
- **.NET 8 SDK**: [Download aqui](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Docker Desktop**: [Download aqui](https://www.docker.com/products/docker-desktop)
- **Visual Studio 2022** ou **VS Code**: [Download aqui](https://visualstudio.microsoft.com/downloads/)
- **Git**: [Download aqui](https://git-scm.com/downloads)

### Verificação dos Pré-requisitos
```bash
# Verificar versão do .NET
dotnet --version

# Verificar Docker
docker --version
docker-compose --version

# Verificar Git
git --version
```

## Configuração Inicial

### 1. Clone o Repositório
```bash
git clone https://github.com/qapabilities/QApabilitiesSchool.git
cd QApabilitiesSchool
```

### 2. Configuração do Ambiente
```bash
# Restaurar dependências
dotnet restore

# Build da solução
dotnet build
```

## Execução Local

### Opção 1: Docker Compose (Recomendado)

```bash
# Executar todos os serviços
docker-compose up -d

# Verificar status dos containers
docker-compose ps

# Visualizar logs
docker-compose logs -f students-api
```

### Opção 2: Execução Individual

```bash
# Terminal 1 - Students API
cd src/Students/QApabilities.Students.API
dotnet run

# Terminal 2 - Courses API (quando implementado)
cd src/Courses/QApabilities.Courses.API
dotnet run

# Terminal 3 - Enrollments API (quando implementado)
cd src/Enrollments/QApabilities.Enrollments.API
dotnet run
```

## URLs de Acesso

Após a execução, você pode acessar:

- **Students API**: http://localhost:5001/swagger
- **Courses API**: http://localhost:5002/swagger
- **Enrollments API**: http://localhost:5003/swagger
- **Gateway**: http://localhost:5000
- **RabbitMQ Management**: http://localhost:15672 (admin/admin123)
- **SQL Server**: localhost,1433 (sa/YourStrong@Passw0rd)

## Testando a API

### 1. Via Swagger UI
1. Acesse http://localhost:5001/swagger
2. Explore os endpoints disponíveis
3. Execute testes diretamente na interface

### 2. Via cURL

#### Criar um Aluno
```bash
curl -X POST "http://localhost:5001/api/students" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao.silva@email.com",
    "cpf": "12345678901",
    "birthDate": "1990-01-01T00:00:00Z",
    "phone": "(11) 99999-9999",
    "address": "Rua das Flores, 123 - São Paulo/SP"
  }'
```

#### Listar Alunos
```bash
curl -X GET "http://localhost:5001/api/students?pageNumber=1&pageSize=10"
```

#### Buscar Aluno por ID
```bash
curl -X GET "http://localhost:5001/api/students/{id}"
```

#### Atualizar Aluno
```bash
curl -X PUT "http://localhost:5001/api/students/{id}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva Atualizado",
    "email": "joao.atualizado@email.com",
    "phone": "(11) 88888-8888",
    "address": "Rua das Palmeiras, 456 - São Paulo/SP"
  }'
```

#### Remover Aluno
```bash
curl -X DELETE "http://localhost:5001/api/students/{id}"
```

## Desenvolvimento

### Estrutura de Pastas
```
src/
├── Shared/                    # Biblioteca compartilhada
├── Students/                  # Microsserviço de alunos
├── Courses/                   # Microsserviço de cursos
├── Enrollments/              # Microsserviço de matrículas
└── Gateway/                  # API Gateway
```

### Adicionando Novos Endpoints

1. **Criar DTO** em `src/Shared/QApabilities.Shared/DTOs/`
2. **Criar Validador** em `src/Students/QApabilities.Students.API/Validators/`
3. **Adicionar método no Repository** em `src/Students/QApabilities.Students.API/Repositories/`
4. **Adicionar método no Service** em `src/Students/QApabilities.Students.API/Services/`
5. **Criar endpoint no Controller** em `src/Students/QApabilities.Students.API/Controllers/`

### Exemplo: Adicionar Endpoint de Busca por Email

#### 1. Adicionar método no Repository
```csharp
// IStudentRepository.cs
Task<Student?> GetByEmailAsync(string email);

// StudentRepository.cs
public async Task<Student?> GetByEmailAsync(string email)
{
    return await _context.Students
        .FirstOrDefaultAsync(s => s.Email == email && s.IsActive);
}
```

#### 2. Adicionar método no Service
```csharp
// IStudentService.cs
Task<ApiResponse<StudentDto>> GetByEmailAsync(string email);

// StudentService.cs
public async Task<ApiResponse<StudentDto>> GetByEmailAsync(string email)
{
    var student = await _studentRepository.GetByEmailAsync(email);
    
    if (student == null)
        return ApiResponse<StudentDto>.ErrorResult("Aluno não encontrado");

    var studentDto = _mapper.Map<StudentDto>(student);
    return ApiResponse<StudentDto>.SuccessResult(studentDto);
}
```

#### 3. Adicionar endpoint no Controller
```csharp
[HttpGet("email/{email}")]
[ProducesResponseType(typeof(ApiResponse<StudentDto>), 200)]
[ProducesResponseType(typeof(ApiResponse<StudentDto>), 404)]
public async Task<ActionResult<ApiResponse<StudentDto>>> GetByEmail(string email)
{
    var result = await _studentService.GetByEmailAsync(email);
    
    if (!result.Success)
        return NotFound(result);
        
    return Ok(result);
}
```

## Debugging

### Visual Studio
1. Abra a solução no Visual Studio
2. Configure múltiplos projetos de inicialização
3. Pressione F5 para iniciar o debug

### VS Code
1. Abra a pasta do projeto no VS Code
2. Configure o launch.json para múltiplos projetos
3. Use F5 para iniciar o debug

### Docker
```bash
# Debug com logs detalhados
docker-compose up --build

# Acessar container
docker exec -it qapabilities-students-api bash

# Ver logs em tempo real
docker-compose logs -f students-api
```

## Testes

### Executar Testes Unitários
```bash
# Todos os testes
dotnet test

# Testes específicos
dotnet test src/Students/QApabilities.Students.API.Tests/

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### Executar Testes de Integração
```bash
# Testes de integração
dotnet test tests/QApabilities.Integration.Tests/
```

## Deploy

### Deploy Local com Docker
```bash
# Build e deploy
./scripts/build-and-deploy.ps1

# Apenas build
docker-compose build

# Deploy para produção
./scripts/build-and-deploy.ps1 -Environment Production
```

### Deploy para Kubernetes
```bash
# Verificar kubectl
kubectl version

# Aplicar manifests
kubectl apply -f infrastructure/k8s/

# Verificar status
kubectl get pods -n qapabilities

# Acessar logs
kubectl logs -f deployment/qapabilities-students-api -n qapabilities
```

## Troubleshooting

### Problemas Comuns

#### 1. Porta já em uso
```bash
# Verificar processos na porta
netstat -ano | findstr :5001

# Matar processo
taskkill /PID <PID> /F
```

#### 2. Docker não inicia
```bash
# Reiniciar Docker Desktop
# Verificar recursos alocados
# Verificar WSL2 (Windows)
```

#### 3. Banco de dados não conecta
```bash
# Verificar string de conexão
# Verificar se SQL Server está rodando
# Verificar firewall
```

#### 4. Build falha
```bash
# Limpar cache
dotnet clean
dotnet restore

# Verificar versão do .NET
dotnet --version
```

### Logs e Monitoramento

#### Health Checks
```bash
# Verificar saúde dos serviços
curl http://localhost:5001/health
```

#### Logs Estruturados
```bash
# Ver logs do Serilog
docker-compose logs students-api | grep "Information"
```

## Próximos Passos

1. **Implementar Microsserviços Restantes**:
   - Courses API
   - Enrollments API
   - Gateway

2. **Adicionar Funcionalidades**:
   - Autenticação e autorização
   - Cache com Redis
   - Message broker com RabbitMQ

3. **Melhorar Monitoramento**:
   - Métricas com Prometheus
   - Tracing com Jaeger
   - Dashboards com Grafana

4. **Implementar CI/CD**:
   - Azure DevOps pipelines
   - GitHub Actions
   - Deploy automático

## Suporte

- **Documentação**: Consulte a pasta `docs/`
- **Issues**: Abra uma issue no repositório
- **Wiki**: Documentação adicional no GitHub

## Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

---

**Boa sorte com o desenvolvimento! 🚀** 