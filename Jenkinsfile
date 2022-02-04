node{
  stage('SCM Checkout'){
    git 'https://github.com/josygeorge0102/OnlineEducationSystem.git'
  }
  stage('Docker compose build'){
    sh 'docker-compose build'
  }
  stage('Docker Hub Login'){
   sh 'docker login -u="josy98" -p="Jo27148193*"' 
  }
  
  stage('Docker compose push'){
    sh 'docker compose push'
  }
}
  
   
   
  

