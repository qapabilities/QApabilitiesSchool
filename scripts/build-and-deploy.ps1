# Script de Build e Deploy para QApabilities
# Autor: QApabilities Team
# Data: 2024

param(
    [string]$Environment = "Development",
    [string]$Registry = "qapabilities",
    [switch]$SkipTests = $false,
    [switch]$DeployToK8s = $false
)

Write-Host "🚀 Iniciando Build e Deploy do QApabilities" -ForegroundColor Green
Write-Host "Environment: $Environment" -ForegroundColor Yellow
Write-Host "Registry: $Registry" -ForegroundColor Yellow

# Função para executar comandos e verificar erros
function Invoke-CommandWithErrorCheck {
    param(
        [string]$Command,
        [string]$Description
    )
    
    Write-Host "📋 $Description..." -ForegroundColor Cyan
    Invoke-Expression $Command
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Erro ao executar: $Description" -ForegroundColor Red
        exit $LASTEXITCODE
    }
    
    Write-Host "✅ $Description concluído com sucesso" -ForegroundColor Green
}

# Limpar builds anteriores
Write-Host "🧹 Limpando builds anteriores..." -ForegroundColor Cyan
dotnet clean
if (Test-Path "bin") { Remove-Item -Recurse -Force "bin" }
if (Test-Path "obj") { Remove-Item -Recurse -Force "obj" }

# Restaurar dependências
Invoke-CommandWithErrorCheck "dotnet restore" "Restaurando dependências"

# Executar testes (se não for pulado)
if (-not $SkipTests) {
    Write-Host "🧪 Executando testes..." -ForegroundColor Cyan
    $testProjects = Get-ChildItem -Path "tests" -Filter "*.csproj" -Recurse
    
    foreach ($testProject in $testProjects) {
        Write-Host "Executando testes em: $($testProject.Name)" -ForegroundColor Yellow
        dotnet test $testProject.FullName --no-build --verbosity normal
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "❌ Testes falharam em: $($testProject.Name)" -ForegroundColor Red
            exit $LASTEXITCODE
        }
    }
    Write-Host "✅ Todos os testes passaram" -ForegroundColor Green
}

# Build das aplicações
Write-Host "🔨 Fazendo build das aplicações..." -ForegroundColor Cyan

$projects = @(
    "src/Shared/QApabilities.Shared",
    "src/Students/QApabilities.Students.API",
    "src/Courses/QApabilities.Courses.API", 
    "src/Enrollments/QApabilities.Enrollments.API",
    "src/Gateway/QApabilities.Gateway"
)

foreach ($project in $projects) {
    if (Test-Path $project) {
        Write-Host "Build: $project" -ForegroundColor Yellow
        Invoke-CommandWithErrorCheck "dotnet build $project -c Release" "Build de $project"
    }
}

# Publicar aplicações
Write-Host "📦 Publicando aplicações..." -ForegroundColor Cyan

foreach ($project in $projects) {
    if (Test-Path $project) {
        $projectName = Split-Path $project -Leaf
        Write-Host "Publicando: $projectName" -ForegroundColor Yellow
        Invoke-CommandWithErrorCheck "dotnet publish $project -c Release -o ./publish/$projectName" "Publicação de $projectName"
    }
}

# Build das imagens Docker
Write-Host "🐳 Build das imagens Docker..." -ForegroundColor Cyan

$services = @(
    @{Name="students-api"; Path="src/Students"; Dockerfile="QApabilities.Students.API/Dockerfile"},
    @{Name="courses-api"; Path="src/Courses"; Dockerfile="QApabilities.Courses.API/Dockerfile"},
    @{Name="enrollments-api"; Path="src/Enrollments"; Dockerfile="QApabilities.Enrollments.API/Dockerfile"},
    @{Name="gateway"; Path="src/Gateway"; Dockerfile="QApabilities.Gateway/Dockerfile"}
)

foreach ($service in $services) {
    if (Test-Path $service.Path) {
        $imageName = "$Registry/$($service.Name):latest"
        Write-Host "Build Docker: $imageName" -ForegroundColor Yellow
        
        Invoke-CommandWithErrorCheck "docker build -t $imageName -f $($service.Path)/$($service.Dockerfile) $($service.Path)" "Build Docker de $($service.Name)"
    }
}

# Deploy para Kubernetes (se solicitado)
if ($DeployToK8s) {
    Write-Host "☸️ Deploy para Kubernetes..." -ForegroundColor Cyan
    
    # Verificar se kubectl está disponível
    try {
        kubectl version --client
    }
    catch {
        Write-Host "❌ kubectl não encontrado. Instale o kubectl para continuar." -ForegroundColor Red
        exit 1
    }
    
    # Aplicar manifests
    Write-Host "Aplicando manifests do Kubernetes..." -ForegroundColor Yellow
    
    $k8sManifests = @(
        "infrastructure/k8s/namespace.yaml",
        "infrastructure/k8s/configmap.yaml", 
        "infrastructure/k8s/secret.yaml",
        "infrastructure/k8s/sqlserver-deployment.yaml",
        "infrastructure/k8s/students-api-deployment.yaml"
    )
    
    foreach ($manifest in $k8sManifests) {
        if (Test-Path $manifest) {
            Write-Host "Aplicando: $manifest" -ForegroundColor Yellow
            Invoke-CommandWithErrorCheck "kubectl apply -f $manifest" "Aplicação de $manifest"
        }
    }
    
    # Aguardar pods ficarem prontos
    Write-Host "⏳ Aguardando pods ficarem prontos..." -ForegroundColor Cyan
    Invoke-CommandWithErrorCheck "kubectl wait --for=condition=ready pod -l app=qapabilities-students-api -n qapabilities --timeout=300s" "Aguardando pods prontos"
    
    Write-Host "✅ Deploy para Kubernetes concluído" -ForegroundColor Green
}

# Deploy local com Docker Compose (padrão)
if (-not $DeployToK8s) {
    Write-Host "🐳 Deploy local com Docker Compose..." -ForegroundColor Cyan
    
    # Parar containers existentes
    Write-Host "Parando containers existentes..." -ForegroundColor Yellow
    docker-compose down
    
    # Build e start dos containers
    Invoke-CommandWithErrorCheck "docker-compose up -d --build" "Deploy com Docker Compose"
    
    Write-Host "✅ Deploy local concluído" -ForegroundColor Green
    Write-Host "🌐 URLs disponíveis:" -ForegroundColor Cyan
    Write-Host "   - Students API: http://localhost:5001/swagger" -ForegroundColor White
    Write-Host "   - Courses API: http://localhost:5002/swagger" -ForegroundColor White
    Write-Host "   - Enrollments API: http://localhost:5003/swagger" -ForegroundColor White
    Write-Host "   - Gateway: http://localhost:5000" -ForegroundColor White
    Write-Host "   - RabbitMQ Management: http://localhost:15672" -ForegroundColor White
}

Write-Host "🎉 Build e Deploy concluído com sucesso!" -ForegroundColor Green 