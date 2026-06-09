# CHANGE THIS TO MATCH YOUR API URL
$baseUrl = "http://localhost:5068/api"

Write-Host ""
Write-Host "====================================="
Write-Host "TESTING REGISTER"
Write-Host "====================================="

$registerBody = @{
    name = "Jonadh"
    email = "jonadh@test.com"
    password = "123456"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod `
        -Uri "$baseUrl/Auth/register" `
        -Method Post `
        -Body $registerBody `
        -ContentType "application/json"

    Write-Host "REGISTER SUCCESS"
    $response | ConvertTo-Json -Depth 10
}
catch {
    Write-Host "REGISTER FAILED"
    Write-Host $_.Exception.Message
}

Write-Host ""

Write-Host "====================================="
Write-Host "TESTING LOGIN"
Write-Host "====================================="

$loginBody = @{
    email = "jonadh@test.com"
    password = "123456"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod `
        -Uri "$baseUrl/Auth/login" `
        -Method Post `
        -Body $loginBody `
        -ContentType "application/json"

    Write-Host "LOGIN SUCCESS"
    $response | ConvertTo-Json -Depth 10
}
catch {
    Write-Host "LOGIN FAILED"
    Write-Host $_.Exception.Message
}

Write-Host ""

Write-Host "====================================="
Write-Host "TESTING ADD TODO"
Write-Host "====================================="

$todoBody = @{
    title = "Learn Angular"
    description = "Finish Todo App"
} | ConvertTo-Json

$todoId = $null

try {
    $response = Invoke-RestMethod `
        -Uri "$baseUrl/Todo" `
        -Method Post `
        -Body $todoBody `
        -ContentType "application/json"

    Write-Host "ADD TODO SUCCESS"
    $response | ConvertTo-Json -Depth 10

    if ($response.id) {
        $todoId = $response.id
    }
}
catch {
    Write-Host "ADD TODO FAILED"
    Write-Host $_.Exception.Message
}

Write-Host ""

Write-Host "====================================="
Write-Host "TESTING GET ALL TODOS"
Write-Host "====================================="

try {
    $response = Invoke-RestMethod `
        -Uri "$baseUrl/Todo" `
        -Method Get

    Write-Host "GET TODOS SUCCESS"
    $response | ConvertTo-Json -Depth 10
}
catch {
    Write-Host "GET TODOS FAILED"
    Write-Host $_.Exception.Message
}

if ($todoId) {

    Write-Host ""

    Write-Host "====================================="
    Write-Host "TESTING UPDATE TODO"
    Write-Host "====================================="

    $updateBody = @{
        title = "Updated Todo"
        description = "Updated Description"
        isCompleted = $true
    } | ConvertTo-Json

    try {
        $response = Invoke-RestMethod `
            -Uri "$baseUrl/Todo/$todoId" `
            -Method Put `
            -Body $updateBody `
            -ContentType "application/json"

        Write-Host "UPDATE SUCCESS"
        $response | ConvertTo-Json -Depth 10
    }
    catch {
        Write-Host "UPDATE FAILED"
        Write-Host $_.Exception.Message
    }

    Write-Host ""

    Write-Host "====================================="
    Write-Host "TESTING DELETE TODO"
    Write-Host "====================================="

    try {
        $response = Invoke-RestMethod `
            -Uri "$baseUrl/Todo/$todoId" `
            -Method Delete

        Write-Host "DELETE SUCCESS"
        $response | ConvertTo-Json -Depth 10
    }
    catch {
        Write-Host "DELETE FAILED"
        Write-Host $_.Exception.Message
    }
}

Write-Host ""
Write-Host "====================================="
Write-Host "API TEST COMPLETE"
Write-Host "====================================="