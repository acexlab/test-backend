pipeline {
 
    agent any
 
    environment {
        IMAGE = "todo-api:${BUILD_NUMBER}"
        NETWORK = "app-net"
        MYSQL_CONT = "todo-mysql"
        API_CONT = "todo-api"
        MYSQL_PWD = "rootpassword"
        MYSQL_DB = "tododb"
    }
 
    stages {
 
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
 
        stage('Build Docker Image') {
            steps {
                bat "docker build -t %IMAGE% ."
            }
        }
 
        stage('Start MySQL') {
            steps {
                bat '''
                docker network create %NETWORK% 2>nul
 
                docker ps -q -f name=^/%MYSQL_CONT%$ > temp.txt
                set /p CONTAINER_ID=<temp.txt
 
                IF "%CONTAINER_ID%"=="" (
                    docker rm -f %MYSQL_CONT% 2>nul
                    docker run -d --name %MYSQL_CONT% --network %NETWORK% ^
                        -e MYSQL_ROOT_PASSWORD=%MYSQL_PWD% ^
                        -e MYSQL_DATABASE=%MYSQL_DB% ^
                        -p 3306:3306 ^
                        mysql:8.0
 
                    echo Waiting for MySQL to initialise...
                    timeout /t 30
                ) ELSE (
                    echo MySQL already running.
                )
                '''
            }
        }
 
        stage('Run API') {
            steps {
                bat '''
                docker rm -f %API_CONT% 2>nul
 
                docker run -d --name %API_CONT% --network %NETWORK% ^
                    -e ConnectionStrings__DefaultConnection="Server=%MYSQL_CONT%;Port=3306;Database=%MYSQL_DB%;User=root;Password=%MYSQL_PWD%;" ^
                    -p 5000:8080 ^
                    %IMAGE%
                '''
            }
        }
    }
}